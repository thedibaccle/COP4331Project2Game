using UnityEngine;
using System.Collections;
using System;
using Parse;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections.Generic; // need this STUPID THING so IENumerable works within result outputs. Makes no damn sense since cothreads are IENumerable FFS!

public class MatchingScript : MonoBehaviour {

	private ParseObject gameMatch;
	private string gameObjName = "QuanvoluteMatch";
	public string gameObjID = null; //  null = probably error, rest = gameObject ID
	public ParseObject playerGame = null;
	private string thisUsernameFound = null;
	private string nextUsernameFound = null;
	private string inProgress = null;

	// Use this for initialization
	void Start () 
	{
		// [done]create a formal game match object
		// [done]Check if match already exists and is in play
		// Check if someone else is waiting for someone to join them
		// [done]Create match if one doesn't exist.	



		// Go to game.
		//createQMatch();

		// these coroutines will need to be inside a coroutine
		DisplayError("Loading... ",false);
		StartCoroutine(performGameMatching(5)); // make everything wait 5 seconds per

	}


	private IEnumerator performGameMatching(int wait)
	{
		int phase = 0; // acts as a counter/stepper to step between checks one at a time and skip phases if they shouldn't be done
		int nextAction = -1; // acts as indexs

		if (phase==0) {
			Debug.Log ("Checking if THIS logged in user is already in a game...(5 second wait)");
			StartCoroutine (checkIfInGame (wait));
			yield return new WaitForSeconds (wait+3);
			if (gameObjID == "error") {
				Debug.LogError ("Error occured while trying to see if person was in game");
			} else {
				if(this.gameObjID==null){phase++;}
				else if(!this.gameObjID.Equals("-1")){nextAction = 1;} // push player into new game
				else{nextAction=-1;}
			}
			Debug.Log ("Check 1: gameObjID => " + gameObjID);
			DisplayError("Check 1 (5 secs)...",false);
		}

		// ====================================
		DisplayError("1/3] nextAction=>" + nextAction + " phase=>" + phase,false);
		if (phase==1) {
			Debug.Log ("Checking if someone else is waiting on a match...(5 second wait)");
			StartCoroutine (checkIfPlayerWaiting (wait));
			yield return new WaitForSeconds (wait+3);
			if (gameObjID == "error") {
				Debug.LogError ("Error occured while trying to see if person was in game");
			} else {
				if(gameObjID==null){phase++;}
				else if(!gameObjID.Equals("-1")){nextAction = 2;} // push player into match against other person waiting
				else{nextAction=-1;}
			}
			Debug.Log ("Check 2: gameObjID => " + gameObjID);
			DisplayError("Check 2 (5 secs)...",false);
		}
		DisplayError("2/3] nextAction=>" + nextAction + " phase=>" + phase,false);
		// ====================================
		if (phase==2) {
			Debug.Log ("Creating a new game since nobody else is available to play against atm...(5 second wait)");
			StartCoroutine (createQMatch (wait));
			yield return new WaitForSeconds (wait+3);
			if (gameObjID == "error") {
				Debug.LogError ("Error occured while trying to see if person was in game");
			} else {
				if(gameObjID==null){phase++;}
				else if(gameObjID.Equals("NewGame")){nextAction = 3;} // a new match has been created, push this player into the waiting for challenger wait scene
				else{nextAction=-1;}
			}
			Debug.Log ("Check 3: gameObjID => " + gameObjID);
			DisplayError("Check 3 (5 secs)...",false);
		}

		DisplayError("3/3] nextAction=>" + nextAction + " phase=>" + phase,false);
		//nextAction = 1; // push player into new game
		//nextAction = 2; // push player into match against other person waiting
		//nextAction = 3; // a new match has been created, push this player into the waiting for challenger wait scene

		// Add the gameObjID to the CurrentUser and update


		///// Note: this stuff is handled by the check itself, it'll never even make it this far if it's true
		if (nextAction == 1) {
			Debug.Log ("TODO: This player is already in a game! (InProgress or WaitingOnChallenge) DO A THING!");
			DisplayError("Found Previous Game! Going into game now...",false);
			// Load the game, and wait wait wait
			Debug.LogWarning("Moving 5");
			Application.LoadLevel ("scnWaiting");
		}
		else if (nextAction == 2) {
			Debug.Log ("TODO: Found someone who needs an opponate. Match them up!");
			DisplayError("Found Game! Going into game now...",false);
			// Load the game, and wait wait wait
			Debug.LogWarning("Moving 4");
			Application.LoadLevel ("scnGame");
		}
		else if (nextAction == 3) {
			// We created a new match, let's go to it!
			Debug.Log ("TODO: Created a new match! Let's wait... and wait... and wait... (a new scene for this would be GOOD");
			DisplayError("Created New Game! Going into game now...",false);
			// Load the game, and wait wait wait
			Debug.LogWarning("Moving 3");
			Application.LoadLevel ("scnWaiting");
		} else 
		{
			string errorMsg = "Error: something happened. nextAction=>" + nextAction + " phase=>" + phase;
			Debug.LogError(errorMsg);
			DisplayError(errorMsg,false);
		}




		yield break;

		// ====================================
	}


