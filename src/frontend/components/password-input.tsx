import { useState } from "react";
import { View, TextInput, Text, TouchableOpacity } from "react-native";
import { Lock, Eye, EyeSlash } from "phosphor-react-native";

interface PasswordInputProps {
  value: string;
  onChangeText: (text: string) => void;
  placeholder?: string;
  label?: string;
}

export const PasswordInput: React.FC<PasswordInputProps> = ({
  value,
  onChangeText,
  placeholder = "Digite sua senha",
  label = "Senha",
}) => {
  const [isSecure, setIsSecure] = useState(true);

  return (
    <View className="mb-5">
      <Text
        className="text-base font-semibold mb-2 ml-4"
        style={{ color: "#001E62" }}
      >
        {label}
      </Text>
      <View className="flex-row items-center bg-[#AEC3F5] rounded-xl px-4 py-4 w-[348px] h-[49px]">
        <View className="mr-3">
          <Lock size={18} color="#001E62" />
        </View>
        <TextInput
          className="flex-1 text-base p-0"
          style={{ color: "#001E62" }}
          value={value}
          onChangeText={onChangeText}
          placeholder={placeholder}
          placeholderTextColor="#001E62"
          secureTextEntry={isSecure}
          autoCapitalize="none"
          autoCorrect={false}
        />
        <TouchableOpacity
          className="p-1"
          onPress={() => setIsSecure(!isSecure)}
        >
          {isSecure ? (
            <Eye size={18} color="#001E62" />
          ) : (
            <EyeSlash size={18} color="#001E62" />
          )}
        </TouchableOpacity>
      </View>
    </View>
  );
};

export default PasswordInput;
