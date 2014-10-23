using UnityEngine;
using System.Collections;

public class Cell : BaseObj {

	public enum cellType {Empty, Player, Canon, Building};
	public cellType type;
	public bool selected;

	//colors
	public Color ColorSelected = Color.red;
	public Color ColorBase = Color.white;

	SpriteRenderer sprite;
	//events
	public delegate void HitEvent(object sender, object args); public HitEvent OnHit;

	protected new void Start(){
		base.Start();
		sprite = GetComponent<SpriteRenderer>();
		Game.cells.Add(this);
	}

	protected new void Update(){
		base.Update();
	}
	
	void OnDestroy(){
		Game.cells.Remove(this);
	}

	public void toggleSelect(){
		/*
		foreach(Cell c in Game.cells){
			if (c == this) sprite.color = ((sprite.color == ColorSelected) ? ColorBase : ColorSelected);
			else c.sprite.color = ColorBase;
		}
		*/
		sprite.color = ((sprite.color == ColorSelected) ? ColorBase : ColorSelected);
	}

}
