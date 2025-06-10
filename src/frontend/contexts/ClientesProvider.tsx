import React, { createContext, useContext, useEffect, useState, ReactNode } from "react";

type Cliente = {
    id: number;
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

export const ClientesProvider = ({ children }: { children: ReactNode }) => {
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
        setTimeout(() => {
            const dadosMock: Cliente[] = [
                {
                    id: 1,
                    nome: "Joana Silva",
                    email: "joana@email.com",
                    cpf: "123.456.789-00",
                    nascimento: "1990-04-12",
                    telefone: "(11) 91234-5678",
                    perfilInvestidor: "Conservador",
                    quantiaInvestida: 10000,
                    bancoInvestido: "Banco XPTO",
                    quantidadeAtivos: 3,
                    status: "Regular",
                },
                {
                    id: 2,
                    nome: "Carlos Souza",
                    email: "carlos@email.com",
                    cpf: "987.654.321-00",
                    nascimento: "1985-08-22",
                    telefone: "(21) 99876-5432",
                    perfilInvestidor: "Moderado",
                    quantiaInvestida: 25000,
                    bancoInvestido: "Banco Alpha",
                    quantidadeAtivos: 5,
                    status: "Inconforme",
                },
                {
                    id: 3,
                    nome: "Ana Pereira",
                    email: "ana@email.com",
                    cpf: "111.222.333-44",
                    nascimento: "1992-11-03",
                    telefone: "(31) 93456-7890",
                    perfilInvestidor: "Sofisticado",
                    quantiaInvestida: 70000,
                    bancoInvestido: "Banco Beta",
                    quantidadeAtivos: 10,
                    status: "Regular",
                },
            ];

            setClientes(dadosMock);

            setClientesPorPerfil({
                Conservador: dadosMock.filter((c) => c.perfilInvestidor === "Conservador"),
                Moderado: dadosMock.filter((c) => c.perfilInvestidor === "Moderado"),
                Sofisticado: dadosMock.filter((c) => c.perfilInvestidor === "Sofisticado"),
            });

            setClientesPorStatus({
                Regulares: dadosMock.filter((c) => c.status === "Regular"),
                Inconformes: dadosMock.filter((c) => c.status === "Inconforme"),
            });
        }, 10);
    }, []);

    return (
        <ClientesContext.Provider value={{ clientes, clientesPorPerfil, clientesPorStatus }}>
            {children}
        </ClientesContext.Provider>
    );
};

export const useClientes = () => useContext(ClientesContext);
