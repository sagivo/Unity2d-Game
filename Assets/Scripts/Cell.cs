using UnityEngine;
using System.Collections;

public class Cell : BaseObj {

	public enum CellType {Empty, Player, Canon, Building};
	public CellType Type;
	//events
	public delegate void HitEvent(object sender, object args); public HitEvent OnHit;

}
