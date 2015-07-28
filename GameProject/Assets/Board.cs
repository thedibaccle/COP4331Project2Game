using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.EventSystems.IPointerClickHandler;

public class Board : MonoBehaviour {
	//public AdjTile adjTile;
	public static GameObject[,] boardData = new GameObject[8, 8];
	public static GameObject capturedPiece = null;
	public static Dictionary<Vector3, GameObject> dicboardData = new Dictionary<Vector3, GameObject>();
	//public GameObject warp, tile, player;
	public static bool playerMadeMove;
	public static string currPlayer;
	int count = 0;
	
	// Use this for initialization
	void Start () {
		//1D GameObject array [64]
		//Only checks adjacent tiles for ONE warp
		//I have to instantiate all of the warps? Noooooooooooooooooooooo
		//Link paired warp tiles together via gameobject variable
		//Physics.gravity = new Vector3 (0, 0, 1);
		
		currPlayer = "PlayerOne";
		//moves = legalMoves.getLegalMoves(currPlayer, boardData);
	}
	
	
	public string WhosTurnIsItNow (int c){
		//Debug.Log (c);
		bool b = (c % 2 == 0);
		count++;
		if(b)
			return "PlayerTwo";
		return "PlayerOne";
	}
	//needs a bool to make sure player can only select one piece at a time
	
	void Update () {
		/*TODO: MAKE SURE THAT THE ANIMATIONS ARE ONLY FOR THE PIECES SELECTED
		 * 	HAVE A COPY OF THE ORIGINAL boardData, SO WHEN THE PIECES MOVE, THE TILES UNDERNEATH
		 *  WILL HAVE THEIR COORDS RESTORED
		 * 
		 */
		
		if (playerMadeMove) {
			currPlayer = WhosTurnIsItNow (count);
			playerMadeMove = false;
		}
		
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
	
	public static void Move (GameObject from, Vector3 to) {
		//while (from.GetComponent<Piece> ().pos != to)
		//Debug.Log ("MOVE DBUG: " + from.GetComponent<Piece> ().pos);
		//boardData.Remove (from.GetComponent<Piece> ().pos);
		Debug.Log ("IN MOVE.... HOW MANY AVAILABLE MOVES DO WE HAVE...." + from.GetComponent<Piece> ().moves.Count);
		Debug.Log ("What are each of the available moves?");
		
		
		Debug.Log ("Location player wants to go: " + to );
		Debug.Log ("Does moves contain to?..." + from.GetComponent<Piece> ().moves.Contains (to));
		if (from.GetComponent<Piece> ().moves.Contains(to)) {
			capturedPiece = from.GetComponent<Piece> ().capturedPiece;
			//Debug.Log ("Piece in the way..." + capturedPiece.tag);
			Debug.Log ("to: " + to + LegalMoves.jumpList.Contains(to));
			//if player clicked on the jump square, then destroy the captured piece)
			//Debug.Log ("captured piece name: " + capturedPiece.tag);
			
			//First, check if the piece has a captured Piece, which means that the enemy piece is adjacent
			if(capturedPiece != null) {
				Debug.Log ("captured piece: " + capturedPiece.GetComponent<Piece> ().pos);
				Debug.Log ("currPlayer " + currPlayer + " capturedPiece " + capturedPiece.tag);
				if(currPlayer != capturedPiece.tag ) {
					
					
					
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
			else {
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
			
		} else
			Debug.Log ("THIS IS NOT AN AVAILABLE FUCKING MOVE YOU STUPID PIECE OF SHIT");
		//boardData.Add (to, from);
		//from.transform.position = Vector3.MoveTowards(from.GetComponent<Piece> ().pos , to , Time.deltaTime * 10);
		/*
		for(int i = 0; i < 8; i++)
			for(int j = 0; j < 8; j++)
				//Debug.Log ( "BOARD DATA: " + boardData[i, j]);
		*/
		
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