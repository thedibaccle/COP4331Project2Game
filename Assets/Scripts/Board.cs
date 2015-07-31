using UnityEngine;
using System.Collections;
using Parse;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;
using System.Collections.Generic; // need this STUPID THING so IENumerable works within result outputs. Makes no damn sense since cothreads are IENumerable FFS!

//using UnityEngine.EventSystems.IPointerClickHandler;

public class Board : MonoBehaviour {
	public static bool isOnlineMode = true; // MODIFY THIS TO TEST ONLINE OR OFFLINE SINGLE PLAYER MODE

	public static bool isInitialized = false; // used to check if Board was ever loaded once before
	private static string _currPlayer;

	public static GameObject[,] boardData = new GameObject[8, 8];
	public static GameObject[] pieceData = new GameObject[32];
	public static bool[, ] isPieceThere;
	public static GameObject capturedPiece = null;
	public static Dictionary<Vector3, GameObject> dicboardData = new Dictionary<Vector3, GameObject>();
	public LegalMoves legalMoves = new LegalMoves();
	public static bool playerMadeMove;
	public static string[,] gameBoardState = getNullBoard ();
	public static bool isPlayerWaiting = true; // enforce this as locked until parse says otherwise.

	// William:
	// Tried to get a ToString() of the object's name from peiceData but the name was CONSTANTLY returning a null reference.
	// So the positions of the peices are being stored in this string instead so that parse can set/get this
	public static string peicePlacement = "";
	public static int turnCounter = 0;
	private static bool playerInitialized = false;
	
	// Use this for initialization
	void Start () {
		//1D GameObject array [64]
		//Only checks adjacent tiles for ONE warp
		//I have to instantiate all of the warps? Noooooooooooooooooooooo
		//Link paired warp tiles together via gameobject variable
		//Physics.gravity = new Vector3 (0, 0, 1);


		GameObject[,] boardData = new GameObject[8, 8];
		GameObject[] pieceData = new GameObject[32];
		bool[, ] isPieceThere;
		GameObject capturedPiece = null;
		Dictionary<Vector3, GameObject> dicboardData = new Dictionary<Vector3, GameObject>();
		LegalMoves legalMoves = new LegalMoves();
		bool playerMadeMove;
		string[,] gameBoardState = getNullBoard ();
		bool isPlayerWaiting = true; // enforce this as locked until parse says otherwise.

		if (!isOnlineMode) 
		{
			_currPlayer = "PlayerOne";
		} else 
		{
			// check if the player is logged in still, if not, send them back to login
			if(ParseUser.CurrentUser == null)
			{
				Application.LoadLevel("scnMainMenu");
			}
		}
		//moves = legalMoves.getLegalMoves(currPlayer, boardData);
	}

	public static void setupCurrPlayer(int thisPlayer)
	{
		// thisPlayer int should be either 1 or 2
		if (!playerInitialized) {
			playerInitialized = true;
			_currPlayer = (thisPlayer % 2 == 0) ? "PlayerTwo" : "PlayerOne"; // this might be flipped, not sure
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
	



	public static string WhoIsThis()
	{
		if (!isOnlineMode) {
			return (turnCounter % 2 == 0) ? "PlayerOne" : "PlayerTwo";
		} else
		{
			return Board._currPlayer;
		}

	}

	//needs a bool to make sure player can only select one piece at a time

	void Update ()
	{
		// SINGLE PLAYER MODE (USE THIS)
		//if (playerMadeMove) {
		//	currPlayer = WhosTurnIsItNow (count);
		//	playerMadeMove = false;
		//}

				//Json.JsonConvert.SerializeObject(pieceData);
			//Debug.LogWarning(Board.gameBoardState);
					
			// TODO: might need a performing move lockout here so that other pieces can't be moved while the thread for making the move takes place

	
		//Debug.Log (currPlayer);
		//Continuously checks if user is selecting a piece
		//have the current player's pieces bob
		//highlight the piece that is being selected
	}

	/*TODO~~: make toString for ALL of possible moves
	 * Convert to string
	 * 
	 * 
	 * 
	 */

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
				Debug.Log ("captured piece: " + capturedPiece.GetComponent<Piece> ().pos);
				Debug.Log ("currPlayer " + WhoIsThis() + " capturedPiece " + capturedPiece.tag);
				if(WhoIsThis() != capturedPiece.tag ) {


						
					//if(LegalMoves.jumpList != null)
					if(LegalMoves.jumpList.Contains(to)) {
						//GameObject.Destroy(capturedPiece);
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
			Debug.Log ("THIS IS NOT AN AVAILABLE FUCKING MOVE YOU STUPID PIECE OF SHIT");
		}

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
					Debug.LogWarning("PIECE DATA: " + _name + "@ " + _row + "x" + _col);
				}
			}

			Debug.Log(getBoardStateToString()); // combines the 2d string array into a single array

			makeTurn();
			//var turnActionsScript = new TurnActions();
			//turnActionsScript.makeTurn ();
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
				}
			}
			else
			{
				for (int j = 0; j < 7; j++) 
				{
					returnString += gameBoardState[i,j] + ",";
				}
				returnString += gameBoardState[i,7]; // don't want the last item to have a comma
			}

		}
		return returnString;
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
	