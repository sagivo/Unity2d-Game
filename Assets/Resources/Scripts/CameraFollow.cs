using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public GameObject follow;
	public float smooth= 5.0f;

	// Use this for initialization
	void Start () {
		if (!follow) follow = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () { 
		if (follow) transform.position = Vector3.Lerp(transform.position, new Vector3(follow.transform.position.x,follow.transform.position.y,-100),Time.deltaTime * smooth);
	}
}
