import React from 'react';
import { View } from 'react-native';
import BarraPesquisa from './barraPesquisa';
import BotaoFiltro from './botaoFiltro';

interface BarraPesquisaProps {
  placeholder?: string;
  onSearch?: (text: string) => void;
  value?: string;
  onChangeText?: (text: string) => void;
}

interface BotaoFiltroProps {
  onPress?: () => void;
  iconSize?: number;
  iconColor?: string;
}

interface PesquisaContainerProps {
  BarraPesquisaProps?: BarraPesquisaProps;
  BotaoFiltroProps?: BotaoFiltroProps;
}

export default function PesquisaContainer({ 
  BarraPesquisaProps = {}, 
  BotaoFiltroProps = {} 
}: PesquisaContainerProps) {
  return (
    <View className="flex-row items-center mx-4 mb-4">
      <BarraPesquisa {...BarraPesquisaProps} />
        <BotaoFiltro {...BotaoFiltroProps} />
    </View>
  );
}