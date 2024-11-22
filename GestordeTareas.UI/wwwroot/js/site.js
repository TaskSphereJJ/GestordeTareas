//dark model
const darkModeToggle = document.querySelector('#darkModeToggle');

darkModeToggle.addEventListener('click', () => {
    document.body.classList.toggle('dark');
});


//funcionalidades del SIDEBAR Y HEADER 
document.addEventListener('DOMContentLoaded', function () {
    const menuButton = document.querySelector('[aria-label="Menu"]');
    const closeButton = document.querySelector('[data-drawer-hide="drawer-navigation"]');
    const sidebar = document.getElementById('drawer-navigation');

    menuButton.addEventListener('click', () => {
        sidebar.style.transform = 'translateX(0)';
    });

    closeButton.addEventListener('click', () => {
        sidebar.style.transform = 'translateX(-100%)';
    });

    document.addEventListener('click', (event) => {
        if (!sidebar.contains(event.target) && !menuButton.contains(event.target)) {
            sidebar.style.transform = 'translateX(-100%)';
        }
    });

    // Ocultar sidebar al hacer clic en un enlace dentro del sidebar
    const sidebarLinks = document.querySelectorAll('#drawer-navigation a');
    sidebarLinks.forEach(link => {
        link.addEventListener('click', () => {
            sidebar.style.transform = 'translateX(-100%)';
        });
    });
});


// Para los selectores del perfil y header 
document.addEventListener('DOMContentLoaded', function () {
    const userMenuButton = document.getElementById('userMenuButton');
    const userMenuOptions = document.getElementById('userMenuOptions');

    userMenuButton.addEventListener('click', function () {
        userMenuOptions.style.display = userMenuOptions.style.display === 'block' ? 'none' : 'block';
    });

    userMenuOptions.addEventListener('click', function (event) {
        if (event.target.tagName === 'A') {
            userMenuOptions.style.display = 'none';
            event.stopPropagation(); // Evita que el evento se propague al contenedor y vuelva a activar el menú
        }
    });
    document.addEventListener('click', function (event) {
        if (!userMenuButton.contains(event.target) && !userMenuOptions.contains(event.target)) {
            userMenuOptions.style.display = 'none';
        }
    });
});
