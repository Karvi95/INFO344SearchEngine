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
				type: "POST",
				url: "DbHandler.php",
				data: { query: query_value },
				cache: false,
				success: function(data) {
					console.log('THIS IS A: ', typeof data, '. HERE IS THE DATA:', data);
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
		} else {
			$("div#results").fadeIn();
			$(this).data('timer', setTimeout(search, 100));
		};
	});

});