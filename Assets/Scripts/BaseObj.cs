using UnityEngine;
using System.Collections;

public class BaseObj : MonoBehaviour {

	public GameController Game;
	
	void Awake () {
		Game = GameController.Instance;
	}
	

}
