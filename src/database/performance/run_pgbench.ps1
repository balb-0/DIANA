# Script PowerShell para executar testes de carga pgbench no Windows
# BTG Pactual - Teste de Performance do Banco de Dados

param(
    [string]$Mode = "menu",
    [switch]$Quick,
    [switch]$Full,
    [switch]$Setup,
    [switch]$Help
)

# Configurações do banco
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
    Write-ColorOutput Green "=== BTG PACTUAL - TESTE DE CARGA DO BANCO DE DADOS ==="
    Write-Host ""
    Write-Host "USAGE:"
    Write-Host "  .\run_pgbench.ps1 [OPTIONS]"
    Write-Host ""
    Write-Host "OPTIONS:"
    Write-Host "  -Quick    Teste rápido (1 minuto, 10 clientes)"
    Write-Host "  -Full     Bateria completa de testes (15 minutos)"
    Write-Host "  -Setup    Apenas criar dados de teste"
    Write-Host "  -Help     Mostrar esta ajuda"
    Write-Host ""
    Write-Host "EXEMPLOS:"
    Write-Host "  .\run_pgbench.ps1 -Quick"
    Write-Host "  .\run_pgbench.ps1 -Full"
    Write-Host "  .\run_pgbench.ps1 -Setup"
    exit 0
}

Write-ColorOutput Green "=== BTG PACTUAL - TESTE DE CARGA DO BANCO DE DADOS ==="
Write-ColorOutput Yellow "Preparando ambiente de teste..."

# Define variável de ambiente para senha
$env:PGPASSWORD = $DB_PASSWORD

# Função para executar comandos SQL
function Run-SQL {
    param($query)
    try {
        psql -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME -c $query
        return $true
    } catch {
        Write-ColorOutput Red "Erro ao executar SQL: $($_.Exception.Message)"
        return $false
    }
}

function Run-SQL-File {
    param($file)
    try {
        psql -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME -f $file
        return $true
    } catch {
        Write-ColorOutput Red "Erro ao executar arquivo SQL: $($_.Exception.Message)"
        return $false
    }
}

# Verifica se o banco está acessível
function Test-DatabaseConnection {
    Write-ColorOutput Yellow "Verificando conexão com o banco..."
    try {
        $result = psql -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME -c "SELECT 1;" 2>$null
        if ($LASTEXITCODE -eq 0) {
            Write-ColorOutput Green "✓ Conexão estabelecida com sucesso"
            return $true
        } else {
            throw "Falha na conexão"
        }
    } catch {
        Write-ColorOutput Red "❌ Erro: Não foi possível conectar ao banco de dados"
        Write-Host "Verifique se o container PostgreSQL está rodando:"
        Write-Host "  docker-compose up -d"
        Write-Host "  docker ps | grep postgres"
        return $false
    }
}

# Função para executar pgbench
function Run-Pgbench {
    param(
        [string]$TestName,
        [int]$Clients,
        [int]$Jobs,
        [int]$Duration,
        [string]$CustomScript = ""
    )
    
    Write-ColorOutput Yellow "`n=== Executando: $TestName ==="
    Write-ColorOutput Cyan "Clientes: $Clients | Jobs: $Jobs | Duração: ${Duration}s"
    
    $timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
    $logFile = "pgbench_${TestName}_${timestamp}.log"
    
    try {
        if ($CustomScript -eq "") {
            # Teste padrão do pgbench
            pgbench -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME `
                    -c $Clients -j $Jobs -T $Duration `
                    --log --log-prefix="$TestName" `
                    --progress=5 2>&1 | Tee-Object -FilePath $logFile
        } else {
            # Teste com script customizado
            pgbench -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME `
                    -c $Clients -j $Jobs -T $Duration `
                    --log --log-prefix="$TestName" `
                    --progress=5 -f $CustomScript 2>&1 | Tee-Object -FilePath $logFile
        }
        
        if ($LASTEXITCODE -eq 0) {
            Write-ColorOutput Green "✓ Teste $TestName concluído. Log: $logFile"
            return $true
        } else {
            Write-ColorOutput Red "❌ Teste $TestName falhou"
            return $false
        }
        
    } catch {
        Write-ColorOutput Red "❌ Erro no teste $TestName`: $($_.Exception.Message)"
        return $false
    }
}

# Função para análise de performance pós-teste
function Get-DatabaseStats {
    Write-ColorOutput Yellow "`n=== ESTATÍSTICAS DO BANCO APÓS TESTES ==="
    
    Write-ColorOutput Cyan "Tamanho das tabelas:"
    Run-SQL "SELECT tablename, pg_size_pretty(pg_total_relation_size(tablename)) as size FROM pg_tables WHERE tablename IN ('clientes', 'investimento', 'historico_risco_declarado', 'historico_risco_calculado') ORDER BY pg_total_relation_size(tablename) DESC;"
    
    Write-ColorOutput Cyan "`nCache hit ratio:"
    Run-SQL "SELECT 'cache hit ratio' as metric, round(100.0 * sum(blks_hit) / sum(blks_hit + blks_read), 2) as percentage FROM pg_stat_database WHERE datname = '$DB_NAME';"
    
    Write-ColorOutput Cyan "`nEstatísticas de tabelas:"
    Run-SQL "SELECT tablename, n_tup_ins, n_tup_upd, n_tup_del, seq_scan, idx_scan FROM pg_stat_user_tables WHERE tablename LIKE '%test%' OR tablename IN ('clientes', 'investimento') ORDER BY seq_scan DESC LIMIT 10;"
}

