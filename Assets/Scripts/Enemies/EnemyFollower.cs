using UnityEngine;
using System.Collections;

public class EnemyFollower : Enemy {
	
	// Use this for initialization
	void Start () 
	{
		Target = GameObject.FindGameObjectWithTag ("Player");
		IsAutoMove = true;
	}	
	
	public void Move()
	{
		if(Target != null)
		{
			float Step = Speed * Time.deltaTime;
			this.transform.position = Vector3.MoveTowards(transform.position, 
			                                              Target.transform.position, 
			                                              Step);
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (IsMoving == true)
		{
			Move();
		}

	}
}
