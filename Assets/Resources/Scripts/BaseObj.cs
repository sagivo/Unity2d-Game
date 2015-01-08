using UnityEngine;
using System.Collections;

public class BaseObj : MonoBehaviour {
		
	public void l(object o){
		Debug.Log(o);
	}
	public static GameController Game;
	protected static Config.RootObject Vars;
	
	protected void Awake () {
		if (Game == null) {
			Game = GameController.Instance;		
			Vars = Config.GetConfig();
		}
	}

	protected static GameController GetGame(){
		return GameController.Instance;
	}

	protected void Start(){}
	protected void Update(){}
		
}
