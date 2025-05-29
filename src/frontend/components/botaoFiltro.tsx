import React from 'react';
import { TouchableOpacity } from 'react-native';
import { Sliders } from 'phosphor-react-native';

interface BotaoFiltroProps {
  onPress?: () => void;
  iconSize?: number;
  iconColor?: string;
}

export default function BotaoFiltro({ 
  onPress,
  iconSize = 20,
  iconColor = "#001E62"
}: BotaoFiltroProps) {
  return (
    <TouchableOpacity 
      onPress={onPress}
      className="bg-azul-claro-principal rounded-2xl p-3 shadow-sm"
    >
      <Sliders size={iconSize} color={iconColor} weight="regular" />
    </TouchableOpacity>
  );
}