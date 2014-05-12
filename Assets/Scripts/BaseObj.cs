using UnityEngine;
using System.Collections;

public class BaseObj : MonoBehaviour {

	public GameController Game;
	
	void Awake () {
		Debug.Log('1');
		Game = GameController.Instance;
	}
	

}
