﻿// Start Ready
$(document).ready(function () {

    // Canvas Click Focus
    $('').click(function () {
        $('input.form-control').focus();
    });

    // Search on Keyup
    $("#textbox").keyup(function () {
        search();
    });


    // Live Search
    // On Search Submit and Get Results
    function search() {
        var input = $("input[name='searchterm']").val();
        $.ajax({
            type: "POST",
            url: "http://a2info344.cloudapp.net/myWebService.asmx/searchTrie",
            data: JSON.stringify({ queryValue: input }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var data = JSON.parse(msg.d);
                var outer = $("<div></div>");
                $("#jsonDiv").html(outer);
                for (var i = 0; i < data.length; i++) {
                    var div = $("<div>").text(data[i]);
                    outer.append(div);
                }
            },
            error: function (msg) {

            }
        });
    }
    
    /*$('body').on("keyup", "input[name='searchterm']", function (e) {
        // Set Timeout
        clearTimeout($.data(this, 'timer'));

        // Set Search String
        var search_string = $(this).val();

        // Do Search
        if (search_string == '') {
            $("h1#texttosayresults").fadeOut();
            $("div#results").fadeOut();
        } else {
            $("h1#texttosayresults").fadeIn();
            $("div#results").fadeIn();
            $(this).data('timer', setTimeout(search, 100));
        };
    }) */
});