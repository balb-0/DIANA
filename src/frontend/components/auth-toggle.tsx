import { useState, useRef, useEffect } from "react";
import { View, TouchableOpacity, Animated } from "react-native";

interface AuthToggleProps {
  onToggle?: (mode: "login" | "register") => void;
  initialMode?: "login" | "register";
}

export const AuthToggle: React.FC<AuthToggleProps> = ({
  onToggle,
  initialMode = "login",
}) => {
  const [activeMode, setActiveMode] = useState<"login" | "register">(
    initialMode
  );
  const slideAnimation = useRef(
    new Animated.Value(initialMode === "login" ? 4 : 132)
  ).current;
  const scaleAnimation = useRef(new Animated.Value(1)).current;

  // Toggle width - adjust based on your design needs
  const toggleWidth = 256;
  const padding = 4;
  const buttonWidth = (toggleWidth - padding * 2) / 2;

  useEffect(() => {
    setActiveMode(initialMode);
    const toValue = initialMode === "login" ? padding : buttonWidth + padding;

    Animated.parallel([
      Animated.timing(slideAnimation, {
        toValue,
        duration: 200,
        useNativeDriver: true,
      }),
      Animated.sequence([
        Animated.timing(scaleAnimation, {
          toValue: 0.98,
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
  }, [initialMode]);

  const handleToggle = (mode: "login" | "register") => {
    if (mode !== activeMode) {
      setActiveMode(mode);
      onToggle?.(mode);
    }
  };

  return (
    <View className="items-center">
      <View
        className="bg-[#001E62] overflow-hidden rounded-full justify-center"
        style={{ width: toggleWidth, height: 48 }}
      >
        {/* Animated sliding background */}
        <Animated.View
          className="absolute bg-[#AEC3F5] rounded-full"
          style={{
            width: buttonWidth,
            height: 40,
            transform: [
              { translateX: slideAnimation },
              { scale: scaleAnimation },
            ],
          }}
        />

        {/* Toggle buttons */}
        <View className="flex-row h-full">
          <TouchableOpacity
            className="flex-1 justify-center items-center"
            onPress={() => handleToggle("login")}
            activeOpacity={0.8}
          >
            <Animated.Text
              className={`text-lg font-semibold ${
                activeMode === "login" ? "text-[#001E62]" : "text-[#AEC3F5]"
              }`}
              style={{
                transform: [{ scale: scaleAnimation }],
              }}
            >
              Entrar
            </Animated.Text>
          </TouchableOpacity>

          <TouchableOpacity
            className="flex-1 justify-center items-center"
            onPress={() => handleToggle("register")}
            activeOpacity={0.8}
          >
            <Animated.Text
              className={`text-lg font-semibold ${
                activeMode === "register" ? "text-[#001E62]" : "text-[#AEC3F5]"
              }`}
              style={{
                transform: [{ scale: scaleAnimation }],
              }}
            >
              Cadastrar
            </Animated.Text>
          </TouchableOpacity>
        </View>
      </View>
    </View>
  );
};

export default AuthToggle;
