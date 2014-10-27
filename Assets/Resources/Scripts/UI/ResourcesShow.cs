using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourcesShow : BaseObj {
	Text minerals, diamonds;

	new void Start () {
		foreach (var c in  GetComponentsInChildren<Text>()){
			if (c.name == "Minerals") minerals = c;
			else if (c.name == "Diamonds") diamonds = c;
		}
	}
	
	new void Update () {
		if (minerals) minerals.text = "Minerals: " + Game.minerals;
		if (diamonds) diamonds.text = "Diamonds: " + Game.diamonds;
	}
}
