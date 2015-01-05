using UnityEngine;
using System.Collections;

public class Liveable : BaseObj {

	public int Health {get{return health;} }
	public int level = 0;
	public enum StatusType {Live, Destroyed, Build, Repair}
	public StatusType status;
	public bool showHealthBar = true;
	public float healthBarYMargin = 2;

	public System.Action<Collider2D> OnHit;
	public System.Action OnDie;
	public System.Action<StatusType> OnStatusChange;
	public System.Action<object> OnBuildStart;
	public System.Action<object> OnBuildEnd;
	public System.Action<object> OnRepairStart;
	public System.Action<object> OnRepairEnd;
	public System.Action<object> OnHealthChanged;
	public Sprite[] spritesPerLevel;
	public float[] upgradeTimePerLevel = new float[]{5,10,30,100};
	public Sprite[] spritesPerBuild;

	protected int health = 100;
	HealthBarController healthBar;
	protected SpriteRenderer spriteRenderer;

	protected new void Start(){
		base.Start();	

		spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
		if (spritesPerLevel != null && spritesPerLevel.Length > 0) spriteRenderer.sprite = spritesPerLevel[level];
		status = StatusType.Live;


		if (showHealthBar) {
			var hb = Instantiate(Resources.Load("Prefabs/UI/HealthBar"),new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y),Quaternion.identity) as GameObject;
			hb.transform.parent = transform;
			healthBar = hb.GetComponent<HealthBarController>();
			healthBar.setHealth(health);
			healthBar.yMargin = healthBarYMargin;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (OnHit != null) OnHit(other);
	}

	public void DecHealth(int by){
		health -= by;
		if (OnHealthChanged!=null) OnHealthChanged(Health); 
		if (health <= 0 && OnDie!=null) OnDie();
		healthBar.setHealth(Health);
	}

	public void build(){
		if (spritesPerBuild.Length > level) spriteRenderer.sprite = spritesPerBuild[level];
		Invoke("upgrade", upgradeTimePerLevel[level]);
		changeStatus(StatusType.Build);
	}

	public void upgrade(){
		level++;
		if (spritesPerLevel != null && spritesPerLevel.Length >= level) spriteRenderer.sprite = spritesPerLevel[level];
		changeStatus(StatusType.Live);
	}

	void changeStatus(StatusType newStatus){
		status = newStatus;
		if (OnStatusChange != null) OnStatusChange(status);
	}
}
