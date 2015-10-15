using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Control : MonoBehaviour {

	public int enemyPop, playerPop;
	public List<Vector3> enemyList, playerList;
	public GameObject[,] boardData = Board.boardData;

	// Use this for initialization
	void Start () {
		enemyList = new List<Vector3> ();
		playerList = new List<Vector3> ();

	}
	/*
	 * THIS CLASS WILL BE CALLED EVERY TIME ALPHABETA GOES DEEPER
	 * INTO ITS SEARCH / WHEN THE FINAL MOVE IS PLAYED
	 * ---In other words, every time the board (or an instance thereof) is updated
	 * we also have to update the information
	 * 
	 * 
	 * TODO: What should the function types be?
	 */

	//Takes in the quads ---- (0,0)-(3,3); (0,4)-(3,7); (4,0)-(7,4); (4,4)-(7,7)

	public void quadrants(){
		/*
		 * Calls population, positionInThisArea, and hotspots
		 * 
		 * What are the hot spot coordinates in each quadrant?
		 * 
		 * 
		 */
	}

	//USES BoardData
	//Takes in the sides--- (0,0)-(0,3), (0,4)-(0,7); (0,0)-(3,0), (4,0)-(7,0); (0,7)-(3,7), (4,7)-(7,7); (7,0)-(7,3), (7,4)-(7,7);
	public void sides(){
		/*
		 * TODO:
		 * Calculate the the population and positionInThisArea (player 1 and 2) in these areas
		 * Checks if the player's are already in a variant of scatter or ladder
		 * (Find out which coordinate pairs are scatter or ladder)
		 * 
		 * Checks if able to create formations SCATTER or LADDER based off of adjacent pieces of sides
		 * 
		 */
	}

	public int hotSpots(){
		/*
		 * This should give us the coeficient of the multiplier for each unoccupied hotspot
		 * 
		 * Hotspots are increased if there are LESS enemies in the quadrant containing hotspots h
		 * This is because hotspots are more efficent when a player is coming from the other end of the board
		 * 
		 * /
	}

	public void population(Vector3 from, Vector3 to){
		/*FROM and TO will give you a rectangle of spaces
		 * 
		 * Find the differences of from.x and to.x + 1, and from.y and to.y + 1
		 *
		 *Create two for loops that go from var = 0 to diff.x and diff.y respectively
		 *If there's a enemy in [varx, vary], add to enemyPop
		 *If player, add to playerPop
		 *otherwise, continue
		 */
	

	}

	public void positionsInThisArea(Vector3 from, Vector3 to){

		/*FROM and TO will give you a rectangle of spaces
		 * 
		 * Find the differences of from.x and to.x + 1, and from.y and to.y + 1
		 *
		 * Create two for loops that go from var = 0 to diff.x and diff.y respectively
		 * IF there's a piece in boardData there, it will add to either enemyList and playerList respectively
		 */

	}

}
                    