	private IEnumerator checkIfPlayerWaiting(int wait)
	{
		
		
		string error;

		// The code for this method is SOOOOOOOOOOO bad
		// It's only functioning under the assumption that two players ONLY EVER will be playing through the app...
		// UGHHHH https://youtu.be/D5bOJT_HtUI

		var query = ParseObject.GetQuery (gameObjName);
		//query = query.WhereNotEqualTo("thisPlayerUsername", ParseUser.CurrentUser.Username);
		//query = query.WhereEqualTo("progressState", "WaitingForChallenger"); // "WaitingForChallenger", "InProgress", "Finished"
		//query.OrderByDescending("updatedAt");
		query.OrderBy("thisMovePerformedDate");
		Task runQuery = query.FindAsync().ContinueWith(t =>
		                                                {
			// if FirstAsync is used then the sorting is ignored. fml now I have to update god only knows how many other queries :(
			IEnumerable<ParseObject> _results = t.Result;
			foreach (var _result in _results)
			{
				// This does not require a network access.
				var _nextusrname = _result.Get<string>("nextPlayerUsername");
				this.nextUsernameFound = _nextusrname;
				if(this.nextUsernameFound==null)
				{
					this.gameObjID = _result.ObjectId.ToString();
				}
			} // the last item will be the one we actually care about. I really hate Parse right now, so much. Passionately.
		});
		yield return new WaitForSeconds(wait);
		


		if (runQuery.IsFaulted || runQuery.IsCanceled)
		{
			error = "Error2: Failed to find results Reason2: " + runQuery.Exception.Message;
			Debug.Log(error);
			// TODO: Run a thread from here to go out and do the next thing(check what)
			//this.gameObjID = "error";
			this.gameObjID = null;
		}
		else
		{


			Debug.Log("Sucess: Results from query: " + this.gameObjID);
			//Application.LoadLevel("ExampleScene");
		}


	}

	
	private IEnumerator createQMatch(int wait)
	{
		string error;
		TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
		int timestamp  = (int) t.TotalSeconds;
		//Console.WriteLine (timestamp);
		
		Debug.Log("Starting Matching...");
		int gameBoardState = 999; // god this thing is a pain to declare as a 2d int array, screw it, object for now
		
		// 8x8 grid, 0 for null, 1 for p1, 2 for p2
		
		this.gameMatch = new ParseObject(gameObjName)
		{
			{"matchCreationDate", timestamp},
			{"thisMovePerformedDate", timestamp},
			{"thisPlayerUsername", ParseUser.CurrentUser.Username},
			{"thisPlayerNumber", 1}, // 1 or 2. If user = maker, then 1. Else, 2
			{"thisTurnNumber", 0}, // init at 0, mark as 1 for the first actual turn
			{"nextPlayerUsername", null},
			{"inProgress", "WaitingForChallenger"},
			{"thisBoardState",  gameBoardState}
		};
		
		Debug.Log("qMatch Object is: " + this.gameMatch.ToString());
		Task runQuery = this.gameMatch.SaveAsync();
		yield return new WaitForSeconds(wait);

		if (runQuery.IsFaulted || runQuery.IsCanceled)
		{
			error = "Error3: Failed to find results Reason3: " + runQuery.Exception.Message;
			Debug.Log(error);
			// TODO: Run a thread from here to go out and do the next thing(check what)
			//this.gameObjID = "error";
			this.gameObjID = null;
		}
		else
		{
			this.gameObjID = "NewGame";
			Debug.Log("Results from query: " + this.gameObjID);
			//Application.LoadLevel("ExampleScene");
		}

		Debug.Log ("Performed runQuery!");
	}
		

