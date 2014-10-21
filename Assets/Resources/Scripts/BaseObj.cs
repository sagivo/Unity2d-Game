using UnityEngine;
using System.Collections;

public class BaseObj : MonoBehaviour {

	public void l(object o){
		Debug.Log(o);
	}

	protected GameController Game;
	
	protected void Awake () {
		Game = GameController.Instance;
	}

	protected void Start(){}
	protected void Update(){}
	
}
