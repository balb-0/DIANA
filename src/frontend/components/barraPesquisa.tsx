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
  placeholder = "Pesquisar por cli",
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
    <View className="flex-row items-center bg-white rounded-2xl px-4 py-3 flex-1 shadow-sm">
      {/* Ícone de pesquisa */}
      <TouchableOpacity onPress={handleSearch} className="mr-3">
        <MagnifyingGlass size={20} color="#9CA3AF" weight="regular" />
      </TouchableOpacity>

      {/* Input de texto */}
      <TextInput
        className="flex-1 text-gray-700 text-base"
        placeholder={placeholder}
        placeholderTextColor="#9CA3AF"
        value={value !== undefined ? value : searchText}
        onChangeText={handleTextChange}
        onSubmitEditing={handleSearch}
        returnKeyType="search"
      />
    </View>
  );
}