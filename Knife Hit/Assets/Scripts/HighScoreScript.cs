using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreScript : MonoBehaviour
{

	void Start()
	{

	}

	void Update()
	{
		GetComponent<Text>().text = "High score: " + GameController.GetHighScore();
	}
}
