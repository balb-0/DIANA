import React from "react";
import { View, Text } from "react-native";
import {
  PiggyBank,
  Money,
  Info,
  Handshake,
  WarningCircle,
} from "phosphor-react-native";

interface CarteiraDetailsMainProps {
  totalAmount: number;
  irregularAmount: number;
  irregularPercentage: number;
  currentProfile: "Conservador" | "Moderado" | "Sofisticado";
  recommendedProfile: "Conservador" | "Moderado" | "Sofisticado";
}

export const CarteiraDetailsMain: React.FC<CarteiraDetailsMainProps> = ({
  totalAmount = 20460.57,
  irregularAmount = 3215.21,
  irregularPercentage = 30,
  currentProfile = "Conservador",
  recommendedProfile = "Moderado",
}) => {
  return (
    <View className="px-4 -mt-16">
      {/* Profile and Status Row */}
      <View className="flex-row justify-between mb-4">
        {/* Profile Badge */}
        <View className="bg-azul-escuro-principal rounded-full px-3 py-1 flex-row items-center gap-1">
          <PiggyBank size={16} color="#AEC3F5" weight="fill" />
          <Text className="text-azul-claro-principal text-sm font-bold">
            {currentProfile}
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
                {totalAmount.toLocaleString("pt-BR", {
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
    </View>
  );
};

export default CarteiraDetailsMain;
