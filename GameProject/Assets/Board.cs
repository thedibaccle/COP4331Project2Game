using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {
	public AdjTile adjTile;
	public GameObject[,] boardData = new GameObject[8,8];
	public Warp warpTile;
//	public GameObject adjTile;
	/*This class will see the whole board and check for:
	 *If opposing pieces are adjacent to eachother
	 *If piece is on AdjacentTile
	 *If piece is on Warp
	 *
	 */

	// Use this for initialization
	void Start () {
		Instantiate ();
	}

	void Instantiate () {
		int alternate = 0;
		//I have to instantiate all of the warps? Noooooooooooooooooooooo

		//MAGENTA
		GameObject warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.magenta;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (1 , -3 , 0);
		boardData [3, 2] = warp;

		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.magenta;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (-1 , 3, 0);
		boardData [4, 5] = warp;

		//RED
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.red;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (1 , 3 , 0);
		boardData [4, 2] = warp;
		 
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.red;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (-1 , -3, 0);
		boardData [3, 5] = warp;

		//YELLOW
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.yellow;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (3 , 1 , 0);
		boardData [5, 3] = warp;
		
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.yellow;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (-3 , -1, 0);
		boardData [2, 4] = warp;

		//GREEN
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.green;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (3 , -1 , 0);
		boardData [5, 4] = warp;
		
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.green;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (-3 , 1 , 0);
		boardData [2, 3] = warp;


		//CYAN
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.cyan;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (5 , -3 , 0);
		boardData [6, 5] = warp;
		
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.cyan;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (-5 , 3 , 0);
		boardData [1, 2] = warp;

		//BLUE
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.blue;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (5 , 3, 0);
		boardData [6, 2] = warp;
		
		warp = GameObject.CreatePrimitive (PrimitiveType.Cube);
		warp.transform.name = "Warp";
		warp.GetComponent<Renderer> ().material.color = Color.blue;
		warp.AddComponent <Warp>();
		warp.transform.position = new Vector3 (-5 , -3, 0);
		boardData [1, 5] = warp;

		for (int i = 0; i < 8; i++) {
			for (int j = 0; j < 8; j++) {
				//Creating Tiles
				if(boardData[i, j] == null) {

					GameObject tile = GameObject.CreatePrimitive (PrimitiveType.Cube);

					if (alternate % 2 == 0)
						tile.GetComponent<Renderer> ().material.color = Color.white;
					else
						tile.GetComponent<Renderer> ().material.color = Color.gray;
				
					//Creating Players
					if(j == 0 || j == 7) {
						GameObject player = GameObject.CreatePrimitive (PrimitiveType.Sphere);

					if( j == 0 ) {
						player.GetComponent <Renderer>().material.color = Color.white;
						player.transform.name = "PlayerOne";
					}
					else {
						player.GetComponent <Renderer>().material.color = Color.black;
						player.transform.name = "PlayerTwo";
					}
					
					player.AddComponent <Piece>();
					player.transform.position = new Vector3 ( i*2 - 7, j*2 - 7 , -2 );

					tile.AddComponent <AdjTile>();
					tile.transform.position = new Vector3 ( i*2 - 7 , j*2 - 7 , 0);
					boardData [i , j] = player;
					continue;
				}

					alternate++;
				

				tile.transform.name = "Tile";
				tile.transform.position = new Vector3 (i * 2 - 7, j * 2 - 7, 0);
				tile.AddComponent <AdjTile>();
				boardData [i, j] = tile;
				}
				else;
			
			}
			alternate--;
		}
		PrintBoardData ();
	}

	void PrintBoardData () {
		for (int i = 0; i < 8; i++) 
			for (int j = 0; j < 8; j++)
				print (boardData [i, j]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
	