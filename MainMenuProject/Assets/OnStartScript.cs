using UnityEngine;
using System.Collections;
using Parse;


public class OnStartScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		// hide all canvases
		GameObject canvasLoginGo = GameObject.Find("canvasLogin");

		canvasLoginGo.SetActive (false);

		if (ParseUser.CurrentUser != null)
		{
			// do stuff with the user
			// Skip to matching game
			Debug.Log(ParseUser.CurrentUser.Email + " is logged in.");
			Application.LoadLevel("scnMatching"); // load/switch to the scnMatching scene.

		}
		else
		{
			// show the signup or login screen
			Debug.Log ("User needs to sign up or login.");
			canvasLoginGo.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
