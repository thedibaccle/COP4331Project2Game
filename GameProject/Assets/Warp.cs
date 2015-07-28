using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Warp : MonoBehaviour {
	
	/*TODO:After every turn, warp checks if a piece is adjacent to it, if so, return relative coords (ie: (1,0))
	 * After end of turn, checks if it is currently colliding with a player piece, return boolean
	 * If piece selected is on warp, have small animation. Double tap to confirm warp
	 */
	// Use this for initialization
	//public Dictionary<Vector2, AdjTile> tiles = new Dictionary<Vector2, AdjTile>();
	public Vector3 pos;
	public float row, col;
	Piece piece;
	public static GameObject warp;
	public GameObject linkedWarp;
	public GameObject collidingWith = null;
	
	public bool isOccupied = false;
	
	void Start ()
	{
		pos = transform.position;
		row = pos.x / 2;
		col = -pos.y / 2;
		warp = this.gameObject;
		//Board.dicboardData.Add (pos, this.gameObject);
		Board.boardData [(int)row, (int)col] = this.gameObject;
		linkedWarp = Board.boardData [14 - (int)row, -14 - (int)col];
		//Board.cacheBoardData [(int)row, (int)col] = this.gameObject;
		this.gameObject.AddComponent<Rigidbody> ();
		this.gameObject.GetComponent<Rigidbody> ().useGravity = false;
		this.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionZ;
		this.gameObject.GetComponent<Rigidbody> ().mass = 9999999;
		
		
	}
	
	// Update is called once per frame
	void Update (){
		
	}
	
	public void anPossibleMove () {
	}
	
	public static float GetAltRow(int row) {
		return 7 - row;
	}
	
	public static float GetAltCol(int col) {
		return - (7 - col);
	}
	
	void OnMouseUp () {
		Debug.Log ("Piece is tapped?: " + Piece.tapped);
		if (Board.currPlayer == Piece.piece.tag) {
			Debug.Log ("Warp is selected" + Piece.selectedPiece + " " + pos);
			//Piece.piece.GetComponent<Piece> ().moves = Piece.piece.GetComponent<Piece> ().legalMoves.getLegalMoves(Piece.piece);//new LegalMoves().getLegalMoves(Piece.piece);
			for (int i = 0; i < Piece.piece.GetComponent<Piece> ().moves.Count; i++)
				Debug.Log ("Move #" + i + "for " + Piece.piece.tag + ": " + Piece.piece.GetComponent<Piece> ().moves[i]);
			//Debug.Log (Piece.selectedPiece + " " + pos);
			if (Piece.tapped && !isOccupied) {
				Piece.tapped = false;
				Board.Move (Piece.piece, pos);
				
			}
			
			//Debug.Log (Piece.selectedPiece);
		} else if (!Piece.tapped)
			Debug.Log ("Select a piece first.");
		else
			Debug.Log ("Not this player's turn!");
	}
	
	
	void OnCollisionEnter (Collision c){
		//Update LegalMoves list
		
		if (c.gameObject.tag == "PlayerOne" || c.gameObject.tag == "PlayerTwo") {
			collidingWith = c.gameObject;
			isOccupied = true;
			Debug.Log ("Warp is colliding!");
			//Piece.isOnWarp = true;
		}
	}
	
	void OnCollisionExit (Collision c){
		//Piece.isOnWarp = false;
		collidingWith = null;
		isOccupied = false;
	}
}