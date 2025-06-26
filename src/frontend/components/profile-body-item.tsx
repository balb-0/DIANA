import { View, Text } from "react-native";
import {
  Envelope,
  IdentificationCard,
  Wallet,
  PiggyBank,
  CheckCircle,
} from "phosphor-react-native";

interface ProfileBodyItemProps {
  id: string;
  name: string;
  email?: string;
  cpf?: string;
  type?: string;
  assets?: string;
  value: number;
  bank?: string;
}

export function ProfileBodyItem({
  id,
  name,
  email,
  cpf,
  type,
  assets,
  value,
  bank,
}: ProfileBodyItemProps) {
  return (
    <View className="w-full bg-[#AEC3F5] rounded-2xl p-[16px]">
      <View className="flex-row items-center gap-3">
        <View className="w-12 h-12 bg-[#001E62] rounded-full items-center justify-center">
          <Wallet size={24} color="#AEC3F5" weight="regular" />
        </View>

        <View className="flex-1 gap-2">
          <View className="gap-1">
            <Text className="text-azul-escuro-principal text-[14px] font-semibold">
              {name}
            </Text>
            <View className="bg-[#001E6229] rounded-1 flex-row text-center items-center gap-1 px-3 py-[2px] w-fit rounded-full">
              <Envelope size={12} color="#001E62" weight="regular" />
              <Text className="text-azul-escuro-principal text-[12px]">{email}</Text>
            </View>
          </View>
          <View className="flex-row items-center gap-2 rounded-full px-3 py-[2px] bg-[#001E6229] w-fit">
            <Text className="font-bold text-[12px] text-azul-escuro-principal">ID</Text>
            <Text className="text-[12px] text-azul-escuro-principal">{id}</Text>
          </View>
        </View>
      </View>
    </View>
  );
}
