<?php
ini_set('display_errors', 1);
ini_set('display_startup_errors', 1);
error_reporting(E_ALL);
require_once('BBallPlayer.php');
require_once('Db.php');
?>

<!doctype html>
<html> 

<?php

$myDB = new Db();

if(isset($_POST['searchterm'])) {
	$searchterm = trim($_POST['searchterm']);
	$NBAers = $myDB->search($searchterm);
}

?>
	<head>
		<title> Basketball </title>
	</head>
	<body>
		<form action="searchX.php" method="POST">
		<input type="text" placeholder= "Search for a player!" name="searchterm"> <input type="submit" value="Search">
		</form>
		<div></div>
			<?php
				if (isset($_POST['searchterm']) && (trim($_POST['searchterm']) != '')) {
					if (empty($NBAers)) {
						echo "No Such Player.";	
					} else {
						foreach ($NBAers as $NBA) {
							echo $NBA->getEntireName() . "<br>";
						}	
					}
				
				}

			?>
		
		</body>
</html>
