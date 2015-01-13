/*
 	Divider.cs
 	
 	Michael Stephens
 	www.michaeljohnstephens.com
 	
 	Created:		August 27, 2014
 	Last Edited:	August 27, 2014
 	
 	Provides a divider in-editor drawer.
*/


using UnityEngine;


public class Divider : PropertyAttribute 
{
	#region Properties

	// The amount of vertical space this divider takes up
	public readonly float space = 45;
	
	// The color of the line
	public readonly string col = "white";
	
	// The thickness of the line
	public readonly float thickness = 1;
	
	// The width of the line (percentage of the window)
	public float widthPct = 0.85f;
	
	#endregion
	
	
	#region Constructors
	
	public Divider (string col, float thickness, float widthPct, float space)
	{
		this.col = col;
		this.thickness = thickness;
		this.widthPct = widthPct;
		this.space = space;
	}
	
	public Divider (string col, float thickness, float widthPct)
	{
		this.col = col;
		this.thickness = thickness;
		this.widthPct = widthPct;
	}
	
	public Divider (string col, float thickness)
	{
		this.col = col;
		this.thickness = thickness;
	}
	
	public Divider (string col)
	{
		this.col = col;
	}
	
	public Divider (float widthPct, float thickness, float space)
	{
		this.widthPct = widthPct;
		this.thickness = thickness;
		this.space = space;
	}
	
	public Divider (float widthPct, float thickness)
	{
		this.widthPct = widthPct;
		this.thickness = thickness;
	}
	
	public Divider (float widthPct)
	{
		this.widthPct = widthPct;
	}
	
	public Divider () { }
	
	#endregion
}