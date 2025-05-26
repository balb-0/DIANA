-- Arquivo de procedures e triggers para atualização automática do perfil de risco
-- filepath: c:\Users\Inteli\OneDrive\Desktop\Arquivos para Programação\modulo6\2025-1B-T13-ES06-G03\src\database\03_procedures.sql

-- Função para calcular e atualizar o perfil de risco de um cliente
CREATE OR REPLACE FUNCTION recalcular_perfil_risco()
RETURNS TRIGGER AS $$
DECLARE
    total_investimentos DOUBLE PRECISION;
    total_renda_fixa DOUBLE PRECISION;
    total_renda_variavel DOUBLE PRECISION;
    percentual_rf DOUBLE PRECISION;
    percentual_rv DOUBLE PRECISION;
    perfil_calculado risco_enum;
    novo_id VARCHAR(26);
BEGIN
    -- Gerar um ID único para o registro (simulando ULID)
    novo_id := 'UL' || to_char(NOW(), 'YYMMDDHH24MISS') || LPAD(FLOOR(random() * 10000)::TEXT, 8, '0');
    
    -- Calcular o total de investimentos do cliente
    SELECT COALESCE(SUM(i.valorTotal), 0) INTO total_investimentos
    FROM investimento i
    WHERE i.clientes_id = NEW.clientes_id;
    
    -- Calcular o total em renda fixa
    SELECT COALESCE(SUM(i.valorTotal), 0) INTO total_renda_fixa
    FROM investimento i
    JOIN ativos a ON i.ativos_id = a.id
    WHERE i.clientes_id = NEW.clientes_id AND a.tipo = 'Renda Fixa';
    
    -- Calcular o total em renda variável
    SELECT COALESCE(SUM(i.valorTotal), 0) INTO total_renda_variavel
    FROM investimento i
    JOIN ativos a ON i.ativos_id = a.id
    WHERE i.clientes_id = NEW.clientes_id AND a.tipo != 'Renda Fixa';
    
    -- Calcular percentuais
    IF total_investimentos > 0 THEN
        percentual_rf := (total_renda_fixa / total_investimentos) * 100;
        percentual_rv := (total_renda_variavel / total_investimentos) * 100;
    ELSE
        percentual_rf := 0;
        percentual_rv := 0;
    END IF;
    
    -- Determinar o perfil de risco com base nos percentuais
    IF percentual_rf >= 90 THEN
        perfil_calculado := 'Conservador';
    ELSIF percentual_rv >= 70 THEN
        perfil_calculado := 'Sofisticado';
    ELSE
        perfil_calculado := 'Moderado';
    END IF;
    
    -- Inserir novo registro de perfil calculado
    INSERT INTO historico_risco_calculado (
        id, risco, RV_percentual, RF_percentual, valorTotal, created_at, clientes_id
    ) VALUES (
        novo_id, perfil_calculado, percentual_rv, percentual_rf, total_investimentos, NOW(), NEW.clientes_id
    );
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Trigger para acionar o recálculo do perfil após inserção de um investimento
CREATE TRIGGER after_investimento_insert
AFTER INSERT ON investimento
FOR EACH ROW
EXECUTE FUNCTION recalcular_perfil_risco();

-- Trigger para acionar o recálculo do perfil após atualização de um investimento
CREATE TRIGGER after_investimento_update
AFTER UPDATE ON investimento
FOR EACH ROW
WHEN (OLD.valorTotal IS DISTINCT FROM NEW.valorTotal OR OLD.ativos_id IS DISTINCT FROM NEW.ativos_id)
EXECUTE FUNCTION recalcular_perfil_risco();

-- Trigger para acionar o recálculo do perfil após exclusão de um investimento
CREATE TRIGGER after_investimento_delete
AFTER DELETE ON investimento
FOR EACH ROW
EXECUTE FUNCTION recalcular_perfil_risco();

-- Comentários explicativos
COMMENT ON FUNCTION recalcular_perfil_risco() IS 'Função que calcula o perfil de risco de um cliente com base em seus investimentos';
COMMENT ON TRIGGER after_investimento_insert ON investimento IS 'Trigger que recalcula o perfil quando um novo investimento é inserido';
COMMENT ON TRIGGER after_investimento_update ON investimento IS 'Trigger que recalcula o perfil quando um investimento é atualizado';
COMMENT ON TRIGGER after_investimento_delete ON investimento IS 'Trigger que recalcula o perfil quando um investimento é excluído';