	/*
	 * 
	 * If you need to get an object's latest data from Parse, you can call the FetchAsync method like so:

		Task<ParseObject> fetchTask = myObject.FetchAsync();

	 * 
	 * 
	 * 
	 */




	private IEnumerator checkIfInGame(int wait)
	{


		string error;

		//var query1 = ParseObject.GetQuery(gameObjName);
		//query1 = query1.WhereEqualTo("thisPlayerUsername", ParseUser.CurrentUser.Username);
		//query1 = query1.WhereNotEqualTo("inProgress", "Finished");

		//var query2 = ParseObject.GetQuery(gameObjName);
		//query2 = query2.WhereEqualTo("nextPlayerUsername", ParseUser.CurrentUser.Username);
		//query2 = query2.WhereNotEqualTo("inProgress", "Finished"); 

		// TODO: This is HUGELY flawed logic
		// This assumes the game never ends
		// Even if the game DOES end, it doesn't have a proper relationship to group rows
		// Imagine a scenerio where someone DID complete a game.
		// Now what?! It'll just keep putting the user back into that finished game!!!
		// Currently I'm not sure how to setup the data

		//ParseQuery<ParseObject> query = query1.Or(query2);
		//query.OrderByDescending("updatedAt");

		ParseQuery<ParseObject> query = ParseObject.GetQuery(gameObjName);
		query.OrderBy("thisMovePerformedDate");
		string myUsername = ParseUser.CurrentUser.Username.ToString ();
		string finshedString = "Finished";
		int whichOptionFlag = -1;
		Task runQuery = query.FindAsync().ContinueWith(t =>
	     	{
			// if FirstAsync is used then the sorting is ignored. fml now I have to update god only knows how many other queries :(
			IEnumerable<ParseObject> _results = t.Result;
			foreach (var _result in _results)
			{
				// This does not require a network access.
				var _thisusrname = _result.Get<string>("thisPlayerUsername");
				var _nextusrname = _result.Get<string>("nextPlayerUsername");
				var _inprog = _result.Get<string>("inProgress");
				if(!finshedString.Equals(_inprog) && myUsername.Equals(_thisusrname))
				{
					whichOptionFlag=1;
					break;
				}
				else if(!finshedString.Equals(_inprog) && myUsername.Equals(_nextusrname))
				{
					whichOptionFlag=2;
					break;
				}
				//break;
			} // the last item will be the one we actually care about. I really hate Parse right now, so much. Passionately.
		});
		yield return new WaitForSeconds(wait);

		//Debug.Log ("DERP: " + playerGame.ObjectId.ToString ());
		//this.gameObjID = playerGame.ObjectId.ToString();
		//Debug.Log ("BERP: " + this.gameObjID);

		if (runQuery.IsFaulted || runQuery.IsCanceled)
		{
			error = "Error1: Failed to find results Reason1: " + runQuery.Exception.Message;
			Debug.Log(error);
			// TODO: Run a thread from here to go out and do the next thing(check what)
			//this.gameObjID = "error";
			this.gameObjID = null;
		}
		else
		{
			//this.gameObjID = playerGame.ObjectId.ToString();

			if(whichOptionFlag==1)
			{
				Debug.LogWarning("Moving 1");
				//Debug.LogWarning("this.thisUsernameFound=>" + this.thisUsernameFound + " parseUser=>" + myUsername);
				Application.LoadLevel("scnWaiting");
			}
			else if(whichOptionFlag==2)
			{
				Debug.LogWarning("Moving 2");
				//Debug.LogWarning("this.thisUsernameFound=>" + this.nextUsernameFound + " parseUser=>" + myUsername);
				Application.LoadLevel("scnGame");
			}
			else
			{
				this.gameObjID = null; // game is over or no games exist for this user that they're in
			}

			Debug.Log("Results from query: " + this.gameObjID);
			//Application.LoadLevel("ExampleScene");
		}

	}

	private void DisplayError(string errorMsg, bool append)
	{
		GameObject lblErrorMsgGo = GameObject.Find("lblErrorMsg"); // finding the game object for txtEmail
		Text lblErrorMsgCo = lblErrorMsgGo.GetComponent<Text>();
		
		if (append) {
			lblErrorMsgCo.text += errorMsg + "; ";
		} else {
			lblErrorMsgCo.text = errorMsg;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
