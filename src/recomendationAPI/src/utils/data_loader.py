import pandas as pd
import os
from typing import Tuple

def load_csv(file_path: str) -> pd.DataFrame:
    if not os.path.exists(file_path):
        raise FileNotFoundError(f"File not found: {file_path}")
    return pd.read_csv(file_path)

def load_barema_data(base_path: str) -> pd.DataFrame:
    path_barema = os.path.join(base_path, 'processed', 'barema_por_ativo.csv')
    return load_csv(path_barema)

class DataLoader:
    """Classe utilitária para carregar dados necessários para a API"""
    
    def __init__(self, base_path: str):
        self.base_path = base_path
        
    def load_all_data(self) -> Tuple[pd.DataFrame, pd.DataFrame, pd.DataFrame, pd.DataFrame]:
        """
        Carrega todos os dados necessários para a API
        
        Returns:
            Tuple contendo (df_barema, df_class_tipos, df_subgrupo_dist, df_user_item)
        """
        try:
            # Definir caminhos
            barema_path = os.path.join(self.base_path, 'processed/barema_por_ativo.csv')
            class_tipos_path = os.path.join(self.base_path, 'raw/classificacao_de_tipos.csv')
            subgrupo_dist_path = os.path.join(self.base_path, 'processed/subgrupo_distribuicao.csv')
            user_item_path = os.path.join(self.base_path, 'processed/user_item_binario.csv')
            
            # Carregar dados
            df_barema = pd.read_csv(barema_path)
            df_class_tipos = pd.read_csv(class_tipos_path)
            df_subgrupo_dist = pd.read_csv(subgrupo_dist_path)
            df_user_item = pd.read_csv(user_item_path)
            
            # Debug: mostrar colunas originais
            print(f"Colunas originais do barema: {list(df_barema.columns)}")
            
            # Padronizar nomes de colunas do barema de forma robusta
            df_barema = self._standardize_barema_columns(df_barema)
            
            # Verificar se as colunas necessárias existem após a padronização
            required_cols = ['NomeAtivo', 'ClassificacaoDoAtivo', 'Subgrupo']
            missing_cols = [col for col in required_cols if col not in df_barema.columns]
            if missing_cols:
                raise ValueError(f"Colunas necessárias não encontradas no barema: {missing_cols}")
            
            print(f"Dados carregados com sucesso:")
            print(f"- Barema: {df_barema.shape} - Colunas finais: {list(df_barema.columns)}")
            print(f"- Classificação Tipos: {df_class_tipos.shape}")
            print(f"- Subgrupo Distribuição: {df_subgrupo_dist.shape}")
            print(f"- User Item: {df_user_item.shape}")
            
            return df_barema, df_class_tipos, df_subgrupo_dist, df_user_item
            
        except FileNotFoundError as e:
            raise Exception(f"Arquivo não encontrado: {e}")
        except Exception as e:
            raise Exception(f"Erro ao carregar dados: {e}")
    
    def _standardize_barema_columns(self, df_barema: pd.DataFrame) -> pd.DataFrame:
        """Padroniza os nomes das colunas do barema"""
        # Criar um mapeamento de possíveis variações de nomes de colunas
        column_mapping = {}
        
        # Mapeamento para NomeAtivo
        nome_ativo_variations = [
            'NOME ATIVO', 'Nome Ativo', 'nome_ativo', 'NomeAtivo', 
            'Ativo', 'ATIVO', 'Nome_Ativo', 'NOME_ATIVO'
        ]
        for col in df_barema.columns:
            if col in nome_ativo_variations:
                column_mapping[col] = 'NomeAtivo'
                break
        
        # Mapeamento para ClassificacaoDoAtivo
        class_ativo_variations = [
            'CLASSIFICACAO DO ATIVO', 'Classificacao Do Ativo', 'classificacao_do_ativo', 
            'ClassificacaoDoAtivo', 'Classificacao', 'CLASSIFICACAO', 'TipoAtivo', 
            'TIPO_ATIVO', 'Classificacao_Do_Ativo', 'CLASSIFICACAO_DO_ATIVO'
        ]
        for col in df_barema.columns:
            if col in class_ativo_variations:
                column_mapping[col] = 'ClassificacaoDoAtivo'
                break
        
        # Mapeamento para Subgrupo
        subgrupo_variations = [
            'SUBGRUPO', 'Subgrupo', 'subgrupo', 'SubGrupo', 
            'Sub_Grupo', 'SUB_GRUPO', 'Sub Grupo'
        ]
        for col in df_barema.columns:
            if col in subgrupo_variations:
                column_mapping[col] = 'Subgrupo'
                break
        
        # Aplicar o mapeamento
        if column_mapping:
            df_barema = df_barema.rename(columns=column_mapping)
            print(f"Colunas renomeadas no barema: {column_mapping}")
        else:
            print("AVISO: Nenhuma coluna foi renomeada no barema")
        
        return df_barema
    
    def validate_data_integrity(self, df_barema: pd.DataFrame, df_subgrupo_dist: pd.DataFrame) -> bool:
        """
        Valida a integridade dos dados carregados
        
        Args:
            df_barema: DataFrame do barema
            df_subgrupo_dist: DataFrame de distribuição de subgrupos
            
        Returns:
            True se os dados estão íntegros
        """
        # Verificar colunas essenciais do barema
        required_barema_cols = ['NomeAtivo', 'ClassificacaoDoAtivo', 'Subgrupo']
        missing_barema_cols = [col for col in required_barema_cols if col not in df_barema.columns]
        
        if missing_barema_cols:
            print(f"ERRO: Colunas essenciais ausentes no barema: {missing_barema_cols}")
            return False
        
        # Verificar se há dados válidos
        if df_barema.empty:
            print("ERRO: DataFrame do barema está vazio")
            return False
        
        if df_subgrupo_dist.empty:
            print("ERRO: DataFrame de distribuição de subgrupos está vazio")
            return False
        
        print("Validação de integridade dos dados: PASSOU")
        return True

def load_similarity_matrix(base_path: str, cluster_id: int) -> pd.DataFrame:
    path_similarity_matrix = os.path.join(base_path, 'models', 'similarity_matrices', f'item_sim_matrix_cluster_{cluster_id}.csv')
    return load_csv(path_similarity_matrix)

def load_kmeans_model(base_path: str):
    from joblib import load
    path_kmeans_model = os.path.join(base_path, 'models', 'kmeans_model.joblib')
    return load(path_kmeans_model)