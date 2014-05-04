using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	public float Speed = 200;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal")*Speed*Time.deltaTime, Input.GetAxis("Vertical")*Speed*Time.deltaTime);
	}
}
