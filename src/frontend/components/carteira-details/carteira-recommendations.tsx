import React from "react";
import { View, Text, ScrollView } from "react-native";
import {
  PiggyBank,
  CurrencyCircleDollar,
  ArrowUpRight,
  TrendUp,
} from "phosphor-react-native";

const recommendations = [
  {
    type: "Títulos Internacionais",
    name: "Investimento Recomendado para Perfil Moderado",
    expectedReturn: 12.5,
    minValue: 1000,
    profile: "Moderado",
    bank: "BTG Pactual",
    deadline: "14/03/2025",
  },
  {
    type: "Títulos Internacionais",
    name: "Investimento Recomendado para Perfil Moderado",
    expectedReturn: 12.5,
    minValue: 1000,
    profile: "Moderado",
    bank: "BTG Pactual",
    deadline: "14/03/2025",
  },
  {
    type: "Títulos Internacionais",
    name: "Investimento Recomendado para Perfil Moderado",
    expectedReturn: 12.5,
    minValue: 1000,
    profile: "Moderado",
    bank: "BTG Pactual",
    deadline: "14/03/2025",
  },
];

function CarteiraRecommendations() {
  return (
    <View
      style={{ backgroundColor: "#001E62" }}
      className="rounded-[16px] p-[16px] mx-4 h-[353px] gap-[10px]"
    >
      {/* Header */}
      <View className="flex-row items-center justify-between">
        <Text className="text-[#AEC3F5] text-sm font-semibold">
          Recomendações BTG Pactual
        </Text>
        <View className="bg-[#AEC3F5] rounded-full px-3 py-1 flex-row gap-1 items-center">
          <Text className="text-[#001E62] text-base font-bold">
            {recommendations.length}
          </Text>
          <Text className="text-[#001E62] text-base">opções</Text>
        </View>
      </View>

      {/* Scrollable Recommendation Cards */}
      <ScrollView
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{ gap: 24, paddingBottom: 8 }}
      >
        {recommendations.map((rec, idx) => (
          <View key={idx} className="bg-[#AEC3F5] rounded-2xl p-6 h-[111px]">
            {/* Top Row: Type and Profile */}
            <View className="flex-row items-center justify-between mb-2">
              <View className="flex-row items-center gap-2">
                <CurrencyCircleDollar
                  size={12}
                  color="#001E62"
                  weight="regular"
                />
                <Text className="text-[#001E62] text-[10px] font-medium">
                  {rec.type}
                </Text>
              </View>
              <View className="bg-[#001E62] rounded-full px-4 py-1 flex-row items-center gap-1">
                <PiggyBank size={12} color="#AEC3F5" weight="fill" />
                <Text className="text-[#AEC3F5] text-xs font-bold">
                  {rec.profile}
                </Text>
              </View>
            </View>

            {/* Investment Name */}
            <Text className="text-[#001E62] text-[10px] font-bold mb-2">
              {rec.name}
            </Text>

            {/* Expected Return and Min Value */}
            <View className="flex-row items-center justify-between mt-2">
              <View className="flex-row items-center gap-2">
                <TrendUp size={16} color="#001E62" weight="bold" />
                <Text className="text-[#001E62] text-base font-extrabold">
                  {rec.expectedReturn}% a.a.
                </Text>
              </View>
              <View className="flex-row items-center gap-2">
                <ArrowUpRight size={12} color="#001E62" weight="bold" />
                <Text className="text-[#001E62] text-xs">
                  Min. R${rec.minValue.toLocaleString("pt-BR")}
                </Text>
              </View>
            </View>
          </View>
        ))}
      </ScrollView>
    </View>
  );
}

export default CarteiraRecommendations;
