using UnityEngine;
using System.Collections;

public class CrosshairsController : MonoBehaviour
{
		//public
		public GameObject cursor;

		//private
		private bool isShowns = true;
		private GameObject mouseCursor;
	
		// Use this for initialization
		void Start ()
		{
		}

		void Awake ()
		{
			Screen.showCursor = false;
			if (!mouseCursor) {
				mouseCursor = (GameObject)Instantiate (cursor);
			}
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (isShowns && mouseCursor) {				
				Vector3 mousePos = Input.mousePosition;
				Vector3 wantedPosition = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, 1f));
				mouseCursor.transform.position = wantedPosition;
			}			
		}
}

