import { useLocalSearchParams } from "expo-router";
import { ScrollView, Text, View } from "react-native";
import { useClientes } from "../../contexts/ClientesProvider";

export default function Investor() {
    const { investor } = useLocalSearchParams();
    const { clientesPorPerfil } = useClientes();

    const todosClientes = [
        ...clientesPorPerfil.Conservador,
        ...clientesPorPerfil.Moderado,
        ...clientesPorPerfil.Sofisticado,
    ];

    const cliente = todosClientes.find((c) => c.id === Number(investor));

    if (!cliente) {
        return (
            <View className="flex-1 justify-center items-center bg-azul-claro-secundario">
                <Text className="text-azul-escuro-principal text-xl font-bold">Cliente não encontrado!</Text>
            </View>
        );
    }

    return (
        <View className="flex-1 px-4 py-6 bg-azul-claro-secundario">

            <Text>{cliente.nome}</Text>

            <Text>{cliente.email}</Text>

            <Text>{cliente.cpf}</Text>

            <Text>{cliente.nascimento}</Text>

            <Text>{cliente.telefone}</Text>

            <Text>{cliente.perfilInvestidor}</Text>

            <Text>R$ {cliente.quantiaInvestida.toLocaleString()}</Text>

            <Text>{cliente.bancoInvestido}</Text>

            <Text>{cliente.quantidadeAtivos}</Text>

            <Text className={cliente.status === "Regular" ? "text-green-600" : "text-red-600"}>
                {cliente.status}
            </Text>
        </View>

    );
}
