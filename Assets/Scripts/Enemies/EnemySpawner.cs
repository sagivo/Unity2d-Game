using UnityEngine;
using System.Collections;

public class EnemySpawner : Enemy
{
	public GameObject EnemyToSpawn;

	public int MaxEnemies = 5;
	public float WaitTime = 20f;

	private int currentEnemyCount = 0;
	private float currentTime = 0f;
	private float goalTime = 0f;

	// Use this for initialization
	void Start ()
	{
		goalTime = WaitTime;
	}

	void ReleaseEnemy ()
	{
		Instantiate (EnemyToSpawn, transform.position, transform.rotation);
		currentEnemyCount++;
	}

	void CheckSpawn ()
	{
		if (currentEnemyCount < MaxEnemies) 
		{
			float currDeltaTime = Time.deltaTime * 10;

			if (currentTime == 0) 
			{
				currentTime += currDeltaTime;
			}

			if (currentTime >= goalTime) 
			{
				ReleaseEnemy ();
				goalTime += WaitTime;
			}
			else 
			{
				currentTime += currDeltaTime;
			}
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckSpawn ();
	}
}
