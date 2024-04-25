$(document).ready(function () {
    $(".addToCartBtn").on("click", function () {
        var addToCartBtn = $(this);
        var idOfProduct = addToCartBtn.attr("data-productId");
        var quantityOfItem = 1;

        if ($("#cartItemQuantity").length > 0) {
            quantityOfItem = $("#cartItemQuantity").val();
        } else {
            if (addToCartBtn.attr("data-action") === "add") {
                quantityOfItem = parseInt(addToCartBtn.siblings(".quantity").attr("data-currentQuantity")) + 1;
            } else {
                quantityOfItem = parseInt(addToCartBtn.siblings(".quantity").attr("data-currentQuantity")) - 1;
            }
        }

        if (quantityOfItem != 0) {

            // add to cart
            $.ajax({
                url: "/api/ShoppingCart/",
                type: 'post',
                contentType: "application/json",
                data:
                JSON.stringify({
                    productId: idOfProduct,
                    quantity: quantityOfItem
                })
                ,
                success: function (item) {
                    // notification
                    // bootbox.alert("Product added successfully to your Cart");
                    $("#notificationTone")[0].play();

                    // Get count of items On Navbar
                    var currentCount = parseInt($("#cartItemsNumbers").text());

                    if ($(`.cart-items .shopping-list .item[data-ItemId="${item.id}"]`).length > 0) {
                        currentCount = currentCount - 1;
                        $(".cart-items .shopping-list").find(`.item[data-ItemId="${item.id}"]`).fadeOut("fast").remove();
                    }

                    // add item to cart
                    $(".cartItemsCount").text(currentCount + 1);

                    $(".cart-items .shopping-list").append(
                        `<li class="item" data-ItemId="${item.id}">
                        <a href="javascript:void(0)" class="remove removeFromCartBtn" data-itemId="${item.Id}"
                            title="Remove this item">
                            <i class="lni lni-close"></i>
                        </a>
                        <div class="cart-img-head">
                            <a class="cart-img" href="/products/details/${item.productId}">
                                <img src="${item.product.pictureUrl}" alt="product image">
                            </a>
                        </div>

                        <div class="content">
                            <h4>
                                <a href="/products/details/${item.productId}">
                                    ${item.product.name}
                                </a>
                            </h4>
                            <p class="quantity">
                                ${item.quantity} x
                                <b> - </b>
                                <span class="amount">$${item.product.price}</span>
                            </p>
                        </div>
                    </li>`
                    )

                    $(".cartMsg").fadeOut("fast").remove();

                    // change the color and disable the btn
                    // addToCartBtn.addClass("disabled").attr("disabled", "true");

                    addToCartBtn.siblings(".quantity")
                        .text(item.quantity)
                        .attr("data-currentQuantity", item.quantity)

                },
                error: function (xhr) {
                    if (xhr.status != 500) {
                        bootbox.alert(xhr.responseText);
                    }
                }
            });
        }

    });

    // Not Working with new Elements
    $(".removeFromCartBtn").on("click", function () {
        // remove from my cart
        var removeFromCartBtn = $(this);
        var itemId = removeFromCartBtn.attr("data-itemId");

        bootbox.confirm("are You Sure you want to remove this item from your Cart", function (result) {
            if (result) {
                $.ajax({
                    url: `/api/ShoppingCart/${itemId}`,
                    contentType: "application/json",
                    type: 'delete',
                    success: function (result) {
                        // notification
                        // bootbox.alert(result);
                        $("#notificationTone")[0].play();

                        // remove from cart targeted item
                        removeFromCartBtn.parents(".item").fadeOut("fast").remove();
                        removeFromCartBtn.parents(".cartItem").fadeOut("fast").remove();

                        // remove from navbar cart
                        $(".cart-items .shopping-list")
                            .find(`.item[data-ItemId="${itemId}"]`)
                            .fadeOut("fast").remove();

                        // change count of items On Navbar
                        var currentCount = parseInt($("#cartItemsNumbers").text());
                        $(".cartItemsCount").text(currentCount - 1);
                    },
                    error: function (xhr) {
                        if (xhr.status != 500) {
                            bootbox.alert(xhr.responseText);
                        }
                    }
                });

            }
        });
    });

});