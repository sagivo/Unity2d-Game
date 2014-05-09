using UnityEngine;
using System.Collections;

public class Cell : BaseObj {

	public enum CellType {Empty, Player, Canon, Building};
	public CellType Type;
	SpriteRenderer sprite;
	//events
	public delegate void HitEvent(object sender, object args); public HitEvent OnHit;

	void Start(){
		sprite = gameObject.GetComponent<SpriteRenderer>();
	}

	void OnMouseEnter() {
		sprite.color = Color.cyan;
	} 

	void OnMouseExit() {
		sprite.color = Color.white;
	}
}
