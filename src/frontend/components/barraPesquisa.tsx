import React, { useState } from 'react';
import { View, TextInput, TouchableOpacity } from 'react-native';
import { MagnifyingGlass } from 'phosphor-react-native';

interface BarraPesquisaProps {
  placeholder?: string;
  onSearch?: (text: string) => void;
  value?: string;
  onChangeText?: (text: string) => void;
}

export default function BarraPesquisa({
  placeholder = "Pesquisar por cliente",
  onSearch,
  value,
  onChangeText,
}: BarraPesquisaProps) {
  const [searchText, setSearchText] = useState(value || '');

  const handleTextChange = (text: string) => {
    setSearchText(text);
    onChangeText?.(text);
  };

  const handleSearch = () => {
    onSearch?.(searchText);
  };

  return (
    <View className="flex-row items-center bg-azul-claro-principal shadow-clicaveis rounded-2xl px-4 flex-1 h-fit">
      <TouchableOpacity onPress={handleSearch} className="mr-3">
        <MagnifyingGlass size={20} color="#001E62" weight="regular" />
      </TouchableOpacity>
      <TextInput
        className="flex-1 text-gray-700 text-base"
        placeholder={placeholder}
        placeholderTextColor="#001E62"
        value={value !== undefined ? value : searchText}
        onChangeText={handleTextChange}
        onSubmitEditing={handleSearch}
        returnKeyType="search"
      />
    </View>
  );
}