using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
	public GameObject Follow;
	public float Smooth= 5.0f;

	// Use this for initialization
	void Start () {
		if (!Follow) Follow = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () { 
		if (Follow) transform.position = Vector3.Lerp(transform.position, new Vector3(Follow.transform.position.x,Follow.transform.position.y,-100),Time.deltaTime * Smooth);
	}
}
