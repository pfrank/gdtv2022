using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private TMP_Text waveNumberText;
    private TMP_Text goldText;
    private TMP_Text countdownText;
    private GameObject countdownPanel;
    private Image treeHealth;
    private GameObject towerButtonPanel;
    [SerializeField] private Button towerButtonPrefab;

    private GameObject informationPanel;
    private GameObject towerInfoPanel;
    private TMP_Text towerLevel;
    private TMP_Text towerDamage;
    private GameObject upgradeDamageButton;
    private TMP_Text towerSpeed;
    private GameObject upgradeSpeedButton;
    private TMP_Text towerRange;
    private GameObject upgradeRangeButton;
    private TMP_Text towerKills;
    private GameObject enemyInfoPanel;
    private TMP_Text enemyHealth;
    private TMP_Text enemyDamage;
    private TMP_Text enemySpeed;

    private TMP_Text selectedInfo;

    private static GameObject selected;

    private static UIManager instance;
    public static UIManager Instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        GameObject canvas = GameObject.Find("Canvas");
        countdownPanel = canvas.transform.Find("CountdownPanel").gameObject;
        countdownText = countdownPanel.GetComponentInChildren<TMP_Text>();

        informationPanel = canvas.transform.Find("InformationPanel").gameObject;

        towerButtonPanel = informationPanel.transform.Find("TowerButtonPanel").gameObject;
        treeHealth = canvas.transform.Find("TreeHealthPanel/TreeHealth").GetComponent<Image>();

        
        TowerManager towerManager = gameObject.GetComponent<TowerManager>();
        foreach (Tower tower in towerManager.Towers)
        {
            var newButton = Instantiate(towerButtonPrefab, towerButtonPanel.transform);
            newButton.onClick.AddListener(() => GameManager.Instance.AddTower(tower.gameObject));

            TMP_Text buttonText = newButton.transform.GetComponentInChildren<TMP_Text>();
            buttonText.text = $"{tower.DisplayName} - {tower.Cost}GP";
        }


        selectedInfo = informationPanel.transform.Find("SelectionInfo").GetComponent<TMP_Text>();
        waveNumberText = informationPanel.transform.Find("WaveNumber").GetComponent<TMP_Text>();
        goldText = informationPanel.transform.Find("Gold").GetComponent<TMP_Text>();

        towerInfoPanel = informationPanel.transform.Find("TowerInfoPanel").gameObject;
        towerInfoPanel.SetActive(false);
        towerLevel = towerInfoPanel.transform.Find("Level").GetComponent<TMP_Text>();
        towerDamage = towerInfoPanel.transform.Find("Damage/DamageText").GetComponent<TMP_Text>();
        upgradeDamageButton = towerInfoPanel.transform.Find("Damage/UpgradeDamage").gameObject;
        towerSpeed = towerInfoPanel.transform.Find("Speed/SpeedText").GetComponent<TMP_Text>();
        upgradeSpeedButton = towerInfoPanel.transform.Find("Speed/UpgradeSpeed").gameObject;
        towerRange = towerInfoPanel.transform.Find("Range/RangeText").GetComponent<TMP_Text>();
        upgradeRangeButton = towerInfoPanel.transform.Find("Range/UpgradeRange").gameObject;

        towerKills = towerInfoPanel.transform.Find("Kills").GetComponent<TMP_Text>();
        enemyInfoPanel = informationPanel.transform.Find("EnemyInfoPanel").gameObject;
        enemyInfoPanel.SetActive(false);
        enemyHealth = enemyInfoPanel.transform.Find("Health").GetComponent<TMP_Text>();
        enemyDamage = enemyInfoPanel.transform.Find("Damage").GetComponent<TMP_Text>();
        enemySpeed = enemyInfoPanel.transform.Find("Speed").GetComponent<TMP_Text>();
    }

    public void SetWaveNumber(int waveNumber)
    {
        waveNumberText.text = $"Wave: {waveNumber:N0}";
    }

    public void SetTreeHealth(int currentHealth, int totalHealth)
    {
        SetTreeHealth(currentHealth / (float)totalHealth);
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
        countdownText.text = $"Next Wave Starts in {seconds} Second{(seconds == 1 ? "" : "s")} ...";
    }

    public void ShowCountdownPanel()
    {
        countdownPanel.SetActive(true);
    }
    public void HideCountdownPanel()
    {
        countdownPanel.SetActive(false);
    }

    public void OnClickUpgradeDamage()
    {
        Tower tower = selected.GetComponent<Tower>();
        tower.UpgradeDamage();
    }

    public void OnClickUpgradeSpeed()
    {
        Tower tower = selected.GetComponent<Tower>();
        tower.UpgradeSpeed();
    }

    public void OnClickUpgradeRange()
    {
        Tower tower = selected.GetComponent<Tower>();
        tower.UpgradeRange();
    }

    public void SetSelectedObjectInfo(GameObject? gobj)
    {
        if (gobj)
        {
            selected = gobj;
            Tower tower = gobj.GetComponent<Tower>();
            Enemy enemy = gobj.GetComponent<Enemy>();
            if (tower)
                SetTowerInfo(tower);
            else if (enemy)
                SetEnemyInfo(enemy);
        }
        else
            ClearSelection();
    }

    public void UpdateSelectedObjectInfo()
    {
        if (selected)
        {
            SetSelectedObjectInfo(selected);
        }
    }

    public void ClearSelection()
    {
        selected = null;
        towerInfoPanel.SetActive(false);
        enemyInfoPanel.SetActive(false);
        selectedInfo.text = "";
    }

    public void SetTowerInfo(Tower tower)
    {
        if (towerInfoPanel.activeInHierarchy == false)
            towerInfoPanel.SetActive(true);

        selectedInfo.text = tower.DisplayName;
        towerLevel.text = $"Level: {tower.Level}";
        towerDamage.text = $"Damage: {tower.Damage}";
        towerSpeed.text = $"Speed: {tower.AttackSpeed}";
        towerRange.text = $"Range: {tower.AttackRange}";
        towerKills.text = $"Kills: {tower.Kills}";

        if (!tower.IsDamageMaxLevel)
            ShowUpgradeButton("Damage");
        else
            HideUpgradeButton("Damage");

        if (!tower.IsSpeedMaxLevel)
            ShowUpgradeButton("Speed");
        else
            HideUpgradeButton("Speed");

        if (!tower.IsRangeMaxLevel)
            ShowUpgradeButton("Range");
        else
            HideUpgradeButton("Range");

    }

    public void SetEnemyInfo(Enemy enemy)
    {
        if (enemyInfoPanel.activeInHierarchy == false)
            enemyInfoPanel.SetActive(true);

        enemyHealth.text = $"Health: {enemy.Health}";
        enemyDamage.text = $"Damage: {enemy.Damage}";
        enemySpeed.text = $"Speed: {enemy.Speed}";
    }

    public void SetUpgradeButtonText(string stat, string newText)
    {
        TMP_Text buttonText = null;
        if (stat == "Damage")
            buttonText = upgradeDamageButton.GetComponent<TMP_Text>();
        else if (stat == "Speed")
            buttonText = upgradeSpeedButton.GetComponent<TMP_Text>();
        else if (stat == "Range")
            buttonText = upgradeSpeedButton.GetComponent<TMP_Text>();

        if (buttonText)
            buttonText.text = newText;
    }
    public void ShowUpgradeButton(string stat)
    {
        GameObject button = null;
        if (stat == "Damage")
            button = upgradeDamageButton;
        else if (stat == "Speed")
            button = upgradeSpeedButton;
        else if (stat == "Range")
            button = upgradeRangeButton;

        if (button)
            button.SetActive(true);
    }

    public void HideUpgradeButton(string stat)
    {
        GameObject button = null;
        if (stat == "Damage")
            button = upgradeDamageButton;
        else if (stat == "Speed")
            button = upgradeSpeedButton;
        else if (stat == "Range")
            button = upgradeRangeButton;

        if (button)
            button.SetActive(false);
    }
}
