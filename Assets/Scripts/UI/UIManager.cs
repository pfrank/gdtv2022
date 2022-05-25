using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager: MonoBehaviour
{
    [SerializeField] private TMP_Text waveNumberText;
    [SerializeField] private TMP_Text enemyCountText;
    [SerializeField] private TMP_Text enemyKillCountText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private Image treeHealth;
    [SerializeField] private GameObject towerButtonPanel;
    [SerializeField] private Button towerButtonPrefab;
    [SerializeField] private GameObject upgradePanel;

    private TMP_Text selectedDesc;
    private TMP_Text selectedLevel;
    private TMP_Text selectedDamage;
    private TMP_Text selectedDelay;
    private TMP_Text selectedRadius;

    private static UIManager instance;
    public static UIManager Instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        TowerManager towerManager = gameObject.GetComponent<TowerManager>();
        foreach (Tower tower in towerManager.Towers)
        {
            var newButton = Instantiate(towerButtonPrefab, towerButtonPanel.transform) as Button;
            newButton.onClick.AddListener(() => GameManager.Instance.AddTower(tower.gameObject));

            TMP_Text buttonText = newButton.transform.GetComponentInChildren<TMP_Text>();
            buttonText.text = $"{tower.DisplayName} - {tower.Cost}GP";
        }

        selectedDesc = upgradePanel.transform.Find("Description").GetComponent<TMP_Text>();
        selectedLevel = upgradePanel.transform.Find("Level").GetComponent<TMP_Text>();
        selectedDamage = upgradePanel.transform.Find("Damage").GetComponent<TMP_Text>();
        selectedDelay = upgradePanel.transform.Find("AttackDelay").GetComponent<TMP_Text>();
        selectedRadius = upgradePanel.transform.Find("AttackRadius").GetComponent<TMP_Text>();
    }

    public void SetWaveNumber(int waveNumber)
    {
        waveNumberText.text = $"Wave: {waveNumber:N0}";
    }

    public void SetEnemyCount(int enemyCount)
    {
        enemyCountText.text = $"Enemies: {enemyCount:N0}";
    }

    public void SetEnemyKillCount(int enemyCount)
    {
        enemyKillCountText.text = $"Enemies Stopped: {enemyCount:N0}";
    }

    public void SetTreeHealth(int currentHealth, int totalHealth)
    {
        SetTreeHealth((float)currentHealth / (float)totalHealth);
    }

    public void SetTreeHealth(float percentRemaining)
    {
        treeHealth.fillAmount = percentRemaining;
    }

    public void SetGold(int gold)
    {
        goldText.text = $"Gold: {gold:N0}";
    }

    public void SetCountdownSeconds(int seconds)
    {
        countdownText.text = $"Next Wave Starts in {seconds} Second{(seconds == 1 ? "": "s")} ...";
    }

    public void ShowCountdownPanel()
    {
        countdownPanel.SetActive(true);
    }
    public void HideCountdownPanel()
    {
        countdownPanel.SetActive(false);
    }

    public void OnClickUpgradeTower()
    {

        Debug.Log("UPGRADE TOWER");
        // Go into UPGRADE state
        // On click tower
    }

    public void SetTowerInfo(Tower tower){
        selectedDesc.text = tower.DisplayName;
        selectedLevel.text = $"Level: {tower.Level}";
        selectedDamage.text = $"Damage: {tower.Damage}";
        selectedDelay.text = $"Attack Delay: {tower.AttackDelay}";
        selectedRadius.text = $"Attack Radius: {tower.AttackRadius}";
    }
}
