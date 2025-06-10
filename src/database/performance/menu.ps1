# Script de conveniência para execução rápida
# BTG Pactual - Atalhos para testes

Write-Host "🚀 BTG PACTUAL - ATALHOS DE TESTE" -ForegroundColor Green
Write-Host "=================================" -ForegroundColor Green
Write-Host ""
Write-Host "Escolha uma opção:" -ForegroundColor Yellow
Write-Host "1. 🏃 Teste rápido (2 min)" -ForegroundColor Cyan
Write-Host "2. 📊 Teste completo (10 min)" -ForegroundColor Cyan  
Write-Host "3. 🔧 Apenas setup de dados" -ForegroundColor Cyan
Write-Host "4. 📈 Apenas pgbench" -ForegroundColor Cyan
Write-Host "5. 🐍 Apenas análise Python" -ForegroundColor Cyan
Write-Host "6. 📋 Ver relatórios existentes" -ForegroundColor Cyan
Write-Host "0. ❌ Sair" -ForegroundColor Red
Write-Host ""

$choice = Read-Host "Digite sua escolha (0-6)"

switch ($choice) {
    "1" { 
        Write-Host "🏃 Executando teste rápido..." -ForegroundColor Green
        .\run_hybrid_test.ps1 -Quick 
    }
    "2" { 
        Write-Host "📊 Executando teste completo..." -ForegroundColor Green
        .\run_hybrid_test.ps1 
    }
    "3" { 
        Write-Host "🔧 Preparando dados..." -ForegroundColor Green
        .\run_hybrid_test.ps1 -Setup 
    }
    "4" { 
        Write-Host "📈 Executando apenas pgbench..." -ForegroundColor Green
        .\run_hybrid_test.ps1 -PgBenchOnly 
    }
    "5" { 
        Write-Host "🐍 Executando apenas análise Python..." -ForegroundColor Green
        .\run_hybrid_test.ps1 -PythonOnly 
    }
    "6" { 
        Write-Host "📋 Relatórios encontrados:" -ForegroundColor Yellow
        Get-ChildItem -Filter "*$(Get-Date -Format 'yyyyMMdd')*" | ForEach-Object {
            Write-Host "   📄 $($_.Name)" -ForegroundColor Cyan
        }
    }
    "0" { 
        Write-Host "👋 Saindo..." -ForegroundColor Green
        exit 0 
    }
    default { 
        Write-Host "❌ Opção inválida!" -ForegroundColor Red
    }
}
