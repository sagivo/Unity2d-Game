using UnityEngine;
using System.Collections;

public class Menue : BaseObj {
	public static float longClickTime = .6f;
	Animator anim;
	float openMenueTime = 0;

	protected new void Awake () {
		base.Awake();
		anim = GetComponent<Animator>();
		//anim.SetBool("IsOpen", false);
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
}
