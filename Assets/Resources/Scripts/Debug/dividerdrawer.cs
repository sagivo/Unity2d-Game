/*
 	DividerDrawer.cs
 	
 	Michael Stephens
 	www.michaeljohnstephens.com
 	
 	Created:		August 27, 2014
 	Last Edited:	August 27, 2014
 	
 	Drawer for the divider attribute.
*/


using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;


[CustomPropertyDrawer (typeof (Divider))]
public class DividerDrawer : DecoratorDrawer 
{
	// Used to calculate the height of the box
	public static Texture2D lineTex = null;
	
	
	Divider divider { get { return ((Divider) attribute); } }
	
	
	// Get the height of the element
	public override float GetHeight () 
	{
		return base.GetHeight () + divider.space;
	}
	
	
	// Override the GUI drawing for this attribute
	public override void OnGUI (Rect position) 
	{		
		// Get the color the line should be
		Color co = Color.white;
		switch (divider.col.ToLower ())
		{
			case "white": co = Color.white; break;
			case "red": co = Color.red; break;
			case "blue": co = Color.blue; break;
			case "green": co = Color.green; break;
			case "gray": co = Color.gray; break;
			case "grey": co = Color.grey; break;
			case "black": co = Color.black; break;
		}
		
		// Define the texture we will use to draw the line
		lineTex = new Texture2D (1, 1, TextureFormat.ARGB32, true);
		lineTex.SetPixel (0, 1, co);
		lineTex.Apply ();
		
		// Define the line perameters
		float lineWidth = position.width * divider.widthPct;
		float lineX = ((position.x + position.width) - lineWidth - ((position.width - lineWidth) / 2));
		float lineY = position.y + (divider.space / 2);
		float lineHeight = divider.thickness;
		
		// Draw the actual line
		EditorGUI.DrawPreviewTexture (new Rect (lineX, lineY, lineWidth, lineHeight), lineTex);
	}
}
