document.addEventListener('DOMContentLoaded', function () {
    const menu = document.getElementById('menu');
    const toggleButton = document.getElementById('toggle-menu');
    const toggleButtonIcon = document.getElementById('toggle-menu-icon');
    const toggleButtonName = document.getElementById('toggle-menu-name');
    const logoutBtn = document.getElementsByClassName('logout');

    toggleButton.addEventListener('click', function () {
        //open
        if (menu.style.right === '-100vw' || menu.style.right === '') {
            menu.style.right = '0';
            menu.style.left = '0';
            document.body.style.overflow = 'hidden';
            toggleButtonIcon.src = '../assets/icon/mobile/close-menu.svg';
            toggleButtonName.innerText = 'Đóng';
        
        //close
        } else {
            menu.style.right = '-100vw';
            menu.style.left = '';
            document.body.style.overflow = 'auto';
            toggleButtonIcon.src = '../assets/icon/mobile/menu.svg';
            toggleButtonName.innerText = 'Menu';
        }
    });

    logoutBtn.addEventListener('click', function () {        
            menu.style.right = '-100vw';
            menu.style.left = '';
            document.body.style.overflow = 'auto';
            toggleButtonIcon.src = '../assets/icon/mobile/menu.svg';
            toggleButtonName.innerText = 'Menu';
    });
});