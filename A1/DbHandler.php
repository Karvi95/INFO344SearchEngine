<?php

require_once 'Db.php';

if(isset($_GET['query'])) {
	$myDB = new Db();
	$searchterm = trim($_GET['query']);
	$NBAers = $myDB->search($searchterm);

	$myDB->kill();
/*	echo json_encode($NBAers[0]);*/
	echo $_GET['callback'] . '('.json_encode($NBAers).')';

// Not necessary for PA4 BUT was cool for PA1
/*if (empty($NBAers)) {
	echo "<p id='emptyResults'>No Such Player.</p>";	
} else {
		foreach ($NBAers as $NBA) {
			echo '<div class="container">
					<div id="header" style="height:15%;width:100%;">
					    <div style="float:right" id="tableness">
					        <table class="table table-condensed" border="1" width="44" style="margin-left:5%;float:top;"> 
					            <h2>' . $NBA->getEntireName() . '</h2>' .   
					            '<tr>
					                <th colspan="2" id="nonSpace">Team:</th>
					                <td colspan="2">' . $NBA->getTeam() . '</td>'. 
					                '<th id="lastSp"></th>
					                <th colspan="2" id="nonSpace">GP:</th>
					                <td colspan="2" id="leftLine">' . $NBA->getGP() . '</td>' .
					                '<th id="lastSp"></th>
					                <th colspan="2" id="nonSpace">Min:</th>
					                <td colspan="2" id="leftLine">' . $NBA->getMin() . '</td>' .    
					            '</tr>
					            <tr>
					                <th id="nonSpace">Ast</th>
					                <td>' . $NBA->getAst() . '</td>' .
					                '<th id="nonSpace">Stl</th>
					                <td>' . $NBA->getStl() . '</td>' .
					                '<th id="blankSpaceBottom"></th>
					                <th id="nonSpace">FG_M</th>
					                <td>' . $NBA->getFG_M() . '</td>' .
					                '<th id="nonSpace">FT_M</th>
					                <td>' . $NBA->getFT_M() . '</td>' .
					                '<th id="blankSpaceBottom"></th>
					                <th id="nonSpace">3PT_M</th>
					                <td>' . $NBA->getThreePT_M() . '</td>' .
					                '<th id="nonSpace">Rebounds_Off</th>
					                <td>' . $NBA->getRebounds_Off() . '</td>' .
					                '</tr>
					             <tr>
					                <th id="nonSpace">TO</th>
					                <td>' . $NBA->getTO() . '</td>' .
					                '<th id="nonSpace">Blk</th>
					                <td>' . $NBA->getBlk() . '</td>' .
					                '<th id="blankSpaceBottom"></th>
					                <th id="nonSpace">FG_A</th>
					                <td>' . $NBA->getFG_A() . '</td>' .
					                '<th id="nonSpace">FT_A</th>
					                <td>' . $NBA->getFT_A() . '</td>' .
					                '<th id="blankSpaceBottom"></th>
					                <th id="nonSpace">3PT_A</th>
					                <td>' . $NBA->getThreePT_A() . '</td>' .
					                '<th id="nonSpace">Rebounds_Def</th>
					                <td>' . $NBA->getRebounds_Def() . '</td>' .
					                '</tr>
					             <tr>
					                <th id="nonSpace">PPG</th>
					                <td id="last">' . $NBA->getPPG() . '</td>' .
					                '<th id="nonSpace">PF</th>
					                <td>' . $NBA->getPF() . '</td>' .
					                '<th id="lastSp"></th>
					                <th id="nonSpace">FG_Pct</th>
					                <td id="last">' . $NBA->getFG_Pct() . '</td>' .
					                '<th id="nonSpace">FT_Pct</th>
					                <td>' . $NBA->getFT_Pct() . '</td>' .
					                '<th id="lastSp"></th>
					                <th id="nonSpace">3PT_Pct</th>
					                <td id="last">' . $NBA->getThreePT_Pct() . '</td>' .
					                '<th id="nonSpace">Rebouds_Tot</th>
					                <td id="last">' . $NBA->getRebouds_Tot() . '</td> .
					            </tr>
					        </table>
					    </div>
					</div>
				</div>';							
		}	
	}*/
}