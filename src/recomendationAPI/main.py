from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field
import pandas as pd
import numpy as np # Embora não usado diretamente aqui, pandas depende dele
from typing import List, Tuple, Dict, Optional, Any
from datetime import datetime
import os

# --- Modelos Pydantic ---
class AtivoClienteInput(BaseModel):
    CodigoAtivo: str
    ValorInvestido: float

class RecomendacaoRequest(BaseModel):
    ClienteId: str = Field(..., example="Cliente_Teste_001")
    Ativos: List[AtivoClienteInput] = Field(..., example=[{"CodigoAtivo": "1SANPN", "ValorInvestido": 5000.0}])
    ClassificacaoDeclarada: str = Field(..., example="Moderado")

class AtivoRecomendadoOutput(BaseModel):
    CodigoAtivo: str
    TipoAtivo: Optional[str] = "N/A"
    Subgrupo: Optional[str] = "N/A"
    Score: float

class RecomendacaoResponse(BaseModel):
    ClienteIdSolicitado: str
    PerfilCalculadoCliente: str
    PerfilDeclaradoCliente: str  
    Recomendacoes: List[AtivoRecomendadoOutput]
    Timestamp: str

# --- Configuração do App FastAPI ---
app = FastAPI(title="API de Recomendação BTG (Super Simples)")

# --- Variáveis Globais para os Dados Carregados ---
DF_BAREMA: Optional[pd.DataFrame] = None
DF_ITEM_SIM_MATRIX: Optional[pd.DataFrame] = None
TODOS_ATIVOS_DA_MATRIZ_SIM: List[str] = [] # Para inicializar o item_scores

# --- Função para Carregar Dados na Inicialização ---
def carregar_dados_essenciais():
    global DF_BAREMA, DF_ITEM_SIM_MATRIX, TODOS_ATIVOS_DA_MATRIZ_SIM
    
    base_path = './data/' 
    path_barema = os.path.join(base_path, 'barema_por_ativo.csv')
    path_item_sim = os.path.join(base_path, 'item_similarity_matrix_PRECALCULADA.csv')

    try:
        print(f"Carregando {path_barema}...")
        DF_BAREMA = pd.read_csv(path_barema)

        print(f"Carregando {path_item_sim}...")
        DF_ITEM_SIM_MATRIX = pd.read_csv(path_item_sim, index_col=0)
        TODOS_ATIVOS_DA_MATRIZ_SIM = DF_ITEM_SIM_MATRIX.index.tolist()
        
        print("Dados essenciais carregados com sucesso!")
        print(f"Barema: {DF_BAREMA.shape}, Item Sim Matrix: {DF_ITEM_SIM_MATRIX.shape}")

    except FileNotFoundError as e:
        print(f"Erro CRÍTICO ao carregar CSV: {e}. A API pode não funcionar.")
        DF_BAREMA = pd.DataFrame()
        DF_ITEM_SIM_MATRIX = pd.DataFrame()
    except Exception as e:
        print(f"Erro CRÍTICO inesperado ao carregar dados: {e}")
        DF_BAREMA = pd.DataFrame()
        DF_ITEM_SIM_MATRIX = pd.DataFrame()

@app.on_event("startup")
async def startup_event():
    carregar_dados_essenciais()

