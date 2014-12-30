using UnityEngine;
using System.Collections;

public class Building : Liveable {

	public int mineralCostBuild = 10;
	public int mineralCostRefund = 6;
	public int[] mineralCostUpgrade = new int[]{10,40,100};
	public int diamondCost = 1;

	protected new void Start(){
		base.Start();

		if (Game.minerals >= mineralCostBuild) Game.minerals-= mineralCostBuild; 
		else Game.diamonds -= diamondCost;

		Game.buildings.Add(this);
		if (spritesPerLevel.Length > level) spriteRenderer.sprite = spritesPerLevel[level];
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
		return (Game.minerals >= mineralCostUpgrade[level] || Game.diamonds >= mineralCostUpgrade[level]);
	}

}
