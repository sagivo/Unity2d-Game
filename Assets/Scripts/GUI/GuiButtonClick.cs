using UnityEngine;
using System.Collections;

public class GuiButtonClick : MonoBehaviour {

	public GameObject target;
	public string methodName;

	public Sprite spriteUp;
	public Sprite spriteHover;
	public Sprite spriteDown;

	public SpriteRenderer spriteRenderer;

	private bool isDown=false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonUp(0))
		{
			isDown=false;
		}
	}

	void OnMouseDown() {
		spriteRenderer.sprite=spriteDown;
		isDown=true;
	}

	void OnMouseEnter() {
		if(isDown)
		{
			spriteRenderer.sprite=spriteDown;
		}else{
			spriteRenderer.sprite=spriteHover;
		}
	}

	void OnMouseExit() {
		spriteRenderer.sprite=spriteUp;
	}

	void OnMouseUpAsButton() {
		spriteRenderer.sprite=spriteHover;
		isDown=false;
		target.SendMessage(methodName);
	}
}
