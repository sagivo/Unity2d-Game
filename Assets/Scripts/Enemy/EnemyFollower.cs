using UnityEngine;
using System.Collections;

public class EnemyFollower : Enemy {
	
	// Use this for initialization
	void Start () {
		//OnMove = delegate( MoveTowardTarget);
		Target = GameObject.FindGameObjectWithTag ("Player");
		StartMoving();
	}
	
	
	public void Move()
	{
		if(Target != null)
		{
			float fStep = Speed * Time.deltaTime;
			this.transform.position = Vector3.MoveTowards(transform.position, 
			                                              Target.transform.position, 
			                                              fStep);
		}
	}
	
	
	
	// Update is called once per frame
	void Update () 
	{
		if (isMoving == true)
		{
			Move();
		}

	}
}
