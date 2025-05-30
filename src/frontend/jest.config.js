module.exports = {
  preset: 'react-native',
  setupFiles: ['<rootDir>/jest-setup.js'], // <-- Use setupFiles aqui
  setupFilesAfterEnv: ['<rootDir>/jest-matchers.js'], // <-- Opcional, pode remover ou deixar vazio
  testMatch: ['**/*.test.tsx'],
  moduleNameMapper: {
    '\\.(png|jpg|jpeg|gif|svg)$': 'identity-obj-proxy'
  },
  transformIgnorePatterns: [
    'node_modules/(?!(react-native|@react-native|react-native-.*|expo|expo-.*|@expo|@expo/.*|nativewind|@testing-library/react-native)/)'
  ]
};
