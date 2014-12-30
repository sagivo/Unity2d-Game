using UnityEngine;
using System.Collections;

public class Building : Liveable {

	public int mineralCost = 10;
	public int mineralCostRefund = 6;
	public int diamondCost = 1;

	protected new void Start(){
		base.Start();

		if (Game.minerals >= mineralCost) Game.minerals-= mineralCost; 
		else Game.diamonds -= diamondCost;

		Game.buildings.Add(this);
	}
	
	void OnDestroy(){
		Game.buildings.Remove(this);
	}

	public void refund(){
		Game.minerals += mineralCostRefund;
		Destroy(gameObject);
	}

	public bool enoughResourcesToCreate(){
		return (Game.minerals >= mineralCost || Game.diamonds >= diamondCost);
	}

}
