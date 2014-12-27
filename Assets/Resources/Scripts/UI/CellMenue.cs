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
		if (Input.GetMouseButtonDown(0) && (openMenueTime == 0)) openMenueTime = Time.time + longClickTime;
		else if (Input.GetMouseButtonUp(0)) openMenueTime = 0;
		if (openMenueTime != 0 && openMenueTime <= Time.time) openMenue();
	}

	public void openMenue(){
		cg.interactable = true;
		openMenueTime = 0;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		var hit = Physics2D.Raycast(ray.origin,Vector2.zero ,100 , ( 1 << LayerMask.NameToLayer("Cells") ));
		if (hit.collider){
			var c = hit.collider.GetComponent<Cell>();
			if (c) c.select();
			anim.SetBool("IsOpen", true);
		}
	}

	public void closeMenue(){
		cg.interactable = false;
		anim.SetBool("IsOpen", false);
		Game.menue.GetComponent<RectTransform>().position = new Vector2(100,100);
	}

	public void build(Liveable o){
		Cell cell = Cell.getSelected();
		Liveable newObj = GameObject.Instantiate(o, cell.transform.renderer.bounds.center,Quaternion.identity) as Liveable;
		cell.liveObj = newObj;
		closeMenue(); cell.unSelect();
	}

	public void destroy(){
		Cell cell = Cell.getSelected();
		Game.minerals += cell.liveObj.mineralCostRefund;
		Destroy(cell.liveObj.gameObject);
		closeMenue(); cell.unSelect();
	}

	public void expend(){
		Cell.getSelected().expend();
		closeMenue(); 
	}
}
