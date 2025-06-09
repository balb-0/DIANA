module.exports = {
  preset: 'jest-expo',
  setupFiles: ['<rootDir>/jest-setup.js'],
  setupFilesAfterEnv: ['<rootDir>/jest-matchers.js'],
  testMatch: ['**/*.test.tsx'],
  moduleNameMapper: {
    '\\.(png|jpg|jpeg|gif|svg)$': 'identity-obj-proxy',
  },
  transformIgnorePatterns: [
    'node_modules/(?!(react-native|@react-native|react-native-.*|expo|expo-.*|@expo|@expo/.*|nativewind|@testing-library/react-native)/)',
  ],
};
