using UnityEngine;
using System.Collections;

public class BaseObj : MonoBehaviour {
		
	public void l(object o){
		Debug.Log(o);
	}
	GameController _game;
	[System.NonSerialized]
	public GameController Game;
	public static Config.RootObject Vars = Config.GetConfig();
	
	protected void Awake () {
		Game = GameController.Instance;
	}

	protected static GameController GetGame(){
		return GameController.Instance;
	}

	protected void Start(){}
	protected void Update(){}
		
}
