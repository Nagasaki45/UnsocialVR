using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FlashScreen : MonoBehaviour {

	public float flashSpeed;

	private Image image;
	private Color flashColor;
	private Color IdleColor;


	private void Start ()
	{
		image = GetComponent<Image> ();
		IdleColor = image.color;
		flashColor = new Color(image.color.r, image.color.g, image.color.b, 1f);
	}
	

	private void Update ()
	{
		image.color = Color.Lerp (image.color, IdleColor, flashSpeed * Time.deltaTime);
	}


	public void Flash()
	{
		image.color = flashColor;
	}
}
