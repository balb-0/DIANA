import { ScrollView, View } from "react-native";
import { useLocalSearchParams } from "expo-router";
import { CarteiraDetailsHeader } from "@/components/carteira-details/carteira-details-header";
import { useClientes } from "../contexts/ClientesProvider";
import { useEffect, useState } from "react";
import { CarteiraDetailsBody } from "@/components/carteira-details/carteira-details-body";
import { InvestmentToggle } from "@/components/investment-toggle";
import { CarteiraDetailsMain } from "@/components/carteira-details/carteira-details-main";
import CarteiraRecommendations from "@/components/carteira-details/carteira-recommendations";

export default function InvestorDetails() {
  const { investor } = useLocalSearchParams();
  const { clientes } = useClientes();
  const [cliente, setCliente] = useState<any>(null);
  const [activeTab, setActiveTab] = useState<"investments" | "recommendations">(
    "investments"
  );

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
      <ScrollView
        className="flex-1"
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{ paddingBottom: 96 }}
      >
        <CarteiraDetailsHeader
          name={cliente.nome}
          email={cliente.email}
          phone={cliente.telefone}
          cpf={cliente.cpf}
          birthDate={cliente.nascimento}
        />
        <CarteiraDetailsMain
          totalAmount={20460.57}
          irregularAmount={3215.21}
          irregularPercentage={30}
          currentProfile="Conservador"
          recommendedProfile="Moderado"
        />
        <InvestmentToggle activeTab={activeTab} onToggle={setActiveTab} />
        {activeTab === "investments" ? (
          <CarteiraDetailsBody />
        ) : (
          <CarteiraRecommendations />
        )}
      </ScrollView>
    </View>
  );
}
