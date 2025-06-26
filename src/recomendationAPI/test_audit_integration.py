"""
Script de teste para validar a integração de auditoria MongoDB
"""

import requests
import json
import time

# Configurações
API_BASE_URL = "http://localhost:8000"

def test_audit_integration():
    """Testa a integração completa da auditoria"""
    
    print("🧪 INICIANDO TESTES DE INTEGRAÇÃO DA AUDITORIA\n")
    
    # 1. Testar status da auditoria
    print("1. Verificando status da auditoria...")
    try:
        response = requests.get(f"{API_BASE_URL}/audit/status")
        if response.status_code == 200:
            status = response.json()
            print(f"   ✅ Status da auditoria: {status}")
            if not status.get('mongodb_connected'):
                print("   ❌ MongoDB não está conectado!")
                return False
        else:
            print(f"   ❌ Erro ao verificar status: {response.status_code}")
            return False
    except Exception as e:
        print(f"   ❌ Erro de conexão: {e}")
        return False
    
    # 2. Testar health check
    print("\n2. Verificando health check...")
    try:
        response = requests.get(f"{API_BASE_URL}/health")
        if response.status_code == 200:
            health = response.json()
            print(f"   ✅ Health check: audit_mongodb_connected = {health.get('audit_mongodb_connected')}")
        else:
            print(f"   ❌ Erro no health check: {response.status_code}")
    except Exception as e:
        print(f"   ❌ Erro no health check: {e}")
    
    # 3. Fazer uma requisição de teste para /recomendacoes
    print("\n3. Testando requisição para /recomendacoes...")
    
    test_request = {
        "ClienteId": "TESTE_AUDITORIA_001",
        "Ativos": [
            {
                "CodigoAtivo": "1SANPN",
                "ValorInvestido": 10000.0
            }
        ],
        "ClassificacaoDeclarada": "Moderado"
    }
    
    try:
        start_time = time.time()
        response = requests.post(
            f"{API_BASE_URL}/recomendacoes",
            json=test_request,
            headers={"Content-Type": "application/json"}
        )
        end_time = time.time()
        
        print(f"   📊 Status Code: {response.status_code}")
        print(f"   ⏱️  Tempo de resposta: {(end_time - start_time)*1000:.2f}ms")
        
        if response.status_code == 200:
            result = response.json()
            print(f"   ✅ Recomendações geradas para cliente: {result.get('ClienteIdSolicitado')}")
            print(f"   📈 Cluster Assignado: {result.get('ClusterAssignado')}")
            print(f"   🎯 Total de recomendações: {len(result.get('AtivosRecomendados', []))}")
        else:
            print(f"   ❌ Erro na API: {response.text}")
            
    except Exception as e:
        print(f"   ❌ Erro na requisição: {e}")
        return False
    
    # 4. Aguardar um momento para o log ser processado
    print("\n4. Aguardando processamento da auditoria...")
    time.sleep(2)
    
    # 5. Verificar se a auditoria foi registrada
    print("\n5. Verificando logs de auditoria...")
    try:
        response = requests.get(f"{API_BASE_URL}/audit/client/TESTE_AUDITORIA_001")
        if response.status_code == 200:
            audit_summary = response.json()
            print(f"   ✅ Resumo de auditoria obtido:")
            print(f"      📊 Total de requisições: {audit_summary.get('total_requests')}")
            print(f"      ✅ Requisições bem-sucedidas: {audit_summary.get('successful_requests')}")
            print(f"      📈 Taxa de sucesso: {audit_summary.get('success_rate')}%")
            print(f"      ⏱️  Tempo médio: {audit_summary.get('avg_processing_time_ms')}ms")
            print(f"      🕐 Última requisição: {audit_summary.get('last_request')}")
            
            if audit_summary.get('total_requests', 0) > 0:
                print("   ✅ AUDITORIA FUNCIONANDO CORRETAMENTE!")
                return True
            else:
                print("   ❌ Nenhuma requisição foi auditada")
                return False
        else:
            print(f"   ❌ Erro ao obter resumo: {response.status_code}")
            return False
            
    except Exception as e:
        print(f"   ❌ Erro ao verificar auditoria: {e}")
        return False

def main():
    """Executa os testes"""
    success = test_audit_integration()
    
    print(f"\n{'='*50}")
    if success:
        print("🎉 TODOS OS TESTES PASSARAM - AUDITORIA INTEGRADA COM SUCESSO!")
    else:
        print("❌ FALHA NOS TESTES - VERIFICAR CONFIGURAÇÃO")
    print(f"{'='*50}")

if __name__ == "__main__":
    main()
