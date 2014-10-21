using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Menu : BaseObj {

	Animator anim;
	// Use this for initialization
	new void Awake () {
		base.Awake();

		anim = GetComponent<Animator>();
		anim.SetTrigger("Open");
	}
	
	// Update is called once per frame
	public void Open () {

	}
}
