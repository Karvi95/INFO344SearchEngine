// Start Ready
$(document).ready(function () {

    $('#report').click(function () {
        $.ajax({
            type: "POST",
            url: "admin.asmx/getStatus",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //console.log("ajaxGetStatus success");
                //console.log(msg.d);

                var jsStatus = JSON.parse(msg.d);

                //Status
                $("#status").text(jsStatus);
            },
            error: function (msg) {
                console.log(msg.d);
                console.log("ajaxGetStatus error");
            }
        });

        $.ajax({
            type: "POST",
            url: "admin.asmx/getPerformance",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //console.log("ajaxGetStatus success");
                //console.log(msg.d);

                var jsPref = JSON.parse(msg.d);

                //CPU
                $("#cpu").text(jsPref[0]);

                //RAM
                $("#ram").text(jsPref[1]);

            },
            error: function (msg) {
                console.log(msg.d);
                console.log("ajaxGetStatus error");
            }
        });

        $.ajax({
            type: "POST",
            url: "admin.asmx/getTotal",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //console.log("ajaxGetStatus success");
                //console.log(msg.d);

                var jsTotal = JSON.parse(msg.d);

                //TOTAL
                $("#total").text(jsTotal);

            },
            error: function (msg) {
                console.log(msg.d);
                console.log("ajaxGetStatus error");
            }
        });

        $.ajax({
            type: "POST",
            url: "admin.asmx/sizeQ",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //console.log("ajaxGetStatus success");
                //console.log(msg.d);

                var jsQSize = JSON.parse(msg.d);

                //Queue Size
                $("#qSize").text(jsQSize);

            },
            error: function (msg) {
                console.log(msg.d);
                console.log("ajaxGetStatus error");
            }
        });

        $.ajax({
            type: "POST",
            url: "admin.asmx/sizeI",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //console.log("ajaxGetStatus success");
                //console.log(msg.d);

                var jsISize = JSON.parse(msg.d);

                //Index Size
                $("#iSize").text(jsISize);

            },
            error: function (msg) {
                console.log(msg.d);
                console.log("ajaxGetStatus error");
            }
        });

        $.ajax({
            type: "POST",
            url: "admin.asmx/lastTen",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                //console.log("ajaxGetStatus success");
                //console.log("MESSAGE: " + msg.d);
                var jsLastTen = JSON.parse(msg.d);
                //console.log(jsLastTen[0]);
                var jsLastTenArray = jsLastTen[0].split(" ");
                //console.log("array: " + jsLastTenArray[0]);

                var string = "#lastten";
                //Populating List
                for (var i = 0; i < jsLastTenArray.length; i++) {
                    string = "#lastten" + i;
                    console.log(string);
                    $(string).text(jsLastTenArray[i]);
                }
            },
            error: function (msg) {
                console.log(msg.d);
                console.log("ajaxGetStatus error");
            }
        });

        $.ajax({
            type: "POST",
            url: "admin.asmx/getErrors",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var jsErrorMes = JSON.parse(msg.d);
                console.log(jsErrorMes[0]);
                var jsErrorMesArray = jsErrorMes[0].split("||");
                console.log("array: " + jsErrorMesArray[0]);

                var string = "#errorMes";
                //Populating Error Message
                for (var i = 0; i < jsErrorMesArray.length; i++) {
                    string = "#errorMes" + i;
                    console.log(string);
                    $(string).text(jsErrorMesArray[i]);
                }
                
                var jsErrorUrl = JSON.parse(msg.d);
                console.log(jsErrorUrl[0]);
                var jsErrorUrlArray = jsErrorUrl[1].split(" ");
                console.log("array: " + jsErrorUrlArray[1]);

                var string = "#errorUrl";
                //Populating Error Message
                for (var i = 0; i < jsErrorUrlArray.length; i++) {
                    string = "#errorUrl" + i;
                    console.log(string);
                    $(string).text(jsErrorUrlArray[i]);
                }
            },
            error: function (msg) {
                console.log(msg.d);
                console.log("ajaxGetStatus error");
            }
        });

        //DISPLAYSTATUS();
        //DISPLAYPREF();
        //DISPLAYTOTAL();
        //DISPLAYQUEUESIZE();
        //DISPLAYINDEXSIZE();
        //DISPLAYLASTTEN();
        //DISPLAYERRORS();
    })

    $('#start').click(function () {
        $.ajax({
            type: "POST",
            url: "admin.asmx/startCrawling",
            contentType: "application/json; charset=utf-8",
            success: function (msg) {
                console.log("Successfully started crawling.");
            },
            error: function (msg) {
                console.log("Error when starting to crawl.");
            }
        });
    }),

    $('#stop').click(function () {
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
    }),

    $('#clear').click(function () {
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
    }),

    $('#submit').click(function () {
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
    })
});


//$(document).ready(ajaxGetStatus());