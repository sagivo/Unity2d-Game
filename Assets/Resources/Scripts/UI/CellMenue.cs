using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class CellMenue : BaseObj {
	public static float longClickTime = .6f;
	Animator anim;
	float openMenueTime = 0;

	protected new void Awake () {
		base.Awake();
		anim = GetComponent<Animator>();

		var h = Physics2D.Linecast(Camera.main.transform.position, new Vector3(0,4.35f), 1 << LayerMask.NameToLayer("Cells"));
		if (h) l(h.collider.transform.position);
		Debug.DrawLine(Camera.main.transform.position, new Vector2(0,4.35f), Color.cyan, Mathf.Infinity);

		/*
		h = Physics2D.Linecast(Camera.main.transform.position, new Vector3(0,4.35f), 1 << LayerMask.NameToLayer("Cells"));
		if (h) Destroy(h.collider.gameObject);
		Debug.DrawLine(Camera.main.transform.position, new Vector2(0,4.35f), Color.cyan, Mathf.Infinity);

		Ray ray = new Ray(Camera.main.transform.position, (new Vector3(0,214.35f) - Camera.main.transform.position).normalized);
		l (ray.origin);l (ray.direction);
		var hit = Physics2D.Raycast(ray.origin, ray.direction ,Mathf.Infinity , 1 << LayerMask.NameToLayer("Cells") );
		if (hit.collider){
			//Destroy(hit.collider.gameObject);
		} 
		Debug.DrawRay(ray.origin, ray.direction, Color.yellow, Mathf.Infinity, false);
*/

	}
	
	protected new void Update () {
		base.Update();
		if (Input.GetMouseButtonDown(0) && (openMenueTime == 0)) openMenueTime = Time.time + longClickTime;
		else if (Input.GetMouseButtonUp(0)) openMenueTime = 0;
		if (openMenueTime != 0 && openMenueTime <= Time.time) openMenue();
	}

	public void openMenue(){
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
		anim.SetBool("IsOpen", false);
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
	}
}
