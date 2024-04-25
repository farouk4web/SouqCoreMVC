$(document).ready(function () {

    $(".wishlistBtn").on("click", function () {
        var wishlistBtn = $(this);
        var idOfProduct = wishlistBtn.attr("data-productId");
        var action = wishlistBtn.attr("data-action");
        if (action == "add") {
            // add to wishlist
            $.ajax({
                url: "/api/wishlist/",
                type: 'post',
                contentType: "application/json",
                data:
                    JSON.stringify({
                        productId: idOfProduct
                    })
                ,
                success: function (result) {
                    // bootbox.alert(result);
                    $("#notificationTone")[0].play();

                    // change the Action and the icon
                    wishlistBtn.attr("data-action", "remove");
                    wishlistBtn.find("i").addClass("lni-heart-filled").removeClass("lni-heart");

                    // change count of items On Navbar
                    var currentCount = parseInt($(".navbar-cart .wishlist .total-items").text());
                    $(".navbar-cart .wishlist .total-items").text(currentCount + 1);
                },
                error: function (xhr) {
                    if (xhr.status != 500) {
                        bootbox.alert(xhr.responseText);
                    }
                }
            });
        } else {
            // remove from my wishlist
            $.ajax({
                url: `/api/wishlist/${idOfProduct}`,
                contentType: "application/json",
                type: 'delete',
                success: function (result) {
                    // bootbox.alert(result);
                    $("#notificationTone")[0].play();

                    // change the Action and the icon
                    wishlistBtn.attr("data-action", "add");
                    wishlistBtn.find("i").addClass("lni-heart").removeClass("lni-heart-filled");

                    // change count of items On Navbar
                    var currentCount = parseInt($(".navbar-cart .wishlist .total-items").text());
                    $(".navbar-cart .wishlist .total-items").text(currentCount - 1);
                },
                error: function (xhr) {
                    if (xhr.status != 500) {
                        bootbox.alert(xhr.responseText);
                    }
                }
            });

            //bootbox.confirm("are You Sure you want to remove this item from your wishlist", function (result) {
            //    if (result) {
                 
            //    }
            //});
        }

    });

});