using UnityEngine;
using System.Collections;
using Parse;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	public void TestParse()
	{
		Debug.Log("Starting...");
		ParseObject testObject = new ParseObject ("TestObject");
		testObject ["foo"] = "bar";
		Debug.Log("Object is: " + testObject.ToString());
		testObject.SaveAsync();
		Debug.Log ("Performed Async!");
	}

	// Update is called once per frame
	void Update () {
	
	}
}
