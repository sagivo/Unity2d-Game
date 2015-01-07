using UnityEngine;
using System.Collections;

public class BaseObj : MonoBehaviour {
		
	public void l(object o){
		Debug.Log(o);
	}
	GameController _game;
	public GameController Game;// { get{if (_game == null && GameController.Instance) Game = GameController.Instance; return _game;} set { _game = value;} }
	
	protected void Awake () {
		Game = GameController.Instance;
	}

	protected static GameController GetGame(){
		return GameController.Instance;
	}

	protected void Start(){}
	protected void Update(){}
		
}
