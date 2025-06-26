import { useRef, useEffect } from "react";
import { View, TouchableOpacity, Animated, Text } from "react-native";

interface InvestmentToggleProps {
  activeTab: "investments" | "recommendations";
  onToggle: (tab: "investments" | "recommendations") => void;
}

export const InvestmentToggle: React.FC<InvestmentToggleProps> = ({
  activeTab,
  onToggle,
}) => {
  // Use the same sizes as auth-toggle
  const toggleWidth = 343;
  const toggleHeight = 48;
  const padding = 4;
  const buttonWidth = (toggleWidth - padding * 2) / 2;

  const slideAnimation = useRef(
    new Animated.Value(
      activeTab === "investments" ? padding : buttonWidth + padding
    )
  ).current;
  const scaleAnimation = useRef(new Animated.Value(1)).current;

  useEffect(() => {
    const toValue =
      activeTab === "investments" ? padding : buttonWidth + padding;
    Animated.timing(slideAnimation, {
      toValue,
      duration: 200,
      useNativeDriver: true,
    }).start();
  }, [activeTab]);

  const handleToggle = (tab: "investments" | "recommendations") => {
    onToggle(tab);
    const toValue = tab === "investments" ? padding : buttonWidth + padding;
    Animated.parallel([
      Animated.timing(slideAnimation, {
        toValue,
        duration: 200,
        useNativeDriver: true,
      }),
      Animated.sequence([
        Animated.timing(scaleAnimation, {
          toValue: 0.95,
          duration: 100,
          useNativeDriver: true,
        }),
        Animated.timing(scaleAnimation, {
          toValue: 1,
          duration: 100,
          useNativeDriver: true,
        }),
      ]),
    ]).start();
  };

  return (
    <View className="items-center w-full my-4">
      <View
        className="overflow-hidden rounded-full justify-center"
        style={{
          width: toggleWidth,
          height: toggleHeight,
          backgroundColor: "#001E62",
        }}
      >
        {/* Animated bubble */}
        <Animated.View
          style={{
            position: "absolute",
            backgroundColor: "#AEC3F5",
            borderRadius: 9999,
            width: buttonWidth,
            height: toggleHeight - 8,
            top: 4,
            left: 0,
            transform: [
              { translateX: slideAnimation },
              { scale: scaleAnimation },
            ],
          }}
        />
        {/* Buttons */}
        <View className="flex-row h-full">
          <TouchableOpacity
            className="flex-1 justify-center items-center"
            onPress={() => handleToggle("investments")}
            activeOpacity={0.8}
          >
            <Text
              style={{
                fontSize: 16,
                fontWeight: "600",
                color: activeTab === "investments" ? "#001E62" : "#AEC3F5",
              }}
            >
              Investimentos
            </Text>
          </TouchableOpacity>
          <TouchableOpacity
            className="flex-1 justify-center items-center"
            onPress={() => handleToggle("recommendations")}
            activeOpacity={0.8}
          >
            <Text
              style={{
                fontSize: 16,
                fontWeight: "600",
                color: activeTab === "recommendations" ? "#001E62" : "#AEC3F5",
              }}
            >
              Recomendações
            </Text>
          </TouchableOpacity>
        </View>
      </View>
    </View>
  );
};

export default InvestmentToggle;
