using UnityEngine;
using System.Collections;

public class Kamikazi : Enemy {

	void Start () {
		findTarget();
	}
	 
	void Update () {
		if (Target != null){
			LookAt2d(Target.transform);
			transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, Speed*Time.deltaTime);
		}
	}

	void findTarget() {
		Target = Helpers.CloestToObject(Game.Canons.ToArray(), gameObject);
		if (Target == null) Invoke("findTarget",1);
	}

	private void rotateToPosition(Vector3 mousePos, Vector3 originPos)
	{
		Vector3 canonWorldPos = Camera.main.WorldToScreenPoint (originPos);
		mousePos.x = mousePos.x - canonWorldPos.x;
		mousePos.y = mousePos.y - canonWorldPos.y;
		float angle = Mathf.Atan2 (mousePos.y, mousePos.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, angle));
	}
}
