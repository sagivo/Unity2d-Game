using UnityEngine;

public class SwipeToMove : MonoBehaviour
{
	public float speed = 2f;
	public float minX;
	public float maxX;
	public float minY;
	public float maxY;

	void Update() {
		if (Input.touchCount == 1  && Input.GetTouch(0).phase == TouchPhase.Moved) {
			Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
			transform.Translate(-touchDeltaPosition.x * speed * Time.deltaTime, -touchDeltaPosition.y * speed * Time.deltaTime, 0);
			//transform.position = new Vector3(trans
		}
	}

}