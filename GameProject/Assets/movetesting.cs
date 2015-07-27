using UnityEngine;
using System.Collections;

public class movetesting : MonoBehaviour {

	public static int val = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseDown () {
		//Board.ShowLegalMoves ();
		//Use this to make ALL OF THE INSTANTS OF tapped
	}
	
	public void OnMouseUp () {
		val++;
		//objMoves = new Moves ();
		//base.PrintBoardData ();
		Debug.Log (gameObject.tag + " " +val);
	}
}
