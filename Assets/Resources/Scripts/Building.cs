using UnityEngine;
using System.Collections;

abstract public class Building : Liveable {	
	public int[] buildCostPerLevel;
	public int[] refundPerLevel;

	protected new void Start(){
		base.Start();
		Game.buildings.Add(this);
		//if (buildTimePerLevel[level] > 0) build();	
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

	public bool canBuild(){
		//if (!Game) Game = GameController.Instance;
		return (Game.minerals >= buildCostPerLevel[level]);
	}

}
