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
    // On Search Submit and Get Results
    function search() {
        var query_value = $("input[name='searchterm']").val().trim();

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
                console.log(data[0].EntireName);
                console.log(typeof (data[0].EntireName));
                console.log(data[0].Team);
                console.log(typeof (data[0].Team));


                data[0].EntireName = $("#entirenameData").val();
                data[0].Team = $("#teamData").val();
                //$("#gpData").val() = data['GP'];
                //$("#minData").val() = data['Min'];
                //$("#astData").val() = data['Ast'];
                //$("#stlData").val() = data['Stl'];
                //$("#fgmData").val() = data['FG_M'];
                //$("#ftmData").val() = data['FT_M'];
                //$("#threeptmData").val() = data['ThreePT_M'];
                //$("#reboundsoffData").val() = data['Rebounds_Off'];
                //$("#toData").val() = data['TO'];
                //$("#blkData").val() = data['Blk'];
                //$("#fgaData").val() = data['FG_A'];
                //$("#ftaData").val() = data['FT_A'];
                //$("#threeptaData").val() = data['ThreePT_A'];
                //$("#reboundsdefData").val() = data['Rebounds_Def'];
                //$("#ppgData").val() = data['PPG'];
                //$("#pfData").val() = data['PF'];
                //$("#fgpctData").val() = data['FG_Pct'];
                //$("#ftpctData").val() = data['FT_Pct'];
                //$("#threeptpctData").val() = data['ThreePT_Pct'];
                //$("#reboundstotData").val() = data['Rebounds_Tot'];

            }
        } return false;

        $.ajax({
            type: "post",
            url: "http://kriarvi.cloudapp.net/mywebservice.asmx/searchtrie",
            data: json.stringify({ queryvalue: input }),
            contenttype: "application/json; charset=utf-8",
            datatype: "json",
            success: function (msg) {
                var outer = $("<div></div>");
                $("#jsondiv").html(outer);
                if (input === "") {
                    var div = $("<div>").text("");
                    outer.append(div);
                } else if (msg.d == "no results found.") {
                    var div = $("<div>").text("no results found.");
                    outer.append(div);
                } else {
                    var data = json.parse(msg.d);
                    for (var i = 0; i < data.length; i++) {
                        var div = $("<div>").text(data[i]);
                        outer.append(div);
                    }
                }
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