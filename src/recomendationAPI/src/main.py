from fastapi import FastAPI, HTTPException
from pydantic import BaseModel, Field
import pandas as pd
import joblib
import os
import sys
sys.path.append(os.path.dirname(os.path.abspath(__file__)))
from models.recommendation_engine import RecommendationEngine
from utils.data_loader import DataLoader
from schemas.request_response import RecomendacaoRequest, RecomendacaoResponse
import uvicorn

# --- Configuração do App FastAPI ---
app = FastAPI(title="API de Recomendação com KMeans")

# --- Carregar Dados e Modelos ---
BASE_PATH = './data/'
kmeans_model_path = os.path.join(BASE_PATH, 'models/kmeans_model.joblib')
similarity_matrices_path = os.path.join(BASE_PATH, 'models/similarity_matrices/')

# Usar DataLoader para carregar dados
try:
    print("Iniciando carregamento de dados...")
    data_loader = DataLoader(BASE_PATH)
    DF_BAREMA, DF_CLASS_TIPOS, DF_SUBGRUPO_DIST, DF_USER_ITEM = data_loader.load_all_data()
    
    # Validar integridade dos dados
    if not data_loader.validate_data_integrity(DF_BAREMA, DF_SUBGRUPO_DIST):
        raise Exception("Falha na validação da integridade dos dados")
    
    print("Dados validados e carregados com sucesso!")
    
    # Debug: Mostrar informações dos dados carregados
    print(f"\nResumo dos dados carregados:")
    print(f"Barema - Linhas: {len(DF_BAREMA)}, Colunas: {list(DF_BAREMA.columns)}")
    print(f"Primeiros ativos no barema: {DF_BAREMA['NomeAtivo'].head().tolist()}")
    
except Exception as e:
    print(f"Erro crítico ao carregar dados: {e}")
    raise

# Inicializar o Recommendation Engine
try:
    print("Inicializando Recommendation Engine...")
    recommendation_engine = RecommendationEngine(
        df_barema=DF_BAREMA,
        df_subgrupo_dist=DF_SUBGRUPO_DIST,
        kmeans_model_path=kmeans_model_path,
        similarity_matrices_path=similarity_matrices_path
    )
    print("Recommendation Engine inicializado com sucesso!")
except Exception as e:
    print(f"Erro ao inicializar Recommendation Engine: {e}")
    raise

# --- Endpoints ---
@app.get("/")
async def root():
    return {
        "message": "API de Recomendação com KMeans",
        "status": "online",
        "dados_carregados": {
            "barema_shape": DF_BAREMA.shape,
            "subgrupo_dist_shape": DF_SUBGRUPO_DIST.shape,
            "total_ativos": len(DF_BAREMA)
        }
    }

@app.get("/health")
async def health_check():
    return {
        "status": "healthy",
        "barema_loaded": not DF_BAREMA.empty,
        "model_loaded": recommendation_engine.kmeans_model is not None,
        "matrices_loaded": len(recommendation_engine.similarity_matrices) > 0
    }

@app.post("/recomendacoes", response_model=RecomendacaoResponse)
async def api_gerar_recomendacoes_simplificado(request: RecomendacaoRequest):
    """
    Gera recomendações personalizadas para um cliente baseado em:
    - Análise do perfil RF/RV calculado vs declarado
    - Classificação em cluster via K-Means
    - Recomendações via similaridade item-item do cluster
    """
    try:
        print(f"Processando recomendação para cliente: {request.ClienteId}")
        print(f"Ativos do cliente: {[ativo.CodigoAtivo for ativo in request.Ativos]}")
        
        recommendations = recommendation_engine.generate_recommendations(request)
        
        print(f"Recomendações geradas com sucesso para cliente {request.ClienteId}")
        return recommendations
        
    except Exception as e:
        print(f"Erro detalhado na geração de recomendações: {str(e)}")
        print(f"Tipo do erro: {type(e)}")
        raise HTTPException(status_code=500, detail=f"Erro ao gerar recomendações: {str(e)}")

# Para rodar localmente: uvicorn main:app --reload
if __name__ == "__main__":
    uvicorn.run(app, host="0.0.0.0", port=8000)