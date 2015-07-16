using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class Piece : MonoBehaviour {
	GameObject[,] boardData;
	public Board board;
	Vector2 pos;
	public float row, col;
	Moves objMoves;
	public string name;

	// Use this for initialization
	void Start () {
		//pos = transform.position;
		//boardData = board.boardData;
		getName (name);
	}
	
	public void getName(string s) {
		name = s;
	}

	// Update is called once per frame
	void Update () {
		pos = transform.position;
		getPositionInBoardData ();
	}
	public bool tapped;

	void OnMouseDown () {
		//Board.ShowLegalMoves ();
		//Use this to make ALL OF THE INSTANTS OF tapped
		tapped = true;

		Debug.Log ("down");
	}
	
	void OnMouseUp () {
		tapped = false;
		//objMoves = new Moves ();
		//base.PrintBoardData ();
		Debug.Log ("up");
	}

	public void getPositionInBoardData () {
		row = pos.x/2;
		col = -pos.y/2;	
		
	}

	public void anCurrPlayerBob () {
	}

	public void anSelected () {
	}

}
