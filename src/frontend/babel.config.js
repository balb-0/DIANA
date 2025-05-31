module.exports = function (api) {
  api.cache(true);
  return {
    presets: [
      ["babel-preset-expo", { jsxImportSource: "nativewind" }],
      ...(process.env.NODE_ENV === 'test' ? [
        ['@babel/preset-env', { targets: { node: 'current' } }],
        '@babel/preset-typescript'
      ] : [])
    ],
    plugins: [],
  };
};
