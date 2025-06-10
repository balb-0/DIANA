# BTG Pactual - Script Híbrido de Teste de Carga
# Combina pgbench (teste de carga) + Python (análise EXPLAIN ANALYZE)

param(
    [switch]$Quick,          # Teste rápido (2 minutos)
    [switch]$PgBenchOnly,    # Apenas pgbench
    [switch]$PythonOnly,     # Apenas análise Python
    [switch]$Setup,          # Apenas preparar dados
    [int]$Duration = 300,    # Duração em segundos
    [switch]$Help            # Mostrar ajuda
)

# Configurações
$DB_HOST = "localhost"
$DB_PORT = "5432"
$DB_NAME = "btg_db"
$DB_USER = "btg_user"
$DB_PASSWORD = "btg_pass"

# Função para logs coloridos
function Write-ColorOutput($ForegroundColor) {
    $fc = $host.UI.RawUI.ForegroundColor
    $host.UI.RawUI.ForegroundColor = $ForegroundColor
    if ($args) {
        Write-Output $args
    } else {
        $input | Write-Output
    }
    $host.UI.RawUI.ForegroundColor = $fc
}

# Mostrar ajuda
if ($Help) {
    Write-ColorOutput Green "🔧 BTG PACTUAL - TESTE DE CARGA HÍBRIDO"
    Write-Host ""
    Write-Host "DESCRIÇÃO:"
    Write-Host "  Combina pgbench (teste de carga padrão) com análise Python (EXPLAIN ANALYZE)"
    Write-Host ""
    Write-Host "USO:"
    Write-Host "  .\run_hybrid_test.ps1 [OPTIONS]"
    Write-Host ""
    Write-Host "OPÇÕES:"
    Write-Host "  -Quick              Teste rápido (2 minutos cada ferramenta)"
    Write-Host "  -PgBenchOnly        Executar apenas pgbench"
    Write-Host "  -PythonOnly         Executar apenas análise Python"
    Write-Host "  -Setup              Apenas preparar dados de teste"
    Write-Host "  -Duration <seg>     Duração personalizada (padrão: 300s)"
    Write-Host "  -Help               Mostrar esta ajuda"
    Write-Host ""
    Write-Host "EXEMPLOS:"
    Write-Host "  .\run_hybrid_test.ps1                    # Teste completo"
    Write-Host "  .\run_hybrid_test.ps1 -Quick            # Teste rápido"
    Write-Host "  .\run_hybrid_test.ps1 -PgBenchOnly      # Só pgbench"
    Write-Host "  .\run_hybrid_test.ps1 -Duration 600     # 10 minutos"
    Write-Host ""
    exit 0
}

Write-ColorOutput Green "🚀 BTG PACTUAL - TESTE DE CARGA HÍBRIDO"
Write-ColorOutput Yellow "================================================"

# Configurações baseadas nos parâmetros
if ($Quick) {
    $Duration = 120
    Write-ColorOutput Cyan "⚡ Modo rápido ativado - Duração: 2 minutos por teste"
}

Write-Host "📊 Configuração:"
Write-Host "  🗄️  Banco: $DB_HOST:$DB_PORT/$DB_NAME"
Write-Host "  ⏱️  Duração: ${Duration}s por ferramenta"
Write-Host "  🔧 Modo: $(if ($PgBenchOnly) { 'Apenas pgbench' } elseif ($PythonOnly) { 'Apenas Python' } else { 'Híbrido completo' })"
Write-Host ""

