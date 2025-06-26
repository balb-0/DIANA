import React, { useEffect, useState } from "react";
import { View, Text, ActivityIndicator, ScrollView } from "react-native";
import {
  PiggyBank,
  Money,
  Info,
  Handshake,
  WarningCircle,
  CheckCircle,
} from "phosphor-react-native";
import { perfilService, PerfilClienteResponse } from "@/services/perfilService";
import { clientService, PerfilDeRiscoResponse } from "@/services/clientService";

interface CarteiraDetailsMainProps {
  clienteId: string;
}

export const CarteiraDetailsMain: React.FC<CarteiraDetailsMainProps> = ({
  clienteId,
}) => {
  const [perfilData, setPerfilData] = useState<PerfilClienteResponse | null>(
    null
  );
  const [perfilDeclaradoData, setperfilDeclaradoData] =
    useState<PerfilDeRiscoResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

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

  const loadPerfilDeclarado = async () => {
    setLoading(true);
    setError("");
    const data = await clientService.getPerfilDeRisco(clienteId);

    if (data) {
      setperfilDeclaradoData(data);
    } else {
      setError("Perfil não encontrado ou erro na API");
      setperfilDeclaradoData(null);
    }

    setLoading(false);
  };

  useEffect(() => {
    loadPerfil();
    loadPerfilDeclarado();
  }, [clienteId]);

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

  if (!perfilDeclaradoData) {
    return null;
  }

  const irregularAmount = 0;
  const irregularPercentage = 0;
  const recommendedProfile = perfilData.perfil;
  const perfilRisco = perfilDeclaradoData.perfil_de_risco;

  return (
    <ScrollView className="flex-1 p-4 bg-azul-claro-principal">
      <View className="flex-row justify-between mb-4">
        <View className="bg-azul-escuro-principal rounded-full px-3 py-1 flex-row items-center gap-1">
          <PiggyBank size={16} color="#AEC3F5" weight="fill" />
          <Text className="text-azul-claro-principal text-sm font-bold">
            {perfilRisco}
          </Text>
        </View>

        <View
          id="status"
          className={`${
            perfilRisco !== recommendedProfile ? "bg-[#5A1A1A]" : "bg-[#001E62]"
          } rounded-full px-3 py-1 flex-row items-center gap-1`}
        >
          <Text
            className={`${
              perfilRisco !== recommendedProfile
                ? "text-[#E9A5A5]"
                : "text-[#AEC3F5]"
            } text-sm font-bold`}
          >
            {perfilRisco !== recommendedProfile ? "Irregular" : "Regular"}
          </Text>
          {perfilRisco !== recommendedProfile ? (
            <WarningCircle size={16} color="#E9A5A5" weight="regular" />
          ) : (
            <CheckCircle size={16} color="#AEC3F5" weight="regular" />
          )}
        </View>
      </View>

      <View className="bg-azul-escuro-principal rounded-[12px] p-6">
        {perfilRisco !== recommendedProfile && (
          <View className="flex-row items-center mb-4 mx-auto">
            <Info size={14} color="#AEC3F5" weight="regular" />
            <Text className="text-azul-claro-principal text-xs ml-2 font-bold">
              Perfil indicado para readequação:
            </Text>
            <View
              id="recomendação"
              className="bg-azul-claro-principal rounded-full px-2 py-[2px] ml-2 flex-row items-center gap-1"
            >
              <Handshake size={14} color="#001E62" weight="fill" />
              <Text className="text-azul-escuro-principal text-xs font-bold">
                {recommendedProfile}
              </Text>
            </View>
          </View>
        )}

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
          </View>
        </View>
      </View>
    </ScrollView>
  );
};

export default CarteiraDetailsMain;
