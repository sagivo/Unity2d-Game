using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Helpers {

	public static GameObject CloestToObject(object[] list, GameObject obj){
		List<GameObject> l = new List<GameObject>();
		foreach(var o in list){
			if (o.GetType().IsSubclassOf(typeof(BaseObj))) 
				l.Add(((BaseObj)o).gameObject);
		}
		return CloestToObject(l, obj);
	}

	public static GameObject CloestToObject(List<GameObject> list, GameObject obj){
		if (list.Count == 0) return null;
		float minDistance = 100000f;
		GameObject closest = new GameObject();
		foreach(var entity in list){
			var distance = Vector3.Distance(entity.transform.position, obj.transform.position);
			if (distance < minDistance){ minDistance = distance; closest = entity;}
		}
		return closest;
	}

	public static bool IsSubClassOf<T>(GameObject o){
		MonoBehaviour[] list = o.GetComponents<MonoBehaviour>();
		foreach(MonoBehaviour mb in list)
			if (mb is T) return true;
		return false;
	}

}