# Verificar dependências
function Test-Dependencies {
    Write-ColorOutput Yellow "🔍 Verificando dependências..."
    
    $dependenciesOk = $true
    
    # Verificar Docker
    try {
        $dockerStatus = docker ps --filter "name=btg_postgres" --format "table {{.Status}}" 2>$null
        if ($dockerStatus -match "Up") {
            Write-ColorOutput Green "✅ Container PostgreSQL está rodando"
        } else {
            Write-ColorOutput Red "❌ Container PostgreSQL não está rodando"
            Write-Host "   Execute: docker-compose up -d"
            $dependenciesOk = $false
        }
    } catch {
        Write-ColorOutput Red "❌ Docker não encontrado ou não está rodando"
        $dependenciesOk = $false
    }
    
    # Verificar Python (apenas se necessário)
    if (-not $PgBenchOnly) {
        try {
            $pythonVersion = python --version 2>&1
            Write-ColorOutput Green "✅ Python encontrado: $pythonVersion"
            
            # Verificar psycopg2
            $psycopg2Check = python -c "import psycopg2; print('OK')" 2>&1
            if ($psycopg2Check -ne "OK") {
                Write-ColorOutput Yellow "📦 Instalando psycopg2..."
                pip install psycopg2-binary | Out-Null
                Write-ColorOutput Green "✅ psycopg2 instalado"
            } else {
                Write-ColorOutput Green "✅ psycopg2 disponível"
            }
        } catch {
            Write-ColorOutput Red "❌ Python não encontrado. Instale Python 3.8+"
            $dependenciesOk = $false
        }
    }
    
    # Verificar pgbench (apenas se necessário)
    if (-not $PythonOnly) {
        $pgbenchCheck = docker exec btg_postgres which pgbench 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-ColorOutput Green "✅ pgbench disponível no container"
        } else {
            Write-ColorOutput Red "❌ pgbench não encontrado no container PostgreSQL"
            $dependenciesOk = $false
        }
    }
    
    return $dependenciesOk
}

# Preparar dados de teste
function Setup-TestData {
    Write-ColorOutput Yellow "🗄️ Preparando dados de teste..."
    
    try {
        # Copiar scripts para o container
        docker cp "pgbench_setup.sql" btg_postgres:/tmp/pgbench_setup.sql 2>$null
        docker cp "test_queries.sql" btg_postgres:/tmp/test_queries.sql 2>$null
        
        # Executar setup
        $setupResult = docker exec btg_postgres psql -h localhost -U $DB_USER -d $DB_NAME -f /tmp/pgbench_setup.sql 2>&1
        
        if ($LASTEXITCODE -eq 0) {
            Write-ColorOutput Green "✅ Dados de teste criados com sucesso"
            
            # Mostrar resumo
            $summary = docker exec btg_postgres psql -h localhost -U $DB_USER -d $DB_NAME -c "SELECT 'clientes' as tabela, COUNT(*) as registros FROM clientes WHERE id LIKE 'TEST_%' UNION ALL SELECT 'investimentos', COUNT(*) FROM investimento WHERE clientes_id LIKE 'TEST_%';" -t 2>$null
            Write-Host "📊 Resumo dos dados:"
            $summary | ForEach-Object { Write-Host "   $_" }
        } else {
            Write-ColorOutput Red "❌ Erro ao criar dados de teste"
            Write-Host $setupResult
            return $false
        }
    } catch {
        Write-ColorOutput Red "❌ Erro durante setup: $_"
        return $false
    }
    
    return $true
}

# Executar pgbench
function Run-PgBench {
    param([int]$TestDuration)
    
    Write-ColorOutput Yellow "🏃 Executando pgbench para teste de carga..."
    
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $logFile = "pgbench_results_$timestamp.log"
    
    # Configurações de teste progressivas
    $configurations = @(
        @{clients=10; jobs=4; duration=($TestDuration/3); description="Carga Baixa"},
        @{clients=25; jobs=8; duration=($TestDuration/3); description="Carga Moderada"},
        @{clients=50; jobs=16; duration=($TestDuration/3); description="Carga Alta"}
    )
    
    $allResults = @()
    
    foreach ($config in $configurations) {
        Write-ColorOutput Cyan "📊 $($config.description): $($config.clients) clientes, $($config.jobs) jobs, $($config.duration)s"
        
        try {
            # Executar pgbench
            $pgbenchResult = docker exec btg_postgres pgbench `
                -h localhost -U $DB_USER -d $DB_NAME `
                -c $($config.clients) -j $($config.jobs) `
                -T $($config.duration) `
                -f /tmp/test_queries.sql `
                --progress=30 `
                2>&1
            
            # Extrair métricas
            if ($pgbenchResult -match "tps = ([0-9.]+)") {
                $tps = $matches[1]
                $allResults += @{
                    config = $config.description
                    tps = $tps
                    clients = $config.clients
                    duration = $config.duration
                }
                Write-ColorOutput Green "✅ $($config.description): $tps TPS"
            }
            
            # Salvar log detalhado
            Add-Content -Path $logFile -Value "=== $($config.description) - $(Get-Date) ==="
            Add-Content -Path $logFile -Value $pgbenchResult
            Add-Content -Path $logFile -Value "`n"
            
        } catch {
            Write-ColorOutput Red "❌ Erro no teste $($config.description): $_"
        }
    }
    
    # Resumo final do pgbench
    Write-ColorOutput Green "`n📈 RESUMO PGBENCH:"
    foreach ($result in $allResults) {
        Write-Host "   $($result.config): $($result.tps) TPS ($($result.clients) clientes)"
    }
    
    Write-ColorOutput Cyan "📄 Log detalhado salvo em: $logFile"
    return $allResults
}

