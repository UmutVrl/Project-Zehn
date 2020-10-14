using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Leap;

using TMPro;
using UnityEngine.SceneManagement;


public class LeapZehnMain : MonoBehaviour
{

	public bool isTiming;
	public float ReactionTimer;
	public float totalReactionTime;
	
	private Controller controller;
	private Frame current;
	private Hand handRight;
	private Hand handLeft;

	private IEnumerator watcherCoroutine;
	readonly string[] digits = new string[8] {"1", "2", "3", "4", "5", "6", "7", "8"};
	private int randomNumber;

	// Start is called before the first frame update
	private void Start()
	{
		controller = new Controller();
		current = controller.Frame();
	}

	// Update is called once per frame
	private void Update()
	{

		if (isTiming)
			ReactionTimer += Time.deltaTime;


		if (Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(GestureDetectionMain());
		}
		else if (Input.GetKeyDown(KeyCode.Q))
		{
			StopCoroutine(GestureDetectionMain());
			GameObject.Find("ScreenMain").GetComponent<TextMeshPro>().text = "";
		}
		else if (Input.GetKeyDown(KeyCode.M))
		{
			StopCoroutine(GestureDetectionMain());
			SceneManager.LoadScene("LEAP_Main");
		}

	}

	private IEnumerator GestureDetectionMain()
	{
		var screenMain = GameObject.Find("ScreenMain").GetComponent<TextMeshPro>();
		screenMain.color = new Color(1.0f, 0.64f, 0.0f);
		screenMain.alignment = TextAlignmentOptions.Center;
		
		yield return new WaitForSeconds(1f);
		screenMain.text = "Wenn du bereit bist, zeige eine '10'!";
		yield return new WaitForSeconds(4f);
		yield return new WaitUntil(() => DigitDetector() == 10) ;
//      yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.L));
        yield return new WaitForSeconds(1f);
		
		for (int i = 0; i < 10; i++)
		{
		
		randomNumber = RandomNumberGenerator();
		screenMain.color = new Color(1.0f, 0.64f, 0.0f);
		screenMain.alignment = TextAlignmentOptions.Center;
		
		yield return new WaitForSeconds(3f);
		screenMain.text = " ";
		yield return new WaitForSeconds(1f);
		screenMain.text = "Mach dich bereit!";
		yield return new WaitForSeconds(9f);
		/*screenMain.text = "";
		yield return new WaitForSeconds(1f);
		screenMain.text = "";
		yield return new WaitForSeconds(1f);
		screenMain.text = "";
		yield return new WaitForSeconds(1f);
		screenMain.text = "";
		yield return new WaitForSeconds(1f);
		screenMain.text = "";
		yield return new WaitForSeconds(1f);
		screenMain.text = "";
		yield return new WaitForSeconds(1f);
		screenMain.text = "";
		yield return new WaitForSeconds(1f);
		screenMain.text = "";
		yield return new WaitForSeconds(1f);
		screenMain.text = "";
		yield return new WaitForSeconds(1f);*/
		screenMain.text = "";
		yield return new WaitForSeconds(1f);
		screenMain.text = "LOS!";
		yield return new WaitForSeconds(1f);
		screenMain.text = " ";
		var fixDotDuration = UnityEngine.Random.Range(1f, 1.5f);
		screenMain.color = Color.red;
		RandomTextAlignment();
		yield return new WaitForSeconds(fixDotDuration);
		var screenDigit = digits[randomNumber];

		isTiming = true;
		screenMain.text = screenDigit;
		yield return new WaitUntil(() => DigitDetector() == randomNumber + 1);
//		yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.O));
		isTiming = false;
		var roundedRT = (float) (Math.Round(ReactionTimer, 4));
		screenMain.text = roundedRT + " sec!";
		GetImage();
		totalReactionTime += ReactionTimer;
		ReactionTimer = 0.0f;
	}

		yield return new WaitForSeconds(1f);
		screenMain.text = " ";
		yield return new WaitForSeconds(1f);
		screenMain.alignment = TextAlignmentOptions.Center;
		var totalroundedRT = (float) (Math.Round(totalReactionTime, 4));
		screenMain.text = "Total: " + totalroundedRT + " sec!";
		
	}

	private int DigitDetector() //Hand Digit Calculator
	{
		int a = 0, b = 0, c = 0, d = 0, e = 0, sum = 0;
		int f = 0, g = 0, h = 0, i = 0, j = 0;

		current = controller.Frame();

		if (current.Hands.Count == 1)
		{
			if (current.Hands[0].IsRight)
			{
				handRight = current.Hands[0];
				handLeft = null;
			}
			else if (current.Hands[0].IsLeft)
			{
				handLeft = current.Hands[0];
				handRight = null;
			}
		}
		else if (current.Hands.Count == 2)
		{
			handRight = current.Hands[0];
			handLeft = current.Hands[1];
		}
		else if (current.Hands.Count == 0)
		{
			handRight = null;
			handLeft = null;
		}

		if (handRight != null)
		{
			if (handRight.Fingers[0].IsExtended)
				a = 1;
			if (handRight.Fingers[1].IsExtended)
				b = 1;
			if (handRight.Fingers[2].IsExtended)
				c = 1;
			if (handRight.Fingers[3].IsExtended)
				d = 1;
			if (handRight.Fingers[4].IsExtended)
				e = 1;
		}

		if (handLeft != null)
		{
			if (handLeft.Fingers[0].IsExtended)
				f = 1;
			if (handLeft.Fingers[1].IsExtended)
				g = 1;
			if (handLeft.Fingers[2].IsExtended)
				h = 1;
			if (handLeft.Fingers[3].IsExtended)
				i = 1;
			if (handLeft.Fingers[4].IsExtended)
				j = 1;
		}

		sum = a + b + c + d + e + f + g + h + i + j;
		return sum;
	}

	private void GetImage()
	{
		if (!controller.IsConnected) return;
		var fileName = "ZehnProject_Zahl" +  digits[randomNumber] + DateTime.Now.ToString("_dd-MM-yyyy_hh-mm") + ".png";
		ScreenCapture.CaptureScreenshot(fileName);
	}

	private static int RandomNumberGenerator()
	{
		var numberRandom = UnityEngine.Random.Range (0, 8);
		var previous = numberRandom;
		while (numberRandom == previous)
		{
			numberRandom = UnityEngine.Random.Range(0, 8);
		}
		return numberRandom;

	}

	static void RandomTextAlignment()
	{
		var alignmentText = UnityEngine.Random.Range(0, 3);
		var screenMain = GameObject.Find("ScreenMain").GetComponent<TextMeshPro>();

		if (alignmentText == 0)
			screenMain.alignment = TextAlignmentOptions.Left;
		else if(alignmentText == 1) 
			screenMain.alignment = TextAlignmentOptions.Right;
		else if(alignmentText == 2) 
			screenMain.alignment = TextAlignmentOptions.Center;
	}
	

	
	
}
