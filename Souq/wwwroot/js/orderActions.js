$(document).ready(function () {

    // canceling Order
    $("#js_remove_order").on("click", function () {

        bootbox.confirm("Are you shur you want to Cancel this Order?", function (result) {
            if (result) {
                $("#cancel_order_form").submit();
            }
        });

    });


    // order levels 
    $("#js_confirm_Order").on("click", function () {
        let badge = $(this);
        let orderId = badge.data("order_id");

        bootbox.confirm("Are you shur you want to Confirm this Order?", function (result) {
            if (result) {
                $.ajax({
                    url: `/api/Orders/ConfirmOrder/${orderId}`,
                    method: "post",
                    success: function () {
                        badge.addClass("btn-success").removeClass("btn-warning");
                    },
                    error: function (error) {
                        console.log("status error is: " + error.status);
                        bootbox.alert("Something went wrong, please try again later.");
                    }
                });
            }
        });
    });


    $("#js_shipping_Order").on("click", function () {
        let badge = $(this);
        let orderId = badge.data("order_id");

        bootbox.confirm("Are you shur you want to Shipping this Order?", function (result) {
            if (result) {
                $.ajax({
                    url: `/api/orders/ShippingOrder/${orderId}`,
                    method: "post",
                    success: function () {
                        badge.addClass("btn-success").removeClass("btn-warning");
                    },
                    error: function (error) {
                        console.log("status error is: " + error.status);
                        bootbox.alert("Something went wrong, please try again later.");
                    }
                });
            }
        });

    });

    $("#js_delivered_Order").on("click", function () {
        let badge = $(this);
        let orderId = badge.data("order_id");

        bootbox.confirm("Are you shur you want to delivery this Order?", function (result) {
            if (result) {
                $.ajax({
                    url: `/api/orders/DeliveredOrder/${orderId}`,
                    method: "post",
                    success: function () {
                        badge.addClass("btn-success").removeClass("btn-warning");
                    },
                    error: function (error) {
                        console.log("status error is: " + error.status);
                        bootbox.alert("Something went wrong, please try again later.");
                    }
                });
            }
        });

    });
});