import { ProfileHeader } from "@/components/profile-header";
import { View } from "react-native";
import { ProfileBody } from "@/components/profile-body";

export default function Profile() {
  return (
    <View className="h-full bg-azul-claro-secundario">
      <View className="flex-1 space-y-2">
        <ProfileHeader />
        <View className="flex-1 -mt-10 max-h-[480px]">
          <ProfileBody />
        </View>
      </View>
    </View>
  );
}
