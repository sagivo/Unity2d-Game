using UnityEngine;
using UnityEditor;
using System.Collections;

public class Cell : Liveable {
	public enum cellType {Empty, Player, Canon, Building};
	public cellType type;
	public bool selected;
	public Liveable liveObj;
	int[] expendCostPerLevel = Vars.Balance.Player.Cell.expendCostPerLevel;

	//colors
	Color ColorSelected = Color.red;
	Color ColorInactive = Color.gray;
	Color ColorBase = Color.white;
	Vector2[] distanceEdges = new Vector2[] {new Vector2(3.8f,2.15f), new Vector2(3.8f,-2.2f), new Vector2(0,-4.35f), new Vector2(-3.8f,-2.2f), new Vector2(-3.8f,2.15f), new Vector2(0,4.35f)};
	//Vector2[] distanceEdges = new Vector2[] {new Vector2(3.8f,2.15f)};


	SpriteRenderer sprite;
	//events
	protected new void Awake(){
		base.Awake();
		showHealthBar = false;
	}

	protected new void Start(){
		base.Start();
		sprite = GetComponent<SpriteRenderer>();
		Game.cells.Add(this);

		OnStatusChange += (s) => {
			if (status == StatusType.InActive) sprite.color = ColorInactive;
		};
	}

	protected new void Update(){
		base.Update();
	}
	
	void OnDestroy(){
		Game.cells.Remove(this);
	}

	public void select(){
		if (status != StatusType.Live) return;
		var selected = getSelected();
		if (selected != null) selected.unSelect();
		sprite.color = ColorSelected;
		//Game.menue.GetComponent<RectTransform>().position = new Vector2(transform.position.x + 2, transform.position.y + 2);
	}

	public void unSelect(){	
		if (status != StatusType.Live) return;
		sprite.color = ColorBase;
		renderer.material.color = ColorBase;
	}

	public static void unselectSelected(){
		var cell = getSelected ();
		if (cell!=null) cell.unSelect();
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
		if (!canExpend()) return;
		Game.minerals -= expendCostPerLevel[level];
		unSelect();
		sprite.color = ColorInactive;
		var count = 0;
		foreach (var v2 in distanceEdges){
			//Debug.DrawLine(Camera.main.transform.position, gameObject.transform.position +  new Vector3(v2.x,v2.y,1), Color.yellow, Mathf.Infinity);
			Ray ray = Camera.main.ViewportPointToRay(Camera.main.WorldToViewportPoint(gameObject.transform.position + new Vector3(v2.x,v2.y )));
			var hit = Physics2D.Raycast(ray.origin, ray.direction ,Mathf.Infinity , ( 1 << LayerMask.NameToLayer("Cells") ));
			if (hit.collider == null){
				var go = (GameObject)Resources.Load(Vars.PrefabPaths.cell);
				(GameObject.Instantiate(go, transform.position +  new Vector3(v2.x,v2.y,0),transform.rotation) as GameObject).transform.parent = transform.parent;
				count++;
			} 
		}
		setInactive();
	}

	public bool canExpend(){
		return (!liveObj && Game.minerals >= expendCostPerLevel[level]);
	}

	public void HandleSelect(Drag.DragType dragType, Building building){
		unselectSelected();
		switch (dragType) {
		case Drag.DragType.Build:				
			if (!liveObj && Game.canBuild(building)) select();
			break;
		case Drag.DragType.Upgrade:
			if (liveObj && Game.canUpgrade(building)) select();
			break;
		case Drag.DragType.Refund:
			if (liveObj) select();
			break;
		case Drag.DragType.Expand:
			if (!liveObj && canExpend()) select();
			break;
			default: break;
		}
	}
}