# Executar análise Python
function Run-PythonAnalysis {
    param([int]$TestDuration)
    
    Write-ColorOutput Yellow "🐍 Executando análise Python com EXPLAIN ANALYZE..."
    
    try {
        # Verificar se o arquivo existe
        if (-not (Test-Path "load_test_analyzer.py")) {
            Write-ColorOutput Red "❌ Arquivo load_test_analyzer.py não encontrado"
            return $false
        }
        
        # Executar análise Python com duração personalizada
        $env:PGPASSWORD = $DB_PASSWORD
        
        # Criar script temporário com configuração personalizada
        $tempScript = @"
import sys
import os
sys.path.append(os.path.dirname(os.path.abspath(__file__)))

from load_test_analyzer import DatabaseLoadTestAnalyzer

connection_params = {
    'host': '$DB_HOST',
    'database': '$DB_NAME', 
    'user': '$DB_USER',
    'password': '$DB_PASSWORD',
    'port': $DB_PORT
}

print("🔧 Iniciando análise Python...")
analyzer = DatabaseLoadTestAnalyzer(connection_params, max_workers=50)
analyzer.run_load_test(duration_seconds=$TestDuration, threads_per_query=8)
analyzer.print_summary()

import datetime
report_file = f'python_analysis_{datetime.datetime.now().strftime("%Y%m%d_%H%M%S")}.json'
analyzer.generate_report(report_file)
print(f"📄 Relatório Python salvo em: {report_file}")
"@
        
        $tempScriptFile = "temp_hybrid_test.py"
        $tempScript | Out-File -FilePath $tempScriptFile -Encoding UTF8
        
        # Executar
        python $tempScriptFile
        
        # Limpar arquivo temporário
        Remove-Item $tempScriptFile -ErrorAction SilentlyContinue
        
        Write-ColorOutput Green "✅ Análise Python concluída"
        return $true
        
    } catch {
        Write-ColorOutput Red "❌ Erro na análise Python: $_"
        return $false
    }
}

# Gerar relatório consolidado
function Generate-ConsolidatedReport {
    param($pgbenchResults)
    
    Write-ColorOutput Yellow "📋 Gerando relatório consolidado..."
    
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $reportFile = "hybrid_test_report_$timestamp.md"
    
    $report = @"
# 📊 Relatório de Teste de Carga Híbrido - BTG Pactual

**🕒 Gerado em:** $(Get-Date -Format "dd/MM/yyyy HH:mm:ss")  
**⏱️ Duração total:** $Duration segundos por ferramenta  
**🔧 Modo:** $(if ($PgBenchOnly) { 'Apenas pgbench' } elseif ($PythonOnly) { 'Apenas Python' } else { 'Híbrido completo' })

---

## 🎯 Resumo Executivo

Este relatório consolida os resultados dos testes de carga realizados no banco de dados do BTG Pactual, utilizando uma **abordagem híbrida** que combina:

- **🏃 pgbench:** Teste de carga padronizado para PostgreSQL (métricas de TPS e latência)
- **🐍 Python + EXPLAIN ANALYZE:** Análise detalhada de planos de execução e otimizações

---

## 📊 Resultados pgbench

### Métricas de Performance:
"@

    if ($pgbenchResults) {
        foreach ($result in $pgbenchResults) {
            $report += "`n- **$($result.config):** $($result.tps) TPS com $($result.clients) clientes simultâneos"
        }
    } else {
        $report += "`n- ⚠️ Resultados pgbench não disponíveis (modo PythonOnly ou erro)"
    }

    $report += @"

### Interpretação:
- **TPS (Transações Por Segundo):** Maior é melhor
- **Latência:** Menor é melhor  
- **Taxa de erro:** Deve ser < 1%

---

## 🐍 Resultados Análise Python

### Métricas Coletadas:
- ✅ **Planos de execução** com EXPLAIN ANALYZE
- 📈 **Tempos de resposta** por tipo de query
- 🔍 **Identificação de Sequential Scans**
- 💡 **Sugestões de otimização** automáticas
- 📊 **Análise de performance** por thread

Consulte o arquivo JSON mais recente iniciado com `python_analysis_` para detalhes completos.

---

## 🎯 Queries Testadas

1. **get_client_profile** - Busca perfil completo do cliente (40% do tráfego)
2. **get_client_investments** - Lista investimentos do cliente (25% do tráfego)  
3. **portfolio_summary** - Resumo de portfólio por tipo (15% do tráfego)
4. **compliance_check** - Verificação de compliance (15% do tráfego)
5. **assessor_portfolio** - Relatório do assessor (5% do tráfego)

---

## 💡 Próximos Passos

### Análise dos Resultados:
1. 📊 **Verificar TPS do pgbench** - deve ser > 1.000 TPS
2. 🔍 **Analisar Sequential Scans** no relatório Python
3. ⚡ **Implementar otimizações** sugeridas
4. 🔄 **Re-executar testes** para validar melhorias

### Otimizações Comuns:
- 📁 **Criar índices** para queries com Sequential Scan
- 🔄 **Ajustar parâmetros** do PostgreSQL
- 📊 **Particionar tabelas** grandes (se necessário)
- 🚀 **Implementar cache** para queries frequentes

---

## 🔧 Como Re-executar

```powershell
# Teste completo
.\run_hybrid_test.ps1

# Teste rápido (2 minutos)
.\run_hybrid_test.ps1 -Quick

# Apenas pgbench
.\run_hybrid_test.ps1 -PgBenchOnly

# Apenas análise Python  
.\run_hybrid_test.ps1 -PythonOnly

# Duração customizada (10 minutos)
.\run_hybrid_test.ps1 -Duration 600
```

---

## 📁 Arquivos Gerados

- **Logs pgbench:** `pgbench_results_*.log`
- **Análise Python:** `python_analysis_*.json`  
- **Este relatório:** `hybrid_test_report_*.md`

---

*🤖 Relatório gerado automaticamente pelo sistema de testes híbrido BTG Pactual*
"@
    
    $report | Out-File -FilePath $reportFile -Encoding UTF8
    Write-ColorOutput Green "✅ Relatório consolidado salvo em: $reportFile"
    return $reportFile
}

