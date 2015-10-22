using UnityEngine;
using System.Collections;
using Parse;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;
using System.Collections.Generic; // need this STUPID THING so IENumerable works within result outputs. Makes no damn sense since cothreads are IENumerable FFS!

//using UnityEngine.EventSystems.IPointerClickHandler;

public class Board : MonoBehaviour {
	public static bool isOnlineMode = false; // MODIFY THIS TO TEST ONLINE OR OFFLINE SINGLE PLAYER MODE
	public static ParseObject gameMatch;
	
	public static bool isInitialized = false; // used to check if Board was ever loaded once before
	public static string _currPlayer;
	
	public static GameObject[,] boardData = new GameObject[8, 8];
	public static GameObject[] pieceData = new GameObject[16];
	public static bool[, ] isPieceThere;
	public static GameObject capturedPiece = null;
	public static Dictionary<Vector3, GameObject> dicboardData = new Dictionary<Vector3, GameObject>();
	public static LegalMoves legalMoves = new LegalMoves();
	public static bool playerMadeMove;
	public static string[,] gameBoardState = getNullBoard ();
	public static bool isPlayerWaiting = true; // enforce this as locked until parse says otherwise.
	public static string turnToSave;

	public static ArrayList<GameObject> bPlayer = new ArrayList<GameObject>();
	public static ArrayList<GameObject> rPlayer = new ArrayList<GameObject>();
		// William:
	// Tried to get a ToString() of the object's name from peiceData but the name was CONSTANTLY returning a null reference.
	// So the positions of the peices are being stored in this string instead so that parse can set/get this
	public static string peicePlacement = "";
	public static int turnCounter = 0;
	private static bool playerInitialized = false;
	
	// Use this for initialization
	void Start () {
		
	}
	/*
	public static void InitalizationOfBoard()
	{
		pieceData = new GameObject[16];
		
		dicboardData = new Dictionary<Vector3, GameObject>();
		legalMoves = new LegalMoves();
	}
	*/
	
	public static void setupCurrPlayer(int thisPlayer)
	{
		// thisPlayer int should be either 1 or 2
		if (!playerInitialized) {
			playerInitialized = true;
			if(thisPlayer > 0)
			{
				_currPlayer = (thisPlayer % 2 == 0) ? "PlayerOne":"PlayerTwo"; // this might be flipped, not sure
			}
			else
			{
				_currPlayer = "PlayerOne";
			}
		}
	}
	
	public static string[,] getNullBoard()
	{
		return new string[,]{
			{"null","null","null","null","null","null","null","null"},
			{"null","null","null","null","null","null","null","null"},
			{"null","null","null","null","null","null","null","null"},
			{"null","null","null","null","null","null","null","null"},
			{"null","null","null","null","null","null","null","null"},
			{"null","null","null","null","null","null","null","null"},
			{"null","null","null","null","null","null","null","null"},
			{"null","null","null","null","null","null","null","null"}
		};
	}
	
	public static string getNullBoardToString()
	{
		return "p2_0,p2_1,p2_2,p2_3,p2_4,p2_5,p2_6,p2_7," +
			"null,null,null,null,null,null,null,null," +
				"null,null,null,null,null,null,null,null," +
				"null,null,null,null,null,null,null,null," +
				"null,null,null,null,null,null,null,null," +
				"null,null,null,null,null,null,null,null," +
				"null,null,null,null,null,null,null,null," +
				"p1_0,p1_1,p1_2,p1_3,p1_4,p1_5,p1_6,p1_7";
	}
	
	
	private static void makeTurn()
	{
		// This performs the online parse move making
		
		if (isOnlineMode) {
			
			// This will make the move, and afterwards move to the scnWaiting or perform the script code from that scene
			// the waiting scene/code whichever, will end up being what sets isPlayerWaiting to false once it is this player's turn again.
			Debug.LogWarning ("!!! GOING INTO NEW AREA");
			GameObject _manager = GameObject.Find("_Manager");
			var test = _manager.GetComponent<TurnActions>();
			//if(test==null)
			if(_manager==null)
			{
				Debug.Log ("It's null...");
			}
			else
			{
				test.makeTurn();
			}
			
			
		} else
		{
			
			Board._currPlayer = WhoIsThis();  // set the player to the next one (if it wasn't done already)
			isPlayerWaiting = false; // tell the game that this person ACTUALLY using this app, doesn't have to wait any longer to make a move
		}
	}
	
