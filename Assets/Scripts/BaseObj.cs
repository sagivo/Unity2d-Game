using UnityEngine;
using System.Collections;

public class BaseObj : MonoBehaviour {

	protected GameController Game;
	
	protected void Awake () {
		Game = GameController.Instance;
	}

	protected void Start(){}
	protected void Update(){}
}
