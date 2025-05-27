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
  Moderado: <Handshake size={56} weight="thin" color="#001E62" />,
  Conservador: <PiggyBank size={56} weight="thin" color="#001E62" />,
  Sofisticado: <HandCoins size={56} weight="thin" color="#001E62" />,
  Todas: <Wallet size={56} weight="thin" color="#001E62" />,
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
        <Text className="bg-azul-escuro-principal text-white px-3 py-1 text-sm font-bold rounded-full">
          {perfil}
        </Text>
        <ArrowSquareOut size={16} weight="regular" color="#001E62" />
      </View>

      {/* Ícone central */}
      <View className="flex items-center justify-center flex-1 opacity-30">
        {iconMap[perfil]}
      </View>

      {/* Rodapé com info de carteiras */}
      <View className="mt-2">
        <Text className="text-3xl text-azul-escuro-principal font-bold leading-none">
          {quantidade}
          <Text className="text-base font-normal"> de {total}</Text>
        </Text>
        <Text className="text-sm text-azul-escuro-principal">
          carteiras em <Text className="font-bold">conformidade.</Text>
        </Text>
      </View>
    </View>
  );
}
