// Start Ready
$(document).ready(function () {

    $('select').material_select();

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
            url: "admin.asmx/GetStatus",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                console.log("ajaxGetStatus success");

                var results;
                results = "Worker role is currently: <b>" + msg.d[0] + "</b><br>";
                results += "CPU Utilization Percentage: <b>" + msg.d[1] + "</b><br>";
                results += "RAM Available: <b>" + msg.d[2] + "</b><br>";
                results += "Total URL's Crawled: <b>" + msg.d[3] + "</b><br>";
                results += "Size of URL Queue: <b>" + msg.d[4] + "</b><br>";
                results += "Size of Index: <b>" + msg.d[5] + "</b><br><br>";

                results += "Last 10 URLs Crawled: <br><b>";
                var recentUrls = msg.d[6].split(" ");
                if (recentUrls.length != 1) {
                    for (var i = 0; i < 10; i++) {
                        results += recentUrls[i] + "<br>";
                    }
                }

                results += "<br>";

                results += "</b>Error URLs: <br><b>";
                var errors = msg.d[7].split(" ");
                for (var i = 0; i < errors.length; i++) {
                    results += errors[i] + "<br>";
                }

                results += "</b><br>";

                results += "Number of suggestion titles: <b>" + msg.d[8] + "</b><br>";
                results += "Last suggestion title: <b>" + msg.d[9] + "</b><br>";

                $("#metrics").html(results);
            },
            error: function (msg) {
                console.log("ajaxGetStatus error");
            }
        });
    }
});


//$(document).ready(ajaxGetStatus());