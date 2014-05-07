using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
		public static Game Instance;
		public int SpawnEnemiesEvery = 3;
		public GameObject EnemyToSpwn;
		public List<GameObject> Enemies;

		private GameObject spawnLayer;
		

		void Awake ()
		{
				Instance = this;
		}

		void Start ()
		{
				spawnLayer = GameObject.FindGameObjectWithTag ("SPAWNS");
				Enemies = new List<GameObject> ();
				spawnEnemy ();
		}
	
		void Update ()
		{

		}

		void spawnEnemy ()
		{
				GameObject enemy = (GameObject)Instantiate (EnemyToSpwn, new Vector3 (Random.Range (-40f, 40f), Random.Range (-40f, 40f), -1f), Quaternion.identity);

				Enemies.Add (enemy);
				enemy.transform.parent = spawnLayer.transform;

				Invoke ("spawnEnemy", Random.Range (0, SpawnEnemiesEvery));
		}
}
