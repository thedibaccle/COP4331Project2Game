using UnityEngine;
using System.Collections;
using Parse;
using System.Threading.Tasks;
using UnityEngine.UI;


public class UserManagerScript : MonoBehaviour 
{

	public void TestParse()
	{
		Debug.Log("Starting...");
		ParseObject testObject = new ParseObject ("TestObject");
		testObject ["foo"] = "bar";
		Debug.Log("Object is: " + testObject.ToString());
		Task testing = testObject.SaveAsync();
		Debug.Log ("Performed Async!");
	}

	public void TestTextGrab()
	{
		GameObject txtEmailGo = GameObject.Find("txtEmail"); // finding the game object for txtEmail
		GameObject txtPasswordGo = GameObject.Find("txtPassword"); // finding the game object for txtPassword


		if (txtEmailGo != null)
		{
			//Debug.Log ("Not null");
			// Instantiating the component of type InputField, so we can reference things like .text
			InputField txtEmailCo = txtEmailGo.GetComponent<InputField>();
			InputField txtPasswordCo = txtPasswordGo.GetComponent<InputField>();

			string emailValue = txtEmailCo.text;
			string passwordValue = txtPasswordCo.text;

			txtEmailCo.enabled = false;
			txtPasswordCo.enabled = false;



			if(emailValue != "" && passwordValue != "")
			{
				Debug.Log ("Starting parse!");

				Debug.Log ("First pass through...");
				// This is REALLY ugly but using a seperate method for this was creating funky thread issues so due to time........ (sorry)
				// we're not actually logged in yet, an account was probably all that was created, try again.
				if(ParseUser.CurrentUser == null)
				{
					//Task loggingTask = LoginOrCreateParseAccount(emailValue,passwordValue);
					//DisplayError("Loading...(5 seconds)",false);

					//System.Threading.Thread.Sleep(5*1000);
					//DisplayError("",false);
					// example call: 
					StartCoroutine(LoginWaitCoRoutine(emailValue,passwordValue,5));

				}


			}
			else
			{
				DisplayError("Please provide E-mail and password.",true);
				txtEmailCo.enabled = true;
				txtPasswordCo.enabled = true;
			}
			// Run dat parse!
			//Debug.Log(emailValue);

			//JoinAnyGame(txtEmailCo.text,txtPasswordCo.text);

			//Debug.Log ("Value is: " + temp.text);

		} else {
			DisplayError ("System Error. Unable to find txtEmailGo.",false);
		}
		//Debug.Log ();
		//InputText
	}

	// example call: StartCoroutine(WaitCoRoutine(5));

	IEnumerator LoginWaitCoRoutine(string emailValue, string passwordValue, int wait){
		Task loggingTask = LoginOrCreateParseAccount(emailValue,passwordValue);
		DisplayError("Loading...(" + wait + " seconds)",false);
		Debug.Log ("Waiting for " + wait + " second(s)...");
		yield return new WaitForSeconds(wait);
		DisplayError("",false);
		Debug.Log ("Done! Waited " + wait + " second(s).");
		if(loggingTask !=null && (loggingTask.IsCompleted || loggingTask.IsFaulted || loggingTask.IsCanceled) && ParseUser.CurrentUser == null)
		{
			DisplayError("Error: invalid e-mail address or bad password.",false);
			GameObject txtEmailGo = GameObject.Find("txtEmail"); // finding the game object for txtEmail
			GameObject txtPasswordGo = GameObject.Find("txtPassword"); // finding the game object for txtPassword
			InputField txtEmailCo = txtEmailGo.GetComponent<InputField>();
			InputField txtPasswordCo = txtPasswordGo.GetComponent<InputField>();
			txtEmailCo.enabled = true;
			txtPasswordCo.enabled = true;
		}
		else
		{
			Application.LoadLevel("scnInitMatching");
		}
		yield break;
		Debug.Log ("You'll never see this"); // produces a dead code warning
	}


	private Task LoginOrCreateParseAccount(string emailValue, string passwordValue)
	{
		if (ParseUser.CurrentUser == null) 
		{
			Debug.Log ("No user logged in, trying to login...");
			Task loginTask = ParseUser.LogInAsync (emailValue, passwordValue).ContinueWith (t =>
			{
				if (t.IsFaulted || t.IsCanceled) {
					// The login failed. Check the error to see why.
					// Since login failed, try to just create the login for the user
				
					// TODO: need if statement for "IF email does not exist THEN make new account"
					//       currently it just ASSUMES the login failed ONLY because the account doesn't exist
					// email is shared between username and email to simplify the login process to an instant method
				
					// Temporarily hardcoding the password value.
					// This will allow ANYONE to login regardless of password entered
					// TODO: I need to add proper logic to know that, hey, account exists, password is wrong, stop trying to run this method over and over to create and login and display an error.
				
					Debug.Log ("Login attempt failed, trying to make a new user now...");
					var user = new ParseUser ()
				{
					Username = emailValue,
					Password = passwordValue,
					Email = emailValue
				};
				
					// other fields can be set just like with ParseObject
					//user["phone"] = "415-392-0202";
				
					Task signUpTask = user.SignUpAsync ();
				
				
					// TODO: place if statement that checks that the username wasn't already taken and display message saying so
					//while (!signUpTask.IsCompleted) {
						// do nothing
						// NEVERMINDTODO: add a counter to time this out after 5 minutes.
						//Debug.Log ("Loading....");
					//}

					Debug.Log ("Account created!");
					// TODO: add else if statement that will display error of wrong password if account already exists, but password was wrong
				
				
				
				
				
				} else {
					// Login was successful.
					//this.ChangeSceen (1); // okay, go to game screen now
					Debug.Log ("Login sucessful.");
					// can't do this in the aynch thread call Application.LoadLevel ("scnMatching"); // load/switch to the scnMatching scene.
				}
			}); // END OF ASYNC THREAD

			return loginTask;
		}
		return null;
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

	public void ReturnToLoginScreen()
	{
		if (ParseUser.CurrentUser != null) {
			Debug.Log("Trying to log out of current user...");
			ParseUser.LogOut ();
		} else {
			Debug.LogError("Error: trying to log out of a user that isn't even logged in.");
		}
		Application.LoadLevel("scnMainMenu");
	}


}
