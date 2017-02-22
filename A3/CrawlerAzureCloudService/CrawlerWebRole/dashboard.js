// Start Ready
$(document).ready(function () {

    $('#report').click(function () {
        $.ajax({
            type: "POST",
            url: "admin.asmx/getStatus",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                console.log("ajaxGetStatus success");
                console.log(msg.d);

                var jsStatus = JSON.parse(msg.d);

                //Status
                //$("#status span").child.removeChild();
                $("#status").append('<span>' + jsStatus + '</span>');
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
                console.log("ajaxGetStatus success");
                console.log(msg.d);

                var jsPref = JSON.parse(msg.d);

                //$("#cpu span").child.remove();

                //$("#ram span").child.remove();

                //CPU
                $("#cpu").append('<span>' + jsPref[0] + '</span>');

                //RAM
                $("#ram").append('<span>' + jsPref[1] + '</span>');

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
                console.log("ajaxGetStatus success");
                console.log(msg.d);

                var jsTotal = JSON.parse(msg.d);

                //$("#total span").child.remove();
                //TOTAL
                $("#total").append('<span>' + jsTotal + '</span>');

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
                console.log("ajaxGetStatus success");
                console.log(msg.d);

                var jsQSize = JSON.parse(msg.d);

                //$("#qSize span").child.remove();

                //Queue Size
                $("#qSize").append('<span>' + jsQSize + '</span>');

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
                console.log("ajaxGetStatus success");
                console.log(msg.d);

                var jsISize = JSON.parse(msg.d);

                //Index Size
                $("#iSize").append('<span>' + jsISize + '</span>');

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
    $.ajax({
        type: "GET",
        url: "admin.asmx/lastTen",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (msg) {
            console.log("ajaxGetStatus success");
            console.log(msg.d);

            var lasttendata = JSON.parse(msg);

            //last Ten
            var recentUrls = msg.d[0].split(" ");
            if (recentUrls.length !== 1) {
                for (var i = 0; i < 10; i++) {
                    $("#lastten").append('<li class="collection-item">' + recentUrls[i] + '</li>');
                }
            }

        },
        error: function (msg) {
            console.log(msg.d);
            console.log("ajaxGetStatus error");
        }
    });
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



