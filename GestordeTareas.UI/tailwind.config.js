/** @type {import('tailwindcss').Config} */
module.exports = {
<<<<<<< HEAD
    darkMode: 'class', // Para habilitar el modo oscuro por clase
    content: [
        "./Views/**/*.cshtml", // Todas las vistas en el proyecto
        "./Pages/**/*.cshtml", // Si también usas Razor Pages
        "./wwwroot/**/*.js"    // Archivos JS en wwwroot
    ],
    theme: {
        extend: {
            colors: {
                darkPurple: '#1F232A',
                lightPurple: '#fef9ff',
                lightText: '#280133',
                darkText: "#deacff",
                hoverBG: '#e070ff',
                borderColor: '#6E41FA',
                darkMenu: "#0d0c13",
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
=======
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
>>>>>>> 64537c730ed97ace024cc4bda9a6c34490ba6bf9
