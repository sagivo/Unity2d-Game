using UnityEngine;
using System.Collections;

public class BaseObj : MonoBehaviour {

	protected GameController Game;
	
	void Awake () {
		Game = GameController.Instance;
	}
}
