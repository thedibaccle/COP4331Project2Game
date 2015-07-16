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
	Vector2 pos;
	Piece piece;
	public GameObject objWarp;

	public bool isOccupied = false;

	void Start ()
	{
		pos = transform.position;

	}
	
	// Update is called once per frame
	void Update (){
		//Use pos.x && pos.y to see if there is a collision
		for (int i = -1; i <= 1; i++)
			for (int j = -1; j <= 1; j++) {

			  
			}

	}

	public void anPossibleMove () {
	}

	public int GetAltRow(int row) {
		return 7 - row;
	}

	public int GetAltCol(int col) {
		return 7 - col;
	}


	void OnCollisionEnter (Collision collision){
		//Update LegalMoves list
		isOccupied = true;
	}

	void OnCollisionExit (Collision collision){
		isOccupied = false;
	}
}
