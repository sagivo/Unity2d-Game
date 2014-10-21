using UnityEngine;
using System.Collections;

public class Cell : BaseObj {

	public enum cellType {Empty, Player, Canon, Building};
	public cellType type;
	public bool selected;

	//colors
	public Color ColorSelected = Color.red;
	public Color ColorOver = Color.cyan;
	public Color ColorBase = Color.white;

	SpriteRenderer sprite;
	//events
	public delegate void HitEvent(object sender, object args); public HitEvent OnHit;

	protected new void Start(){
		base.Start();
		Game.cells.Add(this);
		sprite = gameObject.GetComponent<SpriteRenderer>();
	}

	protected new void Update(){
		base.Update();
		if (Input.GetMouseButtonDown(0)) OnMouseDown();
	}
	
	void OnMouseDown() {
		CastRay();
	}
	
	void OnDestroy(){
		Game.cells.Remove(this);
	}

	
	void CastRay() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		var hit = Physics2D.Raycast(ray.origin,Vector2.zero ,100 , ( 1 << LayerMask.NameToLayer("Cells") ));
		if (hit && hit.collider.gameObject == gameObject && hit.collider.GetComponent<Cell>() == this){
			Debug.Log("Hit object: " + hit.collider.gameObject.name);
			hit.collider.gameObject.GetComponent<SpriteRenderer>().color = ((hit.collider.gameObject.GetComponent<SpriteRenderer>().color == ColorSelected) ? ColorBase : ColorSelected);
			//selected = !selected;
		}
		//*/
	}

}
