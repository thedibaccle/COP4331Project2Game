using UnityEngine;
using System.Collections;

public class Moves : MonoBehaviour {
	//public Warp warp = new Board.
	public float fromRow, fromCol, toRow, toCol;
	// Use this for initialization
	public Moves(int r1, int r2, int c1, int c2) {
		fromRow = r1;
		toRow = r2;
		fromCol = c1;
		toCol = c2;
	}

	void MakeMove (Moves move) {

	}

	//	private static bool firstMoveMade = false;

	/*
	bool IsOnlyWarp (Warp warp) {
		return (fromRow == warp.GetAltRow(toRow) && fromCol == warp.GetAltCol(toCol));
	}

	bool IsJump () {
		//Checks if it's a regular jump
		return (Mathf.Abs (fromRow - toRow) == 2 || Mathf.Abs(fromCol - toCol) == 2);
	}

	bool IsJumpWarp () {
		return false;
	}

	bool IsWarpJump () {
		return false;
	}
*/

}
