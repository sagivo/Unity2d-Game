using UnityEngine;
using System.Collections;

public class DestroySprite : BaseObj {

	// Use this for initialization
	void Awake () {
		base.Awake();
		l ("a");
		if (GetComponent<SpriteRenderer>()) Destroy(GetComponent<SpriteRenderer>()); //DEBUG MODE
	}
}
