// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
document.querySelector("#search-button").addEventListener('click', function () {
    document.querySelector("#search-link").href = "/home/home?q=" + document.querySelector("#navbar-search-field").value;
});