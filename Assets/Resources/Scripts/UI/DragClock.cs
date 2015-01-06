using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class DragClock : BaseObj {
	public enum ClockType {Canon, Mineral, Expend, Upgrade, Refund}
	public ClockType clockType;

	float speed = 5f;
	Image img;
	// Use this for initialization
	new void Start () {
		base.Start();
		img = GetComponent<Image>();
	}
	
	// Update is called once per frame
	new void Update () {
		base.Update();
		float val = 0;
		switch (clockType) {
		case ClockType.Canon:
			val = Vars.Balance.Player.AutoCanon.mineralCostUpgrade[0];
			break;
		case ClockType.Mineral:
			val = Vars.Balance.Player.MineralMiner.mineralCostUpgrade[0];
			break;
		case ClockType.Expend:
			break;
			default: break;
		}
		img.fillAmount = Mathf.Lerp(img.fillAmount, (Game.minerals > val) ? 0 : (1f -  Game.minerals.to_f() / val), speed * Time.deltaTime) ;

		//img.fillAmount = .2f;
	}
}
