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
		//Destroy (GameObject.Find("gameCamera"));
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
		StartCoroutine(makeTurnThread()); // make everything wait 5 seconds per
	}


	private IEnumerator makeTurnThread()
	{

		Debug.Log ("Getting last turn details...");
		if(!myTurn) 
		{
			StartCoroutine (getLastTurn ()); // make everything wait 5 seconds per
		} 
		else 
		{
			Application.LoadLevel ("scnGame");
		}
		yield break;
		
		// ====================================
	}


	private IEnumerator getLastTurn()
	{
		
		while (!myTurn) 
		{
		
			string error;


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


			if(!myTurn) 
			{
				StartCoroutine (getLastTurn ()); // make everything wait 5 seconds per
			} 
			else 
			{
				Application.LoadLevel ("scnGame");
			}
			// Force wait 5 seconds before trying again
			Debug.Log ("Waiting 5 seconds...");
			for(int i = 5; i>0; i--)
			{
				yield return new WaitForSeconds (1);
				Debug.Log ("Waiting "+i+"...");
			}
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
