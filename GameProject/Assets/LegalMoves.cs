using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LegalMoves {

	public GameObject capturedPiece = null;
	int fromRow, fromCol, toRow, toCol;
	GameObject possibleWarp, adjSpace, jumpSpace;
	float onlyWarpRow, onlyWarpCol;
	bool isEnemy = false;
	//public static bool isJump = false;
	public static List<Vector3> jumpList = new List<Vector3>();
	//AdjTile tile;
	//Piece piece;
	/*// Use this for initialization
	public LegalMoves(int r1, int r2, int c1, int c2) {
		fromRow = r1;
		toRow = r2;
		fromCol = c1;
		fromCol = c2;
	}
	*/
	public List<Vector3> getLegalMoves (GameObject selectedPiece){
		/*LegalMoves is called by Tile when it's clicked.
		 * Tile is the destination of the selected piece
		 * LegalMoves checks all of the possibilities how the piece can
		 * legally make that move
		 * ----------------
		 * should legal moves check for each tile dynamically or just have a list of Vector3's 
		 * that the given piece is allowed to move?.... definitely the latter
		 */
		List<Vector3> moves = new List<Vector3>();
		//Debug.Log ("Wait is this even called?");
		int row = (int)selectedPiece.GetComponent<Piece> ().pos.x;
		int col = (int)selectedPiece.GetComponent<Piece> ().pos.y;

		possibleWarp = Board.boardData [row/2, -col/2];
		Debug.Log ("Possible Warp that piece is on: " + possibleWarp);

		if (OnlyWarp ()) {
			Debug.Log ("warp row: " + onlyWarpRow + " warp col: " + onlyWarpCol);
			moves.Add (new Vector3 (onlyWarpRow * 2, - onlyWarpCol*2, 0));
		}

		for (int i = -1; i < 2; i++)
			for (int j = -1; j < 2; j++) {
				//Debug.Log ("Checking if adjacent tile search is in bounds  " + (row + i*2) + " " + (col + j*2));
				if (IsInBounds (row + i * 2, - col - j * 2)) {

					//Debug.Log ("I'M IN MUTHAFUCKA");
					adjSpace = Board.boardData [row / 2 + i, (-col / 2) - j];
				//Debug.Log ("boardData pos: " + (row /2 + i) + " " + ( -col / 2 - j));
				Debug.Log ("a pos: " + (row + i*2) + " " + (col + j*2));
				if(adjSpace.tag == "Tile")
				Debug.Log ("Now that we know the position, is it colliding with anything? " + adjSpace.GetComponent<AdjTile> ().collidingWith);
				if(adjSpace.tag == "Warp")
					Debug.Log ("Now that we know the position, is it colliding with anything? " + adjSpace.GetComponent<Warp> ().collidingWith);
					//Debug.Log ("What is space!?    tag: " + space.tag + " pos: " + (row + i*2) + " " + (col + j*2));
					//Debug.Log ("boardData's relative row/col: " + (row/2+i) + " " + (-(col/2)-j));
					if (CanMove ())
						moves.Add (new Vector3 (row + i * 2, col + j * 2, 0));

					if (IsInBounds (row + i * 4, - col - j * 4)) {

						jumpSpace = Board.boardData [row / 2 + i * 2, (-col / 2) - j * 2];
						Debug.Log ("This jumpSpace is a " + jumpSpace);
						if (CanJump ()) {
						selectedPiece.GetComponent<Piece> ().capturedPiece = capturedPiece;
						//isJump = true;
							jumpList.Add (new Vector3 (row + i * 4, col + j * 4, 0));
							moves.Add (new Vector3 (row + i * 4, col + j * 4, 0));
						}

					}

					possibleWarp = Board.boardData [row / 2 + i, - col / 2 - j];
				if (possibleWarp.tag == "Warp" && IsInBounds (getAltRow (row) / 2 + i, -getAltCol (col) / 2 - j)) {
					//This will give the position of all of the adjacent tiles
			
					//This gives the inverse position of the selected piece
					jumpSpace = Board.boardData [getAltRow (row) / 2, - getAltCol (col) / 2];
					Debug.Log ("JumpSpace checking for canWarpJump and canJumpWarp. Pos: " + (getAltRow (row) / 2 + i) + " " + (- getAltCol (col) / 2 - j));
					if (CanWarp ())
						moves.Add (new Vector3 (onlyWarpRow * 2, - onlyWarpCol * 2, 0));

					//This means that the selected piece is adjacent to the captured piece, then warps to the other side
					if (CanJumpWarp ()) {
						Debug.Log ("YES YOU CAN JUMP WARP ALL IS WELL IN THE WORLD!!!!!11");
						selectedPiece.GetComponent<Piece> ().capturedPiece = capturedPiece;
						jumpList.Add (new Vector3 (getAltRow (row), getAltCol (col), 0));
						moves.Add (new Vector3 (getAltRow (row), getAltCol (col), 0));
						//moves.Add (new Vector3((onlyWarpRow + i)*2, (- onlyWarpCol - j)*2));
					}
			
					if (CanWarpJump ()) {
						Debug.Log ("HOOOOOORAY");
						selectedPiece.GetComponent<Piece> ().capturedPiece = capturedPiece;
						jumpList.Add (new Vector3 (getAltRow (row), getAltCol (col), 0));
						moves.Add (new Vector3 (getAltRow (row), getAltCol (col), 0));
					}
				}
			}
				}
				
		for(int i = 0; i < jumpList.Count; i++)
			Debug.Log ("jumplist: " + jumpList[i]);
		return moves;
	}

	int getAltRow (int row) {
		return (14 - row);
	}

	int getAltCol (int col) {
		return (-14 - col);
	}


	bool IsInBounds (int r, int c)
	{
		if (r < 0 || r > 14 || c < 0 || c > 14) {
			Debug.Log ("outta bounds");
			return false;
		}

		return true;
	}

	bool EnemyInSpace (GameObject space) {

		Debug.Log ("In EnemyInSpace...current player: " + Board.currPlayer);
		if (space.tag == "Tile" && space.GetComponent<AdjTile> ().collidingWith.Equals(Board.currPlayer)) {
			Debug.Log ("Is it really an enemy tho? " + !space.GetComponent<AdjTile> ().collidingWith.Equals(Board.currPlayer));
			Debug.Log ("THIS IS DA ENEMY " + space.GetComponent<AdjTile> ().collidingWith.tag);
			Debug.Log ("Enemy in space!!!1 " + space.GetComponent<AdjTile> ().pos );
			return true;
		} else if (space.tag == "Warp" && !space.GetComponent<Warp> ().collidingWith.Equals(Board.currPlayer)) {
			Debug.Log ("THIS IS DA ENEMY " + space.GetComponent<Warp> ().collidingWith.tag);
			Debug.Log ("Enemy in space!!!1 " + space.GetComponent<Warp> ().pos );
			return true;
		}
		//Debug.Log ("Player In Adjacent space: " + space. + "Yep.");
		return false;
	}

	bool IsTileEmpty (GameObject space)
	{
		if (space.tag == "Tile" && !space.GetComponent<AdjTile> ().isOccupied) {
			Debug.Log ("This should be true... " + !space.GetComponent<AdjTile> ().isOccupied + " There is not a player in tile " + space.GetComponent<AdjTile> ().pos );
			return true;
		} else if (space.tag == "Warp" && !space.GetComponent<Warp> ().isOccupied) {

			Debug.Log ("There is not a player in warp " + space.GetComponent<Warp> ().pos );
			return true;
		}
		return false;
	}

	bool CanMove ()
	{
		if(IsTileEmpty (adjSpace))
			return true;

		return false;
	}

	bool CanJump ()
	{
		if (!IsTileEmpty (adjSpace) && EnemyInSpace(adjSpace) && IsTileEmpty (jumpSpace)) {
			Debug.Log ("In CanJump with: " + Board.currPlayer);
			if(adjSpace.tag == "Tile")
				//capturedPiece = adjSpace.GetComponent<AdjTile> ().collidingWith;
				//else
				//capturedPiece = adjSpace.GetComponent<Warp> ().collidingWith;
			return true;
		}

		return false;
	}	

	bool OnlyWarp () {
		if (possibleWarp.tag != "Warp")
			return false;		
		//Debug.Log ("In OnlyWarp");
		Debug.Log ("What the hell is this and is it true or false? " + possibleWarp.GetComponent<Warp> ().linkedWarp.GetComponent<Warp> ().isOccupied); 
		if (!possibleWarp.GetComponent<Warp> ().linkedWarp.GetComponent<Warp> ().isOccupied) {
			onlyWarpRow = possibleWarp.GetComponent<Warp> ().linkedWarp.GetComponent<Warp> ().row;
			onlyWarpCol = possibleWarp.GetComponent<Warp> ().linkedWarp.GetComponent<Warp> ().col;
			Debug.Log ("onlywarprow: " + onlyWarpRow + " onlywarpcol: " + onlyWarpCol);
			return true;
		}

		return false;
	}

	bool CanWarp () {

		Debug.Log ("In CanWarp");
		if (!possibleWarp.GetComponent<Warp> ().linkedWarp.GetComponent<Warp> ().isOccupied
		 && !possibleWarp.GetComponent<Warp> ().isOccupied) {
			onlyWarpRow = possibleWarp.GetComponent<Warp> ().linkedWarp.GetComponent<Warp> ().row;
			onlyWarpCol = possibleWarp.GetComponent<Warp> ().linkedWarp.GetComponent<Warp> ().col;
			Debug.Log ("onlywarprow: " + onlyWarpRow + " onlywarpcol: " + onlyWarpCol);
			return true;
		}

		return false;
	}

	bool CanJumpWarp () {
		//Check if adjacent to a warp that is occupied by enemy
		Debug.Log ("In CanJumpWarp");
		if ( possibleWarp.GetComponent<Warp> ().isOccupied 
		 && EnemyInSpace (possibleWarp) && IsTileEmpty (jumpSpace)	
		 && !possibleWarp.GetComponent<Warp> ().linkedWarp.GetComponent<Warp> ().isOccupied) {
			Debug.Log ("possibleWarp is occupied at: " + possibleWarp.GetComponent<Warp> ().row + " " + possibleWarp.GetComponent<Warp> ().col);

			if(jumpSpace.tag == "Tile")
				Debug.Log ("JumpSpace(tile) is: " + jumpSpace.GetComponent<AdjTile> ().row + " " + jumpSpace.GetComponent<AdjTile> ().col);
			else
				Debug.Log ("JumpSpace(warp) is: " + jumpSpace.GetComponent<Warp> ().row + " " + jumpSpace.GetComponent<Warp> ().col);

			//capturedPiece = possibleWarp.GetComponent<Warp> ().collidingWith;
		
			//Debug.Log ("captured piece is: " + capturedPiece.tag + "at " + capturedPiece.GetComponent<Piece> ().row + " " + capturedPiece.GetComponent<Piece> ().col);
			//isWarp = false;
			return true;
		}
		return false;
	}

	bool CanWarpJump ()
	{
		if ( !possibleWarp.GetComponent<Warp> ().isOccupied && IsTileEmpty (jumpSpace)	
		    && possibleWarp.GetComponent<Warp> ().linkedWarp.GetComponent<Warp> ().isOccupied
		    && EnemyInSpace(possibleWarp.GetComponent<Warp> ().linkedWarp)) {
			Debug.Log ("possibleWarp is occupied at: " + possibleWarp.GetComponent<Warp> ().row + " " + possibleWarp.GetComponent<Warp> ().col);
			
			if(jumpSpace.tag == "Tile")
				Debug.Log ("JumpSpace(tile) is: " + jumpSpace.GetComponent<AdjTile> ().row + " " + jumpSpace.GetComponent<AdjTile> ().col);
			else
				Debug.Log ("JumpSpace(warp) is: " + jumpSpace.GetComponent<Warp> ().row + " " + jumpSpace.GetComponent<Warp> ().col);
			
			//capturedPiece = possibleWarp.GetComponent<Warp> ().linkedWarp.GetComponent<Warp> ().collidingWith;
			
			//Debug.Log ("captured piece is: " + possibleWarp.GetComponent<Warp> ().linkedWarp.GetComponent<Warp> ().collidingWith.tag + "at " +  capturedPiece.GetComponent<Piece> ().row + " " + capturedPiece.GetComponent<Piece> ().col);
			//isWarp = false;
			return true;
		}
		return false;

	}

}
	