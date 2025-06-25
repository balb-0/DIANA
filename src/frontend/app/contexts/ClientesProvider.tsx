import {
  ReactNode,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";
import {
  getClientesPorRisco,
  ClienteCarteira,
} from "@/services/carteiraService";

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

function generateMockData(clientId: string, clientName: string) {
  const hash = Array.from(clientId).reduce(
    (acc, char) => acc + char.charCodeAt(0),
    0
  );

  const emailName = clientName
    .toLowerCase()
    .replace(/\s+/g, ".")
    .normalize("NFD")
    .replace(/[\u0300-\u036f]/g, "");
  const email = `${emailName}@gmail.com`;

  const cpfBase = ((hash % 900000000) + 100000000).toString();
  const cpf = `${cpfBase.slice(0, 3)}.${cpfBase.slice(3, 6)}.${cpfBase.slice(
    6
  )}-${(hash % 90) + 10}`;

  const phoneBase = ((hash % 900000000) + 100000000).toString();
  const phone = `(${phoneBase.slice(0, 2)}) 9${phoneBase.slice(
    2,
    6
  )}-${phoneBase.slice(6)}`;

  const year = 1960 + (hash % 40);
  const month = 1 + (hash % 12);
  const day = 1 + (hash % 28);
  const birthDate = `${day.toString().padStart(2, "0")}/${month
    .toString()
    .padStart(2, "0")}/${year}`;

  return {
    email,
    cpf,
    telefone: phone,
    nascimento: birthDate,
  };
}

export const ClientesProvider = ({
  children,
  tipoRisco,
}: {
  children: ReactNode;
  tipoRisco: string;
}) => {
  const [clientes, setClientes] = useState<Cliente[]>([]);
  const [clientesPorPerfil, setClientesPorPerfil] = useState<ClientesPorPerfil>(
    {
      Conservador: [],
      Moderado: [],
      Sofisticado: [],
    }
  );
  const [clientesPorStatus, setClientesPorStatus] = useState<ClientesPorStatus>(
    {
      Regulares: [],
      Inconformes: [],
    }
  );

  useEffect(() => {
    async function fetchData() {
      if (!tipoRisco) {
        console.warn(
          "ClientesProvider: tipoRisco não definido, abortando fetch."
        );
        return;
      }

      const clientesAPI = await getClientesPorRisco(tipoRisco);

      const perfilInvestidor = mapPerfil(tipoRisco);

      const clientesMapped = clientesAPI.map<Cliente>((c) => {
        const res = generateMockData(c.clienteId, c.clienteNome);
        return {
          id: c.clienteId,
          nome: c.clienteNome,
          email: res.email,
          cpf: res.cpf,
          nascimento: res.nascimento,
          telefone: res.telefone,
          perfilInvestidor,
          quantiaInvestida: 0,
          bancoInvestido: "",
          quantidadeAtivos: 0,
          status: "Regular",
        };
      });

      setClientes(clientesMapped);

      setClientesPorPerfil({
        Conservador:
          perfilInvestidor === "Conservador" || tipoRisco === "geral"
            ? clientesMapped
            : [],
        Moderado:
          perfilInvestidor === "Moderado" || tipoRisco === "geral"
            ? clientesMapped
            : [],
        Sofisticado:
          perfilInvestidor === "Sofisticado" || tipoRisco === "geral"
            ? clientesMapped
            : [],
      });

      setClientesPorStatus({
        Regulares: clientesMapped,
        Inconformes: [],
      });
    }

    fetchData();
  }, [tipoRisco]);

  function mapPerfil(
    tipoRisco: string
  ): "Conservador" | "Moderado" | "Sofisticado" {
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
    <ClientesContext.Provider
      value={{ clientes, clientesPorPerfil, clientesPorStatus }}
    >
      {children}
    </ClientesContext.Provider>
  );
};

export const useClientes = () => useContext(ClientesContext);
