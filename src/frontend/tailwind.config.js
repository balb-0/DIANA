/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./app/**/*.{js,jsx,ts,tsx}", "./components/**/*.{js,jsx,ts,tsx}"],
  presets: [require("nativewind/preset")],
  theme: {
    extend: {
      colors: {
        "azul-escuro-principal": "#001E62",
        "azul-claro-principal": "#AEC3F5",
        "azul-claro-secundario": "#C2D5FF",
        "rosa-secundario": "#FF7CA1",
        "vermelho-secundario": "#E9A5A5",
        "vermelho-principal": "#5A1A1A"
      },
      boxShadow: {
        'clicaveis': '0 4px 8px rgba(0, 0, 0, 0.3)',
      },      
    },
  },
  plugins: [],
}