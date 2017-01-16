<?php

require_once 'Db.php';

if(isset($_POST['query'])) {
	$myDB = new Db();
	$searchterm = trim($_POST['query']);
	$NBAers = $myDB->search($searchterm);

	echo $NBAers;
}

