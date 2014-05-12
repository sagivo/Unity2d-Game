using UnityEngine;
using System.Collections;

public class EnemyFollower : Enemy {

	public GameObject Target;

	void Start () 
	{
		if (Target == null) Target = GameObject.FindGameObjectWithTag("Player");
		IsAutoMove = true;
	}	
	
	void Update () 
	{
		if (IsMoving == true) Move();
	}

	public void Move()
	{
		if(Target != null)
		{
			this.transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Speed * Time.deltaTime);
		}
	}

}
