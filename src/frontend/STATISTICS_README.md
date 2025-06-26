# Sistema de Estatísticas de Conformidade

Este sistema implementa uma "gambiarra" para calcular estatísticas de conformidade dos clientes no frontend, já que não temos acesso direto a essas informações no backend.

## Como Funciona

### 1. Coleta de Dados
O sistema busca clientes por categoria usando os endpoints disponíveis:
- `GET /api/Carteiras/clientes/risco-declarado/Conservador`
- `GET /api/Carteiras/clientes/risco-declarado/Moderado`
- `GET /api/Carteiras/clientes/risco-declarado/Sofisticado`

### 2. Verificação de Conformidade
Para cada cliente encontrado, o sistema:
1. Busca o perfil recomendado: `GET /api/Perfil/{clienteId}`
2. Busca o perfil declarado: `GET /Client/{id}/perfil`
3. Compara os dois perfis - se forem iguais, o cliente está em conformidade

### 3. Cálculo de Estatísticas
O sistema calcula:
- Total de clientes por categoria
- Quantidade de clientes conformes por categoria
- Quantidade de clientes inconformes por categoria

## Arquivos Principais

### `services/statisticsService.ts`
Serviço principal que implementa toda a lógica de:
- Busca de clientes por categoria
- Verificação de conformidade
- Cálculo de estatísticas

### `hooks/useStatistics.ts`
Hook personalizado para facilitar o uso das estatísticas em componentes.

### `contexts/CarteirasProvider.tsx`
Context atualizado para usar o novo serviço de estatísticas.

## Como Usar

### No Context (CarteirasProvider)
O context já está configurado para usar automaticamente o novo sistema. Os dados são atualizados quando `refreshData()` é chamado.

### Em Componentes Individuais
```tsx
import { useStatistics } from '@/hooks/useStatistics';

const MeuComponente = () => {
  const { getEstatisticasCategoria, isLoading, error } = useStatistics();
  
  const [stats, setStats] = useState(null);
  
  useEffect(() => {
    const loadStats = async () => {
      const estatisticas = await getEstatisticasCategoria('Conservador');
      setStats(estatisticas);
    };
    
    loadStats();
  }, []);
  
  // Usar stats.total, stats.conformes, stats.inconformes
};
```

### Para Debug
Use o componente `EstatisticasDebugView` para visualizar todas as estatísticas em tempo real.

## Estrutura de Dados

### EstatisticasCategoria
```typescript
interface EstatisticasCategoria {
  total: number;        // Total de clientes na categoria
  conformes: number;    // Clientes em conformidade
  inconformes: number;  // Clientes fora de conformidade
}
```

### EstatisticasCompletas
```typescript
interface EstatisticasCompletas {
  todas: EstatisticasCategoria;        // Todos os clientes
  conservador: EstatisticasCategoria;  // Apenas conservadores
  moderado: EstatisticasCategoria;     // Apenas moderados
  sofisticado: EstatisticasCategoria;  // Apenas sofisticados
}
```

## Performance

### Otimizações Implementadas
1. **Processamento em Lotes**: Os clientes são processados em grupos de 10 para não sobrecarregar a API
2. **Requisições Paralelas**: Busca perfil recomendado e declarado simultaneamente
3. **Cache no Context**: Os dados ficam cached no CarteirasProvider
4. **Pausas Entre Lotes**: Pequenas pausas entre lotes para não stressar o backend

### Considerações
- O tempo de carregamento inicial pode ser alto dependendo da quantidade de clientes
- O sistema faz muitas requisições (2 por cliente), então pode impactar a performance do backend
- Recomenda-se implementar um loading state adequado

## Endpoints Utilizados
- `GET /api/Carteiras/clientes/risco-declarado/{tipoRisco}` - Buscar clientes por categoria
- `GET /api/Perfil/{clienteId}` - Buscar perfil recomendado
- `GET /Client/{id}/perfil` - Buscar perfil declarado

## Limitações
- Este é um workaround temporário ("gambiarra")
- Performance pode ser impactada com muitos clientes
- Não é a solução ideal - o ideal seria ter esses dados calculados no backend
- Pode haver inconsistências se os dados mudarem durante o processamento

## Melhorias Futuras
1. Implementar cache mais sofisticado
2. Adicionar retry logic para requisições que falharam
3. Implementar paginação se houver muitos clientes
4. Mover o cálculo para o backend quando possível
