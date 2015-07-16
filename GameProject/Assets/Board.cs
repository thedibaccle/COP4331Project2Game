using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEngine.EventSystems.IPointerClickHandler;

public class Board : MonoBehaviour {
	public AdjTile adjTile;
	public GameObject[,] boardData = new GameObject[8,8], cacheBoardData = new GameObject[8,8];
	public Warp warpTile;
	public Piece piece;
	public GameObject warp, tile, player;
	//public int WhosTurnIsIt = 0;
	//Maximum number of pieces on board is 32
	public Moves[] moves;
	public LegalMoves legalMoves = new LegalMoves();
	float setAtRow = -1.0F, setAtCol;
	string currPlayer;
	int count = 0;

	// Use this for initialization
	void Start () {
		//1D GameObject array [64]
		//Only checks adjacent tiles for ONE warp
		int alternate = 0;
		//I have to instantiate all of the warps? Noooooooooooooooooooooo
		//Link paired warp tiles together via gameobject variable

		//MAGENTA
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.magenta;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (6 , -4 , 0);
		warp.transform.parent = this.transform;
		boardData [3, 2] = warp;
		cacheBoardData [3, 2] = warp;

		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.magenta;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (8 , -10, 0);
		warp.transform.parent = this.transform;
		boardData [4, 5] = warp;
		cacheBoardData [4, 5] = warp;

		//RED
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.red;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (8 , -4, 0);
		warp.transform.parent = this.transform;
		boardData [4, 2] = warp;
		cacheBoardData [4, 2] = warp;
		 
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.red;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (6 , -10, 0);
		warp.transform.parent = this.transform;
		boardData [3, 5] = warp;
		cacheBoardData [3, 5] = warp;

		//YELLOW
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.yellow;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (10 , -6, 0);
		warp.transform.parent = this.transform;
		boardData [5, 3] = warp;
		cacheBoardData [5, 3] = warp;
		
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.yellow;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (4 , -8, 0);
		warp.transform.parent = this.transform;
		boardData [2, 4] = warp;
		cacheBoardData [2, 4] = warp;

		//GREEN
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.green;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (10 , -8, 0);
		warp.transform.parent = this.transform;
		boardData [5, 4] = warp;
		cacheBoardData [5, 4] = warp;

		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.green;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (4 , -6, 0);
		warp.transform.parent = this.transform;
		boardData [2, 3] = warp;
		cacheBoardData [2, 3] = warp;

		//CYAN
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.cyan;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (12 , -10, 0);
		warp.transform.parent = this.transform;
		boardData [6, 5] = warp;
		cacheBoardData [6, 5] = warp;

		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.cyan;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (2 , -4, 0);
		warp.transform.parent = this.transform;
		boardData [1, 2] = warp;
		cacheBoardData [1, 2] = warp;

		//BLUE
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.blue;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (12 , -4, 0);
		warp.transform.parent = this.transform;
		boardData [6, 2] = warp;
		cacheBoardData [6, 2] = warp;
		
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.blue;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (2 , -10, 0);
		warp.transform.parent = this.transform;
		boardData [1, 5] = warp;
		cacheBoardData [1, 5] = warp;

		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				//Creating Tiles
				if(boardData[i, j] == null) {

					tile = GameObject.CreatePrimitive (PrimitiveType.Cube);

					if (alternate % 2 == 0)
						tile.GetComponent<Renderer> ().material.color = Color.white;
					else
						tile.GetComponent<Renderer> ().material.color = Color.gray;
				
					//Creating Players
					if(j == 0 || j == 7) {
						player = GameObject.CreatePrimitive (PrimitiveType.Sphere);

					if( j == 0 ) {
						player.GetComponent <Renderer>().material.color = Color.white;
							player.transform.name = "PlayerOne";

						
					}
					else {
						player.GetComponent <Renderer>().material.color = Color.black;
						player.transform.name = "PlayerTwo";

					}
					
					//legalMoves[i] = new Moves();
					player.AddComponent <Piece>();
					player.transform.position = new Vector3 ( i*2, (j-7)*2, -2 );
					player.transform.parent = this.transform;

					tile.AddComponent <AdjTile>();
					tile.transform.position = new Vector3 ( i*2, (j-7)*2, 0);
					tile.transform.parent = this.transform;
					boardData [i , j] = player;

					continue;
				}

					alternate++;
				

				tile.transform.name = "Tile";
				tile.transform.position = new Vector3 (i * 2, (j-7) * 2, 0);
				tile.AddComponent <AdjTile>();
				tile.transform.parent = this.transform;
				boardData [i, j] = tile;
				}

			
			}
			alternate--;
		}


		/*
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.transform.position = new Vector3 (6, -4, 0);
		warp.GetComponent<Renderer> ().material.color = Color.magenta;
		warp
		Instantiate (warp, new Vector3(8 , -10 , 0), Quaternion.identity);

		warp.GetComponent<Renderer> ().material.color = Color.red;
		*/


		PrintBoardData ();
		currPlayer = "PlayerOne";
		//moves = legalMoves.getLegalMoves(currPlayer, boardData);
	}

	public void PrintBoardData () {
		for (int i = 0; i < 8; i++) 
			for (int j = 0; j < 8; j++)
				print (boardData [i, j]);
	}

	public string WhosTurnIsItNow (int c){
		Debug.Log (c);
		bool b = (c % 2 == 0);
		count++;
		if(b)
			return "PlayerOne";
		return "PlayerTwo";
	}
	//needs a bool to make sure player can only select one piece at a time

	void Update () {
		/*TODO: MAKE SURE THAT THE ANIMATIONS ARE ONLY FOR THE PIECES SELECTED
		 * 	HAVE A COPY OF THE ORIGINAL boardData, SO WHEN THE PIECES MOVE, THE TILES UNDERNEATH
		 *  WILL HAVE THEIR COORDS RESTORED
		 * 
		 */

		//Debug.Log (currPlayer);
		//Continuously checks if user is selecting a piece
		//have the current player's pieces bob
		//highlight the piece that is being selected
		for (int i = 0; i < 8; i++)
			for (int j = 0; j < 8; j++) {
			//If player is in the space
			//Debug.Log (beroardData[i, j].name == player.name);

			if(boardData[i, j].name == currPlayer) {
				Debug.Log (currPlayer);
	
				//If it's the current player's piece
				GameObject temp = boardData[i, j];
					Debug.Log ("Let's see if bool is working... " + temp.GetComponent<Piece>().tapped);
					player.GetComponent<Piece> ().anCurrPlayerBob();
					//If said piece is currently being selected
					if(temp.GetComponent<Piece>().tapped){
						temp.GetComponent<Piece> ().anSelected();
						Debug.Log("hooray");
						//animate tiles that are legal moves 
					//player.GetComponent<Piece> ().board = this;
					/*
						ShowLegalMoves(temp);
						
						for(int k = 0; k < moves.Length; k++)
							if(moves [i].fromRow == setAtRow
						  	&& moves [i].fromCol == setAtCol) {
						
								if(boardData[(int)moves [i].toRow, (int)moves [i].toCol] == tile)
									tile.GetComponent<AdjTile> ().anPossibleMove ();
						
								else warp.GetComponent<Warp> ().anPossibleMove ();
					*/
					}
				}
				
			}

	}


	//Where the piece is now
	
	void ShowLegalMoves (GameObject piece){
		//Basically doClickSquare
		//Debug.Log (moves.Length);
		/*
		for (int i = 0; i < moves.Length; i++) {

			//Debug.Log ("piece pos: " + piece.GetComponent<Piece> ().row +"." + piece.GetComponent<Piece> ().col + "\n moves pos: " + moves [i].fromRow + "." + moves [i].fromCol);
			if (moves [i].fromRow == piece.GetComponent<Piece> ().row
			&& moves [i].fromCol == piece.GetComponent<Piece> ().col) {

				setAtRow = piece.GetComponent<Piece> ().row;
				setAtCol = piece.GetComponent<Piece> ().col;
				Debug.Log ("SELECTED PIECE AT row: " + setAtRow + " and col: " + setAtCol);
				return;
			}
			//Checks if user clicks on square
			if (moves [i].fromRow == setAtRow
			 && moves [i].fromCol == setAtCol
			 && moves [i].toRow == tile.GetComponent<AdjTile> ().row
			 && moves [i].toCol == tile.GetComponent<AdjTile> ().col) {
				doMakeMove (moves [i]);
			}
		}
		*/
	}

	GameObject PieceAt(int row, int col) {
		return boardData [row, col];
	}

	void SetPieceAt (int row, int col, GameObject piece) {
		boardData [row, col] = piece;
	}

	void doMakeMove (Moves move) {
	
		//Basically doMakeMove

		MakeMove ((int)move.fromRow, (int)move.fromCol, (int)move.toRow, (int)move.toCol);

		/*//If player's turn and cannot move anywhere, the other wins
		count++;
		//If player hasn't made move yet, then it is not the next player's turn
		if (currPlayer == WhosTurnIsItNow (count))
			legalMoves = check.getLegalMoves (currPlayer);
		else {
			currPlayer = WhosTurnIsItNow (count);
			legalMoves = check.getLegalMoves (currPlayer);
		}
		*/
		currPlayer = WhosTurnIsItNow (count);
		//moves = legalMoves.getLegalMoves (currPlayer, boardData);
	}

	void MakeMove (int fromRow, int fromCol, int toRow, int toCol) {

		boardData [toRow, toCol] = boardData [fromRow, fromCol];
		boardData [fromRow, fromCol] = cacheBoardData [fromRow, fromCol];

	}

	void SelectedPieceOptions () {
	}

}
	