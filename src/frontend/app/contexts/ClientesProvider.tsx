import { ReactNode, createContext, useContext, useEffect, useState } from "react";
import { getClientesPorRisco, ClienteCarteira } from "@/services/carteiraService";

type Cliente = {
    id: string;
    nome: string;
    email: string;
    cpf: string;
    nascimento: string;
    telefone: string;
    perfilInvestidor: "Conservador" | "Moderado" | "Sofisticado";
    quantiaInvestida: number;
    bancoInvestido: string;
    quantidadeAtivos: number;
    status: "Regular" | "Inconforme";
};

type ClientesPorPerfil = {
    Conservador: Cliente[];
    Moderado: Cliente[];
    Sofisticado: Cliente[];
};

type ClientesPorStatus = {
    Regulares: Cliente[];
    Inconformes: Cliente[];
};

type ClientesContextData = {
    clientes: Cliente[];
    clientesPorPerfil: ClientesPorPerfil;
    clientesPorStatus: ClientesPorStatus;
};

const ClientesContext = createContext({} as ClientesContextData);

export const ClientesProvider = ({ children, tipoRisco }: { children: ReactNode; tipoRisco: string }) => {
    const [clientes, setClientes] = useState<Cliente[]>([]);
    const [clientesPorPerfil, setClientesPorPerfil] = useState<ClientesPorPerfil>({
        Conservador: [],
        Moderado: [],
        Sofisticado: [],
    });
    const [clientesPorStatus, setClientesPorStatus] = useState<ClientesPorStatus>({
        Regulares: [],
        Inconformes: [],
    });

    useEffect(() => {
        async function fetchData() {
            if (!tipoRisco) {
                console.warn("ClientesProvider: tipoRisco não definido, abortando fetch.");
                return;
            }

            const clientesAPI = await getClientesPorRisco(tipoRisco);

            const perfilInvestidor = mapPerfil(tipoRisco);

            const clientesMapped = clientesAPI.map<Cliente>((c) => ({
                id:  c.clienteId,
                nome: c.clienteNome,
                email: "",
                cpf: "",
                nascimento: "",
                telefone: "",
                perfilInvestidor,
                quantiaInvestida: 0,
                bancoInvestido: "",
                quantidadeAtivos: 0,
                status: "Regular",
            }));

            setClientes(clientesMapped);

            setClientesPorPerfil({
                Conservador: perfilInvestidor === "Conservador" || tipoRisco === "geral" ? clientesMapped : [],
                Moderado: perfilInvestidor === "Moderado" || tipoRisco === "geral" ? clientesMapped : [],
                Sofisticado: perfilInvestidor === "Sofisticado" || tipoRisco === "geral" ? clientesMapped : [],
            });

            setClientesPorStatus({
                Regulares: clientesMapped,
                Inconformes: [],
            });
        }

        fetchData();
    }, [tipoRisco]);

    function mapPerfil(tipoRisco: string): "Conservador" | "Moderado" | "Sofisticado" {
        switch (tipoRisco) {
            case "conservador":
            case "Conservador":
                return "Conservador";
            case "moderado":
            case "Moderado":
                return "Moderado";
            case "sofisticado":
            case "Sofisticado":
                return "Sofisticado";
            case "geral":
            case "Geral":
                return "Conservador";
            default:
                return "Conservador";
        }
    }

    return (
        <ClientesContext.Provider value={{ clientes, clientesPorPerfil, clientesPorStatus }}>
            {children}
        </ClientesContext.Provider>
    );
};

export const useClientes = () => useContext(ClientesContext);
