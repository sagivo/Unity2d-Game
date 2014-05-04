using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public float Speed = 2;
	public GameObject Player;

	// Use this for initialization
	void Start () {
		if (Player == null) Player = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Player.transform.position);
		transform.position += transform.forward*Speed*Time.deltaTime;

		//thisTransform.position.x = Mathf.Lerp( thisTransform.position.x, Player.position.x + xOffset, Time.deltaTime);
		//thisTransform.position.y = Mathf.Lerp( thisTransform.position.y, Player.position.y + yOffset, Time.deltaTime);

	}
}
