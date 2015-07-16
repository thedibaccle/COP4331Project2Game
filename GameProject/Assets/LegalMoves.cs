using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LegalMoves : MonoBehaviour {

	int fromRow, fromCol, toRow, toCol;
	GameObject[, ] board;
	/*// Use this for initialization
	public LegalMoves(int r1, int r2, int c1, int c2) {
		fromRow = r1;
		toRow = r2;
		fromCol = c1;
		fromCol = c2;
	}
	*/
	public Moves[] getLegalMoves (string currPlayer , GameObject[,] b){
		List<Moves> lMoves = new List<Moves> ();
		board = b;
		for(int row = 0; row < 8; row++)
			for(int col = 0; col < 8; col++) {
			if(board[row, col].name == currPlayer) {

				for(int i = -1; i < 2; i++)
					for(int j = -1; j < 2; j++) {
					Debug.Log (CanMove (row + i, col + j));
						if(CanMove ( row + i, col + j) )
					   		lMoves.Add ( new Moves( row, col, row+i, col+j) );
				}
			}
		}


		if (lMoves.Count == 0)
			return null;

		//How do I get specific items from List?
		Moves[] aMoves = new Moves[lMoves.Count];
		lMoves.CopyTo (aMoves);

		return aMoves;
	}


	bool IsInBounds (int r, int c)
	{
		if( r < 0 || r > 7 || c < 0 || c > 7)
			return false;

		return true;
	}

	bool PlayerInSpace (int r, int c) {
		if (board [r, c].name == "PlayerOne" || board [r, c].name == "PlayerTwo")
			return true;

		return false;
	}

	bool IsAdjacentTileEmpty ()
	{
		return false;
	}

	bool CanMove (int r, int c)
	{
		if(!IsInBounds(r, c) || PlayerInSpace(r, c))
			return false;

		return true;
	}

	bool CanJump ()
	{
		return false;
	}

	bool OnlyWarp ()
	{
		return false;
	}

	bool CanWarp ()
	{
		return false;
	}

	bool CanJumpWarp ()
	{
		return false;
	}

	bool CanWarpJump ()
	{
		return false;

	}

}
	