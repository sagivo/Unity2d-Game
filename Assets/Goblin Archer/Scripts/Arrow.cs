using UnityEngine;
using System.Collections;

public class Arrow : MonoBehaviour {

	public Vector3 v = new Vector3(20, 20, 0);
	public Vector3 a = new Vector3(0, -10, 0);
    public bool right = true;

	void Start () {
		Destroy(this.gameObject, 10);
        if (!right)
            v.x = -v.x;
	}

	void Update () {
	
		transform.position += v*Time.deltaTime;
		v += a * Time.deltaTime;
        
       transform.rotation = Quaternion.LookRotation(v, new Vector3(0,0,-1));
	}
}
