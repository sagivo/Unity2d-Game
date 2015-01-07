using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Drag : BaseObj, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public bool dragOnSurfaces = true;
	public Building buildObj;
	public enum DragType {Build, Upgrade, Expand, Refund};
	public DragType dragType;

	Cell activeCell;
	Vector2 startPos; 

	new void Start(){
		base.Start();
		startPos = gameObject.transform.position;
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
	}

	public void OnDrag(PointerEventData data)
	{
		gameObject.transform.position = data.position;

		Ray ray = Camera.main.ScreenPointToRay(gameObject.transform.position);
		//Debug.DrawLine(Camera.main.transform.position, gameObject.transform.position, Color.yellow, Mathf.Infinity);
		//Debug.DrawLine(ray.direction, ray.origin, Color.yellow, Mathf.Infinity);
		var hit =  Physics2D.Raycast(ray.origin, ray.direction ,Mathf.Infinity , ( 1 << LayerMask.NameToLayer("Cells") ));
		if (hit.collider != null && hit.collider.GetComponent<Cell>() != activeCell){
			activeCell = hit.collider.GetComponent<Cell>();
			activeCell.HandleSelect(dragType,buildObj);
		} else if (!hit.collider && activeCell!=null) activeCell.unSelect();

	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
		gameObject.transform.position = startPos;

		if (activeCell && activeCell.isSelected()){
			switch (dragType) {
			case DragType.Build:
				if (!buildObj.canBuild()) return;
				Building newObj = GameObject.Instantiate(buildObj, activeCell.transform.renderer.bounds.center,Quaternion.identity) as Building;
				activeCell.liveObj = newObj;
				break;
			case DragType.Upgrade:
				if (activeCell.liveObj){
					activeCell.liveObj.build();
				}
				break;
			case DragType.Refund:
				if (activeCell.liveObj) {
					(activeCell.liveObj as Building).refund();
				}
				break;
			case DragType.Expand:
				if (activeCell.canExpend()) activeCell.expend();
				break;
			default:
			break;
			}
			activeCell.unSelect(); 
		}
	}

}