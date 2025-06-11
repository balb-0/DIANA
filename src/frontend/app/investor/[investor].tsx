import { View, Text } from "react-native";
import { useLocalSearchParams } from "expo-router";
import { CarteiraDetailsHeader } from "@/components/carteira-details/carteira-details-header";
import { useClientes } from "../contexts/ClientesProvider";
import { useEffect, useState } from "react";
import { PiggyBank, Wallet, CheckCircle } from "phosphor-react-native";

export default function InvestorDetails() {
  const { investor } = useLocalSearchParams();
  const { clientes } = useClientes();
  const [cliente, setCliente] = useState<any>(null);

  useEffect(() => {
    // Find the client with the matching ID
    const foundCliente = clientes.find((c) => c.id === Number(investor));
    if (foundCliente) {
      setCliente(foundCliente);
    }
  }, [investor, clientes]);

  if (!cliente) {
    return null; // Or a loading state
  }

  return (
    <View className="flex-1 bg-azul-claro-principal">
      <CarteiraDetailsHeader
        name={cliente.nome}
        email={cliente.email}
        phone={cliente.telefone}
        cpf={cliente.cpf}
        birthDate={cliente.nascimento}
      />

      {/* Additional Info */}
      <View className="p-6 space-y-4">
        <View className="flex-row items-center space-x-2">
          <PiggyBank size={24} color="#001E62" weight="regular" />
          <Text className="text-azul-escuro-principal text-lg">
            {cliente.perfilInvestidor}
          </Text>
        </View>

        <View className="flex-row items-center space-x-2">
          <Wallet size={24} color="#001E62" weight="regular" />
          <Text className="text-azul-escuro-principal text-lg">
            R${" "}
            {cliente.quantiaInvestida.toLocaleString("pt-BR", {
              minimumFractionDigits: 2,
            })}
          </Text>
        </View>

        <Text className="text-azul-escuro-principal text-lg">
          {cliente.bancoInvestido}
        </Text>

        <Text className="text-azul-escuro-principal text-lg">
          {cliente.quantidadeAtivos}
        </Text>

        <View className="flex-row items-center space-x-2">
          <CheckCircle
            size={24}
            color={cliente.status === "Regular" ? "#4CAF50" : "#FF0000"}
            weight="regular"
          />
          <Text
            className={
              cliente.status === "Regular" ? "text-[#4CAF50]" : "text-[#FF0000]"
            }
          >
            {cliente.status}
          </Text>
        </View>
      </View>
    </View>
  );
}
