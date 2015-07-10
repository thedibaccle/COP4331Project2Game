using UnityEngine;
using System.Collections;

public class Moves : MonoBehaviour {
	int fromRow, fromCol, toRow, toCol;
	// Use this for initialization
	Moves(int r1, int r2, int c1, int c2) {
		fromRow = r1;
		toRow = r2;
		fromCol = c1;
		fromCol = c2;
	}

	bool IsOnlyWarp () {
	}

	bool IsJump () {
	}

	bool IsJumpWarp () {
	}

	bool IsWarpJump () {
	}


}
