using UnityEngine;
using System.Collections;

public class Cell : BaseObj {
	public enum cellType {Empty, Player, Canon, Building};
	public cellType type;
	public bool selected;
	public Liveable liveObj;

	//colors
	public Color ColorSelected = Color.red;
	public Color ColorBase = Color.white;
	Vector2[] distanceEdges = new Vector2[] {new Vector2(3.8f,2.5f), new Vector2(3.8f,-2.2f), new Vector2(0,-4.35f), new Vector2(-3.8f,-2.2f), new Vector2(-3.8f,2.15f), new Vector2(0,4.35f)};


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

	public void expend(){
		foreach (var v2 in distanceEdges){
			var hit = Physics2D.Linecast(Camera.main.transform.position, transform.position +  new Vector3(v2.x,v2.y), 1 << LayerMask.NameToLayer("Cells"));
			//var hit = Physics2D.Raycast(Camera.main.transform.position, transform.position +  new Vector3(v2.x,v2.y,0) ,100 , ( 1 << LayerMask.NameToLayer("Cells") ));
			if (hit.collider){
				l ("boom");
			} else l ("no");
		}
	}
}
