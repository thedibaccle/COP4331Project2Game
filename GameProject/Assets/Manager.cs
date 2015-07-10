using UnityEngine;
using System.Collections;

public class Manager : MonoBehaviour {
	public Board board;

	//Have selection thing here

	void Start () {
		TwoPlayerLocal ();
	}

	void OnePlayer () {
	}

	void TwoPlayerLocal () {
		SetUp ();
	}

	void TwoPlayerP2P () {

	}

	void SetUp () {
		GameObject boardObject = new GameObject("board");
		boardObject.transform.parent = transform;
		board = boardObject.AddComponent<Board>();
	}
	
	// Use this for initialization

	// Update is called once per frame
	void Update () {
	
	}

	bool CanGo(){
		return false;
	}
	
}

