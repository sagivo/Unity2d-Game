using UnityEngine;
using System.Collections;

public class EnemySpawner : BaseObj {
	public float spawnEvery = 6;
	public GameObject[] enemies; 
	public Transform[] spawnPoints;

	protected new void Start () {
		base.Start();

		InvokeRepeating("spawn", 0, spawnEvery);
	}
	
	protected new void Update () {
		base.Update();
	}

	void spawn(){
		Instantiate(enemies[Random.Range(0,2)], spawnPoints[Random.Range(0, spawnPoints.Length)].position ,Quaternion.identity );
	}
}
