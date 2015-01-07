using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Image))]
public class DragClock : BaseObj {
	public enum ClockType {Canon, Mineral, Expend, Upgrade, Refund}
	public ClockType clockType;
	public bool hasFill {get {return img.fillAmount > 0;}}

	float speedUp = .8f;
	float speedDown = 2f;
	Image img;
	Image childImg;
	float fillVal;

	// Use this for initialization
	new void Start () {
		base.Start();
		img = GetComponent<Image>();
		childImg = gameObject.childrenOnly()[0].GetComponent<Image>();
		Game.OnMineralChange += updateVals;
		updateVals();
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();

		float speedFill = (fillVal < img.fillAmount) ? speedUp * Time.deltaTime : speedDown * Time.deltaTime;
		img.fillAmount = Mathf.MoveTowards(img.fillAmount, fillVal, speedFill);
		childImg.fillAmount = (img.fillAmount == 0) ? 0f : (1-img.fillAmount);
	}

	void updateVals(){
		float val = 0;
		switch (clockType) {
		case ClockType.Canon:
			val = Vars.Balance.Player.AutoCanon.buildCostPerLevel[0];
			break;
		case ClockType.Mineral:
			val = Vars.Balance.Player.MineralMiner.buildCostPerLevel[0]; 
			break;
		case ClockType.Expend:
			val = Vars.Balance.Player.Cell.expendCostPerLevel[0];
			break;
		default: break;
		}
		fillVal = (Game.minerals > val) ? 0 : (1f -  Game.minerals.to_f() / val);
	}

}