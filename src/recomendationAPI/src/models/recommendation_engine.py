from typing import List, Dict
import pandas as pd
import joblib
import os
import numpy as np
from schemas.request_response import RecomendacaoRequest, RecomendacaoResponse, AtivoRecomendadoOutput

class RecommendationEngine:
    def __init__(self, df_barema: pd.DataFrame, df_subgrupo_dist: pd.DataFrame, 
                 kmeans_model_path: str, similarity_matrices_path: str):
        self.df_barema = df_barema
        self.df_subgrupo_dist = df_subgrupo_dist
        self.kmeans_model_path = kmeans_model_path
        self.similarity_matrices_path = similarity_matrices_path
        
        # Carregar modelo K-Means
        try:
            self.kmeans_model = joblib.load(kmeans_model_path)
            print(f"Modelo K-Means carregado de: {kmeans_model_path}")
        except Exception as e:
            print(f"Erro ao carregar modelo K-Means: {e}")
            raise
        
        # Carregar matrizes de similaridade
        self.similarity_matrices = self._load_similarity_matrices()
        
        # Definir subgrupos disponíveis
        if 'CONTA' in self.df_subgrupo_dist.columns:
            self.df_subgrupo_dist.set_index('CONTA', inplace=True)
        self.todos_subgrupos_kmeans = [col for col in self.df_subgrupo_dist.columns if col != 'ClusterKMeans']

    def _load_similarity_matrices(self) -> Dict[int, pd.DataFrame]:
        """Carrega todas as matrizes de similaridade por cluster"""
        matrices = {}
        
        if not os.path.exists(self.similarity_matrices_path):
            print(f"AVISO: Diretório de matrizes não encontrado: {self.similarity_matrices_path}")
            return matrices
        
        for filename in os.listdir(self.similarity_matrices_path):
            if filename.startswith('item_sim_matrix_cluster_') and filename.endswith('.csv'):
                try:
                    cluster_id = int(filename.split('_')[-1].split('.')[0])
                    filepath = os.path.join(self.similarity_matrices_path, filename)
                    matrix = pd.read_csv(filepath, index_col=0)
                    matrices[cluster_id] = matrix
                    print(f"Matriz do Cluster {cluster_id} carregada: {matrix.shape}")
                except Exception as e:
                    print(f"Erro ao carregar matriz do cluster: {e}")
        
        return matrices

    def _calculate_client_profile(self, ativos_cliente: List[Dict]) -> tuple:
        """Calcula o perfil RF/RV do cliente"""
        df_carteira = pd.DataFrame(ativos_cliente)
        df_carteira.rename(columns={'CodigoAtivo': 'NomeAtivo', 'ValorInvestido': 'VALOR INVESTIDO'}, inplace=True)
        
        # Debug: Verificar dados antes do merge
        print(f"DEBUG: Colunas do barema: {list(self.df_barema.columns)}")
        print(f"DEBUG: Colunas da carteira: {list(df_carteira.columns)}")
        print(f"DEBUG: Ativos na carteira: {df_carteira['NomeAtivo'].tolist()}")
        
        # Verificar se as colunas existem antes do merge
        required_cols = ['NomeAtivo', 'ClassificacaoDoAtivo']
        missing_cols = [col for col in required_cols if col not in self.df_barema.columns]
        if missing_cols:
            raise ValueError(f"Colunas necessárias não encontradas no barema: {missing_cols}. Colunas disponíveis: {list(self.df_barema.columns)}")
        
        # Merge com barema para obter classificação
        df_carteira_merged = pd.merge(
            df_carteira,
            self.df_barema[['NomeAtivo', 'ClassificacaoDoAtivo']],
            on='NomeAtivo',
            how='left'
        )
        
        # Debug: Verificar resultado do merge
        print(f"DEBUG: Resultado do merge - Shape: {df_carteira_merged.shape}")
        print(f"DEBUG: Ativos encontrados no barema: {df_carteira_merged.dropna(subset=['ClassificacaoDoAtivo']).shape[0]}")
        
        # Remover ativos não encontrados
        df_carteira_merged.dropna(subset=['ClassificacaoDoAtivo'], inplace=True)
        
        if df_carteira_merged.empty:
            return "Indefinido (sem ativos válidos)", 0.0, 0.0
        
        # Calcular distribuição RF/RV
        dist_financeira = df_carteira_merged.groupby('ClassificacaoDoAtivo')['VALOR INVESTIDO'].sum()
        total_investido = dist_financeira.sum()
        
        if total_investido > 0:
            percentual_financeiro = (dist_financeira / total_investido * 100).fillna(0)
            rf_pct = percentual_financeiro.get('Renda Fixa', 0.0)
            rv_pct = percentual_financeiro.get('Renda Variável', 0.0)
            
            if rf_pct >= 90.0:
                perfil_calculado = "Conservador"
            elif rv_pct >= 70.0:
                perfil_calculado = "Sofisticado"
            else:
                perfil_calculado = "Moderado"
        else:
            perfil_calculado = "Indefinido"
            rf_pct = 0.0
            rv_pct = 0.0
        
        return perfil_calculado, rf_pct, rv_pct

    def _calculate_subgroup_distribution(self, ativos_cliente: List[Dict]) -> pd.Series:
        """Calcula a distribuição de subgrupos do cliente"""
        df_carteira = pd.DataFrame(ativos_cliente)
        df_carteira.rename(columns={'CodigoAtivo': 'NomeAtivo', 'ValorInvestido': 'VALOR INVESTIDO'}, inplace=True)
        
        # Merge com barema para obter subgrupos
        df_carteira_merged = pd.merge(
            df_carteira,
            self.df_barema[['NomeAtivo', 'Subgrupo']],
            on='NomeAtivo',
            how='left'
        )
        
        df_carteira_merged.dropna(subset=['Subgrupo'], inplace=True)
        
        if df_carteira_merged.empty:
            # Retornar série com zeros para todos os subgrupos
            return pd.Series(0.0, index=self.todos_subgrupos_kmeans)
        
        # Calcular distribuição de subgrupos
        subgrupo_dist_valores = df_carteira_merged.groupby('Subgrupo')['VALOR INVESTIDO'].sum()
        total_investido = subgrupo_dist_valores.sum()
        
        if total_investido > 0:
            subgrupo_dist_pct = (subgrupo_dist_valores / total_investido * 100).fillna(0)
        else:
            subgrupo_dist_pct = pd.Series(0.0, index=subgrupo_dist_valores.index)
        
        # Criar vetor completo para K-Means
        vetor_cliente = pd.Series(0.0, index=self.todos_subgrupos_kmeans)
        for subgrupo, pct in subgrupo_dist_pct.items():
            if subgrupo in vetor_cliente.index:
                vetor_cliente[subgrupo] = pct
        
        return vetor_cliente

    def generate_recommendations(self, request: RecomendacaoRequest) -> RecomendacaoResponse:
        """Gera recomendações para o cliente"""
        try:
            # Converter ativos para formato de dicionário
            ativos_cliente = [
                {"CodigoAtivo": ativo.CodigoAtivo, "ValorInvestido": ativo.ValorInvestido}
                for ativo in request.Ativos
            ]
            
            # Calcular perfil do cliente
            perfil_calculado, rf_pct, rv_pct = self._calculate_client_profile(ativos_cliente)
            
            # Determinar tipo de ativo sugerido para ajuste
            type_to_buy = None
            
            if perfil_calculado != request.ClassificacaoDeclarada:
                if request.ClassificacaoDeclarada == "Conservador" and rf_pct < 90.0:
                    type_to_buy = "Renda Fixa"
                elif request.ClassificacaoDeclarada == "Sofisticado" and rv_pct < 70.0:
                    type_to_buy = "Renda Variável"
                elif request.ClassificacaoDeclarada == "Moderado":
                    if rf_pct >= 90.0:
                        type_to_buy = "Renda Variável"
                    elif rv_pct >= 70.0:
                        type_to_buy = "Renda Fixa"
            
            # Calcular distribuição de subgrupos e cluster
            vetor_subgrupos = self._calculate_subgroup_distribution(ativos_cliente)
            vetor_cliente_np = vetor_subgrupos.fillna(0).values.reshape(1, -1)
            cluster_id = self.kmeans_model.predict(vetor_cliente_np)[0]
            
            print(f"Cliente classificado no Cluster: {cluster_id}")
            
            # Gerar recomendações do cluster
            recomendacoes = self._get_recommendations_from_cluster(
                cluster_id, ativos_cliente, type_to_buy
            )
            
            return RecomendacaoResponse(
                ClienteIdSolicitado=request.ClienteId,
                PerfilCalculadoCliente=perfil_calculado,
                PerfilDeclaradoCliente=request.ClassificacaoDeclarada,
                ClusterAssignado=cluster_id,
                TipoAtivoSugeridoAjuste=type_to_buy,
                AtivosRecomendados=recomendacoes
            )
            
        except Exception as e:
            print(f"Erro interno no RecommendationEngine: {e}")
            raise Exception(f"Erro ao gerar recomendações: {e}")

    def _get_recommendations_from_cluster(self, cluster_id: int, ativos_cliente: List[Dict], 
                                        type_to_buy: str = None) -> List[AtivoRecomendadoOutput]:
        """Gera recomendações baseadas na similaridade do cluster"""
        
        # Verificar se matriz existe para o cluster
        if cluster_id not in self.similarity_matrices:
            print(f"AVISO: Matriz de similaridade não encontrada para cluster {cluster_id}")
            return []
        
        matriz_sim = self.similarity_matrices[cluster_id]
        
        if matriz_sim.empty:
            print(f"AVISO: Matriz de similaridade vazia para cluster {cluster_id}")
            return []
        
        # Calcular scores de similaridade
        item_scores = pd.Series(0.0, index=matriz_sim.index)
        ativos_cliente_set = set([ativo['CodigoAtivo'] for ativo in ativos_cliente])
        
        for ativo_cliente in ativos_cliente_set:
            if ativo_cliente in matriz_sim.index:
                similar_assets = matriz_sim.loc[ativo_cliente]
                # Remover ativos que o cliente já possui
                similar_assets_filtered = similar_assets.drop(
                    list(ativos_cliente_set.intersection(similar_assets.index)), 
                    errors='ignore'
                )
                item_scores = item_scores.add(similar_assets_filtered, fill_value=0)
            else:
                print(f"Ativo {ativo_cliente} não encontrado na matriz do cluster {cluster_id}")
        
        # Filtrar por tipo de ativo se especificado
        if type_to_buy:
            ativos_do_tipo = self.df_barema[
                self.df_barema['ClassificacaoDoAtivo'] == type_to_buy
            ]['NomeAtivo'].tolist()
            item_scores = item_scores[item_scores.index.isin(ativos_do_tipo)]
        
        # Top 10 recomendações
        top_recommendations = item_scores.sort_values(ascending=False).head(10)
        
        # Criar lista de recomendações
        recomendacoes = []
        for ativo, score in top_recommendations.items():
            if score > 0:  # Apenas scores positivos
                ativo_info = self.df_barema[self.df_barema['NomeAtivo'] == ativo]
                
                if not ativo_info.empty:
                    tipo_ativo = ativo_info.iloc[0].get('ClassificacaoDoAtivo', 'N/A')
                    subgrupo = ativo_info.iloc[0].get('Subgrupo', 'N/A')
                else:
                    tipo_ativo = 'N/A'
                    subgrupo = 'N/A'
                
                recomendacoes.append(AtivoRecomendadoOutput(
                    CodigoAtivo=ativo,
                    TipoAtivo=tipo_ativo,
                    Subgrupo=subgrupo,
                    Score=round(float(score), 4)
                ))
        
        return recomendacoes