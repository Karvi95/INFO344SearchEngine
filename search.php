<?php
// ini_set('display_errors', 1);
// ini_set('display_startup_errors', 1);
// error_reporting(E_ALL);
require_once('BBallPlayer.php');
?>

<!doctype html>
<html> 

<?php
$hostname = "ards.c8vxadxemz8c.us-west-2.rds.amazonaws.com";
$username = "info344user";
$password = "password";
$dbname = "BasketballPlayerDB";

$dbh = new PDO("mysql:host=$hostname;dbname=$dbname",$username,$password);
$dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

$searchterm = htmlentities($_POST['searchterm']);

$NBAers = search($searchterm, $dbh);

function search($searchterm, $dbh) {
	$sql = 'SELECT * 
			FROM NBAInfo 
			WHERE EntireName LIKE :searchterm';
	$sth = $dbh->prepare($sql, array(PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY));
	$sth->execute(array(':searchterm' => "%".$searchterm."%"));
	$sth->setFetchMode(PDO::FETCH_CLASS|PDO::FETCH_PROPS_LATE,'BBallPlayer',
		array('EntireName', 'Team','GP','Min','FG_M','FG_A','FG_Pct','ThreePT_M','ThreePT_A','ThreePT_Pct','FT_M','FT_A','FT_Pct','Rebounds_Off','Rebounds_Def','Rebounds_Tot','Ast','TO','Stl','Blk','PF','PPG'));
	$results = $sth->fetchAll();
	return $results;
}



?>
	<head>
		<title> Basketball </title>
	</head>
	<body>
		<form action="search.php" method="POST">
		<input type="text" placeholder= "Search for a player!" name="searchterm"> <input type="submit" value="Search">
		</form>
		<div></div>
			<?php
				if ((trim($_POST['searchterm']) != '') && isset($_POST['searchterm'])) {
					foreach ($NBAers as $NBA) {
						// var_dump($NBAers);
						if (is_null($NBAers)) {
							echo "No Such Player.";		
						} else {
							echo $NBA->getEntireName() . "<br>";
						}
					}					
				}

			?>
		
		</body>
</html>
