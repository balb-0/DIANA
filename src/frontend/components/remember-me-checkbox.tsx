import { TouchableOpacity, Text } from "react-native";
import { Square, CheckSquare } from "phosphor-react-native";

interface RememberMeCheckboxProps {
  checked: boolean;
  onToggle: () => void;
}

export const RememberMeCheckbox: React.FC<RememberMeCheckboxProps> = ({
  checked,
  onToggle,
}) => {
  return (
    <TouchableOpacity
      onPress={onToggle}
      className="flex-row items-center"
      activeOpacity={0.7}
    >
      {checked ? (
        <CheckSquare size={24} color="#001E62" />
      ) : (
        <Square size={24} color="#001E62" />
      )}
      <Text className="ml-2 text-[#001E62] text-base">Lembrar de mim</Text>
    </TouchableOpacity>
  );
};

export default RememberMeCheckbox;
