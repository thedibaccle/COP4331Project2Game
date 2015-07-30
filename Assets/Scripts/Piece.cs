using UnityEngine;
using System.Collections.Generic;
using System.Collections;


[RequireComponent(typeof(BoxCollider))]
public class Piece: MonoBehaviour {
	public static Vector3 selectedPiece;
	public GameObject capturedPiece = null;
	public static GameObject piece; 
	//GameObject[,] boardData;
	//public Board board;
	public Vector3 pos;
	public float row, col;
	//Moves objMoves;
	public List<Vector3> moves;
	public LegalMoves legalMoves = new LegalMoves();
	//public static bool isOnWarp, isOnTile;
	//public static bool playerMadeMove;
	//public string name;
	public static bool tapped;
	public bool isSplit = false;
	public static int pieceCounter = 0;
	// Use this for initialization
	void Start () {
		pos = transform.position;
		pos.y = (float)((int)pos.y);
		getPositionInBoardData ();
		//boardData = board.boardData;
		//selectedPiece = transform.position;
		getName (name);
		piece = this.gameObject;
		this.gameObject.AddComponent<Rigidbody> ();
		this.gameObject.GetComponent<Rigidbody> ().useGravity = false;


		//piece.GetComponent<Rigidbody> ().AddExplosionForce (500, pos, 2);

		Board.pieceData [pieceCounter] = this.gameObject;
		pieceCounter++;
		Board.dicboardData.Add (pos, this.gameObject);
		Board.gameBoardState[(int)row, (int)col] = this.name;
		//Board.peicePlacement += this.name + "@" + (-this.pos.y/2) + "x" + this.pos.x/2;
		//Board.boardData [(int)row, (int)col] = this.gameObject;
		//isOnWarp = false;
		//isOnTile = true;
	}
	
	public void getName(string s) {
		name = s;
	}

	// Update is called once per frame
	void Update () {
		//transform.position = selectedPiece;
		//pos = transform.position;
		getPositionInBoardData ();
		//piece = this.gameObject;
	}


	public void OnMouseDown () {
		//Board.ShowLegalMoves ();
		//Use this to make ALL OF THE INSTANTS OF tapped
		tapped = true;


	}
	
	public void OnMouseUp () {
		//objMoves = new Moves ();
		//base.PrintBoardData ();
		piece = this.gameObject;
		Debug.Log ("Selecting: " + gameObject.tag + "where row: " + pos.y + " and col: " + pos.x);

		//if(moves.Count == 0)
			moves  = new LegalMoves().getLegalMoves (piece);

		/*TODO: Check if first click
		 *Is this the second click
		 *Is it selected twice
		 *Is this different piece
		 *If different, is move legal?
		 *if move legal, move first piece to second position
		 * 
		 */

		//if (selectedPiece != null ) {
			selectedPiece = transform.position;

			Debug.Log (selectedPiece);
	

	}
	

	public void getPositionInBoardData () {
		row = -pos.y/2;
		col = pos.x/2;
		
	}

	void OnCollisionEnter (Collision c) {
		//Debug.Log ("Colliding with: " + c.collider.gameObject.name);
		//if (c.gameObject.name == "Tile") {
		//	Debug.Log ("Piece hitting tile");
			//c.gameObject.GetComponent<AdjTile> ().collidingWith = this.gameObject;
			//Destroy (c.gameObject);

	}

	void OnCollisionExit (Collision c) {
	}

	public void anCurrPlayerBob () {

	}

	public void anSelected () {
	}

	void anMove (Vector3 to){
		
	}


}
