using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

[RequireComponent(typeof(Image))]
public class DragClock : BaseObj {
	public enum ClockType {Canon, Mineral, Expend, Upgrade, Refund}
	public ClockType clockType;

	float speed = .5f;
	Image img;
	Image childImg;

	// Use this for initialization
	new void Start () {
		base.Start();
		img = GetComponent<Image>();
		childImg = transform.childrenOnly()[0].GetComponent<Image>();
		Game.OnMineralChange += ()=>{
		};
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
		float val = 0;
		switch (clockType) {
		case ClockType.Canon:
			val = Vars.Balance.Player.AutoCanon.buildCostPerLevel[0];
			break;
		case ClockType.Mineral:
			val = Vars.Balance.Player.MineralMiner.buildCostPerLevel[0];
			break;
		case ClockType.Expend:
			break;
		default: break;
		}
		var newVal = (Game.minerals > val) ? 0 : (1f -  Game.minerals.to_f() / val);
		//if (newVal == img.fillAmount) return;
		float speedFill = (newVal < img.fillAmount) ? speed * Time.deltaTime : 1;
		img.fillAmount = Mathf.MoveTowards(img.fillAmount, newVal, speedFill);
		childImg.fillAmount = (img.fillAmount == 0) ? 0 : (1-img.fillAmount);
	}
}
