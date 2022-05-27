using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager: MonoBehaviour
{
    [SerializeField] private TMP_Text waveNumberText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private Image treeHealth;
    [SerializeField] private GameObject towerButtonPanel;
    [SerializeField] private Button towerButtonPrefab;
    [SerializeField] private GameObject upgradePanel;

    private TMP_Text selectedDesc;
    private TMP_Text selectedKills;
    private TMP_Text selectedLevel;
    private TMP_Text selectedDamage;
    private TMP_Text selectedSpeed;
    private TMP_Text selectedRange;

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
        selectedKills = upgradePanel.transform.Find("Kills").GetComponent<TMP_Text>();
        selectedDamage = upgradePanel.transform.Find("Damage").GetComponent<TMP_Text>();
        selectedSpeed = upgradePanel.transform.Find("AttackSpeed").GetComponent<TMP_Text>();
        selectedRange = upgradePanel.transform.Find("Range").GetComponent<TMP_Text>();
    }

    public void SetWaveNumber(int waveNumber)
    {
        waveNumberText.text = $"Wave: {waveNumber:N0}";
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
        selectedDesc.text = "";
        selectedKills.text = $"Kills: ";
        selectedLevel.text = $"Level: ";
        selectedDamage.text = $"Damage: ";
        selectedSpeed.text = $"Speed: ";
        selectedRange.text = $"Range: ";

        if (tower)
        {
            selectedDesc.text = tower.DisplayName;
            selectedKills.text += tower.Kills;
            selectedLevel.text += tower.Level;
            selectedDamage.text += tower.Damage;
            selectedSpeed.text += tower.AttackSpeed;
            selectedRange.text += tower.AttackRange;
        }
    }
}
