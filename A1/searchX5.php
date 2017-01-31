<?php
// ini_set('display_errors', 1);
// ini_set('display_startup_errors', 1);
// error_reporting(E_ALL);
?>

<!doctype html>
<html> 
	<head>
		<title> Basketball is Fun! </title>
        <meta name="author" content="Arvindram Krishnamoorthy">
        <meta name="description" content="Searchable NBA Database">
        
        <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.0.0-alpha1/jquery.js"></script>
        <link href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.5/css/bootstrap.min.css" rel="stylesheet">

        <link href="//netdna.bootstrapcdn.com/font-awesome/latest/css/font-awesome.css" rel="stylesheet">
        
        <link rel="stylesheet" href="search5.css">
        
        <link href = "testVideo.css" rel="stylesheet" type = "text/css"/>
        <script src="testVideo.js"></script>
        
        <script src="ajax.js"></script>
        <link href="style.css" rel="stylesheet" type="text/css" />
	</head>
	<body>
	    <div>
            <h1 id="title">NBA Player Database</h1>
            <canvas id=c></canvas>
			<form action="searchX5.php" method="POST">
				<div id="divSearch">
					<input type="text" placeholder= "Search for a player!" name="searchterm" class="form-control"> <!-- <input type="submit" class="btn btn-danger" value="Search"> -->
				</div>
			</form>
        </div>
        <h1 id="texttosayresults">Results:</h1>				
		<div id="results"></div>
        <div id="wrap_video">
            <div id="video_box">
                <div>
                    <video id=v class="bgvid" autoplay="autoplay" muted="muted" preload="auto" loop>
                        <source src="Dunks.webm" type=video/webm>
                    </video>
                </div>
            </div>
        </div>		
	</body>
</html>