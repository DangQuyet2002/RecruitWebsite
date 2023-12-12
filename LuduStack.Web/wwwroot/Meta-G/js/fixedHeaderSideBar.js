window.onscroll = function() {fixed()};
//sidebar
var sideBar = document.getElementById("side-bar");
//header
var navbar = document.getElementById("navbar");
var header = document.getElementById("header");
var sticky = navbar.offsetTop;

function fixed() {
  if (window.scrollY > sticky) {
    navbar.classList.add("sticky");
    if(window.innerWidth > 420) {
      sideBar.classList.add("sticky-side-bar");
    }
    header.style.paddingTop = '92px';
  } else {
    navbar.classList.remove("sticky");
    if(window.innerWidth > 420) {
      sideBar.classList.remove("sticky-side-bar");
    }
    header.style.paddingTop = '0';
  }
}