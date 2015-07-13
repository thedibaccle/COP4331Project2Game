using UnityEngine;
using System.Collections;
using Parse;
using System.Threading.Tasks;
using System;
using UnityEngine.UI;
using System.Collections.Generic; // need this STUPID THING so IENumerable works within result outputs. Makes no damn sense since cothreads are IENumerable FFS!

public class TurnActions : MonoBehaviour {

	private string gameObjName = "QuanvoluteMatch";
	//private ParseObject previousTurnOnParse;
	private ParseObject gameMatch;
	private int matchCreationDate = 0;
	private int thisMovePerformedDate = 0;
	private string thisUsernameFound = null;
	private string nextUsernameFound = null;
	private int thisTurnNumber = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void makeTurn()
	{
		//if new game
		//new record is known
		// if joining, the second person goes first
		// marks as in progress, signs their name, and tracks other player

		StartCoroutine(makeTurnThread(5)); // make everything wait 5 seconds per
	}
	private IEnumerator makeTurnThread(int wait)
	{
		Debug.Log ("Getting last turn details...(5 second wait)");
		DisplayError ("Looking up last turn details...", false);
		StartCoroutine (getLastTurn (wait));
		yield return new WaitForSeconds (wait);

		DisplayError ("Creating new move...", false);
		StartCoroutine (makeNewTurn (wait));
		yield return new WaitForSeconds (wait);

		DisplayError ("Going to waiting screen...", false);
		Application.LoadLevel ("scnWaiting");
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
		//query.OrderByDescending("updatedAt");
		query.OrderBy("thisMovePerformedDate");
		Task runQuery = query.FindAsync().ContinueWith(t =>
		                                                {
			// if FirstAsync is used then the sorting is ignored. fml now I have to update god only knows how many other queries :(
			IEnumerable<ParseObject> _results = t.Result;
			foreach (var _result in _results)
			{
				// This does not require a network access.
				var _matchCreationDate = _result.Get<int>("matchCreationDate");
				var _thismvperformeddate = _result.Get<int>("thisMovePerformedDate");
				var _thisusrname = _result.Get<string>("thisPlayerUsername");
				var _nextusrname = _result.Get<string>("nextPlayerUsername");
				var _turnNumber = _result.Get<int>("thisTurnNumber");
				this.matchCreationDate = _matchCreationDate;
				this.thisMovePerformedDate = _thismvperformeddate;
				this.thisUsernameFound = _thisusrname;
				this.nextUsernameFound = _nextusrname;
				this.thisTurnNumber = _turnNumber;
				
			} // the last item will be the one we actually care about. I really hate Parse right now, so much. Passionately.
			Debug.Log("this.thisMovePerformedDate => " + this.thisMovePerformedDate);
			Debug.Log("this.thisUsernameFound => " + this.thisUsernameFound);
			Debug.Log("this.nextUsernameFound => " + this.nextUsernameFound);
		});
		yield return new WaitForSeconds(wait);
		
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
			Debug.Log("Query Sucessful.");

			//Application.LoadLevel("ExampleScene");
		}
		
	}



	private IEnumerator makeNewTurn(int wait)
	{
		string error;

		TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
		int timestamp  = (int) t.TotalSeconds;
		//Console.WriteLine (timestamp);
		
		Debug.Log("Making Move...");
		int gameBoardState = 999; // god this thing is a pain to declare as a 2d int array, screw it, object for now
		
		// 8x8 grid, 0 for null, 1 for p1, 2 for p2
		// Overwrites the current obj with whatever this is
		//var matchCreationDate = previousTurnOnParse.Get<int>("matchCreationDate");

		// !!!!!!!
		/// This is NOT A TYPO, the next player in this case is what the previous' turn's thisPlayer was!
		//var nextPlayerName = previousTurnOnParse.Get<string>("thisPlayerUsername");
		// !!!!!!!!
		this.thisTurnNumber++;

		Debug.LogWarning ("thisPlayerUsername=>"+ParseUser.CurrentUser.Username.ToString()+" nextPlayerUsername=>"+this.thisUsernameFound);

		this.gameMatch = new ParseObject(gameObjName)
		{
			{"matchCreationDate", this.matchCreationDate},
			{"thisMovePerformedDate", timestamp},
			{"thisPlayerUsername", ParseUser.CurrentUser.Username.ToString()},
			{"thisTurnNumber", this.thisTurnNumber}, // init at 0, mark as 1 for the first actual turn
			{"nextPlayerUsername", this.thisUsernameFound},
			{"inProgress", "InProgress"},
			{"thisBoardState",  gameBoardState}
		};
		
		Debug.Log("qMatch Object is: " + this.gameMatch.ToString());
		Task qMatchSyncTask = this.gameMatch.SaveAsync();
		yield return new WaitForSeconds(wait);
		
		if (qMatchSyncTask.IsFaulted || qMatchSyncTask.IsCanceled)
		{
			Debug.Log("Error " + qMatchSyncTask.Exception.Message);
			error = "Failed to sign up Parse User. Reason: " + qMatchSyncTask.Exception.Message;
			// TODO: Run a thread from here to go out and do the next thing(check what)
			//this.gameObjID = "error";
		}
		else
		{
			//this.gameObjID = playerGame.ObjectId.ToString();
			//Debug.Log("Results from query: " + this.gameObjID);
			Debug.Log ("New Turn Action Performed Sucessfully.");
			//Application.LoadLevel("ExampleScene");
		}
		
		//Debug.Log ("Performed qMatchAsync!");
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
}
