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
        var input = $("input[name='searchterm']").val();
        $.ajax({
            type: "POST",
            url: "http://a2info344.cloudapp.net/myWebService.asmx/searchTrie",
            data: JSON.stringify({ queryValue: input }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                var outer = $("<div></div>");
                $("#jsonDiv").html(outer);
                if (input === "")
                {
                    var div = $("<div>").text("");
                    outer.append(div);
                } else if (msg.d == "No results found.") {
                    var div = $("<div>").text("No results found.");
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

            }
        });
    }
});
