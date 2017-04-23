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
                    $("#tableness").css('display', 'block');
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
                    $("#tableness").css('display', 'none');
                    $("#entirenameData").text('');
                    $("td").text('');
                }
                $("#jsonDiv").css('display', 'block');
            }
        } else {
            $("#jsonDiv").css('display', 'none');
            $("#tableness").css('display', 'none');
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
                console.log(msg);
                console.log(msg.d);
                var outer = $("<div></div>");
                $("#jsonDiv").html(outer);
                if (query_value === "") {
                    var div = $("<div class='container-fluid pa2suggestDiv'>").text("");
                    outer.append(div);
                } else if (msg.d === "No results found.") {
                    var div = $("<div class='container-fluid pa2suggestDiv'>").text("No suggestions found.");
                    outer.append(div);
                } else {
                    var data = JSON.parse(msg.d);
                    for (var i = 0; i < data.length; i++) {
                        var div = $("<div class='container-fluid pa2suggestDiv'>").text(data[i]);
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
                var jsResultMes = JSON.parse(msg.d);
                $("#pa3Results").html("");
                for (var i = 0; i < jsResultMes.length; i++) {
                    console.log(i);
                    console.log(jsResultMes[i]);
                    console.log('SPLITING');
                    var jsResultMesArray = jsResultMes[i].split("|");
                    console.log(jsResultMesArray);
                    var outer = $("<div class='container-fluid pa3resultDiv'></div>");
                    var ref = $("<a class='pa3resultA'></a>");
                    ref.text(jsResultMesArray[0]);
                    ref.attr("href", "" + jsResultMesArray[1]);
                    outer.append(ref);
                    $("#pa3Results").append(outer);
                }
            },
            error: function (msg) {
            }
        });
    }
});
