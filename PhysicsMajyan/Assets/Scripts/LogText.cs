using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogText : MonoBehaviour {

    static GameObject obj;

	// Use this for initialization
	void Start () {
        obj = GameObject.Find("Canvas/LogText");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    static public void AddLogMessage(string message)
    {
        obj.GetComponent<Text>().text += message;
    }

    static public void SetLogMessage(string message)
    {
        obj.GetComponent<Text>().text = message;
    }

    static public void ResetLogMessage()
    {
        obj.GetComponent<Text>().text = "";
    }
}
