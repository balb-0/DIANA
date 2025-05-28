import { View, Text } from "react-native";
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
}

/* Propriedades de cada ícone */
const iconMap = {
  Moderado: <Handshake size={48} weight="thin" color="#001E62" />,
  Conservador: <PiggyBank size={48} weight="thin" color="#001E62" />,
  Sofisticado: <HandCoins size={48} weight="thin" color="#001E62" />,
  Todas: <Wallet size={48} weight="thin" color="#001E62" />,
};

export default function CardCarteirasPerfil({
  perfil,
  quantidade,
  total,
}: CardCarteirasPerfilProps) {
  return (
    <View className="rounded-lg w-[174px] h-[220px] p-4 bg-azul-claro-principal shadow-clicaveis justify-between">
      {/* Topo com tag e ícone de ação */}
      <View className="flex-row justify-between items-center">
        <Text className="bg-azul-escuro-principal text-azul-claro-principal px-3 py-1 text-sm font-bold rounded-full">
          {perfil}
        </Text>
        <ArrowSquareOut size={14} weight="regular" color="#001E624D" />
      </View>

      {/* Ícone central */}
      <View className="flex items-center justify-center flex-1 opacity-30">
        {iconMap[perfil]}
      </View>

      {/* Rodapé com info de carteiras */}
      <View className="mt-2 items-center justify-center">
        <Text className="text-3xl text-azul-escuro-principal font-bold leading-none">
          {quantidade}
          <Text className="text-[10px] font-light"> de </Text>
          <Text className="font-bold text-xl">{total}</Text>
        </Text>
        <Text className="text-[10px] text-azul-escuro-principal font-light">
          carteiras em
          <Text className="font-bold"> conformidade.</Text>
        </Text>
      </View>
    </View>
  );
}
