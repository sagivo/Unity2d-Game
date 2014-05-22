using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
	
	void OnTriggerExit2D(Collider2D other) {
		Destroy(other.gameObject);
	}
}
