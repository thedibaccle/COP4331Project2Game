using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class BoardData 
{
		//Needs two separate List<GameObject> for each player
		public GameObject[,] boardData;
		public Stack<GameObject> comp;
		public Stack<GameObject> player;
		public bool aiTurn; //Starts as player two. PLACEHOLDER
}
	
struct GameSim
{
		public BoardData game;
		public Vector3 bestMove;
		public GameObject selectedPiece;
}

public class AI : MonoBehaviour {

		const int NROWS = 8;
		const int NCOLS = 8;
		const int LARGE_VAL = 10000000;


	// Use this for initialization
	void Start (){

	}
	
	public static void timeToThink(GameObject[,] boardData, Stack<GameObject> them, 
	                               Stack<GameObject> us, bool turn, int difficulty)
	{
		BoardData think = new BoardData ();
		think.boardData = boardData;
		think.comp = us;
		think.player = them;
		think.aiTurn = turn;
		//Debug.Log ("SCP files" + think.comp.Count);
		move (think, difficulty);
	}
	
	static void move(BoardData board, int time)
	{
		GameSim currBoard = new GameSim ();
		currBoard.game = clone (board);
		currBoard.selectedPiece = null;
		Debug.Log ("IN AI HOORAY");
		currBoard.bestMove = new Vector3(7,7,0);
		int depth = getDepth (currBoard.game, time);

		ScorePieces (currBoard, depth);


		//Vector3 moveTo = 
		//currBoard.selectedPiece.transform.position = moveTo;
		
	}

	static BoardData clone(BoardData game)
	{
		GameObject[,] board = new GameObject[8, 8];
		board = game.boardData;
		//board = ;
		return game;
	}

	//Takes in the two lists takes ratio of the sizes (redPlayer / bluePlayer)
	static float getPopulationRatio(Stack<GameObject> comp , Stack<GameObject> player)
	{
		return (float)(comp.Count / player.Count);
	}

	/*
	 * Finds how deep we must search based on the difficulty. 
	 * On a scale from 1-10, how far will a level 1 look ahead compared to a level 5?
	 */
	static int getDepth (BoardData game, int difficulty) {
		
		int nOpponentPieces = game.player.Count;
		float popRatio = getPopulationRatio (game.comp, game.player);
		
		if (difficulty == 1)
			return 3;
		else if (difficulty == 2)
			return 6;
		else if (difficulty == 3)
			return (int)Mathf.Ceil ((difficulty*difficulty) /(2*popRatio));
		
		return 5;
		
	}

	static void ScorePieces(GameSim node, int depth)
	{
		Stack<GameObject> pieces;
		List<int> bestMoves = new List<int>();
		if (node.game.aiTurn)
			pieces = node.game.comp;
		else
			pieces = node.game.player;
		/*THIS IS WHERE CLASS CONTROL COMES IN
		 * We'll need something to pick the pieces that are more likely to score
		 * higher than others. Right now, all this does is pick them unbiasedly
		 */
		int nMoves = pieces.Count;

		for (int i = 0; i < nMoves; i++) 
		{
			node.selectedPiece  =  pieces.Peek ();
			node.selectedPiece.GetComponent<Piece>().moves = new LegalMoves().getLegalMoves(node.selectedPiece);
			Debug.Log ("AI SELECETING PIECE AT: " + node.selectedPiece.transform.position + "  " + nMoves);
			bestMoves.Add(alphaBeta (ref node, depth, -1000000000, 1000000000));
		}
	}

	static int alphaBeta(ref GameSim node, int depth, int alpha, int beta){
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
		GameObject piece = node.selectedPiece;
		//GameObject availableMoves = piece.GetComponent<Piece> ().moves;


		//See if the computer has reached the 'surface' or if there are no more moves available
		if (depth == 0 || NoMoves (piece.GetComponent<Piece> ().moves))
			return score (node);
		else 
		{

			int forcedMove = -1;

			for(int i = 0; i < piece.GetComponent<Piece> ().moves.Count; i++)
			{
				movePiece (ref node.game, piece);
			}

		}

		return 0;

	}
	
	static bool NoMoves(List<Vector3> moves)
	{
		if (moves.Count == 0)
			return true;

		return false;
	}

	public static void movePiece(ref BoardData g, GameObject piece)
	{
		///if(g.boardData[piece.GetComponent<Piece>().row, piece.GetComponent<Piece> ().col] == 
	}

	public static void removePiece(ref BoardData g, GameObject piece)
	{
	}

	//Removes a captured piece. Takes in the piece that it captured (you'll have to set this)
	public static void removeCapturedPiece(ref BoardData g, GameObject piece)
	{

	}

	//Splits the selected piece and moves other piece to other side
	public static void splitPiece(ref BoardData g, GameObject piece)
	{
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

	static int score(GameSim node)
	{
		/*
		 * This is where we call class Control. In here, we will keep track of moves made and
		 * score the instance of the board based on the components in Control AND AI.
		 * 
		 */
		return 0;
	}


}
