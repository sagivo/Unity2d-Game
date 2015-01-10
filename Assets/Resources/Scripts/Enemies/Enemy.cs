using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Enemy : Liveable 
{
	public float speed = 1.5f;
	protected GameObject target;
	public int costKill = 1;
	protected int[] killBonusMinerals = new int[]{1,5,10};
	[System.NonSerialized]
	protected int[] damagePerLevel;

	protected new void Start(){
		base.Start();
		Game.enemies.Add(this);

		OnDie += () => {
			Game.minerals += killBonusMinerals[level];
		};
	}

	void OnDestroy()
	{
		Game.enemies.Remove(this);
	}

}
