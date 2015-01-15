using UnityEngine;

public class SwipeToMove : BaseObj
{

	public float speed = 12f;
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;
	float minDelta = 1;
	new void Update() {
		base.Update();
		if (Input.touchCount == 1  && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(0).deltaPosition.magnitude >  minDelta) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			transform.Translate(-touchDeltaPosition.x * speed * Time.deltaTime , -touchDeltaPosition.y * speed * Time.deltaTime, 0);
		}
	}

}