	///////////////////////////////
	
	
	
	//TURNCOUNTER IS NOT WORKING CORRECTLY. CHANGE T EXPLICIT NUMBERS
	public static string WhoIsThis()
	{
		Debug.Log ("Turn counter number!!!!! " + turnCounter);	
		if (!isOnlineMode) {
			
			return (turnCounter % 2 == 0) ? "PlayerOne" : "PlayerTwo";
		} else
		{
			
			if(!Board.gameMatch.Equals(null) && !Board.gameMatch["thisTurnNumber"].Equals(null))
			{
				Debug.Log ("DERP: " + gameMatch["thisTurnNumber"]);
				turnCounter = gameMatch.Get<int>("thisTurnNumber"); // this might not be necessary
			}
			else
			{
				turnCounter = 0;
			}
			
			if(turnCounter > 0)
			{
				_currPlayer = (turnCounter % 2 == 0) ? "PlayerOne":"PlayerTwo"; // this might be flipped, not sure
			}
			else
			{
				_currPlayer = "PlayerOne";
			}
			Debug.LogWarning("Returning back: " + _currPlayer);
			return _currPlayer;
		}
		
		
		
	}
	
	//needs a bool to make sure player can only select one piece at a time
	
	void Update ()
	{
		
	}
	
	
	public static void Move (GameObject from, Vector3 to) 
	{
		if (isPlayerWaiting) {
			Debug.Log ("Sorry, it's not your turn!!!!");
			return; // do nothing
		}
		//while (from.GetComponent<Piece> ().pos != to)
		//Debug.Log ("MOVE DBUG: " + from.GetComponent<Piece> ().pos);
		//boardData.Remove (from.GetComponent<Piece> ().pos);
		Debug.Log ("IN MOVE.... HOW MANY AVAILABLE MOVES DO WE HAVE...." + from.GetComponent<Piece> ().moves.Count);
		//Debug.Log ("What are each of the available moves?");
		
		Debug.Log ("to: " + to);
		Debug.Log ("Location player wants to go: " + to + " ... but can it go? " + from.GetComponent<Piece> ().moves.Contains (to));
		
		//for (int i = 0; i < from.GetComponent<Piece> ().moves.Count; i++)
		//	Debug.Log ("Move #" + i + from.GetComponent<Piece> ().moves[i].z	.Equals(to.z));
		
		if (from.GetComponent<Piece> ().moves.Contains(to)) 
		{
			capturedPiece = from.GetComponent<Piece> ().capturedPiece;
			//Debug.Log ("Piece in the way..." + capturedPiece.tag);
			Debug.Log ("to: " + to + LegalMoves.jumpList.Contains(to));
			//if player clicked on the jump square, then destroy the captured piece)
			//Debug.Log ("captured piece name: " + capturedPiece.tag);
			
			//First, check if the piece has a captured Piece, which means that the enemy piece is adjacent
			if(capturedPiece != null)
			{
				/// Let's remove captured peices from our pieceData array
				
				
				Debug.Log ("captured piece: " + capturedPiece.GetComponent<Piece> ().pos);
				Debug.Log ("currPlayer " + WhoIsThis() + " capturedPiece " + capturedPiece.tag);
				if(WhoIsThis() != capturedPiece.tag ) {
					
					//if(LegalMoves.jumpList != null)
					if(LegalMoves.jumpList.Contains(to)) {
						
						//removeFromPeiceData(capturedPiece.GetComponent<Piece>().name);
						//GameObject.Destroy(capturedPiece);
						removeFromPeiceData(capturedPiece.GetComponent<Piece>().name);
						capturedPiece.GetComponent<Piece> ().pos = new Vector3(7, -7, -100);
						capturedPiece.GetComponent<Piece> ().transform.position = capturedPiece.GetComponent<Piece> ().pos;
						
						
						
						
						
						
						Debug.Log ("Destroyed " + capturedPiece.tag + " at pos: " + capturedPiece.GetComponent<Piece> ().pos);
					}
					to.z = -0.5F;
					from.transform.position = to;
					
					from.GetComponent<Piece> ().pos = to;
					playerMadeMove = true;
					from.GetComponent<Piece> ().moves.Clear ();
					from.GetComponent<Piece> ().capturedPiece = null;
					LegalMoves.jumpList.Clear ();
					capturedPiece = null;
				}
				else
					Debug.Log ("This is YOUR piece stupid!");
			}
			//This is just for moves that don't involve capturing a piece
			else
			{
				to.z = -0.5f;
				from.transform.position = to;
				
				from.GetComponent<Piece> ().pos = to;
				playerMadeMove = true;
				from.GetComponent<Piece> ().moves.Clear ();
				from.GetComponent<Piece> ().capturedPiece = null;
				LegalMoves.jumpList.Clear ();
				capturedPiece = null;
				
			}
			
			//If player reaches other side of board and isn't already split, then split!
			
			//Make 
			float r = (float)(UnityEngine.Random.Range(2, 12)/2);
			float random = (float)Math.Ceiling(r) * 2;
			int saveMe = 0;
			//Debug.Log ("This works " + boardData[(int)random/2 , 7] + " " + (from.GetComponent<Piece> ().));
			//For White pieces
			if(from.GetComponent<Piece> ().pos.y == 0 && from.tag == "PlayerOne" && !from.GetComponent<Piece> ().isSplit) {
				//If tile is taken, then randomize again
				while(boardData[(int)random/2 , 7].GetComponent<AdjTile> ().isOccupied && saveMe < 20160) {
					//Debug.Log ("Lol occupied");
					r = (UnityEngine.Random.Range(2, 12)/2);
					random = (float)Math.Ceiling(r);
					saveMe++;
				}
				if(saveMe == 20160){}
				//Make piece and tell the board that they have been split
				else {
					GameObject split = (GameObject)Instantiate(from, new Vector3(random, -14, -0.5f), Quaternion.identity);
					from.GetComponent<Piece> ().isSplit = true;
					split.GetComponent<Piece> ().isSplit = true;
				}
			}
			//For Black pieces
			else if(from.GetComponent<Piece> ().row == 7 && from.tag == "PlayerTwo"){
				while(boardData[(int)random/2 , 0].GetComponent<AdjTile> ().isOccupied && saveMe < 20160) {
					//Debug.Log ("Lol occupied");
					r = (UnityEngine.Random.Range(2, 12)/2);
					random = (float)Math.Ceiling(r);
					saveMe++;
				}
				if(saveMe == 20160){}
				else {
					GameObject split = (GameObject)Instantiate(from, new Vector3(random, 0, -0.5f), Quaternion.identity);
					from.GetComponent<Piece> ().isSplit = true;
					split.GetComponent<Piece> ().isSplit = true;
				}
			}
			
			/*
			if(to.y == 0 && from.GetComponent<Piece> ().tag == "PlayerOne")
				Instantiate(from , new Vector3 (Random.Range(1, 13), -14, -0.5f), Quaternion.identity);
				//Instantiate object in random row at that col
			if(to.y == -14 && from.GetComponent<Piece> ().tag == "PlayerTwo")
				Instantiate(from , new Vector3 (Random.Range(1, 13), 0, -0.5f), Quaternion.identity);
				//Instantiate object in random row at that col
				*/
			
		}
		else
		{
			Debug.Log ("THIS IS NOT AN AVAILABLE MOVE");
		}
		
		Debug.LogWarning ("THIS IS TO CHECK IF PLAYERMADE MOVE IS TRUE..... " + playerMadeMove);
		if (playerMadeMove) 
		{
			isPlayerWaiting = true;
			turnCounter++; // this alone can track the player turns in offline mode since it's being mod returned on method call of WhoIsThis()
			playerMadeMove = false;
			//var turnActionsScript = new TurnActions();
			//turnActionsScript.makeTurn();
			//Debug.LogWarning(dicboardData.ToString());
			//int counter = 0;
			
			// Reset our boardState 2d string to null
			gameBoardState = getNullBoard();
			
			
			
			
			
			// Let's store where the peices are now in that string
			for (int i = 0; i < 16; i++)
			{
				// declare local variables so that the values don't have to be fetched through dot operators multiple times if needed
				// This is a minor cost saving optomization
				// ProTip: NEVER EVER optomize before finishing everything up first.
				// There's some sage quote about this, I'm totally breaking this rule, but I'm pretty sure I can get away with it in this case.
				
				if(pieceData[i] !=null)
				{
					int _row = (int) -pieceData[i].GetComponent<Piece>().pos.y/2;
					int _col = (int) pieceData[i].GetComponent<Piece>().pos.x/2;
					string _name = pieceData[i].name;
					
					gameBoardState[_row,_col] = pieceData[i].name;
					//Debug.Log("PIECE DATA: " + _name + "@ " + _row + "x" + _col);
				}
			}
			
			Debug.Log("About to send this to TurnActions: " + getBoardStateToString()); // combines the 2d string array into a single array
			turnToSave=getBoardStateToString();
			makeTurn();
			//var turnActionsScript = new TurnActions();
			//turnActionsScript.makeTurn ();
		}
		
	}
	
