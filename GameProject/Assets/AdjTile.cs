using UnityEngine;
using System.Collections;

public class AdjTile : MonoBehaviour
{
	public Vector2 pos;
	public Piece piece;
	public bool isOccupied = false;
	public GameObject objTile;
	//Put this in board or warp?
	public Vector2[,] aTilePositions = new Vector2[8,8];
	//This class needs to tell
	// Use this for initialization
	void Start () {
		pos = transform.position;
		//for (int i = 0; i < nAdjTiles; i++)
		//	aTilePositions [i] = GameObject.FindGameObjectWithTag ("at" + i).transform.position;
	}
	 
	// Update is called once per frame
	void Update ()
	{

	}

	void OnCollisionEnter (Collision c)
	{
		//if(c.gameObject.tag == "player")
			isOccupied = true;
	}

	void OnCollisionExit (Collision c)
	{
		isOccupied = false;
	}
	
}
