using UnityEngine;
using System.Collections;

public class TouchLiveable : BaseObj {
	Ray ray;	

	new void Update () {
		base.Update();

		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
		{
			ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
			var hit = Physics2D.Raycast(ray.origin,Vector2.zero ,Mathf.Infinity , ( 1 << LayerMask.NameToLayer("Liveable") ));

			if (hit.collider && hit.collider.GetComponent<Liveable>().IsSelectable){
				hit.collider.gameObject.GetComponent<Liveable>().Select();
			}
		}
	}
}
