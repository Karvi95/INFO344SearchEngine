<?php

require_once 'Db.php';

if(isset($_POST['query'])) {
	$myDB = new Db();
	$searchterm = trim($_POST['query']);
	$NBAers = $myDB->search($searchterm);


if (empty($NBAers)) {
	echo "<p id='emptyResults'>No Such Player.</p>";	
} else {
		foreach ($NBAers as $NBA) {
			echo '<table class="table table-bordered">'
			. '<img src="' . $NBA->getImgURL() . '" alt="Cover">
				<h1>' . $NBA->getEntireName() . '</h1>' .	
				'<tr>
				 	<th colspan="2" id="nonSpace">Team:</th>
				  	<td colspan="2">' . $NBA->getTeam() . '</td>'. 
				  	'<th id="blankSpaceBottom"></th>
				  	<th colspan="2" id="nonSpace">Games Played:</th>
				  	<td colspan="2" id="leftLine">' . $NBA->getGP() . '</td>' .
				  	'<th id="blankSpaceBottom"></th>
				  	<th colspan="2" id="nonSpace">Minutes:</th>
				  	<td colspan="2" id="leftLine">' . $NBA->getMin() . '</td>' .	
				'</tr>
				<tr>
					<th id="nonSpace">Assists</th>
				  	<td>' . $NBA->getAst() . '</td>' .
				  	'<th id="nonSpace">Steals</th>
				  	<td>' . $NBA->getStl() . '</td>' .
			  		'<th id="blankSpaceBottom"></th>
				  	<th id="nonSpace">Field Goals Made</th>
				  	<td>' . $NBA->getFG_M() . '</td>' .
			  		'<th id="nonSpace">Free Throws Made</th>
				  	<td>' . $NBA->getFT_M() . '</td>' .
			  		'<th id="blankSpaceBottom"></th>
				  	<th id="nonSpace">Three-Pointers Made</th>
				  	<td>' . $NBA->getThreePT_M() . '</td>' .
			  		'<th id="nonSpace">Offensive Rebounds</th>
				  	<td>' . $NBA->getRebounds_Off() . '</td>' .
			  		'</tr>
				 <tr>
					<th id="nonSpace">Turnovers</th>
				  	<td>' . $NBA->getTO() . '</td>' .
			  		'<th id="nonSpace">Blocks</th>
				  	<td>' . $NBA->getBlk() . '</td>' .
			  		'<th id="blankSpaceBottom"></th>
				  	<th id="nonSpace">Field Goals Attempted</th>
				  	<td>' . $NBA->getFG_A() . '</td>' .
			  		'<th id="nonSpace">Free Throws Attempted</th>
				  	<td>' . $NBA->getFT_A() . '</td>' .
			  		'<th id="blankSpaceBottom"></th>
				  	<th id="nonSpace">Three-Pointers Attempted</th>
				  	<td>' . $NBA->getThreePT_A() . '</td>' .
			  		'<th id="nonSpace">Defensive Rebounds</th>
				  	<td>' . $NBA->getRebounds_Def() . '</td>' .
			  		'</tr>
				 <tr>
					<th id="nonSpace">Points Per Game</th>
				  	<td>' . $NBA->getPPG() . '</td>' .
			  		'<th id="nonSpace">Personal Fouls</th>
				  	<td>' . $NBA->getPF() . '</td>' .
			  		'<th></th>
				  	<th id="nonSpace">Field Goal Pecentage</th>
				  	<td>' . $NBA->getFG_Pct() . '</td>' .
			  		'<th id="nonSpace">Free Throws Percentage</th>
				  	<td>' . $NBA->getFT_Pct() . '</td>' .
			  		'<th></th>
				  	<th id="nonSpace">Three-Pointer Percentage</th>
				  	<td>' . $NBA->getThreePT_Pct() . '</td>' .
			  	   	'<th id="nonSpace">Total Rebounds</th>
				  	<td>' . $NBA->getRebouds_Tot() . '</td>' .
				'</tr>
			</table>';							
		}	
	}
}