	private static void removeFromPeiceData(string removePieceName)
	{
		// Let's store where the peices are now in that string
		for (int i = 0; i < 16; i++)
		{
			if(pieceData[i] !=null)
			{
				string _name = pieceData[i].GetComponent<Piece>().name; // wait, I thought I could only get name from component? errr... some code might be overredudent then...
				if(_name == removePieceName)
				{
					pieceData[i] = null;
				}
			}
		}
		
	}
	public static string getBoardStateToString()
	{
		// William:
		// sadly this is the best way I could come up with last minute to convert a 2d string array to a single string to store in Parse API
		// I need WAY more time from 7-28-2015 to be able to learn how to do this properly/better :(
		string returnString = "";
		for (int i = 0; i < 8; i++) 
		{
			if(i<7)
			{
				for (int j = 0; j < 8; j++) 
				{
					returnString += gameBoardState[i,j] + ",";
					Debug.Log("gameBoardState["+i+","+j+"]=>" + gameBoardState[i,j]);
				}
			}
			else
			{
				for (int j = 0; j < 7; j++) 
				{
					returnString += gameBoardState[i,j] + ",";
					Debug.Log("gameBoardState["+i+","+j+"]=>" + gameBoardState[i,j]);
				}
				returnString += gameBoardState[i,7]; // don't want the last item to have a comma
			}
			
		}
		return returnString;
	}
	
