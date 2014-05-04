using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public int SpawnEnemiesEvery = 3;
	public GameObject EnemyToSpwn;

	// Use this for initialization
	void Start () {
		if (!EnemyToSpwn) EnemyToSpwn = GameObject.FindWithTag("Enemy");
		spawnEnemy();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void spawnEnemy(){
		Instantiate(EnemyToSpwn,new Vector3(Random.Range(-40f,40f),Random.Range(-40f,40f),-1f),Quaternion.identity );
		Invoke("spawnEnemy",Random.Range(0,SpawnEnemiesEvery));
	}
}
