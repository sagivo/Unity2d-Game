using UnityEngine;
using System.Collections;

public class DestroySprite : BaseObj {

	// Use this for initialization
	new void Awake () {
		base.Awake();
		if (GetComponent<SpriteRenderer>()) Destroy(GetComponent<SpriteRenderer>()); //DEBUG MODE
	}
}
