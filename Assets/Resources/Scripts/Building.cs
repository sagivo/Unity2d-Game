using UnityEngine;
using System.Collections;

abstract public class Building : Liveable {	
	public int[] buildCostPerLevel;
	public int[] refundPerLevel;

	protected new void Awake(){
		base.Awake();
		OnStatusChange += (s) => {
			if (status == StatusType.Build){
				if (buildCostPerLevel!=null && buildCostPerLevel.Length >  level) Game.minerals -= buildCostPerLevel[level];
				gameObject.transform.rotation = Quaternion.identity;
			}
		};
	}

	protected new void Start(){
		base.Start();
		Game.buildings.Add(this);

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
	
	public bool canBuild(){
		//if (!Game) Game = GameController.Instance;
		return (Game.minerals >= buildCostPerLevel[level]);
	}

}
