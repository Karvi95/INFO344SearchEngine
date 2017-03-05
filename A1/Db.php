<?php 

require_once('BBallPlayer.php');

class Db {
	private $hostname, $username, $password, $dbname, $dbh;
	
	function __construct() {
	}

	function search($searchterm) {
		$results = array();

		if (strlen($searchterm) >= 1 && $searchterm !== ' ') {
		
			$hostname = 'ards.c8vxadxemz8c.us-west-2.rds.amazonaws.com';
			$username = 'info344user';
			$password = 'password';
			$dbname = 'BasketballPlayerDB';
			
			$dbh = new PDO("mysql:host=$hostname;dbname=$dbname",$username,$password);
			$dbh -> exec("SET CHARACTER SET utf8");
			$dbh -> setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);

			$searchterm = $dbh->quote($searchterm);

			$sql = "SELECT * 
					FROM `NBAInfo` 
					WHERE `EntireName` = {$searchterm}
					LIMIT 1";

// No longer needed for PA4 but kept 'cause was cool for PA1
/*			$sql = "SELECT * 
					FROM `NBAInfo` 
					WHERE CONCAT( levenshtein(`EntireName`, {$searchterm}) < 2, 
								  levenshtein(`Suffix`, {$searchterm}) < 2 , 
								  levenshtein(`LName`, {$searchterm}) < 2 ,
								  levenshtein(`MName`, {$searchterm}) < 2 ,  
								  levenshtein(`FName`, {$searchterm}) < 2
								)";*/

			$sth = $dbh->query($sql);
			$sth->setFetchMode(PDO::FETCH_CLASS|PDO::FETCH_PROPS_LATE,'\BBallPlayer',
				array('EntireName', 'FName', 'MName', 'LName', 'Suffix', 'Team','GP','Min','FG_M','FG_A','FG_Pct','ThreePT_M','ThreePT_A','ThreePT_Pct','FT_M','FT_A','FT_Pct','Rebounds_Off','Rebounds_Def','Rebounds_Tot','Ast','TO','Stl','Blk','PF','PPG'));
			$results = $sth->fetchAll();

			return $results;
		}
	}

	function kill() {
		$dbh = null;
	}
}