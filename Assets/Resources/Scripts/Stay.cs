using UnityEngine;
using System.Collections;

public class Stay : MonoBehaviour {

	void Awake () {
		DontDestroyOnLoad(gameObject);
	}
}
