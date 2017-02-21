// Start Ready
$(document).ready(function () {

    DISPLAYSTATUS();

    function STARTCRAWLING() {
        $.ajax({
            type: "POST",
            url: "admin.asmx/startCrawling",
            data: "{url: 'http://www.cnn.com/robots.txt'}",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                console.log("Successfully started crawling.");
            },
            error: function (msg) {
                console.log("Error when starting to crawl.");
            }
        });
    }

    function STOPCRAWLING() {
        $.ajax({
            type: "POST",
            url: "admin.asmx/stopCrawling",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                console.log("Successfully stopped crawling.");
            },
            error: function (msg) {
                console.log("Error when stopping the crawl.");
            }
        });
    }

    function CLEARINDEX() {
        $.ajax({
            type: "POST",
            url: "admin.asmx/clearIndex",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                console.log("Successfully cleared index.");
            },
            error: function (msg) {
                console.log("Error when clearing index.");
            }
        });
    }

    function URLTOPAGE() {
        $("#formValidate").validate({
            rules: {
                uname: {
                    required: true,
                    minlength: 10
                }
            }
        });

        var input = $("input[name='searchterm']").val();

        $.ajax({
            type: "POST",
            url: "admin.asmx/retrieveTitle",
            data: JSON.stringify({ URL: input }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                console.log("Successfully found title.");

                var result = "The Page's Title is ";
                $("#titleSearchResult").html("<br>" + result + "<b>" + msg.d + "</b>");
            },
            error: function (msg) {
                console.log("Error finding title.");
            }
        });
    }

    function DISPLAYSTATUS() {
        $.ajax({
            type: "POST",
            url: "admin.asmx/Report",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                console.log("ajaxGetStatus success");
                console.log(msg.d);

                //Status
                $("#status").append('<span>' + msg.d[0] + '</span>');

                //CPU
                $("#cpu").append('<span>' + msg.d[1] + '</span>');

                //RAM
                $("#ram").append('<span>' + msg.d[2] + '</span>');

                //TOTAL
                $("#total").append('<span>' + msg.d[3] + '</span>');

                //Queue Size
                $("#qSize").append('<span>' + msg.d[4] + '</span>');

                //Index Size
                $("#iSize").append('<span>' + msg.d[5] + '</span>');

                //last Ten
                var recentUrls = msg.d[6].split(" ");
                if (recentUrls.length != 1) {
                    for (var i = 0; i < 10; i++) {
                        $("#lastten").append('<li class="collection-item">' + recentUrls[i] + '</li>');
                    }
                }

                //Error messages
                var errorsMes = msg.d[7].split("||");
                for (var i = 0; i < errorsMes.length; i++) {
                    $("#errorMes").append('<li class="collection-item">' + errorsMes[i] + '</li>');
                }

                //Error urls
                var errorsUrls = msg.d[8].split(" ");
                for (var i = 0; i < errorsUrls.length; i++) {
                    $("#errorUrl").append('<li class="collection-item">' + errorsUrls[i] + '</li>');
                }

            },
            error: function (msg) {
                console.log("ajaxGetStatus error");
            }
        });
    }
});


//$(document).ready(ajaxGetStatus());