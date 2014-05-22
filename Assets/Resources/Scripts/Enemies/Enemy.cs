using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : Liveable 
{
	public float Speed = 3;
	public GameObject Target;

	SpriteRenderer spriteRenderer;

	protected new void Start(){
		spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
		base.Start();
		Game.Enemies.Add(this);

		OnHit = (o) => {
			//if (System.Array.IndexOf(hitTags, o.gameObject.tag) > -1) Destroy(gameObject);
			if (o.gameObject.IsSubClassOf<Bullet>()){
				DecHealth(o.gameObject.GetComponent<Bullet>().Damage);
				spriteRenderer.color = Color.red;
				CancelInvoke("switchBackToOriginalColor");
				Invoke("switchBackToOriginalColor", .2f);
			}
		};

		OnDie += () => {
			Destroy(gameObject);
		};
	}

	void OnDestroy()
	{
		Game.Enemies.Remove(this);
	}

	void switchBackToOriginalColor(){
		spriteRenderer.color = Color.white;
	}

}
