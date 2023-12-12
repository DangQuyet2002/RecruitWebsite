var sideBarItem = document.querySelectorAll('.border-side-bar-item');
var extMenu = document.querySelector('.extended-scrollable-menu-container');
var sideBar = document.querySelector('#side-bar');
var overLay = document.getElementById('overlay-extended-menu');
var btnClose = document.getElementById('button-close-scrollable-menu');

sideBarItem.forEach((el) =>
    el.addEventListener('click', () => {
        console.log(window.innerWidth);
        if (extMenu.style.display === 'flex') {
            // extMenu.style.display = 'none';
            // sideBar.style.clipPath =
            //     'polygon(0 0,calc(100% - 8px) 0,100% 8px,100% calc(100% - 8px),calc(100% - 8px) 100%,0 100%)';     
            // sideBar.style.height = '300px';
            // overLay.style.display = 'none';
            //console.log('cc');

        } else {
            overLay.style.display = 'block';
            extMenu.style.display = 'flex';
            sideBar.style.clipPath = 'none';
            sideBar.style.height = 'auto';
            if(window.innerWidth < 421) {
                sideBar.style.bottom = '56px';
                sideBar.style.top = '10%';
            }
        }
    }),
);

overLay.addEventListener('click', () => {
    extMenu.style.display = 'none';
    sideBar.style.clipPath =
        'polygon(0 0,calc(100% - 8px) 0,100% 8px,100% calc(100% - 8px),calc(100% - 8px) 100%,0 100%)';
    sideBar.style.height = '300px';
    overLay.style.display = 'none';
    if(window.innerWidth < 421) {
        sideBar.style.bottom = 'unset';
        sideBar.style.top = 'unset';
    }
});

btnClose.addEventListener('click', () => {
    extMenu.style.display = 'none';
    sideBar.style.clipPath =
        'polygon(0 0,calc(100% - 8px) 0,100% 8px,100% calc(100% - 8px),calc(100% - 8px) 100%,0 100%)';
    sideBar.style.height = '300px';
    overLay.style.display = 'none';
    if(window.innerWidth < 421) {
        sideBar.style.bottom = 'unset';
        sideBar.style.top = 'unset';
    }
});