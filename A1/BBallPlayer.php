<?php 
class BBallPlayer { 
    private $EntireName, $FName, $MName, $LName, $Suffix, $Team, $GP, $Min, $FG_M, $FG_A, $FG_Pct, $ThreePT_M, $ThreePT_A, $ThreePT_Pct, $FT_M, $FT_A, $FT_Pct, $Rebounds_Off, $Rebounds_Def,$Rebounds_Tot,$Ast,$TO,$Stl,$Blk,$PF,$PPG; 

    function __construct($EntireName, $FName, $MName, $LName, $Suffix, $Team, $GP, $Min, $FG_M, $FG_A, $FG_Pct, $ThreePT_M, $ThreePT_A, $ThreePT_Pct, $FT_M, $FT_A, $FT_Pct, $Rebounds_Off, $Rebounds_Def, $Rebounds_Tot, $Ast, $TO, $Stl, $Blk, $PF, $PPG) {
    	$this->$EntireName=$EntireName;
    	$this->$FName=$FName;
        $this->$MName=$MName;
        $this->$LName=$LName;
        $this->$Suffix=$Suffix;
        $this->$Team=$Team;                    
    	$this->$GP=$GP;                             # Games Played
    	$this->$Min=$Min;                           # Minutes         
    	$this->$FG_M=$FG_M;                         # Field Goals Made
    	$this->$FG_A=$FG_A;                         # Field Goals Attempted
    	$this->$FG_Pct=$FG_Pct;                     # Field Goal Percentage
    	$this->$ThreePT_M=$ThreePT_M;               # Three-Pointers Made
    	$this->$ThreePT_A=$ThreePT_A;               # Three-Pointers Attempted
    	$this->$ThreePT_Pct=$ThreePT_Pct;           # Three-Pointer Percentage
    	$this->$FT_M=$FT_M;                         # Free Throws Made
    	$this->$FT_A=$FT_A;                         # Free Throws Made
    	$this->$FT_Pct=$FT_Pct;                     # Free Throw Percentage
    	$this->$Rebounds_Off=$Rebounds_Off;         # Offensive Rebounds
    	$this->$Rebounds_Def=$Rebounds_Def;         # Defensive Rebounds
    	$this->$Rebounds_Tot=$Rebounds_Tot;         # Total Rebounds
    	$this->$Ast=$Ast;                           # Assists
    	$this->$TO=$TO;                             # Turnovers 
    	$this->$Stl=$Stl;                           # Steal
    	$this->$Blk=$Blk;                           # Blocks
    	$this->$PF=$PF;                             # Personal Fouls
    	$this->$PPG=$PPG;                           # Points per Game
    }

    public function getEntireName() {
    	return $this->EntireName;
    }

    public function getFName() {
        return $this->FName;
    }

    public function getLName() {
        return $this->LName;
    }

    public function getSuffix() {
        return $this->EntireName;
    }    

    public function getTeam() {
    	return $this->Team;
    }

    public function getGP() {
    	return $this->GP;
    }

    public function getMin() {
    	return $this->Min;
    }

    public function getFG_M() {
    	return $this->FG_M;
    }

    public function getFG_A() {
    	return $this->FG_A;
    }

    public function getFG_Pct() {
    	return $this->FG_Pct;
    }

    public function getThreePT_M() {
    	return $this->ThreePT_M;
    }

    public function getThreePT_A() {
    	return $this->ThreePT_A;
    }

    public function getThreePT_Pct() {
    	return $this->ThreePT_Pct;
    }

    public function getFT_M() {
    	return $this->FT_M;
    }

    public function getFT_A() {
    	return $this->FT_A;
    }

    public function getFT_Pct() {
    	return $this->FT_Pct;
    }

    public function getRebounds_Off() {
    	return $this->Rebounds_Off;
    }

    public function getRebounds_Def() {
    	return $this->Rebounds_Def;
    }

    public function getRebouds_Tot() {
    	return $this->Rebounds_Tot;
    }

    public function getAst() {
    	return $this->Ast;
    }

    public function getTO() {
    	return $this->TO;
    }

    public function getStl() {
    	return $this->Stl;
    }

    public function getBlk() {
    	return $this->Blk;
    }

    public function getPF() {
    	return $this->PF;
    }

    public function getPPG() {
    	return $this->PPG;
    }
    
    public function getImgURL() {
        return 'https://nba-players.herokuapp.com/players/' . $this->getLName() . '/' . $this->getFName();
    }
} 

