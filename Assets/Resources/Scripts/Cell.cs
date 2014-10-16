using UnityEngine;
using System.Collections;

public class Cell : BaseObj {

	public enum cellType {Empty, Player, Canon, Building};
	public cellType type;
	public bool selected;

	//colors
	public Color ColorSelected = Color.red;
	public Color ColorOver = Color.cyan;
	public Color ColorBase = Color.white;

	SpriteRenderer sprite;
	//events
	public delegate void HitEvent(object sender, object args); public HitEvent OnHit;

	protected new void Start(){
		base.Start();
		Game.Cells.Add(this);
		sprite = gameObject.GetComponent<SpriteRenderer>();
	}

	void OnMouseEnter() {
		if (!selected) sprite.color = ColorOver;
	} 

	void OnMouseExit() {
		if (!selected) sprite.color = ColorBase;
	}

	void OnMouseDown() {
		sprite.color = (sprite.color == ColorSelected) ? ColorBase : ColorSelected;
		selected = !selected;
	}
	
	void OnDestroy(){
		Game.Cells.Remove(this);
	}

}
