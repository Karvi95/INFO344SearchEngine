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
                console.log("ajaxGetStatus success");
                console.log("MESSAGE: " + msg.d);
                var jsLastTen = JSON.parse(msg.d);
                console.log(jsLastTen[0]);
                var jsLastTenArray = jsLastTen[0].split(" ");
                console.log("array: " + jsLastTenArray[0]);

                var string = "#lastten";
                //Populating List
                for (var i = 0; i < jsLastTenArray.length; i++) {
                    string = "#lastten" + i;
                    console.log(string);
                    $(string).text(jsLastTenArray[i]);
                }
                //$("#lastten0").text(jsLastTen[0]);
                //$("#lastten1").text(jsLastTen[1]);
                //$("#lastten2").text(jsLastTen[2]);
                //$("#lastten3").text(jsLastTen[3]);
                //$("#lastten4").text(jsLastTen[4]);
                //$("#lastten5").text(jsLastTen[5]);
                //$("#lastten6").text(jsLastTen[6]);
                //$("#lastten7").text(jsLastTen[7]);
                //$("#lastten8").text(jsLastTen[8]);
                //$("#lastten8").text(jsLastTen[9]);

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
function DISPLAYSTATUS() {
   
}

function DISPLAYPREF() {
    
}
    
function DISPLAYTOTAL() {
    
}

function DISPLAYQUEUESIZE() {

}

function DISPLAYINDEXSIZE() {

}

function DISPLAYLASTTEN() {

}
    
function DISPLAYERRORS() {
    $.ajax({
        type: "GET",
        url: "admin.asmx/getErrors",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            console.log("ajaxGetStatus success");
            console.log(msg.d);

            var lasttendata = JSON.parse(msg);

            //Error messages
            var errorsMes = msg.d[0].split("||");
            for (var j = 0; j < errorsMes.length;j++) {
                $("#errorMes").append('<li class="collection-item">' + errorsMes[j] + '</li>');
            }

            //Error urls
            var errorsUrls = msg.d[1].split(" ");
            for (var k = 0; k < errorsUrls.length; k++) {
                $("#errorUrl").append('<li class="collection-item">' + errorsUrls[k] + '</li>');
            }

        },
        error: function (msg) {
            console.log(msg.d);
            console.log("ajaxGetStatus error");
        }
    });
}



