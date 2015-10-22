using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AI : MonoBehaviour {

	/*WHAT WE NEED TO RUN THIS CLASS:
	 * List of player pieces (GameObject)
	 * BoardData
	 * Instance of Control
	 * 
	 * 
	 * 
	 */


	struct BoardData 
	{
		//Needs two separate List<GameObject> for each player
		ArrayList<GameObject> rPlayer = Board.rPlayer;
		ArrayList<GameObject> bPlayer = Board.bPlayer;
		public string WhosTurn = "PlayerTwo"; //Starts as player two. PLACEHOLDER
	}

	struct GameSim
	{
		public BoardData game;
		public Vector3 bestMove;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//Takes in the two lists takes ratio of the sizes (redPlayer / bluePlayer)
	int getPopulationRatio()
	{
		return 1;
	}

	//Removes a captured piece. Takes in the piece that it captured (you'll have to set this)
	public static void removePiece(ref BoardData game, GameObject capturedPiece)
	{

	}

	//Splits the selected piece and moves other piece to other side
	public static void splitPiece(ref BoardData game, GameObject selectedPiece)
	{
	}

	/*
	 * Finds how deep we must search based on the difficulty. 
	 * On a scale from 1-10, how far will a level 1 look ahead compared to a level 10?
	 */
	int getDepth (BoardData game, int difficulty) {
		return 0;
	}

	//Checks if the player will win with this move
	bool willWin (){
		return false;
	}

	/*Checks if a piece is being threatened		
	 * Piece is threatened if the jump is not an exchange in pieces
	 * or if the player ratio is not in the AB's favor
	*/
	bool JumpThreat (){
		return false;
	}

	int score()
	{
		/*
		 * This is where we call class Control. In here, we will keep track of moves made and
		 * score the instance of the board based on the components in Control AND AI.
		 * 
		 */
		return 0;
	}

	Vector3 alphaBeta(int Depth, int alpha, int beta){
		/*
		 * We will check each piece for the player, but we will order them by priority
		 * The ones that are closer to more enemies, or more open to movement, depending on the board or how the computer
		 * is choosing to play.
		 * 
		 * The selected piece will then call it's LegalMoves to see all of the viable options.
		 * With the list LegalMoves, this is where the AI does it's THINKING
		 * It moves this piece to each of these possible places then SCORES this move based off of all of the conditional functions
		 * that we've made. HEY LOOK IT'S STARTING TO ALL COME TOGETHER.
		 * Highest scoring move will be the best move.
		 * 
		 * 
		 * Remember that alphabeta has to take in account the other player's (best possible) moves in order to go deeper into it's search
		 * 
		 */
	}
}