# Função para preparar dados
function Setup-TestData {
    Write-ColorOutput Yellow "Preparando dados de teste..."
    
    if (-not (Test-Path "pgbench_setup.sql")) {
        Write-ColorOutput Red "❌ Arquivo pgbench_setup.sql não encontrado!"
        return $false
    }
    
    $success = Run-SQL-File "pgbench_setup.sql"
    if ($success) {
        Write-ColorOutput Green "✓ Dados de teste criados com sucesso"
        
        # Inicializar pgbench (tabelas padrão)
        Write-ColorOutput Yellow "Inicializando pgbench..."
        try {
            pgbench -h $DB_HOST -p $DB_PORT -U $DB_USER -d $DB_NAME -i -s 10 2>$null
            if ($LASTEXITCODE -eq 0) {
                Write-ColorOutput Green "✓ Pgbench inicializado"
            }
        } catch {
            Write-ColorOutput Yellow "⚠️ Aviso: Não foi possível inicializar pgbench padrão"
        }
        
        return $true
    }
    return $false
}

# Função principal de teste rápido
function Start-QuickTest {
    Write-ColorOutput Green "`n🏃 TESTE RÁPIDO - 1 MINUTO"
    
    if (-not (Setup-TestData)) {
        return
    }
    
    # Teste com queries BTG
    Run-Pgbench -TestName "btg_quick" -Clients 10 -Jobs 4 -Duration 60 -CustomScript "test_queries.sql"
    
    Get-DatabaseStats
    Write-ColorOutput Green "`n🎉 Teste rápido concluído!"
}

# Função principal de teste completo
function Start-FullTest {
    Write-ColorOutput Green "`n🎯 BATERIA COMPLETA DE TESTES - 15 MINUTOS"
    
    if (-not (Setup-TestData)) {
        return
    }
    
    # 1. Teste de aquecimento
    Write-ColorOutput Yellow "`n1️⃣ Aquecimento (30s)..."
    Run-Pgbench -TestName "warmup" -Clients 5 -Jobs 2 -Duration 30
    
    # 2. Teste com queries BTG - carga baixa
    Write-ColorOutput Yellow "`n2️⃣ Carga baixa - Queries BTG (2 min)..."
    Run-Pgbench -TestName "btg_low" -Clients 10 -Jobs 4 -Duration 120 -CustomScript "test_queries.sql"
    
    # 3. Teste com queries BTG - carga moderada
    Write-ColorOutput Yellow "`n3️⃣ Carga moderada - Queries BTG (3 min)..."
    Run-Pgbench -TestName "btg_moderate" -Clients 25 -Jobs 8 -Duration 180 -CustomScript "test_queries.sql"
    
    # 4. Teste com queries BTG - carga alta
    Write-ColorOutput Yellow "`n4️⃣ Carga alta - Queries BTG (5 min)..."
    Run-Pgbench -TestName "btg_high" -Clients 50 -Jobs 16 -Duration 300 -CustomScript "test_queries.sql"
    
    # 5. Teste de stress
    Write-ColorOutput Yellow "`n5️⃣ Teste de stress (5 min)..."
    Run-Pgbench -TestName "stress" -Clients 100 -Jobs 20 -Duration 300
    
    Get-DatabaseStats
    Generate-Report
    Write-ColorOutput Green "`n🎉 BATERIA COMPLETA CONCLUÍDA!"
}

# Função para gerar relatório
function Generate-Report {
    $reportFile = "relatorio_teste_carga_$(Get-Date -Format 'yyyyMMdd_HHmmss').md"
    
    $content = @"
# BTG Pactual - Relatório de Teste de Carga do Banco de Dados

**Data:** $(Get-Date -Format 'dd/MM/yyyy HH:mm:ss')
**Banco:** PostgreSQL - $DB_NAME
**Host:** $DB_HOST`:$DB_PORT

## Configuração do Teste

- **Dados de Teste:** 10.000 clientes, 50.000 investimentos, 25.000 históricos
- **Duração Total:** ~15 minutos (bateria completa)
- **Progressão:** Warmup → Low → Moderate → High → Stress

## Arquivos de Log Gerados

"@
    
    # Listar logs gerados
    Get-ChildItem -Filter "pgbench_*.log" | ForEach-Object {
        $content += "- $($_.Name)`n"
    }
    
    $content += @"

## Interpretação dos Resultados

