using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Drag : BaseObj, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public bool dragOnSurfaces = true;

	
	public void OnBeginDrag(PointerEventData eventData)
	{
		var canvas = gameObject.FindInParents<Canvas>();
		l(canvas);
	}

	public void OnDrag(PointerEventData data)
	{
		gameObject.transform.position = data.position;
	}
	
	public void OnEndDrag(PointerEventData eventData)
	{
	}
}