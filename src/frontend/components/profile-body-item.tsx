import { View, Text } from "react-native";
import {
  Envelope,
  IdentificationCard,
  Wallet,
  PiggyBank,
  CheckCircle,
  WarningCircle,
} from "phosphor-react-native";

interface ProfileBodyItemProps {
  name: string;
  email: string;
  cpf: string;
  status: string;
  assets: number;
  value: number;
  bank: string;
  id: string | number;
}

export function ProfileBodyItem({
  name,
  email,
  cpf,
  status,
  assets,
  value,
  bank,
  id,
}: ProfileBodyItemProps) {
  return (
    <View className="w-full bg-[#AEC3F5] rounded-2xl p-[16px]">
      <View className="flex-row items-center gap-3">
        <View className="w-12 h-12 bg-[#001E62] rounded-full items-center justify-center">
          <Wallet size={24} color="#AEC3F5" weight="regular" />
        </View>

        <View className="flex-1">
          <Text className="text-[#001E62] text-[14px] font-semibold">
            {name}
          </Text>
          <View className="flex-row items-center gap-2 mt-1">
            <View className="bg-[#001E62] rounded-full px-3 py-1 flex-row items-center gap-1">
              <PiggyBank size={16} color="#AEC3F5" weight="fill" />
              <Text className="text-[#AEC3F5] text-xs font-bold">
                Conservador
              </Text>
            </View>
            <Text className="text-[#001E62] text-sm">{bank}</Text>
          </View>
        </View>
      </View>

      <View className="flex-row justify-between mt-4 ">
        <View className="bg-[#001E6229] rounded-[4px] flex-row text-center items-center gap-1 px-[12px] py-[2px]">
          <Envelope size={12} color="#001E62" weight="regular" />
          <Text className="text-[#001E62] text-[12px]">{email}</Text>
        </View>

        <View className=" bg-[#001E6229] rounded-[4px] flex-row text-center items-center gap-1 px-[12px] py-[2px]">
          <IdentificationCard size={12} color="#001E62" weight="regular" />
          <Text className="text-[#001E62] text-[12px]">{cpf}</Text>
        </View>
      </View>

      <View className="flex-row items-center justify-between mt-4">
        <View className="flex-row items-center gap-2">
          <View
            className={`px-3 py-1 rounded-full ${
              status === "Regular" ? "bg-azul-escuro-principal" : "bg-vermelho-principal"
            } flex-row items-center gap-1`}
          >
            <Text className={`text-sm ${status === "Regular" ? "text-azul-claro-principal" : "text-vermelho-secundario"
            }`}>{status}</Text>
            {status !== "Regular" ? (
              <WarningCircle size={14} weight="bold" color="#E9A5A5" />
            ) : (
              <CheckCircle size={14} weight="bold" color="#AEC3F5" />
            )}
          </View>
          <Text className="text-[#001E62] text-base">
            <Text className="font-bold">{assets}</Text>
            <Text className="text-sm"> Ativos</Text>
          </Text>
        </View>
        <Text className="text-[#001E62] text-xl font-bold">
          R${value.toLocaleString("pt-BR", { minimumFractionDigits: 2 })}
        </Text>
      </View>
    </View>
  );
}
