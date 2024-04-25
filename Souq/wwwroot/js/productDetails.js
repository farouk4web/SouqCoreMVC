
// product images selector
const current = document.getElementById("current");
const opacity = 0.6;
const imgs = document.querySelectorAll(".img");
imgs.forEach(img => {
    img.addEventListener("click", (e) => {
        //reset opacity
        imgs.forEach(img => {
            img.style.opacity = 1;
        });
        current.src = e.target.src;
        //adding class
        //current.classList.add("fade-in");
        //opacity
        e.target.style.opacity = opacity;
    });
});



$(document).ready(function () {
    $("#deleteProductBtn").on("click", function () {
        bootbox.confirm("are you sure you want to delete this product for ever you cant go back?", function (result) {
            if (result) {
                $("#deleteProductForm").submit();
            }
        });
    });
});