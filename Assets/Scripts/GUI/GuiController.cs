using UnityEngine;
using System.Collections;

public class GuiController : Singleton<GuiController> {

	public GameObject menuMain;
	public GameObject menuGame;
	public GameObject menuPause;
	public GameObject menuBuild;

	public TextMesh inGameScore;
	public TextMesh inGameCurrency;
	public TextMesh inGameCoreHealth;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateScoreLabels(int val) {
		inGameScore.text=""+val;
	}

	public void UpdateCurrencyLabels(int val) {
		inGameCurrency.text=""+val;
	}

	public void UpdateCoreHealthLabels(int val) {
		inGameCoreHealth.text=""+val+"%";
	}

	public void ShowMenuMain() {
		menuMain.SetActive(true);
		menuGame.SetActive(false);
		menuPause.SetActive(false);
		menuBuild.SetActive(false);
	}

	public void ShowMenuGame() {
		menuMain.SetActive(false);
		menuGame.SetActive(true);
		menuPause.SetActive(false);
		menuBuild.SetActive(false);
	}

	public void ShowMenuPause() {
		Debug.Log("ShowMenuPause");
	}

	public void ShowMenuBuild() {
	}
}
