using UnityEngine;
using System.Collections;

public abstract class Liveable : BaseObj {
	public int health;
	public int level;
	public enum StatusType {Live, Destroyed, Build, InActive, Repair}
	public StatusType status;
	public bool showHealthBar = true;
	public float healthBarYMargin = 2;
	public System.Action<Hitable> OnHit;
	public System.Action OnDie;
	public System.Action<StatusType> OnStatusChange;
	public System.Action<object> OnBuildStart;
	public System.Action<object> OnBuildEnd;
	public System.Action<object> OnRepairStart;
	public System.Action<object> OnRepairEnd;
	public System.Action<object> OnHealthChanged;
	public Sprite[] spritesPerLevel; 
	public float[] buildTimePerLevel = new float[]{0,10,30,100};	
	public Sprite[] spritesPerBuild;
	HealthBarController healthBar;
	protected SpriteRenderer spriteRenderer;

	//inspecor
	public int[] healthPerLevel; // {get { return _healthPerLevel; } set{ _healthPerLevel = value; health = healthPerLevel[level]; if (showHealthBar) healthBar.divider = health; }}

	protected new void Start(){
		base.Start();	

		spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
		if (spritesPerLevel != null && spritesPerLevel.Length > 0) spriteRenderer.sprite = spritesPerLevel[level];
		status = StatusType.Live;
		if (healthPerLevel!=null && healthPerLevel.Length > level) health = healthPerLevel[level];  

		if (showHealthBar) {
			var hb = Instantiate(Resources.Load(configs.prefabPaths.uiHealthbar),new Vector3(transform.position.x, transform.position.y - transform.lossyScale.y),Quaternion.identity) as GameObject;
			hb.transform.parent = transform;
			healthBar = hb.GetComponent<HealthBarController>();
			healthBar.setHealth(health);
			healthBar.divider = health;
			healthBar.yMargin = healthBarYMargin;
		}

		OnHit += (o) => {
			if (o.hits!=null && System.Array.IndexOf(o.hits, this.GetType()) > -1 ){
				decHealth(o.damage);
				spriteRenderer.color = Color.red;
				CancelInvoke("switchBackToOriginalColor");
				Invoke("switchBackToOriginalColor", .2f);
			}
		};

		build();
	}

	void switchBackToOriginalColor(){
		spriteRenderer.color = Color.white;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		var hit = other.GetComponent<Hitable>();
		if (hit && OnHit != null) OnHit(hit);
	}

	public void decHealth(int by){
		health -= by;
		if (OnHealthChanged!=null) OnHealthChanged(health); 
		healthBar.setHealth(health);
		if (health <= 0){ //DIE!!!
			if (OnDie!=null) OnDie();
			Destroy(gameObject);
		}
	}

	public void build(){
		if (spritesPerBuild != null && spritesPerBuild.Length > level) spriteRenderer.sprite = spritesPerBuild[level];
		changeStatus(StatusType.Build);
		if (showHealthBar) healthBar.gameObject.SetActive(false);
		Invoke("upgradeDone", buildTimePerLevel[level]);
	}

	public virtual void upgradeDone(){
		level++;
		health = healthPerLevel[level];
		if (spritesPerLevel != null && spritesPerLevel.Length >= level) spriteRenderer.sprite = spritesPerLevel[level];
		changeStatus(StatusType.Live);

		if (showHealthBar) {
			healthBar.gameObject.SetActive(true);
			healthBar.setHealth(health);
			healthBar.divider = health;
		}
	}

	public void setInactive(){
		changeStatus(StatusType.InActive);
	}

	void changeStatus(StatusType newStatus){
		status = newStatus;
		if (OnStatusChange != null) OnStatusChange(status);
	}
}
