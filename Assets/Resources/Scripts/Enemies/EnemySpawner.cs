using UnityEngine;
using System.Collections;

public class EnemySpawner : BaseObj {
	public float spawnEvery = 4;
	public GameObject[] Enemies; 

	protected new void Start () {
		base.Start();

		Invoke("spawn", Random.Range(0,spawnEvery));
	}
	
	protected new void Update () {
		base.Update();
	}

	void spawn(){
		Instantiate(Enemies[0],new Vector3( transform.position.x + Random.Range(-5,5), transform.position.y + Random.Range(-2,2)),Quaternion.identity );
		Invoke("spawn", Random.Range(0,spawnEvery));
	}
}
