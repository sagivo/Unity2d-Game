using UnityEngine;
using System.Collections;

public class EnemySpwaner : Enemy {
	public GameObject EnemyTestFollow;

	// Use this for initialization
	void Start () {
		//OnMove = delegate( MoveTowardTarget);
		Target = GameObject.FindGameObjectWithTag ("Player");
		StartMoving();
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*if (isMoving == true)
		{
			Move();
		}*/

		Instantiate(EnemyTestFollow, transform.position, transform.rotation);
		//Enemies.Add(temp);

	}
}
