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
    novo_id := 'UL' || to_char(NOW(), 'YYMMDDHH24MISS') || LPAD(FLOOR(random() * 10000)::TEXT, 8, '0');
    
    SELECT COALESCE(SUM(i.valorTotal), 0) INTO total_investimentos
    FROM investimento i
    WHERE i.clientes_id = NEW.clientes_id;
    
    SELECT COALESCE(SUM(i.valorTotal), 0) INTO total_renda_fixa
    FROM investimento i
    JOIN ativos a ON i.ativos_id = a.id
    WHERE i.clientes_id = NEW.clientes_id AND a.tipo = 'Renda Fixa';
    
    SELECT COALESCE(SUM(i.valorTotal), 0) INTO total_renda_variavel
    FROM investimento i
    JOIN ativos a ON i.ativos_id = a.id
    WHERE i.clientes_id = NEW.clientes_id AND a.tipo != 'Renda Fixa';
    
    IF total_investimentos > 0 THEN
        percentual_rf := (total_renda_fixa / total_investimentos) * 100;
        percentual_rv := (total_renda_variavel / total_investimentos) * 100;
    ELSE
        percentual_rf := 0;
        percentual_rv := 0;
    END IF;
    
    IF percentual_rf >= 90 THEN
        perfil_calculado := 'Conservador';
    ELSIF percentual_rv >= 70 THEN
        perfil_calculado := 'Sofisticado';
    ELSE
        perfil_calculado := 'Moderado';
    END IF;
    
    INSERT INTO historico_risco_calculado (
        id, risco, RV_percentual, RF_percentual, valorTotal, created_at, clientes_id
    ) VALUES (
        novo_id, perfil_calculado, percentual_rv, percentual_rf, total_investimentos, NOW(), NEW.clientes_id
    );
    
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION obter_perfil_risco_cliente(cliente_id VARCHAR)
RETURNS TABLE (
    perfil risco_enum,
    percentual_renda_variavel DOUBLE PRECISION,
    percentual_renda_fixa DOUBLE PRECISION,
    valor_total DOUBLE PRECISION,
    data_calculo TIMESTAMP,
    nome_cliente VARCHAR,
    id_cliente VARCHAR
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        hrc.risco,
        hrc.RV_percentual,
        hrc.RF_percentual,
        hrc.valorTotal,
        hrc.created_at,
        c.nome,
        c.id
    FROM 
        historico_risco_calculado hrc
    JOIN 
        clientes c ON hrc.clientes_id = c.id
    WHERE 
        hrc.clientes_id = cliente_id
    ORDER BY 
        hrc.created_at DESC
    LIMIT 1;

    IF NOT FOUND THEN
        RAISE NOTICE 'Nenhum perfil de risco encontrado para o cliente %', cliente_id;
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION obter_perfil_cliente(cliente_id VARCHAR)
RETURNS risco_enum AS $$
DECLARE
    perfil_cliente risco_enum;
BEGIN
    SELECT risco INTO perfil_cliente
    FROM historico_risco_calculado
    WHERE clientes_id = cliente_id
    ORDER BY created_at DESC
    LIMIT 1;
    
    RETURN perfil_cliente;
END;
$$ LANGUAGE plpgsql;

COMMENT ON FUNCTION recalcular_perfil_risco() IS 'Função que calcula o perfil de risco de um cliente com base em seus investimentos';
COMMENT ON FUNCTION obter_perfil_risco_cliente(VARCHAR) IS 'Função que retorna o perfil de risco mais recente de um cliente com detalhes completos';
COMMENT ON FUNCTION obter_perfil_cliente(VARCHAR) IS 'Função simplificada que retorna apenas o tipo de perfil de risco de um cliente';


CREATE TRIGGER after_investimento_insert
AFTER INSERT ON investimento
FOR EACH ROW
EXECUTE FUNCTION recalcular_perfil_risco();

CREATE TRIGGER after_investimento_update
AFTER UPDATE ON investimento
FOR EACH ROW
WHEN (OLD.valorTotal IS DISTINCT FROM NEW.valorTotal OR OLD.ativos_id IS DISTINCT FROM NEW.ativos_id)
EXECUTE FUNCTION recalcular_perfil_risco();

CREATE TRIGGER after_investimento_delete
AFTER DELETE ON investimento
FOR EACH ROW
EXECUTE FUNCTION recalcular_perfil_risco();

COMMENT ON TRIGGER after_investimento_insert ON investimento IS 'Trigger que recalcula o perfil quando um novo investimento é inserido';
COMMENT ON TRIGGER after_investimento_update ON investimento IS 'Trigger que recalcula o perfil quando um investimento é atualizado';
COMMENT ON TRIGGER after_investimento_delete ON investimento IS 'Trigger que recalcula o perfil quando um investimento é excluído';

CREATE OR REPLACE FUNCTION obter_quantidade_clientes_conformes()
RETURNS INTEGER AS $$
DECLARE
    quantidade_clientes INTEGER := 0;
BEGIN
    SELECT COUNT(*) INTO quantidade_clientes
    FROM historico_risco_calculado hrc
    WHERE hrc.id IN (
        SELECT DISTINCT ON (hrc_sub.clientes_id) hrc_sub.id
        FROM historico_risco_calculado hrc_sub
        ORDER BY hrc_sub.clientes_id, hrc_sub.created_at DESC
    )
    AND EXISTS (
        SELECT 1 
        FROM historico_risco_declarado hrd
        WHERE hrd.clientes_id = hrc.clientes_id
        AND hrd.risco = hrc.risco
        AND hrd.id IN (
            SELECT DISTINCT ON (hrd_sub.clientes_id) hrd_sub.id
            FROM historico_risco_declarado hrd_sub
            WHERE hrd_sub.clientes_id = hrd.clientes_id
            ORDER BY hrd_sub.clientes_id, hrd_sub.created_at DESC
        )
    )
    AND EXISTS (
        SELECT 1 FROM clientes c 
        WHERE c.id = hrc.clientes_id 
        AND c.is_active = TRUE
    );
    
    RETURN quantidade_clientes;
END;
$$ LANGUAGE plpgsql;

COMMENT ON FUNCTION obter_quantidade_clientes_conformes() IS 'Função que retorna a quantidade de clientes conformes (risco declarado igual ao risco calculado mais recente)';

CREATE OR REPLACE FUNCTION obter_quantidade_clientes_inconformes()
RETURNS INTEGER AS $$
DECLARE
    quantidade_clientes INTEGER := 0;
BEGIN
    SELECT COUNT(*) INTO quantidade_clientes
    FROM historico_risco_calculado hrc
    WHERE hrc.id IN (
        SELECT DISTINCT ON (hrc_sub.clientes_id) hrc_sub.id
        FROM historico_risco_calculado hrc_sub
        ORDER BY hrc_sub.clientes_id, hrc_sub.created_at DESC
    )
    AND EXISTS (
        SELECT 1 FROM clientes c 
        WHERE c.id = hrc.clientes_id 
        AND c.is_active = TRUE
    )
    AND NOT EXISTS (
        SELECT 1 
        FROM historico_risco_declarado hrd
        WHERE hrd.clientes_id = hrc.clientes_id
        AND hrd.risco = hrc.risco
        AND hrd.id IN (
            SELECT DISTINCT ON (hrd_sub.clientes_id) hrd_sub.id
            FROM historico_risco_declarado hrd_sub
            WHERE hrd_sub.clientes_id = hrd.clientes_id
            ORDER BY hrd_sub.clientes_id, hrd_sub.created_at DESC
        )
    );
    
    RETURN quantidade_clientes;
END;
$$ LANGUAGE plpgsql;

COMMENT ON FUNCTION obter_quantidade_clientes_inconformes() IS 'Função que retorna a quantidade de clientes inconformes (risco declarado diferente do risco calculado mais recente)';

CREATE OR REPLACE PROCEDURE obter_soma_conformes_inconformes(
    OUT total_conforme INT,
    OUT total_inconforme INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    -- Obtém risco calculado mais recente por cliente
    WITH risco_calculado AS (
        SELECT DISTINCT ON (clientes_id) clientes_id, risco
        FROM historico_risco_calculado
        ORDER BY clientes_id, created_at DESC
    ),
    -- Obtém risco declarado mais recente por cliente
    risco_declarado AS (
        SELECT DISTINCT ON (clientes_id) clientes_id, risco
        FROM historico_risco_declarado
        ORDER BY clientes_id, created_at DESC
    )
    -- Conta clientes ativos com risco declarado igual ao calculado (conformes)
    SELECT COUNT(*) INTO total_conforme
    FROM risco_calculado rc
    JOIN clientes c ON rc.clientes_id = c.id
    JOIN risco_declarado rd ON rc.clientes_id = rd.clientes_id
    WHERE c.is_active = TRUE
    AND rc.risco = rd.risco;

    -- Conta clientes ativos com risco declarado diferente do calculado (inconformes)
    SELECT COUNT(*) INTO total_inconforme
    FROM risco_calculado rc
    JOIN clientes c ON rc.clientes_id = c.id
    JOIN risco_declarado rd ON rc.clientes_id = rd.clientes_id
    WHERE c.is_active = TRUE
    AND rc.risco <> rd.risco;
END;
$$;

COMMENT ON PROCEDURE obter_soma_conformes_inconformes()
IS 'Retorna a soma de clientes conformes e inconformes, baseado na comparação do risco declarado mais recente com o risco calculado mais recente';