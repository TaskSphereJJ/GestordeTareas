/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        darkPurple: '#0A002C',
        lightPurple: '#ffff',
        lightText: '#280133',
        hoverBG: '#e070ff',
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
