/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class',
  theme: {
    extend: {
      colors: {
        dark: '#0A002C',
        purple: {
          700: '#6b46c1',
        },
      },
    },
  },
  variants: {
    extend: {
      backgroundColor: ['dark'],
    },
  },
  plugins: [],
};
