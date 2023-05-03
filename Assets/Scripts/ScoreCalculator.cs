using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreCalculator : MonoBehaviour {

	public static int scoreBoard = 0;
	
	public void Score(int points)
	{
		 scoreBoard += points;
		 gameObject.GetComponent<Text>().text = scoreBoard.ToString();
	}
	public static void ResetScore()
	{
		scoreBoard = 0;
	}
}
