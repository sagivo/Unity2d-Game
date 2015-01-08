using UnityEngine;
using System.Collections;

public class Stay : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		DontDestroyOnLoad(gameObject);
	}
}
