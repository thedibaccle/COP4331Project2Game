using UnityEngine;
using System.Collections;
using Parse;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;
using System.Collections.Generic; // need this STUPID THING so IENumerable works within result outputs. Makes no damn sense since cothreads are IENumerable FFS!

//using UnityEngine.EventSystems.IPointerClickHandler;

public class Board : MonoBehaviour {
	//public AdjTile adjTile;
	public static GameObject[,] boardData = new GameObject[8, 8];
	public static GameObject[] pieceData = new GameObject[16];
	public static bool[, ] isPieceThere;
	public static GameObject capturedPiece = null;
	public static Dictionary<Vector3, GameObject> dicboardData = new Dictionary<Vector3, GameObject>();
	//public Warp warpTile;
	//public Piece piece;
	//public GameObject warp, tile, player;
	public static bool playerMadeMove;
	//public int WhosTurnIsIt = 0;
	//Maximum number of pieces on board is 32
	public Moves[] moves;
	public LegalMoves legalMoves = new LegalMoves();
	//float setAtRow = -1.0F, setAtCol;

	public static string[,] gameBoardState = getNullBoard ();
	public static bool isPlayerWaiting = true;

	// the underscore indicates (sorta!!) that it's supposed to be a private/local variable NOT to be accessed outside of declaration scope
	// It's typically used as a head's up to other programmers that it's meant to be this way SOMETIMES. It's a weird unwritten rule.
	// In other words, just a heads up of what/why
	private static string _currPlayer;

	private static bool isOnlineMode = false; // MODIFY THIS TO TEST ONLINE OR OFFLINE SINGLE PLAYER MODE

	/// Variables from TurnActions
	private static void makeTurn()
	{
		// This performs the online parse move making
		if (isOnlineMode) {

			// This will make the move, and afterwards move to the scnWaiting or perform the script code from that scene
			// the waiting scene/code whichever, will end up being what sets isPlayerWaiting to false once it is this player's turn again.
			Debug.LogWarning ("!!! GOING INTO NEW AREA");
			var test = new TurnActions ();
			test.makeTurn ();
		} else
		{

			Board._currPlayer = WhoIsThis();  // set the player to the next one (if it wasn't done already)
			isPlayerWaiting = false; // tell the game that this person ACTUALLY using this app, doesn't have to wait any longer to make a move
		}
	}

	///////////////////////////////




	// William:
	// Tried to get a ToString() of the object's name from peiceData but the name was CONSTANTLY returning a null reference.
	// So the positions of the peices are being stored in this string instead so that parse can set/get this
	public static string peicePlacement = "";

	public static int turnCounter = 0;

	private static bool playerInitialized = false;
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
	// Use this for initialization
	void Start () {
		//1D GameObject array [64]
		//Only checks adjacent tiles for ONE warp
		//I have to instantiate all of the warps? Noooooooooooooooooooooo
		//Link paired warp tiles together via gameobject variable
		//Physics.gravity = new Vector3 (0, 0, 1);
		if (!isOnlineMode){_currPlayer = "PlayerOne";}
		//moves = legalMoves.getLegalMoves(currPlayer, boardData);
	}



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
		/*TODO: MAKE SURE THAT THE ANIMATIONS ARE ONLY FOR THE PIECES SELECTED
		 * 	HAVE A COPY OF THE ORIGINAL boardData, SO WHEN THE PIECES MOVE, THE TILES UNDERNEATH
		 *  WILL HAVE THEIR COORDS RESTORED
		 * 
		 */


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
		Debug.Log ("What are each of the available moves?");


		Debug.Log ("Location player wants to go: " + to );
		Debug.Log ("Does moves contain to?..." + from.GetComponent<Piece> ().moves.Contains (to));
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
				from.transform.position = to;

				from.GetComponent<Piece> ().pos = to;
				playerMadeMove = true;
				from.GetComponent<Piece> ().moves.Clear ();
				from.GetComponent<Piece> ().capturedPiece = null;
				LegalMoves.jumpList.Clear ();
				capturedPiece = null;

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
		//boardData.Add (to, from);
		//from.transform.position = Vector3.MoveTowards(from.GetComponent<Piece> ().pos , to , Time.deltaTime * 10);
		/*
		for(int i = 0; i < 8; i++)
			for(int j = 0; j < 8; j++)
				//Debug.Log ( "BOARD DATA: " + boardData[i, j]);
		*/


		if (playerMadeMove) 
		{
			isPlayerWaiting = true;
			turnCounter++; // this alone can track the player turns in offline mode since it's being mod returned on method call of WhoIsThis()
			//playerMadeMove = false;
			//var turnActionsScript = new TurnActions();
			//turnActionsScript.makeTurn();
			//Debug.LogWarning(dicboardData.ToString());
			int counter = 0;

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
	