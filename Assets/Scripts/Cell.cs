using UnityEngine;
using System.Collections;

public class Cell : BaseObj {

	public enum CellType {Empty, Player, Canon, Building};
	public CellType Type;
	public bool Selected;
	SpriteRenderer sprite;
	//events
	public delegate void HitEvent(object sender, object args); public HitEvent OnHit;

	void Start(){
		sprite = gameObject.GetComponent<SpriteRenderer>();
	}

	void OnMouseEnter() {
		if (!Selected) sprite.color = Color.cyan;
	} 

	void OnMouseExit() {
		if (!Selected) sprite.color = Color.white;
	}

	void OnMouseDown() {
		sprite.color = (sprite.color == Color.red) ? Color.white : Color.red;
		Selected = !Selected;
	}
}
