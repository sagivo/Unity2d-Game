using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float Speed = 10;
	public GameObject Player;

	// Use this for initialization
	void Start () {
		if (Player == null) Player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		if (Player){
			transform.LookAt(Player.transform.position);
			transform.position += transform.forward*Speed*Time.deltaTime;
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		Destroy(gameObject);
	}
}
