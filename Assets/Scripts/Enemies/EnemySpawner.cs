using UnityEngine;
using System.Collections;

public class EnemySpawner : Enemy
{
	public GameObject EnemyToSpawn;
	public int MaxEnemies = 5;
	public float SpawnEvery = 20f;
	//private
	int currentEnemyCount = 0;

	float nextSpawn;
	// Use this for initialization
	void Start ()
	{
		nextSpawn = Time.time + SpawnEvery;
	}

	void ReleaseEnemy ()
	{
		Instantiate (EnemyToSpawn, transform.position, transform.rotation);
		currentEnemyCount++;
	}

	void CheckSpawn ()
	{
		if (currentEnemyCount < MaxEnemies && Time.time > nextSpawn) 
		{
			ReleaseEnemy ();
			nextSpawn = Time.time + SpawnEvery;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		CheckSpawn ();
	}
}
