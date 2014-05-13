using UnityEngine;
using System.Collections;

public class Helpers {
	public GameObject CloestToObject(GameObject[] list, GameObject obj){
		float minDistance = 100000f;
		GameObject closest = new GameObject();
		foreach(var entity in list){
			var distance = Vector3.Distance(entity.transform.position, obj.transform.position);
			if (distance < minDistance){ minDistance = distance; closest = entity;}
		}
		return closest;
	}
}
