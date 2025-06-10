#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
BTG Pactual - Analisador de Teste de Carga com EXPLAIN ANALYZE
Ferramenta híbrida que complementa o pgbench com análise detalhada de performance
"""

import psycopg2
import threading
import time
import json
import concurrent.futures
from datetime import datetime
import statistics
from typing import List, Dict, Tuple
import logging
import sys
import os

class DatabaseLoadTestAnalyzer:
    """
    Analisador avançado de carga de banco com EXPLAIN ANALYZE
    Complementa o pgbench com análise detalhada de planos de execução
    """
    
    def __init__(self, connection_params: dict, max_workers: int = 50):
        self.connection_params = connection_params
        self.max_workers = max_workers
        self.results = []
        self.lock = threading.Lock()
        
        # Configurar logging
        logging.basicConfig(
            level=logging.INFO,
            format='%(asctime)s - %(levelname)s - %(message)s',
            handlers=[
                logging.FileHandler('load_test_analysis.log', encoding='utf-8'),
                logging.StreamHandler(sys.stdout)
            ]
        )
        self.logger = logging.getLogger(__name__)
        
        # Queries críticas baseadas no modelo BTG
        self.critical_queries = {
            "get_client_profile": {
                "sql": """
                    SELECT c.*, a.nome as assessor_nome, a.unidade_btg,
                           hrd.risco as risco_declarado,
                           hrc.risco as risco_calculado,
                           hrc.valorTotal as patrimonio_total
                    FROM clientes c
                    LEFT JOIN assessores a ON c.assessores_id = a.id
                    LEFT JOIN LATERAL (
                        SELECT risco FROM historico_risco_declarado 
                        WHERE clientes_id = c.id 
                        ORDER BY created_at DESC LIMIT 1
                    ) hrd ON true
                    LEFT JOIN LATERAL (
                        SELECT risco, valorTotal FROM historico_risco_calculado 
                        WHERE clientes_id = c.id 
                        ORDER BY created_at DESC LIMIT 1
                    ) hrc ON true
                    WHERE c.id = %s AND c.is_active = true;
                """,
                "description": "Busca perfil completo do cliente"
            },
            
            "get_client_investments": {
                "sql": """
                    SELECT i.*, a.nome as ativo_nome, a.tipo, a.subgrupo
                    FROM investimento i
                    JOIN ativos a ON i.ativos_id = a.id
                    WHERE i.clientes_id = %s
                    ORDER BY i.data DESC
                    LIMIT 20;
                """,
                "description": "Lista investimentos do cliente"
            },
            
            "portfolio_summary": {
                "sql": """
                    SELECT 
                        a.tipo,
                        COUNT(*) as quantidade_ativos,
                        SUM(i.valorTotal) as valor_total,
                        AVG(i.valorTotal) as valor_medio,
                        ROUND((SUM(i.valorTotal) * 100.0 / SUM(SUM(i.valorTotal)) OVER()), 2) as percentual
                    FROM investimento i
                    JOIN ativos a ON i.ativos_id = a.id
                    WHERE i.clientes_id = %s
                    GROUP BY a.tipo
                    ORDER BY valor_total DESC;
                """,
                "description": "Resumo de portfólio por tipo de ativo"
            },
            
            "compliance_check": {
                "sql": """
                    SELECT 
                        c.id,
                        c.nome,
                        c.categoria,
                        hrd.risco as risco_declarado,
                        hrc.risco as risco_calculado,
                        hrc.RV_percentual,
                        hrc.RF_percentual,
                        CASE 
                            WHEN hrd.risco != hrc.risco THEN 'DIVERGENTE'
                            ELSE 'COMPATIVEL'
                        END as status_compliance
                    FROM clientes c
                    LEFT JOIN LATERAL (
                        SELECT risco FROM historico_risco_declarado 
                        WHERE clientes_id = c.id 
                        ORDER BY created_at DESC LIMIT 1
                    ) hrd ON true
                    LEFT JOIN LATERAL (
                        SELECT risco, RV_percentual, RF_percentual FROM historico_risco_calculado 
                        WHERE clientes_id = c.id 
                        ORDER BY created_at DESC LIMIT 1
                    ) hrc ON true
                    WHERE c.assessores_id = %s
                    AND c.is_active = true;
                """,
                "description": "Verificação de compliance por assessor"
            },
            
            "assessor_portfolio": {
                "sql": """
                    SELECT 
                        a.unidade_btg,
                        COUNT(DISTINCT c.id) as total_clientes,
                        COUNT(DISTINCT CASE WHEN c.is_active THEN c.id END) as clientes_ativos,
                        COALESCE(SUM(i.valorTotal), 0) as volume_total,
                        COALESCE(AVG(i.valorTotal), 0) as ticket_medio
                    FROM assessores a
                    LEFT JOIN clientes c ON a.id = c.assessores_id
                    LEFT JOIN investimento i ON c.id = i.clientes_id
                    WHERE a.id = %s
                    AND (i.data IS NULL OR i.data >= CURRENT_DATE - INTERVAL '30 days')
                    GROUP BY a.unidade_btg, a.id;
                """,
                "description": "Relatório de performance do assessor"
            }
        }

    def get_connection(self):
        """Cria conexão com o banco"""
        try:
            return psycopg2.connect(**self.connection_params)
        except Exception as e:
            self.logger.error(f"Erro ao conectar no banco: {e}")
            raise

    def execute_with_explain(self, query_name: str, query: str, params: tuple = None) -> Dict:
        """
        Executa query com EXPLAIN ANALYZE e coleta métricas
        """
        start_time = time.time()
        
        try:
            with self.get_connection() as conn:
                with conn.cursor() as cursor:
                    # Executa EXPLAIN ANALYZE
                    explain_query = f"EXPLAIN (ANALYZE, BUFFERS, FORMAT JSON) {query}"
                    cursor.execute(explain_query, params)
                    explain_result = cursor.fetchone()[0]
                    
                    # Executa query normal para medir tempo real
                    cursor.execute(query, params)
                    results = cursor.fetchall()
                    
                    execution_time = (time.time() - start_time) * 1000  # em ms
                    
                    # Extrai métricas do plano de execução
                    plan_metrics = self._extract_plan_metrics(explain_result[0])
                    
                    return {
                        'success': True,
                        'query_name': query_name,
                        'execution_time': execution_time,
                        'rows_returned': len(results),
                        'explain_plan': explain_result,
                        'plan_metrics': plan_metrics,
                        'timestamp': datetime.now().isoformat(),
                        'params': str(params) if params else None
                    }
                    
        except Exception as e:
            return {
                'success': False,
                'query_name': query_name,
                'error': str(e),
                'execution_time': (time.time() - start_time) * 1000,
                'timestamp': datetime.now().isoformat(),
                'params': str(params) if params else None
            }

    def _extract_plan_metrics(self, plan: Dict) -> Dict:
        """
        Extrai métricas importantes do plano de execução
        """
        metrics = {
            'total_cost': plan.get('Total Cost', 0),
            'actual_time': plan.get('Actual Total Time', 0),
            'seq_scans': 0,
            'index_scans': 0,
            'nested_loops': 0,
            'hash_joins': 0,
            'sort_operations': 0,
            'expensive_operations': []
        }
        
        def traverse_plan(node):
            node_type = node.get('Node Type', '')
            
            # Contar tipos de operações
            if 'Seq Scan' in node_type:
                metrics['seq_scans'] += 1
            elif 'Index Scan' in node_type or 'Index Only Scan' in node_type:
                metrics['index_scans'] += 1
            elif 'Nested Loop' in node_type:
                metrics['nested_loops'] += 1
            elif 'Hash Join' in node_type:
                metrics['hash_joins'] += 1
            elif 'Sort' in node_type:
                metrics['sort_operations'] += 1
            
            # Identificar operações caras
            if node.get('Total Cost', 0) > 1000:
                metrics['expensive_operations'].append({
                    'operation': node_type,
                    'cost': node.get('Total Cost', 0),
                    'time': node.get('Actual Total Time', 0)
                })
            
            # Recursão para nós filhos
            for child in node.get('Plans', []):
                traverse_plan(child)
        
        traverse_plan(plan)
        return metrics

    def generate_test_parameters(self) -> Dict[str, List[tuple]]:
        """
        Gera parâmetros de teste baseados nos dados criados
        """
        params = {}
        
        try:
            with self.get_connection() as conn:
                with conn.cursor() as cursor:
                    # IDs de clientes
                    cursor.execute("""
                        SELECT id FROM clientes 
                        WHERE id LIKE 'TEST_%' 
                        AND is_active = true
                        ORDER BY RANDOM() 
                        LIMIT 1000
                    """)
                    client_ids = [row[0] for row in cursor.fetchall()]
                    
                    # IDs de assessores
                    cursor.execute("""
                        SELECT id FROM assessores 
                        WHERE id LIKE 'ASSESS_TEST_%' 
                        ORDER BY RANDOM() 
                        LIMIT 100
                    """)
                    assessor_ids = [row[0] for row in cursor.fetchall()]
                    
                    params = {
                        'get_client_profile': [(client_id,) for client_id in client_ids],
                        'get_client_investments': [(client_id,) for client_id in client_ids],
                        'portfolio_summary': [(client_id,) for client_id in client_ids],
                        'compliance_check': [(assessor_id,) for assessor_id in assessor_ids],
                        'assessor_portfolio': [(assessor_id,) for assessor_id in assessor_ids]
                    }
                    
                    self.logger.info(f"Parâmetros gerados: {len(client_ids)} clientes, {len(assessor_ids)} assessores")
                    
        except Exception as e:
            self.logger.error(f"Erro ao gerar parâmetros: {e}")
            
        return params

    def worker_thread(self, query_name: str, query_info: dict, test_params: List[tuple], 
                     duration_seconds: int, thread_id: int):
        """
        Thread worker que executa queries continuamente
        """
        start_time = time.time()
        local_results = []
        
        self.logger.info(f"Thread {thread_id} iniciada para {query_name}")
        
        import random
        
        while time.time() - start_time < duration_seconds:
            # Seleciona parâmetros aleatórios
            params = random.choice(test_params) if test_params else None
            
            result = self.execute_with_explain(query_name, query_info['sql'], params)
            result['thread_id'] = thread_id
            
            local_results.append(result)
            
            # Pequena pausa para evitar sobrecarga
            time.sleep(0.05)
        
        # Thread-safe append dos resultados
        with self.lock:
            self.results.extend(local_results)
            
        self.logger.info(f"Thread {thread_id} finalizada. Executou {len(local_results)} queries")

    def run_load_test(self, duration_seconds: int = 300, threads_per_query: int = 10):
        """
        Executa teste de carga com análise detalhada
        """
        self.logger.info(f"🚀 Iniciando teste de carga: {duration_seconds}s, {threads_per_query} threads por query")
        
        # Gera parâmetros de teste
        test_params = self.generate_test_parameters()
        
        if not test_params:
            self.logger.error("❌ Não foi possível gerar parâmetros de teste")
            return
        
        # Lista de threads
        threads = []
        
        # Cria threads para cada query crítica
        for query_name, query_info in self.critical_queries.items():
            params_list = test_params.get(query_name, [None])
            
            for i in range(threads_per_query):
                thread = threading.Thread(
                    target=self.worker_thread,
                    args=(query_name, query_info, params_list, duration_seconds, i)
                )
                threads.append(thread)
                thread.start()
        
        # Aguarda todas as threads
        for thread in threads:
            thread.join()
        
        self.logger.info(f"✅ Teste concluído. Total de execuções: {len(self.results)}")

    def analyze_results(self) -> Dict:
        """
        Analisa resultados coletados e gera relatório detalhado
        """
        if not self.results:
            return {"error": "Nenhum resultado para analisar"}
        
        analysis = {
            'summary': {},
            'by_query': {},
            'performance_issues': [],
            'optimization_suggestions': [],
            'explain_analysis': {}
        }
        
        # Análise geral
        successful_queries = [r for r in self.results if r['success']]
        failed_queries = [r for r in self.results if not r['success']]
        
        if successful_queries:
            execution_times = [r['execution_time'] for r in successful_queries]
            
            analysis['summary'] = {
                'total_queries': len(self.results),
                'successful_queries': len(successful_queries),
                'failed_queries': len(failed_queries),
                'success_rate': len(successful_queries) / len(self.results) * 100,
                'avg_response_time': statistics.mean(execution_times),
                'median_response_time': statistics.median(execution_times),
                'p95_response_time': statistics.quantiles(execution_times, n=20)[18] if len(execution_times) > 20 else max(execution_times),
                'p99_response_time': statistics.quantiles(execution_times, n=100)[98] if len(execution_times) > 100 else max(execution_times),
                'min_response_time': min(execution_times),
                'max_response_time': max(execution_times)
            }
        
        # Análise por query
        from collections import defaultdict
        by_query = defaultdict(list)
        
        for result in successful_queries:
            by_query[result['query_name']].append(result)
        
        for query_name, query_results in by_query.items():
            times = [r['execution_time'] for r in query_results]
            
            analysis['by_query'][query_name] = {
                'executions': len(query_results),
                'avg_time': statistics.mean(times),
                'median_time': statistics.median(times),
                'max_time': max(times),
                'min_time': min(times),
                'description': self.critical_queries[query_name]['description']
            }
            
            # Identifica problemas de performance
            avg_time = statistics.mean(times)
            if avg_time > 200:  # > 200ms
                analysis['performance_issues'].append({
                    'query': query_name,
                    'issue': 'Tempo de resposta alto',
                    'avg_time': avg_time,
                    'severity': 'HIGH' if avg_time > 500 else 'MEDIUM'
                })
        
        # Análise de planos de execução
        self._analyze_explain_plans(analysis, successful_queries)
        
        return analysis

    def _analyze_explain_plans(self, analysis: Dict, successful_queries: List[Dict]):
        """
        Analisa planos de execução para identificar otimizações
        """
        seq_scans = []
        expensive_operations = []
        optimization_opportunities = []
        
        for result in successful_queries:
            if 'plan_metrics' in result:
                metrics = result['plan_metrics']
                query_name = result['query_name']
                
                # Sequential Scans
                if metrics['seq_scans'] > 0:
                    seq_scans.append(query_name)
                
                # Operações caras
                if metrics['total_cost'] > 10000:
                    expensive_operations.append({
                        'query': query_name,
                        'cost': metrics['total_cost'],
                        'time': metrics['actual_time']
                    })
                
                # Muitos nested loops podem indicar falta de índices
                if metrics['nested_loops'] > 3:
                    optimization_opportunities.append({
                        'query': query_name,
                        'issue': 'Muitos Nested Loops',
                        'suggestion': 'Considere criar índices para melhorar JOINs'
                    })
        
        # Sugestões de otimização
        if seq_scans:
            analysis['optimization_suggestions'].append({
                'type': 'Sequential Scans Detectados',
                'queries': list(set(seq_scans)),
                'suggestion': 'Criar índices apropriados para evitar varreduras sequenciais',
                'priority': 'HIGH'
            })
        
        if expensive_operations:
            analysis['optimization_suggestions'].append({
                'type': 'Operações Custosas',
                'operations': expensive_operations[:5],  # Top 5
                'suggestion': 'Revisar estrutura das queries e considerar otimização',
                'priority': 'MEDIUM'
            })
        
        if optimization_opportunities:
            analysis['optimization_suggestions'].extend(optimization_opportunities)

    def generate_report(self, output_file: str = None):
        """
        Gera relatório detalhado em JSON
        """
        analysis = self.analyze_results()
        
        report = {
            'test_info': {
                'timestamp': datetime.now().isoformat(),
                'duration_seconds': 300,  # padrão
                'total_threads': self.max_workers,
                'queries_tested': list(self.critical_queries.keys()),
                'database_params': {k: v for k, v in self.connection_params.items() if k != 'password'}
            },
            'analysis': analysis,
            'raw_results_sample': self.results[:10] if self.results else []  # Amostra dos resultados
        }
        
        if output_file:
            with open(output_file, 'w', encoding='utf-8') as f:
                json.dump(report, f, indent=2, ensure_ascii=False, default=str)
            self.logger.info(f"📄 Relatório salvo em: {output_file}")
        
        return report

    def print_summary(self):
        """
        Imprime resumo dos resultados no console
        """
        analysis = self.analyze_results()
        
        if 'error' in analysis:
            print(f"❌ {analysis['error']}")
            return
        
        summary = analysis['summary']
        
        print("\n" + "="*70)
        print("📊 BTG PACTUAL - RELATÓRIO DE TESTE DE CARGA COM EXPLAIN ANALYZE")
        print("="*70)
        
        print(f"\n📈 MÉTRICAS GERAIS:")
        print(f"   📋 Total de Queries: {summary['total_queries']:,}")
        print(f"   ✅ Queries com Sucesso: {summary['successful_queries']:,}")
        print(f"   📊 Taxa de Sucesso: {summary['success_rate']:.1f}%")
        print(f"   ⏱️  Tempo Médio: {summary['avg_response_time']:.2f}ms")
        print(f"   📊 Tempo Mediano: {summary['median_response_time']:.2f}ms")
        print(f"   📈 P95: {summary['p95_response_time']:.2f}ms")
        print(f"   🔺 P99: {summary['p99_response_time']:.2f}ms")
        print(f"   ⚡ Tempo Mínimo: {summary['min_response_time']:.2f}ms")
        print(f"   🐌 Tempo Máximo: {summary['max_response_time']:.2f}ms")
        
        print(f"\n🎯 PERFORMANCE POR QUERY:")
        for query_name, stats in analysis['by_query'].items():
            print(f"   📋 {query_name}:")
            print(f"      📊 Execuções: {stats['executions']:,}")
            print(f"      ⏱️  Tempo Médio: {stats['avg_time']:.2f}ms")
            print(f"      🔺 Tempo Máximo: {stats['max_time']:.2f}ms")
            print(f"      📝 Descrição: {stats['description']}")
        
        if analysis['performance_issues']:
            print(f"\n⚠️  PROBLEMAS DE PERFORMANCE IDENTIFICADOS:")
            for issue in analysis['performance_issues']:
                severity_icon = "🔴" if issue.get('severity') == 'HIGH' else "🟡"
                print(f"   {severity_icon} {issue['query']}: {issue['issue']} ({issue['avg_time']:.2f}ms)")
        
        if analysis['optimization_suggestions']:
            print(f"\n💡 SUGESTÕES DE OTIMIZAÇÃO:")
            for suggestion in analysis['optimization_suggestions']:
                priority_icon = "🔥" if suggestion.get('priority') == 'HIGH' else "📌"
                print(f"   {priority_icon} {suggestion['type']}: {suggestion['suggestion']}")
                if 'queries' in suggestion:
                    print(f"      🎯 Queries afetadas: {', '.join(suggestion['queries'])}")


def main():
    """
    Função principal para executar o teste
    """
    print("🔧 BTG Pactual - Analisador de Teste de Carga")
    print("=" * 50)
    
    # Configuração de conexão
    connection_params = {
        'host': 'localhost',
        'database': 'btg_db',
        'user': 'btg_user',
        'password': 'btg_pass',
        'port': 5432
    }
    
    # Testar conexão
    try:
        test_conn = psycopg2.connect(**connection_params)
        test_conn.close()
        print("✅ Conexão com banco estabelecida")
    except Exception as e:
        print(f"❌ Erro de conexão: {e}")
        print("\n🔧 Verifique se o Docker está rodando:")
        print("   docker-compose up -d")
        return
    
    # Criar analisador
    analyzer = DatabaseLoadTestAnalyzer(connection_params, max_workers=50)
    
    print("\n🚀 Iniciando teste de carga com análise EXPLAIN...")
    
    # Executa teste (5 minutos, 8 threads por query)
    duration = int(input("⏱️  Duração do teste em segundos (padrão 300): ") or "300")
    threads = int(input("🧵 Threads por query (padrão 8): ") or "8")
    
    analyzer.run_load_test(duration_seconds=duration, threads_per_query=threads)
    
    # Imprime resultados
    analyzer.print_summary()
    
    # Gera relatório detalhado
    report_file = f'load_test_detailed_report_{datetime.now().strftime("%Y%m%d_%H%M%S")}.json'
    analyzer.generate_report(report_file)
    
    print(f"\n✅ Teste concluído! Verifique o arquivo {report_file} para detalhes completos.")


if __name__ == "__main__":
    main()
