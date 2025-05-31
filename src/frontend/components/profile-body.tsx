import { ClockCounterClockwise } from "phosphor-react-native";
import { View, Text, ScrollView } from "react-native";
import { ProfileBodyItem } from "./profile-body-item";

interface ProfileBodyProps {
  children?: React.ReactNode;
}

export function ProfileBody({ children }: ProfileBodyProps) {
  return (
    <View
      style={{ backgroundColor: "#001E62" }}
      className="flex-1 rounded-[16px] p-[16px] mx-4"
    >
      <View className="flex-row items-center gap-2 mb-6">
        <ClockCounterClockwise size={24} color="#AEC3F5" weight="regular" />
        <Text className="text-[#AEC3F5] text-[18px]">
          Histórico de carteiras acessadas
        </Text>
      </View>

      <ScrollView
        showsVerticalScrollIndicator={false}
        contentContainerStyle={{ gap: 16 }}
      >
        <ProfileBodyItem
          name="Fulano da Silva Soares de S. Martins"
          email="fulanodesilva@gmail.com"
          cpf="111.111.111-11"
          type="Regular"
          assets={14}
          value={12000.0}
          bank="BTG Pactual"
        />
        <ProfileBodyItem
          name="Fulano da Silva Soares de S. Martins"
          email="fulanodesilva@gmail.com"
          cpf="111.111.111-11"
          type="Irregular"
          assets={14}
          value={12000.0}
          bank="BTG Pactual"
        />
        <ProfileBodyItem
          name="Fulano da Silva Soares de S. Martins"
          email="fulanodesilva@gmail.com"
          cpf="111.111.111-11"
          type="Irregular"
          assets={14}
          value={12000.0}
          bank="BTG Pactual"
        />
      </ScrollView>
    </View>
  );
}
