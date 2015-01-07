using UnityEngine;
using System.Collections;

public abstract class Liveable : BaseObj {
	int _health;
	public int health {get{return _health;} set{ _health = value; }}
	public int level;
	public enum StatusType {Live, Destroyed, Build, InActive, Repair}
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
	[System.NonSerialized]	
	protected float[] buildTimePerLevel = new float[]{0,10,30,100};	
	public Sprite[] spritesPerBuild;
	protected abstract int[] healthPerLevel {get;}
	HealthBarController healthBar;
	protected SpriteRenderer spriteRenderer;

	protected new void Start(){
		base.Start();	

		spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
		//if (spritesPerLevel != null && spritesPerLevel.Length > 0) spriteRenderer.sprite = spritesPerLevel[level];
		status = StatusType.Live;
		health = healthPerLevel[level];

		if (showHealthBar) {
			var hb = Instantiate(Resources.Load(Vars.PrefabPaths.uiHealthbar),new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y),Quaternion.identity) as GameObject;
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

	public void decHealth(int by){
		_health -= by;
		if (OnHealthChanged!=null) OnHealthChanged(health); 
		if (health <= 0 && OnDie!=null) OnDie();
		healthBar.setHealth(health);
	}

	public void build(){
		if (spritesPerBuild != null && spritesPerBuild.Length > level) spriteRenderer.sprite = spritesPerBuild[level];
		Invoke("upgrade", buildTimePerLevel[level]);
		changeStatus(StatusType.Build);
		if (showHealthBar) healthBar.gameObject.SetActive(false);
	}

	public void upgrade(){
		level++;
		if (spritesPerLevel != null && spritesPerLevel.Length >= level) spriteRenderer.sprite = spritesPerLevel[level];
		changeStatus(StatusType.Live);
		if (showHealthBar) healthBar.gameObject.SetActive(true);
	}

	public void setInactive(){
		changeStatus(StatusType.InActive);
	}

	void changeStatus(StatusType newStatus){
		status = newStatus;
		if (OnStatusChange != null) OnStatusChange(status);
	}
}
