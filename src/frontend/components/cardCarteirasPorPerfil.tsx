import { View, Text, TouchableOpacity } from "react-native";
import {
  Handshake,
  PiggyBank,
  HandCoins,
  Wallet,
  ArrowSquareOut,
} from "phosphor-react-native";

interface CardCarteirasPerfilProps {
  perfil: "Moderado" | "Conservador" | "Sofisticado" | "Todas";
  quantidade: number;
  total: number;
  onPress?: () => void;
}

const iconMap = {
  Moderado: <Handshake size={48} weight="light" color="#001E62" />,
  Conservador: <PiggyBank size={48} weight="light" color="#001E62" />,
  Sofisticado: <HandCoins size={48} weight="light" color="#001E62" />,
  Todas: <Wallet size={48} weight="light" color="#001E62" />,
};

export default function CardCarteirasPerfil({
  perfil,
  quantidade,
  total,
  onPress,
}: CardCarteirasPerfilProps) {
  return (
    <TouchableOpacity onPress={onPress} className="rounded-lg w-[45%] aspect-[174/220] p-4 bg-azul-claro-principal shadow-clicaveis justify-between">
      <View className="flex-row justify-between items-center">
        <Text className="bg-azul-escuro-principal text-azul-claro-principal px-3 py-1 text-sm font-bold rounded-full">
          {perfil}
        </Text>
        <ArrowSquareOut size={14} weight="bold" color="#001E624D" />
      </View>
      <View className="flex items-center justify-center flex-1 opacity-30">
        {iconMap[perfil]}
      </View>
      <View className="mt-2 items-center justify-center">
        <Text className="text-3xl text-azul-escuro-principal font-bold leading-none">
          {quantidade}
          <Text className="text-[10px] font-light"> de </Text>
          <Text className="font-bold text-xl">{total}</Text>
        </Text>
        <Text className="text-[10px] text-azul-escuro-principal font-light pt-1 text-center">
          carteiras em
          <Text className="font-bold"> conformidade.</Text>
        </Text>
      </View>
    </TouchableOpacity>
  );
}
