const arrow = document.getElementById('side-bar-arrow');
const sideBarItems = document.querySelectorAll('.border-side-bar-item');
var sideBar = document.getElementById('side-bar');

arrow.addEventListener('click', () => {
    if(sideBarItems[0].style.display !== 'none') {
        sideBarItems.forEach(el => el.style.display = 'none');
        arrow.firstElementChild.src = '../assets/icon/viewAllGames/arrow-down.svg';
        
    } else {
        sideBarItems.forEach(el => el.style.display = 'block');
        arrow.firstElementChild.src = '../assets/icon/viewAllGames/arrow-up.svg';
    }
})