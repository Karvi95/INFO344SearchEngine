// Start Ready
$(document).ready(function() {  

	// Canvas Click Focus
	$('canvas#c').click(function() {    
		$('input.form-control').focus();
	});

	// Live Search
	// On Search Submit and Get Results
	function search() {
		var query_value = $("input[name='searchterm']").val().trim();
		if(query_value !== '') {
			$.ajax({
				type: "GET",
				url: "DbHandler.php",
				data: { query: query_value },
				dataType: "HTML",
				cache: false,
				success: function(data) {
					$("div#results").html(data); 
				}
			});
		} return false;    
	}

	$('body').on("keyup", "input[name='searchterm']" , function(e) {
		// Set Timeout
		clearTimeout($.data(this, 'timer'));

		// Set Search String
		var search_string = $(this).val();
		
		// Do Search
		if (search_string == '') {
			$("div#results").fadeOut();
			$("h1#texttosayresults").fadeOut();
		} else {
			$("div#results").fadeIn();
			$("h1#texttosayresults").fadeIn();
			$(this).data('timer', setTimeout(search, 100));
		};
	});

});