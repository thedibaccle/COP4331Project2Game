using UnityEngine;
using System.Collections;
using Parse;
using System.Threading.Tasks;
using UnityEngine.UI;


public class UserManagerScript : MonoBehaviour 
{

	public void LoginButtonPressed()
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
				if(ParseUser.CurrentUser == null)
				{
					StartCoroutine(LoginWaitCoRoutine(emailValue,passwordValue));
				}
			}
			else
			{
				DisplayError("Please provide E-mail and password.",true);
				txtEmailCo.enabled = true;
				txtPasswordCo.enabled = true;
			}
		}
		else
		{
			DisplayError ("System Error. Unable to find txtEmailGo.",false);
		}
	}
	
	IEnumerator LoginWaitCoRoutine(string emailValue, string passwordValue){
		Task loggingTask = LoginOrCreateParseAccount(emailValue,passwordValue);

		Debug.Log ("Waiting...");
		int durationTime = 0;
		for (int i = 0; i < 30; i++) 
		{
			if(i%4==0){DisplayError("Loading",false);}
			else if(i%4==1){DisplayError("Loading.",false);}
			else if(i%4==2){DisplayError("Loading..",false);}
			else if(i%4==3){DisplayError("Loading...",false);}

			if(loggingTask.IsCompleted){break;}
			else
			{
				durationTime++;
				yield return new WaitForSeconds (1);
			}
		}
		DisplayError("",false);
		Debug.Log ("Done! Waited " + durationTime + " second(s).");
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

					Debug.Log ("Account created!");
					// TODO: add else if statement that will display error of wrong password if account already exists, but password was wrong
				}
				else
				{
					Debug.Log ("Login sucessful.");
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
