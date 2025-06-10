import React from "react";
import { useClientes } from "@/contexts/ClientesProvider";
import { ClockCounterClockwise } from "phosphor-react-native";
import { View, Text, ScrollView, TouchableOpacity } from "react-native";
import { ProfileBodyItem } from "./profile-body-item";
import { RelativePathString, router } from "expo-router";

export function ProfileBody() {
  const { clientes } = useClientes();

  const navegarParaCliente = (id: number) => {
    router.push(`/investor/${id}` as RelativePathString);
  };

  return (
    <View
      className="flex-1 rounded-2xl p-4 mx-4 bg-azul-escuro-principal"
    >
      <View className="flex-row items-center gap-1 mb-6">
        <ClockCounterClockwise size={17} color="#AEC3F5" weight="regular" />
        <Text className="text-azul-claro-principal text-sm">
          Histórico de carteiras acessadas
        </Text>
      </View>

      <ScrollView
        className="flex-1"
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{ gap: 16 }}
      >
        {clientes.map((cliente) => (
          <TouchableOpacity
            key={cliente.id}
            onPress={() => navegarParaCliente(cliente.id)}
          >
            <ProfileBodyItem
              investorProfile={cliente.perfilInvestidor}
              id={cliente.id}
              name={cliente.nome}
              email={cliente.email}
              cpf={cliente.cpf}
              status={cliente.status}
              assets={cliente.quantidadeAtivos}
              value={cliente.quantiaInvestida}
              bank={cliente.bancoInvestido}
            />
          </TouchableOpacity>
        ))}
      </ScrollView>
    </View>
  );
}
