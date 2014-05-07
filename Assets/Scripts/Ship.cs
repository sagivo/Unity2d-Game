using UnityEngine;
using System.Collections;

public class Ship : MonoBehaviour {

	public float Speed = 2000;
	public GameObject Bullet;
	public GameObject Canon;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		rigidbody2D.velocity = new Vector2(Input.GetAxis("Horizontal")*Speed*Time.deltaTime, Input.GetAxis("Vertical")*Speed*Time.deltaTime);
		if(Input.GetButtonDown("Fire1")) putCanon();
		if(Input.GetButtonDown("Jump")) shot();				
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Boundary") Debug.Log("BOOM!");
	}

	void shot()
	{
		Instantiate(Bullet, new Vector3(transform.position.x, transform.position.y + 5 , -1) , transform.rotation);
	}

	void putCanon(){
		Instantiate(Canon, transform.position, transform.rotation);
	}
}
