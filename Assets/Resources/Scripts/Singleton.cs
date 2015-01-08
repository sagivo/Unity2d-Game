using UnityEngine;
using System.Collections;


public class Singleton<T> : BaseObj where T : MonoBehaviour
{
	protected static T instance;
	public static T Instance
	{
		get
		{
			if(instance == null)
			{
				instance = (T) FindObjectOfType(typeof(T));
				
				if (instance == null)
				{
					Debug.LogError("An instance of " + typeof(T) + 
					               " is needed in the scene, but there is none.");
				}
			}
			return instance;
		}
	}

	new void Awake(){
		base.Awake();
		if(instance == null) { instance = this as T;  }
		else { if(this != instance) Destroy(this.gameObject);}
	}
}