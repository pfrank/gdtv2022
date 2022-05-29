using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text waveNumberText;
    [SerializeField] private TMP_Text goldText;
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private Image treeHealth;
    [SerializeField] private GameObject towerButtonPanel;
    [SerializeField] private Button towerButtonPrefab;

    private GameObject informationPanel;
    private GameObject towerInfoPanel;
    private TMP_Text towerLevel;
    private TMP_Text towerDamage;
    private TMP_Text towerSpeed;
    private TMP_Text towerRange;
    private TMP_Text towerKills;
    private GameObject enemyInfoPanel;
    private TMP_Text enemyHealth;
    private TMP_Text enemyDamage;
    private TMP_Text enemySpeed;

    private TMP_Text selectedInfo;

    private GameObject selected;

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
            var newButton = Instantiate(towerButtonPrefab, towerButtonPanel.transform);
            newButton.onClick.AddListener(() => GameManager.Instance.AddTower(tower.gameObject));

            TMP_Text buttonText = newButton.transform.GetComponentInChildren<TMP_Text>();
            buttonText.text = $"{tower.DisplayName} - {tower.Cost}GP";
        }

        informationPanel = GameObject.Find("Canvas/InformationPanel");
        selectedInfo = informationPanel.transform.Find("SelectionInfo").GetComponent<TMP_Text>();
        towerInfoPanel = informationPanel.transform.Find("TowerInfoPanel").gameObject;
        towerInfoPanel.SetActive(false);
        towerLevel = towerInfoPanel.transform.Find("Level").GetComponent<TMP_Text>();
        towerDamage = towerInfoPanel.transform.Find("Damage").GetComponent<TMP_Text>();
        towerSpeed = towerInfoPanel.transform.Find("Speed").GetComponent<TMP_Text>();
        towerRange = towerInfoPanel.transform.Find("Range").GetComponent<TMP_Text>();
        towerKills = towerInfoPanel.transform.Find("Kills").GetComponent<TMP_Text>();
        enemyInfoPanel = informationPanel.transform.Find("EnemyInfoPanel").gameObject;
        enemyInfoPanel.SetActive(false);
        enemyHealth = enemyInfoPanel.transform.Find("Health").GetComponent<TMP_Text>();
        enemyDamage = enemyInfoPanel.transform.Find("Damage").GetComponent<TMP_Text>();
        enemySpeed = enemyInfoPanel.transform.Find("Speed").GetComponent<TMP_Text>();
    }

    private void Update()
    {

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

    public void OnClickUpgradeTower()
    {

        Debug.Log("UPGRADE TOWER");
        // Go into UPGRADE state
        // On click tower
    }

    public void SetSelectedObjectInfo(GameObject? gameObject)
    {
        if (gameObject)
        {
            selected = gameObject;
            Tower tower = gameObject.GetComponent<Tower>();
            Enemy enemy = gameObject.GetComponent<Enemy>();
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
    }

    public void SetEnemyInfo(Enemy enemy)
    {
        if (enemyInfoPanel.activeInHierarchy == false)
            enemyInfoPanel.SetActive(true);

        enemyHealth.text = $"Health: {enemy.Health}";
        enemyDamage.text = $"Damage: {enemy.Damage}";
        enemySpeed.text = $"Speed: {enemy.Speed}";
    }
}
