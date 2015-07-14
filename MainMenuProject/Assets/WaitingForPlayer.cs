using UnityEngine;
using System.Collections;
using Parse;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Collections.Generic; // need this STUPID THING so IENumerable works within result outputs. Makes no damn sense since cothreads are IENumerable FFS!

public class WaitingForPlayer : MonoBehaviour {

	private string gameObjName = "QuanvoluteMatch";
	private ParseObject previousTurnOnParse;
	private ParseObject gameMatch;
	private bool myTurn = false;
	private int thisMovePerformedDate = 0;
	private string thisUsernameFound = null;
	private string nextUsernameFound = null;

	// Use this for initialization
	void Start () 
	{
		checkIfMyTurn ();
	}

	public void checkIfMyTurn()
	{
		//if new game
		//new record is known
		// if joining, the second person goes first
		// marks as in progress, signs their name, and tracks other player
		Debug.Log ("Checking for player...");
		DisplayError (DateTime.Now + ": Checking for other player...",false);
		StartCoroutine(makeTurnThread(5)); // make everything wait 5 seconds per
	}


	private IEnumerator makeTurnThread(int wait)
	{
		Debug.Log ("Getting last turn details...(5 second wait)");
		if (!myTurn) 
		{
			StartCoroutine (getLastTurn (5)); // make everything wait 5 seconds per
		} else 
		{
			Application.LoadLevel ("scnGame");
		}
		yield break;
		
		// ====================================
	}


	private IEnumerator getLastTurn(int wait)
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
		

		var query = ParseObject.GetQuery(gameObjName);
		//query.OrderByDescending("thisMovePerformedDate");
		// HOURS AND HOURS AND HOURS OF FRUSTRATION!!!! THIS DOES NOT WORK!
		// Must order by asc and simply obtain the last row -_-; beyond annoyed at this point
		query.OrderBy("thisMovePerformedDate");
		bool foundMyGame = false;
		string myUsername = ParseUser.CurrentUser.Username.ToString ();
		string finshedString = "Finished";
		Task runQuery = query.FindAsync().ContinueWith(t =>
		                                                {
			// if FirstAsync is used then the sorting is ignored. fml now I have to update god only knows how many other queries :(
			IEnumerable<ParseObject> _results = t.Result;
			foreach (var _result in _results)
			{
				// This does not require a network access.
				var _thismvperformeddate = _result.Get<int>("thisMovePerformedDate");
				var _thisusrname = _result.Get<string>("thisPlayerUsername");
				var _nextusrname = _result.Get<string>("nextPlayerUsername");
				var _inprog = _result.Get<string>("inProgress");

				if(!finshedString.Equals(_inprog) && (myUsername.Equals(_nextusrname) || myUsername.Equals(_thisusrname)))
				{
					this.thisUsernameFound = _thisusrname;
					this.nextUsernameFound = _nextusrname;
					if(myUsername.Equals(_nextusrname))
					{
						this.myTurn = true;
					}
					//this.gameMatch = _result;
					break;
				}

			} // the last item will be the one we actually care about. I really hate Parse right now, so much. Passionately.
		});
		yield return new WaitForSeconds(wait);
		runQuery.Wait();
		//Debug.Log ("DERP: " + playerGame.ObjectId.ToString ());
		//this.gameObjID = playerGame.ObjectId.ToString();
		//Debug.Log ("BERP: " + this.gameObjID);

		if (runQuery.IsFaulted || runQuery.IsCanceled)
		{
			Debug.Log("Error " + runQuery.Exception.Message);
			error = "Failed to sign up Parse User. Reason: " + runQuery.Exception.Message;
			// TODO: Run a thread from here to go out and do the next thing(check what)

		}
		else
		{

			//Debug.Log("Results from query: " + previousTurnOnParse.ToString());

			Debug.Log("This Game's: ParseUsername => " + ParseUser.CurrentUser.Username.ToString());
			Debug.Log("This Game's: thisUsernameFound => " + this.thisUsernameFound);
			Debug.Log("This Game's: nextUsernameFound => " + this.nextUsernameFound);


			//Application.LoadLevel("ExampleScene");
		}




		yield return new WaitForSeconds(wait);
		
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
