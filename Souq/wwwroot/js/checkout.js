$(document).ready(function () {

    $(".addressItem label").on("click", function () {
        var country = $(this).attr("data-country");
        var state = $(this).attr("data-state");
        var city = $(this).attr("data-city");
        var street = $(this).attr("data-street");
        var moreAboutAddress = $(this).attr("data-moreAboutAddress");

        $("#NewAddress_Country").val(country);
        $("#NewAddress_State").val(state);
        $("#NewAddress_City").val(city);
        $("#NewAddress_Street").val(street);
        $("#NewAddress_MoreAboutAddress").val(moreAboutAddress);

        $(".adress").find("label").removeClass("active");
        $(this).addClass("active");

        $(".adress").find(".addressItem").removeClass("active");
        $(this).parents(".addressItem").addClass("active");
    });

    $(".newAddressForm input").on("keydown", function () {

        $(".adress .addressItem label").removeClass("active");
        $(".adress .addressItem input").prop("checked", false);


        $(".adress").find(".addressItem").removeClass("active");


        $(".adress .addressItem.new").addClass("active");
        $(".adress .addressItem label.new").addClass("active");

        $(".adress .addressItem input.new").prop("checked", true);

        console.log("Fsdfsd");
    });

});