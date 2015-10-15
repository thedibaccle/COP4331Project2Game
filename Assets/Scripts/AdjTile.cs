using UnityEngine;
using System.Collections;

public class AdjTile : MonoBehaviour
{
	public Vector3 pos;
	//public Piece piece;
	public bool isOccupied = false;
	//public static GameObject objTile;
	public float row, col;
	public GameObject collidingWith = null;
	//bool wasPieceTapped = Piece.tapped;

	void Start () {
		pos = transform.position;
		pos.z = 0;
		pos.y = (float)(int)pos.y;
		row = pos.x / 2;
		col = -pos.y / 2;
		//objTile = this.gameObject;
		Board.boardData [(int)row, (int)col] = this.gameObject;
		//Board.cacheBoardData [(int)row, (int)col] = this.gameObject;
		if (this.gameObject.GetComponent<Rigidbody>() == null) {
			this.gameObject.AddComponent<Rigidbody> ();
			this.gameObject.GetComponent<Rigidbody> ().useGravity = false;
			this.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionZ;
			this.gameObject.GetComponent<Rigidbody> ().mass = 9999999;
		}

	}
	 
	// Update is called once per frame
	void Update ()
	{
		//Debug.Log (Piece.selectedPiece);
	}



	public void anPossibleMove () {
		//Make the tile spin if it is a possible move
	}
	

	void OnMouseUp () {
		Debug.Log ("Piece is tapped?: " + Piece.tapped);
		if (Board.WhoIsThis() == Piece.piece.tag) {
			//Piece.piece.GetComponent<Piece> ().moves = Piece.piece.GetComponent<Piece> ().legalMoves.getLegalMoves(Piece.piece);//new LegalMoves().getLegalMoves(Piece.piece);
			for (int i = 0; i < Piece.piece.GetComponent<Piece> ().moves.Count; i++)
				Debug.Log ("Move #" + i + "for " + Piece.piece.tag + ": " + Piece.piece.GetComponent<Piece> ().moves[i]);
		
			Debug.Log ("Tile is selected" + Piece.selectedPiece + " " + pos);
			//If piece is tapped, and this isn't occupied AND iti
			if (Piece.tapped && !isOccupied) {
				Piece.tapped = false;
				Board.Move (Piece.piece, pos);
				//Board.pieceData [(int)pos.x / 2, -(int) pos.y / 2] = Piece.piece;
				//Board.pieceData [(int)Piece.piece.GetComponent<Piece> ().row, (int)Piece.piece.GetComponent<Piece> ().col] = null;
			}

			//Debug.Log (Piece.selectedPiece);
		} else if (!Piece.tapped)
			Debug.Log ("Select a piece first.");
		else
			Debug.Log ("Not this player's turn!");
	}

	void OnCollisionEnter (Collision c)
	{
		if (c.gameObject.tag == "PlayerOne" || c.gameObject.tag == "PlayerTwo") {
			collidingWith = c.gameObject;

			Debug.Log ("Tile Is colliding at " + pos);
			//Piece.isOnTile = true;
			isOccupied = true;
		}
	}

	void OnCollisionExit (Collision c)
	{
		//Piece.isOnTile = false;
		collidingWith = null;
		isOccupied = false;
	}
	
}
