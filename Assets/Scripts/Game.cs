using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour {
	public static Game Instance;
	public int SpawnEnemiesEvery = 3;
	public GameObject EnemyToSpwn;
	public List<GameObject> Enemies;

	void Awake(){
		Instance = this;
	}

	void Start () {
		Enemies = new List<GameObject>();
		if (!EnemyToSpwn) EnemyToSpwn = GameObject.FindWithTag("Enemy");
		spawnEnemy();
	}
	
	void Update () {
	}

	void spawnEnemy(){
		Enemies.Add( (GameObject)Instantiate(EnemyToSpwn,new Vector3(Random.Range(-40f,40f),Random.Range(-40f,40f),-1f),Quaternion.identity) );
		Invoke("spawnEnemy",Random.Range(0,SpawnEnemiesEvery));
	}
}
