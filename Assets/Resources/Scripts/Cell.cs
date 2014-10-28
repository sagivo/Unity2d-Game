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

	public void select(){	
		sprite.color = ColorSelected;
		Game.menue.GetComponent<RectTransform>().position = new Vector2(transform.position.x + 2, transform.position.y + 2);
	}

	public void unSelect(){	
		sprite.color = ColorBase;
	}
	
	public bool isSelected(){
		return sprite.color == ColorSelected;
	}

	public static Cell getSelected(){
		foreach (var c in GetGame().cells)
			if (c.isSelected()) return c;
		return null;
	}



}
