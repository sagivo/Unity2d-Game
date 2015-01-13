using UnityEngine;
using System.Collections;

public abstract class Liveable : BaseObj {
	public enum StatusType {Live, Destroyed, Build, InActive, Repair}
	public System.Action<Hitable> OnHit;
	public System.Action OnDie;
	public System.Action<StatusType> OnStatusChange;
	public System.Action<object> OnBuildStart;
	public System.Action<object> OnBuildEnd;
	public System.Action<object> OnRepairStart;
	public System.Action<object> OnRepairEnd;
	public System.Action<object> OnHealthChanged;
	[Header("Status")]
	public int health;
	public int level;
	public StatusType status;
	[Header("Sprites")]
	public GameObject[] gameObjectsPerLevel;
	public GameObject[] buildGameObjectsPerLevel;
	public Sprite[] spritesPerLevel; 
	//public Sprite[] spritesPerBuild;
	[Header("Times")]
	public float[] buildTimePerLevel = new float[]{0,0,0,0};	
	[Header("Visual")]
	public bool showHealthBar = true;
	public float healthBarYMargin = 2;
	
	protected SpriteRenderer spriteRenderer;
	Animator animator;
	HealthBarController healthBar;
	string dynamicName = "Dynamic GO";

	//inspecor
	public int[] healthPerLevel; // {get { return _healthPerLevel; } set{ _healthPerLevel = value; health = healthPerLevel[level]; if (showHealthBar) healthBar.divider = health; }}

	protected new void Awake(){
		base.Awake();
		spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();//TODO: REMOVE AFTER CLEAN OLD SPRITES
	}

	protected new void Start(){
		base.Start();	

		//if (spritesPerLevel != null && spritesPerLevel.Length > 0) spriteRenderer.sprite = spritesPerLevel[level];
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
		if (buildGameObjectsPerLevel != null && buildGameObjectsPerLevel.Length > level) loadDynamicGO(buildGameObjectsPerLevel[level]);
		changeStatus(StatusType.Build);
		if (showHealthBar) healthBar.gameObject.SetActive(false);
		Invoke("upgradeDone", buildTimePerLevel[level]);
	}

	public virtual void upgradeDone(){
		level++;
		health = healthPerLevel[level];
		if (gameObjectsPerLevel!=null && gameObjectsPerLevel.Length > level) loadDynamicGO(gameObjectsPerLevel[level]);
		else if (spritesPerLevel != null && spritesPerLevel.Length >= level) spriteRenderer.sprite = spritesPerLevel[level];
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

	void loadDynamicGO(GameObject go){
		if (!go) return;
		if (transform.Find(dynamicName)) Destroy(transform.Find(dynamicName).gameObject);
		go = Instantiate(go, transform.position, transform.rotation) as GameObject;
		go.name = dynamicName;
		go.transform.parent = gameObject.transform;
		spriteRenderer = go.gameObject.GetComponentInChildren<SpriteRenderer>();
		animator = go.GetComponent<Animator>();
	}

	protected void setValForAnimator(string str, float val){
		if (animator) animator.SetFloat(str, val);
		else e ("not found animator!");
	}
}
