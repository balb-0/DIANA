import { usePathname, useRouter, RelativePathString } from "expo-router";
import { View, TouchableOpacity, Animated } from "react-native";
import { useMemo, useRef } from "react";
import { House, Wallet, User } from "phosphor-react-native";

type TabId = "" | "wallet" | "profile";

export const BottomNavigation = () => {
  const pathname = usePathname();
  const router = useRouter();
  const scaleAnimation = useRef(new Animated.Value(1)).current;

  const tabs = useMemo(
    () => [
      { id: "", icon: House },
      { id: "wallet", icon: Wallet },
      { id: "profile", icon: User },
    ] as const,
    []
  );

  const navWidth = 380;
  const tabSize = 56;

  const activeTab: TabId = (pathname.replace("/", "") || "") as TabId;

  const handleTabPress = (tabId: TabId) => {
    if (tabId !== activeTab) {
      Animated.sequence([
        Animated.timing(scaleAnimation, {
          toValue: 0.95,
          duration: 125,
          useNativeDriver: true,
        }),
        Animated.timing(scaleAnimation, {
          toValue: 1,
          duration: 125,
          useNativeDriver: true,
        }),
      ]).start();
      router.push(`/${tabId}` as RelativePathString);
    }
  };

  return (
    <View className="absolute bottom-0 w-full items-center">
      <View className="relative" style={{ width: navWidth }}>
        <View
          className="flex-row absolute w-full z-50 justify-between px-20 top-[-10px]"
        >
          {tabs.map((tab) => {
            const IconComponent = tab.icon;
            const isActive = activeTab === tab.id;

            return (
              <TouchableOpacity
                key={tab.id}
                className="justify-center items-center"
                style={{ width: tabSize, height: tabSize }}
                onPress={() => handleTabPress(tab.id)}
                activeOpacity={0.8}
              >
                <Animated.View
                  style={{
                    transform: [{ scale: isActive ? scaleAnimation : 1 }],
                  }}
                >
                  <View className="bg-[#001E62] rounded-full p-2">
                    <View
                      className={`rounded-full p-2 ${
                        isActive ? "bg-[#AEC3F5]" : "bg-[#001E62]"
                      }`}
                    >
                      <IconComponent
                        size={isActive ? 32 : 22}
                        color={isActive ? "#001E62" : "#AEC3F5"}
                        weight={isActive ? "fill" : "regular"}
                      />
                    </View>
                  </View>
                </Animated.View>
              </TouchableOpacity>
            );
          })}
        </View>
        <View
          className="bg-azul-escuro-principal overflow-hidden relative"
          style={{
            width: navWidth,
            height: 60,
            borderTopLeftRadius: 46,
            borderTopRightRadius: 46,
          }}
        />
      </View>
    </View>
  );
};

export default BottomNavigation;
