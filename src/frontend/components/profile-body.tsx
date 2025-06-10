import React from "react";
import { useClientes } from "@/contexts/ClientesProvider";
import { ClockCounterClockwise } from "phosphor-react-native";
import { View, Text, ScrollView } from "react-native";
import { ProfileBodyItem } from "./profile-body-item";

interface ProfileBodyProps {
  children?: React.ReactNode;
}

export function ProfileBody({ children }: ProfileBodyProps) {
  const { clientes } = useClientes();

  return (
    <View
      style={{ backgroundColor: "#001E62" }}
      className="flex-1 rounded-[16px] p-[16px] mx-4"
    >
      <View className="flex-row items-center gap-2 mb-6">
        <ClockCounterClockwise size={24} color="#AEC3F5" weight="regular" />
        <Text className="text-[#AEC3F5] text-[18px]">
          Histórico de carteiras acessadas
        </Text>
      </View>

      <ScrollView
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{ gap: 16 }}
      >
        {clientes.map((cliente) => (
          <ProfileBodyItem
            key={cliente.id}
            id={cliente.id}
            name={cliente.nome}
            email={cliente.email}
            cpf={cliente.cpf}
            status={cliente.status}
            assets={cliente.quantidadeAtivos}
            value={cliente.quantiaInvestida}
            bank={cliente.bancoInvestido}
          />
        ))}
      </ScrollView>
    </View>
  );
}
