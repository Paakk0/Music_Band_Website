document.addEventListener("DOMContentLoaded", function () {
    const header = document.querySelector("header");
    const navbarToggler = document.querySelector("#navbar-toggler");

    navbarToggler.addEventListener("click", function () {
        header.classList.toggle('hidden');
        if (header.classList.contains('hidden')) {
            navbarToggler.textContent = "Show"
            navbarToggler.classList.add('nav-hidden');
        }
        else {
            navbarToggler.textContent = "Hide"
            navbarToggler.classList.remove('nav-hidden');
        }
    });
});