# Função principal
function Main {
    $startTime = Get-Date
    
    # Verificar dependências
    if (-not (Test-Dependencies)) {
        Write-ColorOutput Red "❌ Dependências não atendidas. Abortando."
        exit 1
    }
    
    # Setup de dados (sempre executar, a menos que seja PythonOnly)
    if (-not $PythonOnly -or $Setup) {
        if (-not (Setup-TestData)) {
            Write-ColorOutput Red "❌ Falha no setup de dados. Abortando."
            exit 1
        }
    }
    
    if ($Setup) {
        Write-ColorOutput Green "✅ Setup concluído. Use os outros parâmetros para executar os testes."
        exit 0
    }
    
    $pgbenchResults = $null
    
    # Executar pgbench
    if (-not $PythonOnly) {
        Write-ColorOutput Yellow "`n🚀 FASE 1: TESTE DE CARGA COM PGBENCH"
        Write-Host "=" * 50
        $pgbenchResults = Run-PgBench -TestDuration $Duration
    }
    
    # Executar análise Python
    if (-not $PgBenchOnly) {
        Write-ColorOutput Yellow "`n🚀 FASE 2: ANÁLISE COM EXPLAIN ANALYZE"
        Write-Host "=" * 50
        Run-PythonAnalysis -TestDuration $Duration
    }
    
    # Gerar relatório consolidado
    Write-ColorOutput Yellow "`n📋 FASE 3: RELATÓRIO CONSOLIDADO"
    Write-Host "=" * 50
    $reportFile = Generate-ConsolidatedReport -pgbenchResults $pgbenchResults
    
    $endTime = Get-Date
    $totalDuration = ($endTime - $startTime).TotalMinutes
    
    Write-ColorOutput Green "`n🎉 TESTE HÍBRIDO CONCLUÍDO COM SUCESSO!"
    Write-Host "=" * 50
    Write-Host "⏱️  Tempo total: $($totalDuration.ToString('F1')) minutos"
    Write-Host "📄 Relatório principal: $reportFile"
    Write-Host ""
    Write-ColorOutput Cyan "📁 Verifique os arquivos gerados para análise detalhada:"
    Get-ChildItem -Filter "*$(Get-Date -Format 'yyyyMMdd')*" | Where-Object { $_.Name -match "(pgbench|python|hybrid)" } | ForEach-Object {
        Write-Host "   📄 $($_.Name)"
    }
}

# Executar função principal
Main
