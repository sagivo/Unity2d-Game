using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class CellMenue : BaseObj {
	public static float longClickTime = .6f;
	Animator anim;
	float openMenueTime = 0;
	CanvasGroup cg;

	protected new void Start () {
		base.Start();
		anim = GetComponent<Animator>();
		cg = GetComponent<CanvasGroup>();
	}
	
	protected new void Update () {
		base.Update();
		if (Input.GetMouseButtonDown(0) && (openMenueTime == 0) && (Input.touchCount <= 1)) openMenueTime = Time.time + longClickTime;
		else if (Input.GetMouseButtonUp(0)) openMenueTime = 0;
		//if (openMenueTime != 0 && openMenueTime <= Time.time) openMenue(); TODO:uncomment when you want cell menue!
	}

	public void openMenue(){
		cg.interactable = true;
		openMenueTime = 0;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var hit = Physics2D.Raycast(ray.origin,Vector2.zero ,100 , ( 1 << LayerMask.NameToLayer("Cells") ));
		if (hit.collider){
			var c = hit.collider.GetComponent<Cell>();
			if (c) c.select();
		}
	}

	public void closeMenue(){
		cg.interactable = false;
		anim.SetBool("IsOpen", false);
		Game.menue.GetComponent<RectTransform>().position = new Vector2(100,100);
	}

	public void build(Building o){
		if (!o.canBuild()) return;
		Cell cell = Cell.getSelected();
		Building newObj = GameObject.Instantiate(o, cell.transform.renderer.bounds.center,Quaternion.identity) as Building;
		cell.liveObj = newObj;
		closeMenue(); cell.unSelect();
	}

	public void destroy(){
		Cell cell = Cell.getSelected();
		(cell.liveObj as Building).refund();
		closeMenue(); cell.unSelect();
	} 

	public void expend(){
		var c = Cell.getSelected();
		if (!c.canExpend()) return;
		c.expend();
		closeMenue(); 
	}

	public void upgrade2(){
		Cell cell = Cell.getSelected();
		var building = (cell.liveObj as Building);
		if (building.enoughResourcesToUpgrade()) {
			Game.minerals -= building.buildCostPerLevel[building.level];
			building.upgrade();
		}
		closeMenue(); 
	}
}
