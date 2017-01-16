<?php 
class DbX {
	private $dbh;
	function __construct() {
		$hostname = 'ards.c8vxadxemz8c.us-west-2.rds.amazonaws.com';
		$username = 'info344user';
		$password = 'password';
		$dbname = 'BasketballPlayerDB';
		
		$dbh = new PDO("mysql:host=$hostname;dbname=$dbname",$username,$password);
		$dbh->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
		var_dump($dbh);
	}

	public function search($searchterm) {
		$sql = 'SELECT * 
				FROM NBAInfo 
				WHERE EntireName LIKE :searchterm';
		$sth = $$dbh->prepare($sql, array(PDO::ATTR_CURSOR => PDO::CURSOR_FWDONLY));
		$sth->execute(array(':searchterm' => "%".$searchterm."%"));
		$sth->setFetchMode(PDO::FETCH_CLASS|PDO::FETCH_PROPS_LATE,'BBallPlayer',
			array('EntireName', 'Team','GP','Min','FG_M','FG_A','FG_Pct','ThreePT_M','ThreePT_A','ThreePT_Pct','FT_M','FT_A','FT_Pct','Rebounds_Off','Rebounds_Def','Rebounds_Tot','Ast','TO','Stl','Blk','PF','PPG'));
		$results = $sth->fetchAll();
		
		return $results;
	}

	public function kill() {
		$dbh = null;
	}
}