### Métricas Principais:
- **TPS (Transactions Per Second)**: Transações por segundo - **Objetivo: >1000**
- **Latência Média**: Tempo médio de resposta - **Objetivo: <50ms**
- **95º Percentil**: 95% das transações abaixo deste tempo - **Objetivo: <100ms**
- **Taxa de Erro**: Percentual de transações que falharam - **Objetivo: <0.1%**

### Sinais de Problemas:
- TPS muito baixo (<500): Possível gargalo de I/O ou CPU
- Latência alta (>100ms): Queries mal otimizadas ou falta de índices
- Alta taxa de erro: Problemas de conectividade ou timeouts
- Cache hit ratio <95%: Pouca memória ou queries ineficientes

## Próximos Passos

1. **Analisar logs detalhados** de cada teste individual
2. **Verificar queries lentas** com EXPLAIN ANALYZE
3. **Otimizar índices** conforme necessário
4. **Ajustar configurações** do PostgreSQL se necessário
5. **Repetir testes** após otimizações

## Comandos para Análise Detalhada

``````sql
-- Ver estatísticas de performance
SELECT * FROM pg_stat_database WHERE datname = '$DB_NAME';

-- Cache hit ratio (deve ser >95%)
SELECT round(100.0 * sum(blks_hit) / sum(blks_hit + blks_read), 2) as cache_hit_ratio 
FROM pg_stat_database WHERE datname = '$DB_NAME';

-- Tabelas com mais sequential scans
SELECT tablename, seq_scan, seq_tup_read, idx_scan 
FROM pg_stat_user_tables 
ORDER BY seq_scan DESC LIMIT 10;
``````

"@
    
    $content | Out-File -FilePath $reportFile -Encoding UTF8
    Write-ColorOutput Green "📄 Relatório consolidado salvo: $reportFile"
}

# Menu interativo
function Show-Menu {
    Write-ColorOutput Green "`n🎯 BTG PACTUAL - MENU DE TESTES DE PERFORMANCE"
    Write-ColorOutput White "================================================"
    Write-ColorOutput Yellow "1. Teste rápido (1 minuto)"
    Write-ColorOutput Yellow "2. Bateria completa (15 minutos)"
    Write-ColorOutput Yellow "3. Apenas preparar dados de teste"
    Write-ColorOutput Yellow "4. Ver estatísticas do banco"
    Write-ColorOutput Yellow "5. Limpar dados de teste"
    Write-ColorOutput Yellow "0. Sair"
    Write-ColorOutput White "================================================"
    
    $choice = Read-Host "Escolha uma opção"
    
    switch ($choice) {
        "1" { Start-QuickTest }
        "2" { Start-FullTest }
        "3" { 
            if (Setup-TestData) {
                Write-ColorOutput Green "✓ Dados preparados com sucesso"
            }
        }
        "4" { Get-DatabaseStats }
        "5" {
            Write-ColorOutput Yellow "Limpando dados de teste..."
            Run-SQL "DELETE FROM investimento WHERE clientes_id LIKE 'TEST_%';"
            Run-SQL "DELETE FROM historico_risco_declarado WHERE clientes_id LIKE 'TEST_%';"
            Run-SQL "DELETE FROM historico_risco_calculado WHERE clientes_id LIKE 'TEST_%';"
            Run-SQL "DELETE FROM clientes WHERE id LIKE 'TEST_%';"
            Run-SQL "DELETE FROM assessores WHERE id LIKE 'ASSESS_TEST_%';"
            Run-SQL "DELETE FROM ativos WHERE id LIKE 'ATIVO_TEST_%';"
            Write-ColorOutput Green "✓ Dados de teste removidos"
        }
        "0" { 
            Write-ColorOutput Green "Saindo..."
            exit 0 
        }
        default { 
            Write-ColorOutput Red "Opção inválida!"
            Show-Menu 
        }
    }
}

# Verificar pré-requisitos
function Test-Prerequisites {
    # Testar se pgbench está disponível
    try {
        $null = pgbench --version 2>$null
        Write-ColorOutput Green "✓ pgbench encontrado"
    } catch {
        Write-ColorOutput Red "❌ pgbench não encontrado. Instale PostgreSQL client tools."
        return $false
    }
    
    # Testar se psql está disponível
    try {
        $null = psql --version 2>$null
        Write-ColorOutput Green "✓ psql encontrado"
    } catch {
        Write-ColorOutput Red "❌ psql não encontrado. Instale PostgreSQL client tools."
        return $false
    }
    
    return $true
}

# ==============================================================================
# EXECUÇÃO PRINCIPAL
# ==============================================================================

# Verificar pré-requisitos
if (-not (Test-Prerequisites)) {
    Write-Host "`nInstale PostgreSQL client tools:"
    Write-Host "https://www.postgresql.org/download/windows/"
    exit 1
}

# Verificar conexão com banco
if (-not (Test-DatabaseConnection)) {
    exit 1
}

# Executar baseado nos parâmetros
if ($Quick) {
    Start-QuickTest
} elseif ($Full) {
    Start-FullTest
} elseif ($Setup) {
    Setup-TestData
} else {
    Show-Menu
}
