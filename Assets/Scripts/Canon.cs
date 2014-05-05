using UnityEngine;
using System.Collections;

public class Canon : MonoBehaviour {

	public GameObject Follow;
	// Use this for initialization
	void Start () {
		if (!Follow) Follow = GameObject.FindWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(Follow.transform.position);
	}
}
