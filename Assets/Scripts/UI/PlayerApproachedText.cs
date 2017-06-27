using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerApproachedText : MonoBehaviour {

	public float flashSpeed;

	private Text text;
	private Color flashColor;
	private Color IdleColor;


	private void Start ()
	{
		text = GetComponent<Text> ();
		IdleColor = text.color;
		flashColor = new Color(text.color.r, text.color.g, text.color.b, 1f);
	}
	

	private void Update ()
	{
		text.color = Color.Lerp (text.color, IdleColor, flashSpeed * Time.deltaTime);
	}


	public void Flash()
	{
		text.color = flashColor;
	}
}