	public static void convertToGameBoard(string boardStateString)
	{
		
		
		
		//boardStateString = "p2_0,p2_1,p2_2,p2_3,p2_4,p2_5,p2_6,p2_7,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,blank,p1_0,p1_1,p1_2,p1_3,p1_4,p1_5,p1_6,p1_7";
		//Debug.LogWarning (boardStateString);
		string[] _boardPieces = boardStateString.Split(',');
		pieceData = new GameObject[16];
		gameBoardState = new string[8,8];
		int _counter = 0;
		int i = 7;
		int j = 7;
		int pieceCounter = 0;
		GameObject _tempGameObject;
		
		// This is kinda... 2am sorta code.
		// But I need to "remove" player pieces (aka move them out of the way)
		string[] _allKnownPlayerPieces = {"p2_0","p2_1","p2_2","p2_3","p2_4","p2_5","p2_6","p2_7","p1_0","p1_1","p1_2","p1_3","p1_4","p1_5","p1_6","p1_7"};
		
		
		foreach (string _knownPiece in _allKnownPlayerPieces)
		{
			// look to see if the known player piece came from parse
			// if not, remove it from the game board visibly
			// (Wait, does that remove it from peiceData, also?)
			bool _foundMatch = false;
			foreach (string _piece in _boardPieces)
			{
				if(_piece == _knownPiece)
				{
					_foundMatch=true;
				}
			}
			
			if(!_foundMatch)
			{
				_tempGameObject = GameObject.Find(_knownPiece);
				_tempGameObject.GetComponent<Piece> ().pos = new Vector3 (-999, -999, -999);
				_tempGameObject.GetComponent<Piece> ().transform.position = _tempGameObject.GetComponent<Piece> ().pos;
			}
		}
		
		
		
		_tempGameObject = null;
		
		
		
		foreach (string _piece in _boardPieces)
		{
			
			i++;
			if(i>7)
			{
				i = 0;
				j++;
			}
			if(j>7)
			{
				j=0;
			}
			
			//Debug.Log("boardData[" + i + ","+j+"] = " + _piece);
			if(_piece.Equals(null) || _piece.Equals("null"))
			{
				_tempGameObject = null;
			}
			else
			{
				// find where the string is that has this name
				// get wherever the hell it is right now (x,y)
				// transform it to the new i,j values
				
				
				// How do I do this properly?
				_tempGameObject = GameObject.Find(_piece);
				//if(boardData[i,j] == null)
				// because the x/y are based on position that includes gaps between pieces this has to be calculated.
				// example: 0,0 = 0,0
				//          0,1 = 0,2 (since there was 1 gap between those spots)
				//          0,2 = 0,4 (since there was 2 gaps between those spots)
				Vector3 positionInGame = _tempGameObject.GetComponent<Piece>().pos;
				//Debug.Log ("Wanting to put down i=" + i + " j=" + j);
				///////////////////////////////////////////////////////////////////////////////
				//// Sanity Check Board Layout
				///////////////////////////////////////////////////////////////////////////////
				////
				///    [00,00]||[02,00]||[04,00]||[06,00]||[08,00]||[10,00]||[12,00]||[14,00]
				///    [00,02]||[02,02]||[04,02]||[06,02]||[08,02]||[10,02]||[12,02]||[14,02]
				///    [00,04]||[02,04]||[04,04]||[06,04]||[08,04]||[10,04]||[12,04]||[14,04]
				///    [00,06]||[02,06]||[04,06]||[06,06]||[08,06]||[10,06]||[12,06]||[14,06]
				///    [00,08]||[02,08]||[04,08]||[06,08]||[08,08]||[10,08]||[12,08]||[14,08]
				///    [00,10]||[02,10]||[04,10]||[06,10]||[08,10]||[10,10]||[12,10]||[14,10]
				///    [00,12]||[02,12]||[04,12]||[06,12]||[08,12]||[10,12]||[12,12]||[14,12]
				///    [00,14]||[02,14]||[04,14]||[06,14]||[08,14]||[10,14]||[12,14]||[14,14]
				/// 
				///////////////////////////////////////////////////////////////////////////////
				Debug.LogWarning("Was at: " + (int) positionInGame.x + ", " + (int)positionInGame.y);
				//Debug.LogWarning("Was at: " + (int) pieceData[i,j].pos.x + ", " + (int)positionInGame.y);
				
				// was: 6,-14
				// needs to be: 6,-4
				// is    : -6, -4
				
				// i = x, j = y
				positionInGame = new Vector3(i*2,-1*(j*2), positionInGame.z);
				
				_tempGameObject.GetComponent<Piece>().pos = positionInGame;
				_tempGameObject.GetComponent<Piece>().transform.position = positionInGame;
				Debug.LogWarning("Now at: " + (int) positionInGame.x + ", " + (int)positionInGame.y);
				pieceData[pieceCounter] = _tempGameObject;
				pieceCounter++;
			}
			
			
			
			
			_counter++;
		}
		Debug.Log (_counter);
		
		
	}
	