# --- Endpoint Principal ---
@app.post("/recomendacoes", response_model=RecomendacaoResponse)
async def api_gerar_recomendacoes_simplificado(request: RecomendacaoRequest):
    if DF_BAREMA is None or DF_BAREMA.empty or \
       DF_ITEM_SIM_MATRIX is None or DF_ITEM_SIM_MATRIX.empty:
        raise HTTPException(status_code=503, detail="Serviço indisponível: Dados essenciais não carregados. Verifique os logs.")

    # 1. Calcular perfil do cliente da requisição e necessidade de ajuste (type_to_buy)
    ativos_cliente_input_dict = {a.CodigoAtivo: a.ValorInvestido for a in request.Ativos}
    
    # Filtrar barema para os ativos do cliente
    # Usamos 'NOME ATIVO' do barema para casar com 'CodigoAtivo' do request
    filtered_barema_cliente = DF_BAREMA[DF_BAREMA['NOME ATIVO'].isin(ativos_cliente_input_dict.keys())].copy()
    
    perfil_calculado_cliente = "Moderado" # Default
    type_to_buy = None
    ajuste_mensagem = f"Perfil Declarado: {request.ClassificacaoDeclarada}."
    distribuicao_financeira_cliente = pd.Series(dtype='float64')


    if not filtered_barema_cliente.empty:
        filtered_barema_cliente['VALOR INVESTIDO'] = filtered_barema_cliente['NOME ATIVO'].map(ativos_cliente_input_dict)
        distribuicao_financeira_cliente = filtered_barema_cliente.groupby('CLASSIFICACAO DO ATIVO')['VALOR INVESTIDO'].sum()
        total_investido_cliente = distribuicao_financeira_cliente.sum()

        if total_investido_cliente > 0:
            percentual_financeiro_cliente = (distribuicao_financeira_cliente / total_investido_cliente * 100).fillna(0)
            rf_pct = percentual_financeiro_cliente.get('Renda Fixa', 0.0)
            rv_pct = percentual_financeiro_cliente.get('Renda Variável', 0.0)

            if rf_pct >= 90.0: perfil_calculado_cliente = "Conservador"
            elif rv_pct >= 70.0: perfil_calculado_cliente = "Sofisticado/Arrojado"
            
            ajuste_mensagem = f"Perfil Calculado: {perfil_calculado_cliente}. Perfil Declarado: {request.ClassificacaoDeclarada}."

            if perfil_calculado_cliente != request.ClassificacaoDeclarada:
                ajuste_mensagem += " Recomenda-se ajuste."
                if request.ClassificacaoDeclarada == "Conservador" and rf_pct < 90.0: type_to_buy = "Renda Fixa"
                elif request.ClassificacaoDeclarada == "Sofisticado/Arrojado" and rv_pct < 70.0: type_to_buy = "Renda Variável"
                elif request.ClassificacaoDeclarada == "Moderado":
                    if rf_pct >= 90.0: type_to_buy = "Renda Variável"
                    elif rv_pct >= 70.0: type_to_buy = "Renda Fixa"
        else: # Cliente com ativos no request, mas não encontrados no barema ou valor total 0
            perfil_calculado_cliente = "Indefinido (ativos não encontrados no barema ou valor 0)"
            ajuste_mensagem = f"Não foi possível calcular o perfil (ativos não reconhecidos ou valor 0). Perfil Declarado: {request.ClassificacaoDeclarada}."
    else: # Cliente sem ativos no request ou nenhum ativo encontrado no barema
        perfil_calculado_cliente = "Indefinido (sem ativos no request ou não encontrados no barema)"
        ajuste_mensagem = f"Não foi possível calcular o perfil (carteira vazia ou ativos não reconhecidos). Perfil Declarado: {request.ClassificacaoDeclarada}."

    # 2. Lógica de recomendação Item-Item
    # Inicializa item_scores como uma Series do Pandas com todos os ativos possíveis como índice
    item_scores = pd.Series(0.0, index=TODOS_ATIVOS_DA_MATRIZ_SIM if TODOS_ATIVOS_DA_MATRIZ_SIM else DF_ITEM_SIM_MATRIX.index)
    
    ativos_cliente_set = set(ativos_cliente_input_dict.keys())

    for asset_cliente in ativos_cliente_input_dict.keys():
        if asset_cliente in DF_ITEM_SIM_MATRIX.index:
            similar_assets_series = DF_ITEM_SIM_MATRIX.loc[asset_cliente]
            # Mantém apenas os ativos que o cliente NÃO possui
            similar_assets_to_consider = similar_assets_series.drop(list(ativos_cliente_set.intersection(similar_assets_series.index)), errors='ignore')
            item_scores = item_scores.add(similar_assets_to_consider, fill_value=0)
        else:
            print(f"Aviso no request {request.ClienteId}: Ativo do cliente '{asset_cliente}' não encontrado na matriz de similaridade item-item.")
    
    # Filtrar por type_to_buy
    recomendacoes_series_filtradas = item_scores
    if type_to_buy:
        print(f"Filtrando recomendações para o cliente {request.ClienteId} para incluir apenas ativos do tipo: {type_to_buy}")
        ativos_para_filtrar = item_scores.index.tolist()
        # Usar 'NOME ATIVO' para o merge, pois é a coluna de identificação no DF_BAREMA
        info_barema_para_filtro = DF_BAREMA[DF_BAREMA['NOME ATIVO'].isin(ativos_para_filtrar)][['NOME ATIVO', 'CLASSIFICACAO DO ATIVO']]
        
        ativos_do_tipo_correto = info_barema_para_filtro[info_barema_para_filtro['CLASSIFICACAO DO ATIVO'] == type_to_buy]['NOME ATIVO'].tolist()
        recomendacoes_series_filtradas = item_scores[item_scores.index.isin(ativos_do_tipo_correto)]

    # 3. Top 10 Recomendações
    top_10_series = recomendacoes_series_filtradas.sort_values(ascending=False).head(10)
    
    recomendacoes_finais_output: List[AtivoRecomendadoOutput] = []
    for ativo_rec_codigo, score in top_10_series.items():
        # Usar 'NOME ATIVO' para buscar no DF_BAREMA
        info_ativo_barema_match = DF_BAREMA[DF_BAREMA['NOME ATIVO'] == ativo_rec_codigo]
        if not info_ativo_barema_match.empty:
            info_ativo = info_ativo_barema_match.iloc[0]
            recomendacoes_finais_output.append(AtivoRecomendadoOutput(
                CodigoAtivo=ativo_rec_codigo,
                TipoAtivo=info_ativo['CLASSIFICACAO DO ATIVO'],
                Subgrupo=info_ativo['Subgrupo'],
                Score=round(score, 4)
            ))
        else:
             recomendacoes_finais_output.append(AtivoRecomendadoOutput(CodigoAtivo=ativo_rec_codigo, Score=round(score, 4)))

    return RecomendacaoResponse(
        ClienteIdSolicitado=request.ClienteId,
        PerfilCalculadoCliente=perfil_calculado_cliente,
        PerfilDeclaradoCliente=request.ClassificacaoDeclarada, 
        Recomendacoes=recomendacoes_finais_output,
        Timestamp=datetime.now().isoformat()
    )

# Para rodar localmente: uvicorn main:app --reload
if __name__ == "__main__":
    import uvicorn
    uvicorn.run(app, host="0.0.0.0", port=8000)