using UnityEngine;
using System.Collections;
using Parse;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;
using System.Collections.Generic; 

public class Manager : MonoBehaviour {
	public Board board;
	bool didRotateCheck = false;

	//Have selection thing here

	void Start () {
		TwoPlayerLocal ();
		initBoard ();
	}

	void initBoard()
	{
		Debug.LogWarning (" ++++++++++++++++ BOARD STARTED UP! ++++++++++++++++++++++ ");
		//1D GameObject array [64]
		//Only checks adjacent tiles for ONE warp
		//I have to instantiate all of the warps? Noooooooooooooooooooooo
		//Link paired warp tiles together via gameobject variable
		//Physics.gravity = new Vector3 (0, 0, 1);
		Debug.Log("this.gameMatch[\"nextPlayerUsername\"] => " + Board.gameMatch["thisPlayerUsername"]);
		Debug.Log("this.gameMatch[\"nextPlayerUsername\"] => " + Board.gameMatch["nextPlayerUsername"]);
		
		
		
		//gameMatch = null; // DO NOT ALLOW THIS TO EXIST, IT NEEDS TO BE LEFT ALONE, LET OTHER SCRIPTS MANIP THIS!
		
		
		Board.pieceData = new GameObject[16];
		
		Board.dicboardData = new Dictionary<Vector3, GameObject>();
		Board.legalMoves = new LegalMoves();
		
		
		
		// LoadPiecesToBoard()
		
		bool isPlayerWaiting = true; // enforce this as locked until parse says otherwise.
		
		if (!Board.isOnlineMode) 
		{
			Board._currPlayer = "PlayerOne";
		} 
		else 
		{
			
			
			
			// check if the player is logged in still, if not, send them back to login
			if(ParseUser.CurrentUser == null || Board.gameMatch == null)
			{
				Application.LoadLevel("scnMainMenu");
			}
			else
			{
				Debug.Log ("GAME ON! GameObjID = " + Board.gameMatch.ObjectId);
			}

			Board.setupCurrPlayer(Board.turnCounter);
			// TODO: This is the method that will conver the string to positionals
			// then we need to actually make the pieces BE there....
			// This is the method I'm stuck inside...
			// reset to a standard null game board
			string boardString = Board.getNullBoardToString();
			if(Board.gameMatch["thisBoardState"]!=null)
			{
				boardString = Board.gameMatch["thisBoardState"].ToString();
			}
			//Debug.LogWarning("running convert");
			Board.convertToGameBoard(boardString);
			
			
			//turnToSave = Board.getBoardStateToString();
		}
		//moves = legalMoves.getLegalMoves(currPlayer, boardData);
		
		Debug.LogWarning (Board.turnCounter + "sWHOSE TURN IS IT ANYWAYS?: " + Board.WhoIsThis());

	}
	void OnePlayer () {
	}

	void TwoPlayerLocal () {
		SetUp ();
	}

	void TwoPlayerP2P () {

	}


	private string boardState1dString = "";


	void SetUp () 
	{
		GameObject boardObject;
		if (!Board.isInitialized) {
			boardObject = new GameObject ("board");
			board = boardObject.AddComponent<Board> ();
			
			boardObject.transform.parent = transform;
			Board.isInitialized = true;
		} else {
			//boardObject = Board;
		}
		// TODO: Run parse coroutine

		

		//TODO:
		// If thisPlayerNumber = this turn's player
		// and if this turn's player == parse.username
		// then isPlayerWaiting = false;
		// else isPlayerWaiting = true;

		// v !!!!!!!!!! DANGER
		Board.isPlayerWaiting = false; // DEBUG ONLY
		// ^ !!!!!!!!!! DANGER

		//Board.setupCurrPlayer (1); // debug using 1 to indicate player 1
		//Board.currPlayer = "PlayerOne"; // from parse

		// Let's store where the peices are now in that string
		/*
		for (int i = 0; i < 16; i++)
		{
			// declare local variables so that the values don't have to be fetched through dot operators multiple times if needed
			// This is a minor cost saving optomization
			// ProTip: NEVER EVER optomize before finishing everything up first.
			// There's some sage quote about this, I'm totally breaking this rule, but I'm pretty sure I can get away with it in this case.
			
			if(Board.pieceData[i] !=null)
			{
				int _row = (int) -Board.pieceData[i].GetComponent<Piece>().pos.y/2;
				int _col = (int) Board.pieceData[i].GetComponent<Piece>().pos.x/2;
				string _name = Board.pieceData[i].name;
				
				Board.gameBoardState[_row,_col] = Board.pieceData[i].name;
				//Debug.LogWarning("MANAGER PEICE DATA: " + _name + "@ " + _row + "x" + _col);
			}
		}
		*/
		// TODO: Setup where peices go!

		// end of parse coroutine


	}
	
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		/*
		if (Board.WhoIsThis () == "PlayerTwo" && !didRotateCheck) {
			this.gameObject.transform.Rotate (new Vector3 (0, 0, 180));
			didRotateCheck=true;
		} else if(!didRotateCheck) {
			this.gameObject.transform.Rotate (new Vector3 (0, 0, 0));
			didRotateCheck=true;
		}
		*/
	
	}



	bool CanGo(){
		return false;
	}
	
}

