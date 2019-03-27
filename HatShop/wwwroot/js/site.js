// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
console.log("Page Loaded");

fetch("/cart/count")
    .then(response => response.json()
        .then(jsonData =>
            document.querySelector(".cart-count").innerText = jsonData
    )
    );