using UnityEngine;
using System.Collections;

public class Cell : BaseObj {

	public enum CellType {Empty, Player, Canon, Building};
	public CellType Type;
	public bool Selected;

	//colors
	public Color ColorSelected = Color.red;
	public Color ColorOver = Color.cyan;
	public Color ColorBase = Color.white;

	SpriteRenderer sprite;
	//events
	public delegate void HitEvent(object sender, object args); public HitEvent OnHit;

	void Start(){
		Debug.Log('2');
		Game.Cells.Add(this);
		sprite = gameObject.GetComponent<SpriteRenderer>();
	}

	void OnMouseEnter() {
		if (!Selected) sprite.color = ColorOver;
	} 

	void OnMouseExit() {
		if (!Selected) sprite.color = ColorBase;
	}

	void OnMouseDown() {
		sprite.color = (sprite.color == ColorSelected) ? ColorBase : ColorSelected;
		Selected = !Selected;
	}
	
	void OnDestroy(){
		Game.Cells.Remove(this);
	}

}
