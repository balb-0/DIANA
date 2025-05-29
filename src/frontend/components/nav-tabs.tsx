import { useState, useRef, useMemo } from "react";
import { View, TouchableOpacity, Animated } from "react-native";
import { House, Wallet, User } from "phosphor-react-native";

interface BottomNavigationProps {
  onTabChange?: (tab: string) => void;
  initialTab?: string;
}

export const BottomNavigation: React.FC<BottomNavigationProps> = ({
  onTabChange,
  initialTab = "home",
}) => {
  const [activeTab, setActiveTab] = useState<string>(initialTab);
  const scaleAnimation = useRef(new Animated.Value(1)).current;

  const tabs = useMemo(
    () => [
      { id: "home", icon: House },
      { id: "folder", icon: Wallet },
      { id: "profile", icon: User },
    ],
    []
  );

  // Navigation calculations
  const navWidth = 380;
  const tabSize = 56;

  const handleTabPress = (tabId: string) => {
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

      setActiveTab(tabId);
      onTabChange?.(tabId);
    }
  };

  return (
    <View className="fixed bottom-0 w-full items-center rounded-none">
      <View className="relative" style={{ width: navWidth }}>
        {/* Icons row - positioned above the navigation bar */}
        <View
          className="flex-row absolute w-full"
          style={{
            top: -24,
            zIndex: 1000,
            justifyContent: "space-between",
            paddingHorizontal: 60,
          }}
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
                  <View className="bg-[#001E62] rounded-full p-[4.5px]">
                    <View
                      className={`rounded-full p-4 ${
                        isActive ? "bg-[#AEC3F5]" : "bg-[#001E62]"
                      }`}
                    >
                      <IconComponent
                        size={24}
                        color={isActive ? "#001E62" : "#AEC3F5"}
                      />
                    </View>
                  </View>
                </Animated.View>
              </TouchableOpacity>
            );
          })}
        </View>

        {/* Navigation bar background */}
        <View
          className="bg-[#001E62] overflow-hidden relative"
          style={{
            width: navWidth,
            height: 60,
            borderTopLeftRadius: 40,
            borderTopRightRadius: 40,
          }}
        />
      </View>
    </View>
  );
};

export default BottomNavigation;
