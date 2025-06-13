import React from "react";
import { View, Text, ScrollView } from "react-native";
import {
  PiggyBank,
  CalendarBlank,
  CurrencyCircleDollar,
} from "phosphor-react-native";

const investments = [
  {
    type: "Títulos Internacionais",
    name: "Investimento da Silva te Deixa Rico Soares",
    value: 5000,
    date: "14/03/2024",
    profile: "Conservador",
    bank: "BTG Pactual",
  },
  {
    type: "Títulos Internacionais",
    name: "Investimento da Silva te Deixa Rico Soares",
    value: 5000,
    date: "14/03/2024",
    profile: "Conservador",
    bank: "BTG Pactual",
  },
  {
    type: "Títulos Internacionais",
    name: "Investimento da Silva te Deixa Rico Soares",
    value: 5000,
    date: "14/03/2024",
    profile: "Conservador",
    bank: "BTG Pactual",
  },
];

export function CarteiraDetailsBody() {
  return (
    <View
      style={{ backgroundColor: "#001E62" }}
      className="rounded-[16px] p-[16px] mx-4 h-[353px] gap-[10px]"
    >
      {/* Header */}
      <View className="flex-row items-center justify-between">
        <Text className="text-[#AEC3F5] text-sm font-semibold">
          BTG Pactual
        </Text>
        <View className="bg-[#AEC3F5] rounded-full px-3 py-1 flex-row gap-1 items-center">
          <Text className="text-[#001E62] text-base font-bold">
            {investments.length}
          </Text>
          <Text className="text-[#001E62] text-base">ativos</Text>
        </View>
      </View>

      {/* Scrollable Investment Cards */}
      <ScrollView
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{ gap: 24, paddingBottom: 8 }}
      >
        {investments.map((inv, idx) => (
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
                  {inv.type}
                </Text>
              </View>
              <View className="bg-[#001E62] rounded-full px-4 py-1 flex-row items-center gap-1">
                <PiggyBank size={12} color="#AEC3F5" weight="fill" />
                <Text className="text-[#AEC3F5] text-xs font-bold">
                  {inv.profile}
                </Text>
              </View>
            </View>

            {/* Investment Name */}
            <Text className="text-[#001E62] text-[10px] font-bold mb-2">
              {inv.name}
            </Text>

            {/* Value and Date */}
            <View className="flex-row items-center justify-between mt-2">
              <Text className="text-[#001E62] text-base font-extrabold">
                R$
                {inv.value.toLocaleString("pt-BR", {
                  minimumFractionDigits: 2,
                })}
              </Text>
              <View className="flex-row items-center gap-2">
                <CalendarBlank size={12} color="#001E62" weight="regular" />
                <Text className="text-[#001E62] text-xs">{inv.date}</Text>
              </View>
            </View>
          </View>
        ))}
      </ScrollView>
    </View>
  );
}

export default CarteiraDetailsBody;
