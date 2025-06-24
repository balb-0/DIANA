import React, { useEffect, useState } from "react";
import { View, Text, ActivityIndicator, ScrollView } from "react-native";
import {
  PiggyBank,
  Money,
  Info,
  Handshake,
  WarningCircle,
} from "phosphor-react-native";
import { perfilService, PerfilClienteResponse } from "@/services/perfilService";

export const CarteiraDetailsMain: React.FC = () => {
  const [perfilData, setPerfilData] = useState<PerfilClienteResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  // fixo para agora → clienteId = "27"
  const clienteId = "27.0";

  const loadPerfil = async () => {
    setLoading(true);
    setError("");
    const data = await perfilService.getPerfil(clienteId);

    if (data) {
      setPerfilData(data);
    } else {
      setError("Perfil não encontrado ou erro na API");
      setPerfilData(null);
    }

    setLoading(false);
  };

  useEffect(() => {
    loadPerfil();
  }, []);

  if (loading) {
    return (
      <View className="flex-1 justify-center items-center">
        <ActivityIndicator size="large" color="#001E62" />
      </View>
    );
  }

  if (error.length > 0) {
    return (
      <View className="flex-1 justify-center items-center p-4">
        <Text className="text-red-500 text-center">{error}</Text>
      </View>
    );
  }

  if (!perfilData) {
    return null;
  }

  // Agora renderizamos o conteúdo normal
  const irregularAmount = 0;  // se você tiver lógica futura, pode colocar aqui
  const irregularPercentage = 0;
  const recommendedProfile = perfilData.perfil;  // ou lógica para sugerir outro

  return (
    <ScrollView className="flex-1 p-4 bg-azul-claro-principal">

      {/* Profile and Status Row */}
      <View className="flex-row justify-between mb-4">
        {/* Profile Badge */}
        <View className="bg-azul-escuro-principal rounded-full px-3 py-1 flex-row items-center gap-1">
          <PiggyBank size={16} color="#AEC3F5" weight="fill" />
          <Text className="text-azul-claro-principal text-sm font-bold">
            {perfilData.perfil}
          </Text>
        </View>

        {/* Irregular Status */}
        <View className="bg-[#5A1A1A] rounded-full px-3 py-1 flex-row items-center gap-1">
          <Text className="text-[#E9A5A5] text-sm font-bold">
            {irregularPercentage}% Irregular
          </Text>
          <WarningCircle size={16} color="#E9A5A5" weight="regular" />
        </View>
      </View>

      {/* Main Card */}
      <View className="bg-azul-escuro-principal rounded-[12px] p-6">
        {/* Recommended Profile Section */}
        <View className="flex-row items-center mb-4 mx-auto">
          <Info size={14} color="#AEC3F5" weight="regular" />
          <Text className="text-azul-claro-principal text-xs ml-2 font-bold">
            Perfil indicado para readequação:
          </Text>
          <View className="bg-azul-claro-principal rounded-full px-2 py-[2px] ml-2 flex-row items-center gap-1">
            <Handshake size={14} color="#001E62" weight="fill" />
            <Text className="text-azul-escuro-principal text-xs font-bold">
              {recommendedProfile}
            </Text>
          </View>
        </View>

        {/* Amount Card */}
        <View className="bg-azul-claro-principal rounded-[6px] p-4 flex flex-row items-center justify-around">
          <Money size={30} color="#001E62" weight="regular" />
          <View className="flex flex-col items-center gap-2">
            <View className="flex-row items-center mb-2">
              <Text className="text-azul-escuro-principal text-2xl">
                <Text className="text-[16px] font-bold">R$</Text>{" "}
                {perfilData.valorTotal.toLocaleString("pt-BR", {
                  minimumFractionDigits: 2,
                })}
              </Text>
              <Text className="text-azul-escuro-principal text-base ml-2">
                investidos
              </Text>
            </View>
            {/* Irregular Amount */}
            <View className="bg-[#5A1A1A] rounded-lg px-4 py-2">
              <Text className="text-[#E9A5A5] text-base">
                <Text className="text-xs font-bold">R$</Text>{" "}
                {irregularAmount.toLocaleString("pt-BR", {
                  minimumFractionDigits: 2,
                })}{" "}
                <Text className="text-xs">irregular</Text>
              </Text>
            </View>
          </View>
        </View>
      </View>
    </ScrollView>
  );
};

export default CarteiraDetailsMain;
