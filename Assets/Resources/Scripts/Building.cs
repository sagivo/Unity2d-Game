using UnityEngine;
using System.Collections;

public class Building : Liveable {

	public int mineralCostBuild = 10;
	public int mineralCostRefund = 6;
	public int diamondCost = 1;
	public Sprite[] buildSprites; 
	[UnityEngine.HideInInspector]
	public int[] mineralCostUpgrade = new int[]{1,5,10,20,40,80};
	[UnityEngine.HideInInspector]
	protected float buildTime = 1f;

	protected new void Start(){
		base.Start();

		if (Game.minerals >= mineralCostBuild) Game.minerals-= mineralCostBuild; 
		else Game.diamonds -= diamondCost;

		Game.buildings.Add(this);

		if (upgradeTimePerLevel[level] > 0) build();
	}
	
	void OnDestroy(){
		Game.buildings.Remove(this);
	}

	public void refund(){
		Game.minerals += mineralCostRefund;
		Destroy(gameObject);
	}

	public bool enoughResourcesToCreate(){
		return (Game.minerals >= mineralCostBuild || Game.diamonds >= diamondCost);
	}

	public bool enoughResourcesToUpgrade(){
		return (Game.minerals >= mineralCostUpgrade[level]);
	}

}
