using UnityEngine;
using System.Collections;

public class Canon : MonoBehaviour {

	public GameObject Follow;

	Game game;
	// Use this for initialization
	void Start () {
		game = Game.Instance;
		if (!Follow) Follow = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (game.Enemies.Count > 0) followEnemy();
	}

	void followEnemy(){
		GameObject enemy = new GameObject(); var minDistance = 10000f;
		foreach(var e in game.Enemies){
			var distance = Vector2.Distance(e.transform.position, transform.position);
			if (distance < minDistance){
				enemy = e;
				minDistance = distance;
			}
		}
		transform.LookAt(enemy.transform.position);
	}
}
