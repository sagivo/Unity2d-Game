using UnityEngine;
using System.Collections;

public class Cell : BaseObj {

	public enum cellType {Empty, Player, Canon, Building};
	public cellType type;
	public bool selected;
	public static float longClickTime = .5f;
	static float lastClick;

	//colors
	public Color ColorSelected = Color.red;
	public Color ColorOver = Color.cyan;
	public Color ColorBase = Color.white;

	SpriteRenderer sprite;
	//events
	public delegate void HitEvent(object sender, object args); public HitEvent OnHit;

	protected new void Start(){
		base.Start();
		sprite = GetComponent<SpriteRenderer>();
		Game.cells.Add(this);
		sprite = gameObject.GetComponent<SpriteRenderer>();
	}

	protected new void Update(){
		base.Update();
		if (Input.GetMouseButtonDown(0)) OnMouseDown();
		if (Input.GetMouseButtonUp(0)) OnMouseUp();
	}
	
	void OnMouseDown() {
		if (lastClick == 0) lastClick = Time.time;
	}

	void OnMouseUp() {
		if (lastClick != 0 && lastClick + longClickTime <= Time.time) CastRay();
		lastClick = 0;
	}
	
	void OnDestroy(){
		Game.cells.Remove(this);
	}

	
	void CastRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		var hit = Physics2D.Raycast(ray.origin,Vector2.zero ,100 , ( 1 << LayerMask.NameToLayer("Cells") ));
		if (hit.collider){
			//Debug.Log(hit.collider.tag + " " + hit.collider.gameObject.name);
			hit.collider.GetComponent<SpriteRenderer>().color = ((hit.collider.GetComponent<SpriteRenderer>().color == ColorSelected) ? ColorBase : ColorSelected);
			//selected = !selected;
		}
		//*/
	}

}
