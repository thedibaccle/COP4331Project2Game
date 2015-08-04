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
	//private string thisUsernameFound = null;
	//private string nextUsernameFound = null;
	private int thisTurnNumber = 0;
		
	// Use this for initialization
	void Start () {
		//makeTurn ();
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
		Debug.Log ("Getting last turn details...(5 second wait)");
		DisplayError ("Looking up last turn details...", false);
		StartCoroutine (makeTheDamnTurn ());
	}



	protected IEnumerator makeTheDamnTurn()
	{
		
		Debug.Log ("Step 1");
		string error;


		var query = ParseObject.GetQuery(gameObjName);
		//query.OrderByDescending("updatedAt");
		query.OrderBy("thisMovePerformedDate");
		bool foundMyGame = false;
		string myUsername = ParseUser.CurrentUser.Username.ToString ();
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
				//var _thisplayernumber = _result.Get<int>("thisPlayerNumber");
				var _turnNumber = _result.Get<int>("thisTurnNumber");
				//this.matchCreationDate = _matchCreationDate;
				//this.thisMovePerformedDate = _thismvperformeddate;
				string thisUserNameFound = _thisusrname;
				string nextUsernameFound = _nextusrname;
				//this.thisTurnNumber = _turnNumber;
				//this.thisTurnNumber = _turnNumber;

				if(myUsername.Equals(thisUserNameFound) || myUsername.Equals(nextUsernameFound))
				{
					foundMyGame=true;
					this.gameMatch = _result;
				}
				else if(!foundMyGame && nextUsernameFound == null) // haven't found a game, but here's one who needs a friend
				{
					this.gameMatch = _result;
				}
				else
				{

				}
			} // the last item will be the one we actually care about. I really hate Parse right now, so much. Passionately.
			Debug.Log("this.thisMovePerformedDate => " + this.thisMovePerformedDate);
			Debug.Log("this.thisUsernameFound => " + this.gameMatch["thisPlayerUsername"]);
			Debug.Log("this.nextUsernameFound => " + this.gameMatch["nextPlayerUsername"]);
		});
		Debug.Log ("Step 2");
		int durationTime = 0;
		for (int i = 0; i < 30; i++) 
		{
			if(runQuery.IsCompleted){break;}
			else
			{
				durationTime++;
				yield return new WaitForSeconds (1);
			}
		}
		Debug.Log ("Done! Waited " + durationTime + " second(s).");
		
		//Debug.Log ("DERP: " + playerGame.ObjectId.ToString ());
		//this.gameObjID = playerGame.ObjectId.ToString();
		//Debug.Log ("BERP: " + this.gameObjID);
		Debug.Log ("Step 3");
		if (runQuery.IsFaulted || runQuery.IsCanceled)
		{
			Debug.Log("Error " + runQuery.Exception.Message);
			error = "Failed to sign up Parse User. Reason: " + runQuery.Exception.Message;
			// TODO: Run a thread from here to go out and do the next thing(check what)
		}
		else
		{
			Debug.Log ("Step 4");
			Debug.Log("Query Sucessful.");
			string otherPlayer = this.gameMatch.Get<string>("thisPlayerUsername");
			int thisPlayerNumber = this.gameMatch.Get<int>("thisPlayerNumber");
			Debug.Log ("Made it here>>");
			string _boardState = Board.turnToSave;
			Debug.Log ("Board going into Parse: " + _boardState);
		
				// Now let's update it with some new data.  In this case, only cheatMode
				// and score will get sent to the cloud.  playerName hasn't changed.
			this.gameMatch["thisPlayerUsername"] = myUsername;
			this.gameMatch["thisPlayerNumber"] = (thisPlayerNumber==1)?2:1; // alternate between 1 and 2;
			this.gameMatch["nextPlayerUsername"] = otherPlayer;
			this.gameMatch["inProgress"] = "InProgress";
			this.gameMatch["thisBoardState"] = _boardState;
			this.gameMatch["thisTurnNumber"] = (this.thisTurnNumber+1);
				//this.gameMatch.SaveAsync();
			/*
			var bigObject = new ParseObject(gameObjName);
			//bigObject.ObjectId = this.gameMatch.ObjectId;
			bigObject["thisPlayerUsername"] = myUsername;
			bigObject["thisPlayerNumber"] = (thisPlayerNumber==1)?2:1; // alternate between 1 and 2;
			bigObject["nextPlayerUsername"] = this.thisUsernameFound;
			bigObject["inProgress"] = "InProgress";
			bigObject["thisBoardState"] = _boardState;
			bigObject["thisTurnNumber"] = (this.thisTurnNumber+1);
		*/
			//Task saveTask = bigObject.SaveAsync();
			//Debug.LogWarning("Dirty?: " + this.gameMatch.IsDirty);
			Debug.Log ("Step 5");
			Task performGameAsync=this.gameMatch.SaveAsync();
			durationTime = 0;
			for (int i = 0; i < 30; i++) 
			{
				Debug.Log ("Tick:" + this.gameMatch.ObjectId);
				if(performGameAsync.IsCompleted){break;}
				else
				{
					durationTime++;
					yield return new WaitForSeconds (1);
				}
			}
			Debug.Log ("Step 6");
			Debug.Log ("<<Made it here (is complete? =>" + performGameAsync.IsCompleted + ")");
			Debug.Log ("Done! Waited for Async save: " + durationTime + " second(s).");

			//Board.gameMatch = this.gameMatch;


			Debug.Log ("Going to waiting screen...");
			Board.turnCounter = this.thisTurnNumber;
			Application.LoadLevel ("scnWaiting");
			yield break;
			//Application.LoadLevel("ExampleScene");
		}
		
	}

	private static void DisplayError(string errorMsg, bool append)
	{
		GameObject lblErrorMsgGo = GameObject.Find("lblErrorMsg"); // finding the game object for txtEmail
		//Text lblErrorMsgCo = lblErrorMsgGo.GetComponent<Text>();
		
		//string _previousErrorMsg = lblErrorMsgCo.text;
		string _previousErrorMsg = ""; // we shoudl be using a lbl.text but can't due to game size
		string _output = errorMsg;
		if (append) 
		{
			_output = _previousErrorMsg + errorMsg + "; ";
		} else 
		{
			_output = errorMsg;
		}

		Debug.Log (_output);
		//lblErrorMsgCo.text = _output;
	}
}
