// Start Ready
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
    // Get Results for PA1
    function search() {
        var query_value = $("input[name='searchterm']").val().trim();
        var dup = query_value;

        if (query_value !== '') {
            console.log('TEST');
            $.ajax({
                type: "GET",
                url: "http://ec2-35-166-117-23.us-west-2.compute.amazonaws.com/DbHandler.php",
                data: { query: query_value },
                dataType: "jsonp",
                contentType: "application/json; charset=utf-8",
                crossDomain: true,
                cache: false,
                success: onDataReceived
            });

            function onDataReceived(data) {
                //console.log("jQuery works");
                $("#results").html(JSON.stringify(data));
                console.log(data);
                console.log(data.length);
                //console.log(data[0]);
                //console.log(typeof(data[0].Team));
                if (data.length > 0) {
                    $("#entirenameData").text(data[0].EntireName);
                    $("#teamData").text(data[0].Team);
                    $("#gpData").text(data[0].GP);
                    $("#minData").text(data[0].Min);
                    $("#astData").text(data[0].Ast);
                    $("#stlData").text(data[0].Stl);
                    $("#fgmData").text(data[0].FG_M);
                    $("#ftmData").text(data[0].FT_M);
                    $("#threeptmData").text(data[0].ThreePT_M);
                    $("#reboundsoffData").text(data[0].Rebounds_Off);
                    $("#toData").text(data[0].TO);
                    $("#blkData").text(data[0].Blk);
                    $("#fgaData").text(data[0].FG_A);
                    $("#ftaData").text(data[0].FT_A);
                    $("#threeptaData").text(data[0].ThreePT_A);
                    $("#reboundsdefData").text(data[0].Rebounds_Def);
                    $("#ppgData").text(data[0].PPG);
                    $("#pfData").text(data[0].PF);
                    $("#fgpctData").text(data[0].FG_Pct);
                    $("#ftpctData").text(data[0].FT_Pct);
                    $("#threeptpctData").text(data[0].ThreePT_Pct);
                    $("#reboundstotData").text(data[0].Rebounds_Tot);
                } else {
                    $("#entirenameData").text('');
                    $("td").text('');
                }
            }
        } else {
            $("#entirenameData").text('');
            $("td").text('');
        }

        // Live Search
        // Get Results for PA2
        $.ajax({
            type: "POST",
            url: "SearchQuery.asmx/searchTrie",
            data: JSON.stringify({ queryValue: query_value }),
            contentType: "application/json; charset=utf-8",
            dataType: "JSON",
            success: function (msg) {
                var outer = $("<div></div>");
                $("#jsonDiv").html(outer);
                if (query_value === "") {
                    var div = $("<div>").text("");
                    outer.append(div);
                } else if (msg.d == "no results found.") {
                    var div = $("<div>").text("no results found.");
                    outer.append(div);
                } else {
                    var data = JSON.parse(msg.d);
                    for (var i = 0; i < data.length; i++) {
                        var div = $("<div>").text(data[i]);
                        outer.append(div);
                    }
                }
            },
            error: function (msg) {
                console.log(msg);
            }
        });

        // Live Search
        // Get Results for PA3
        $.ajax({
            type: "POST",
            url: "admin.asmx/search",
            data: JSON.stringify({ input: query_value }),
            contentType: "application/json; charset=utf-8",
            dataType: "JSON",
            success: function (msg) {
                console.log(msg);


            },
            error: function (msg) {
            }
        });

        //$('body').on("keyup", "input[name='searchterm']", function (e) {
        //    // Set Timeout
        //    clearTimeout($.data(this, 'timer'));

        //    // Set Search String
        //    var search_string = $(this).val();

        //    // Do Search
        //    if (search_string == '') {
        //        $("div#results").fadeOut();
        //        $("h1#texttosayresults").fadeOut();
        //    } else {
        //        $("div#results").fadeIn();
        //        $("h1#texttosayresults").fadeIn();
        //        $(this).data('timer', setTimeout(search, 100));
        //    };
        //});
    }
});