	public static void printBoardData()
	{
		///////////////////////////////////////////////////////////////////////////////
		//// Sanity Check Board Layout
		///////////////////////////////////////////////////////////////////////////////
		////
		///    [00,00]||[02,00]||[04,00]||[06,00]||[08,00]||[10,00]||[12,00]||[14,00]
		///    [00,02]||[02,02]||[04,02]||[06,02]||[08,02]||[10,02]||[12,02]||[14,02]
		///    [00,04]||[02,04]||[04,04]||[06,04]||[08,04]||[10,04]||[12,04]||[14,04]
		///    [00,06]||[02,06]||[04,06]||[06,06]||[08,06]||[10,06]||[12,06]||[14,06]
		///    [00,08]||[02,08]||[04,08]||[06,08]||[08,08]||[10,08]||[12,08]||[14,08]
		///    [00,10]||[02,10]||[04,10]||[06,10]||[08,10]||[10,10]||[12,10]||[14,10]
		///    [00,12]||[02,12]||[04,12]||[06,12]||[08,12]||[10,12]||[12,12]||[14,12]
		///    [00,14]||[02,14]||[04,14]||[06,14]||[08,14]||[10,14]||[12,14]||[14,14]
		/// 
		///////////////////////////////////////////////////////////////////////////////
		for (int i = 0; i<8; i++) {
			for(int j = 0; j<8; j++)
			{
				Debug.Log ("[00,00]||[02,00]||[04,00]||[06,00]||[08,00]||[10,00]||[12,00]||[14,00]");
			}
		}
	}
	
	public static GameObject ObjectAt (float x, float y, float z) {
		Vector3 pos = new Vector3 (x, y, z);
		return dicboardData [pos];
	}
	
	public static string PlayerAtSpace (float row, float col) {
		Vector3 pos = new Vector3 (row, col, -0.5f);
		return " ";
	}
	
}
