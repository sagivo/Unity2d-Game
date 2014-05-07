using UnityEngine;
using System.Collections;

public class BullsEyeController : MonoBehaviour
{

		// Use this for initialization
		void Start ()
		{
	
		}
	
		// Update is called once per frame
		void Update ()
		{
				Vector3 mousePos = Input.mousePosition;
		
				Vector3 wantedPos = Camera.main.ScreenToWorldPoint (new Vector3 (mousePos.x, mousePos.y, mousePos.z));

				wantedPos.z = -22f;

				transform.position = wantedPos;
		
		}
}
