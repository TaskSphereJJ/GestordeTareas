/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: 'class', // Habilita el modo oscuro con la clase 'dark'
  content: [
    './Views/**/*.cshtml', // Vistas Razor
    './wwwroot/**/*.html',  // Archivos HTML
    './wwwroot/**/*.js',    // Archivos JS
  ],
  theme: {
    extend: {
      colors: {
        lightPurple: '#fffff',
        darkPurple: '#1f242a',
        lightText: '#333',
        darkText: '#f3f3f3',
        borderColor: '#6b7280',
      },
    },
  },
  plugins: [],
};
