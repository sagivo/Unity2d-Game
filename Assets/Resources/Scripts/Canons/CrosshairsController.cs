using UnityEngine;
using System.Collections;

public class CrosshairsController : MonoBehaviour
{
	//public
	public GameObject cursor;
	//private
	private bool isShowns = true;
	private GameObject mouseCursor;

	void Awake ()
	{
		Cursor.visible = false;
		if (!mouseCursor) mouseCursor = (GameObject)Instantiate (cursor);

	}

	void Update ()
	{
		if (isShowns && mouseCursor) {				
			Vector3 mousePos = Input.mousePosition;
			Vector3 wantedPosition = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 1f));
			mouseCursor.transform.position = wantedPosition;
		}			
	}
}

