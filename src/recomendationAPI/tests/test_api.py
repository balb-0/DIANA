import pytest
from fastapi.testclient import TestClient
import sys
import os

sys.path.append(os.path.join(os.path.dirname(__file__), '..', 'src'))

from main import app

client = TestClient(app)

def test_root_endpoint():
    """Testa o endpoint raiz"""
    response = client.get("/")
    assert response.status_code == 200
    data = response.json()
    assert "message" in data

def test_health_endpoint():
    """Testa o endpoint de health check"""
    response = client.get("/health")
    assert response.status_code == 200
    data = response.json()
    assert "status" in data
    assert data["status"] == "healthy"

def test_recommendations_endpoint():
    """Testa o endpoint de recomendações com dados válidos"""
    response = client.post("/recomendacoes", json={
        "ClienteId": "Cliente_Teste_001",
        "Ativos": [
            {"CodigoAtivo": "1SANPN", "ValorInvestido": 5000.0},
            {"CodigoAtivo": "1832421D CN", "ValorInvestido": 1436.22}
        ],
        "ClassificacaoDeclarada": "Moderado"
    })
    
    assert response.status_code == 200
    data = response.json()
    
    assert "ClienteIdSolicitado" in data
    assert "PerfilCalculadoCliente" in data
    assert "PerfilDeclaradoCliente" in data
    assert "ClusterAssignado" in data
    assert "AtivosRecomendados" in data
    assert "MensagemAjuste" in data
    
    assert isinstance(data["AtivosRecomendados"], list)
    assert isinstance(data["ClusterAssignado"], int)

def test_recommendations_endpoint_empty_assets():
    """Testa o endpoint de recomendações com lista de ativos vazia"""
    response = client.post("/recomendacoes", json={
        "ClienteId": "Cliente_Teste_002",
        "Ativos": [],
        "ClassificacaoDeclarada": "Conservador"
    })
    
    assert response.status_code in [200, 400, 500]

def test_recommendations_endpoint_invalid_profile():
    """Testa o endpoint com perfil inválido"""
    response = client.post("/recomendacoes", json={
        "ClienteId": "Cliente_Teste_003",
        "Ativos": [
            {"CodigoAtivo": "1SANPN", "ValorInvestido": 1000.0}
        ],
        "ClassificacaoDeclarada": "PerfilInexistente"
    })
    
    assert response.status_code in [200, 400, 422]

if __name__ == "__main__":
    pytest.main([__file__])