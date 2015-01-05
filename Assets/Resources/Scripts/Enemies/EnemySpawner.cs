using UnityEngine;
using System.Collections;

public class EnemySpawner : BaseObj {
	public float spawnEvery = 4;
	public GameObject[] enemies; 
	public Transform[] spawnPoints;

	protected new void Start () {
		base.Start();

		Invoke("spawn", Random.Range(0,spawnEvery));
	}
	
	protected new void Update () {
		base.Update();
	}

	void spawn(){
		Instantiate(enemies[Random.Range(0,enemies.Length)], spawnPoints[Random.Range(0, spawnPoints.Length)].position ,Quaternion.identity );
		Invoke("spawn", Random.Range(0,spawnEvery));
	}
}
