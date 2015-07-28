﻿using UnityEngine;
using System.Collections;

public class AdjTile : MonoBehaviour
{
	public Vector3 pos;
	public Piece piece;
	public bool isOccupied = false;
	public static GameObject objTile;
	public Rigidbody rigBod;
	//Put this in board or warp?
	public Vector2[,] aTilePositions = new Vector2[8,8];
	public float row, col;
	public GameObject collidingWith = null;

	void Start () {
		pos = transform.position;
		row = pos.x / 2;
		col = -pos.y / 2;
		objTile = this.gameObject;
		Board.dicboardData.Add (pos, this.gameObject);
		Board.boardData [(int)row, (int)col] = this.gameObject;
		this.gameObject.AddComponent<Rigidbody> ();
		this.gameObject.GetComponent<Rigidbody> ().useGravity = false;
		this.gameObject.GetComponent<Rigidbody> ().constraints = RigidbodyConstraints.FreezePositionZ;

	}
	
	// Update is called once per frame
	void Update ()
	{
		Board.boardData [(int)row, (int)col] = this.gameObject;
		//Debug.Log (Piece.selectedPiece);
	}
	
	
	
	public void anPossibleMove () {
		//Make the tile spin if it is a possible move
	}
	
	public void getPositionInBoardData () {
		row = pos.x/2;
		col = -pos.y/2;	
		
	}

	void OnMouseUp () {
		Debug.Log ("Piece is tapped?: " + Piece.tapped);
			//Piece.piece.GetComponent<Piece> ().moves = Piece.piece.GetComponent<Piece> ().legalMoves.getLegalMoves(Piece.piece);//new LegalMoves().getLegalMoves(Piece.piece);
			for (int i = 0; i < Piece.piece.GetComponent<Piece> ().moves.Count; i++)
				Debug.Log ("Move #" + i + "for " + Piece.piece.tag + ": " + Piece.piece.GetComponent<Piece> ().moves[i]);
		
			Debug.Log ("Tile is selected" + Piece.selectedPiece + " " + pos);
			//If piece is tapped, and this isn't occupied AND iti
			if (Piece.tapped && !isOccupied) {
				Piece.tapped = false;
				Board.Move (Piece.piece, pos);
			}
			
			//Debug.Log (Piece.selectedPiece);
			else if (!Piece.tapped)
			Debug.Log ("Select a piece first.");
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