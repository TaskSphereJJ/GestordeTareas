/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        darkPurple: '#1F232A',
        lightPurple: '#fef9ff',
        lightText: '#280133',
        darkText: "#deacff",
        hoverBG: '#e070ff',
        borderColor:'#6E41FA',
        darkMenu: "#060214",
        lightMenu: "#fef9ff"
      },
    },
  },
  variants: {
    extend: {
      textColor: ['dark'],
      backgroundColor: ['dark'],
    },
  },
  plugins: [],
};