import { View, TextInput, Text } from "react-native";
import { Envelope } from "phosphor-react-native";

interface EmailInputProps {
  value: string;
  onChangeText: (text: string) => void;
  placeholder?: string;
  label?: string;
}

export const EmailInput: React.FC<EmailInputProps> = ({
  value,
  onChangeText,
  placeholder = "Digite seu e-mail",
  label = "E-mail",
}) => {
  return (
    <View className="mb-5">
      <Text
        className="text-base font-semibold mb-1 ml-4"
        style={{ color: "#001E62" }}
      >
        {label}
      </Text>
      <View className="flex-row items-center bg-[#AEC3F5] rounded-xl px-4 py-4 w-[348px] h-[49px]">
        <View className="mr-3">
          <Envelope size={18} color="#001E6280" />
        </View>
        <TextInput
          className="flex-1 text-base p-0"
          style={{ color: "#001E62" }}
          value={value}
          onChangeText={onChangeText}
          placeholder={placeholder}
          placeholderTextColor="#001E6280"
          keyboardType="email-address"
          autoCapitalize="none"
          autoCorrect={false}
        />
      </View>
    </View>
  );
};
export default EmailInput;
