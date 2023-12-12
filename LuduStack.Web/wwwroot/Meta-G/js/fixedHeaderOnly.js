window.onscroll = function() {fixedHeaderOnly()};
//header
var navbar = document.getElementById("navbar");
var header = document.getElementById("header");
var sticky = navbar.offsetTop;

function fixedHeaderOnly() {
  if (window.scrollY > sticky) {
    navbar.classList.add("sticky");
    header.style.paddingTop = '92px';
  } else {
    navbar.classList.remove("sticky");
    header.style.paddingTop = '0';
  }
}