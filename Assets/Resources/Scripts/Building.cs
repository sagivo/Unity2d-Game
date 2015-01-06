using UnityEngine;
using System.Collections;

public class Building : Liveable {

	public int diamondCost = 1;
	public Sprite[] buildSprites; 
	[UnityEngine.HideInInspector]
	public int[] buildCostPerLevel;
	public int[] refundPerLevel;
	[UnityEngine.HideInInspector]

	protected new void Start(){
		base.Start();

		Game.buildings.Add(this);
		if (buildTimePerLevel[level] > 0) build();
	}
	
	void OnDestroy(){
		Game.buildings.Remove(this);
	}

	public void refund(){
		Game.minerals += refundPerLevel[level];
		Destroy(gameObject);
	}

	public bool enoughResourcesToCreate(){
		return (Game.minerals >= buildCostPerLevel[level]);
	}

	public bool enoughResourcesToUpgrade(){
		return (Game.minerals >= buildCostPerLevel[level]);
	}

	new void build(){
		base.build();
		Game.minerals -= buildCostPerLevel[level];
	}
}
