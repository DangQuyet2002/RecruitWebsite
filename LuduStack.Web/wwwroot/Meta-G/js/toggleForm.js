const overlay = document.getElementById('overlay');
const loginModel = document.getElementById('login-form');
const closeBtn = document.querySelectorAll('.close-btn-form');
const loginRef = document.querySelectorAll('.login-ref');

const registerRef = document.getElementById('register-ref');
const registerModel = document.getElementById('register-form');

// Show the login model
function showLoginModel() {
    overlay.style.display = 'block';
    loginModel.style.display = 'block';
    registerModel.style.display = 'none';
    document.body.style.overflow = 'hidden';
}

// Hide the login model
function hideModel() {
    overlay.style.display = 'none';
    loginModel.style.display = 'none';
    registerModel.style.display = 'none';
    document.body.style.overflow = 'auto';
}

function showRegisterModel() {
    registerModel.style.display = 'block';
    loginModel.style.display = 'none';
}

// Event listener for showing the login model
document.addEventListener('DOMContentLoaded', () => {
    showLoginModel();
});

loginRef.forEach((el) =>
    el.addEventListener('click', () => {
        showLoginModel();
    }),
);

registerRef.addEventListener('click', () => {
    showRegisterModel();
});

// Event listener for closing the login model
closeBtn.forEach((el) =>
    el.addEventListener('click', () => {
        hideModel();
    }),
);

// Event listener for clicking on the overlay to close the model
overlay.addEventListener('click', () => {
    hideModel();
});

// Prevent the model from closing when clicking inside the model
loginModel.addEventListener('click', (e) => {
    e.stopPropagation();
});
registerModel.addEventListener('click', (e) => {
    e.stopPropagation();
});
