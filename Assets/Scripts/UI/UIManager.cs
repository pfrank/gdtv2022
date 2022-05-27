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
    [SerializeField] private GameObject upgradePanel;
    [SerializeField] private GameObject selectionIndicator;

    private GameObject selected;
    private TMP_Text selectedDesc;
    private TMP_Text selectedInfo1;
    private TMP_Text selectedInfo2;
    private TMP_Text selectedInfo3;
    private TMP_Text selectedInfo4;
    private TMP_Text selectedInfo5;

    private static UIManager instance;
    public static UIManager Instance;

    public GameObject Selected
    {
        get
        {
            return selected;
        }
    }

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

        selectedDesc = upgradePanel.transform.Find("Description").GetComponent<TMP_Text>();
        selectedInfo1 = upgradePanel.transform.Find("Info1").GetComponent<TMP_Text>();
        selectedInfo2 = upgradePanel.transform.Find("Info2").GetComponent<TMP_Text>();
        selectedInfo3 = upgradePanel.transform.Find("Info3").GetComponent<TMP_Text>();
        selectedInfo4 = upgradePanel.transform.Find("Info4").GetComponent<TMP_Text>();
        selectedInfo5 = upgradePanel.transform.Find("Info5").GetComponent<TMP_Text>();
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
        if (gameObject == null || gameObject.tag == "Ground")
        {
            ClearInfo();
            return;
        }

        selected = gameObject;
        Tower tower = gameObject.GetComponent<Tower>();
        Enemy enemy = gameObject.GetComponent<Enemy>();
        if (tower)
            SetTowerInfo(tower);
        else if (enemy)
            SetEnemyInfo(enemy);
    }

    private void SetSelectionIndicator(GameObject gobj)
    {
        if (selectionIndicator == null)
            return;

        selectionIndicator.transform.SetParent(gobj.transform, false);
        selectionIndicator.GetComponent<SpriteRenderer>().enabled = true;
    }
    private void ClearSelectionIndicator()
    {
        if (selectionIndicator == null)
            return;

        selectionIndicator.transform.SetParent(null, false);
        selectionIndicator.GetComponent<SpriteRenderer>().enabled = false;
    }

    public void SetEnemyInfo(Enemy enemy)
    {
        if (selectionIndicator != null)
            SetSelectionIndicator(enemy.gameObject);

        selectedDesc.text = "";
        selectedInfo1.text = $"Damage: ";
        selectedInfo2.text = $"Speed: ";

        if (enemy)
        {
            selectedDesc.text = enemy.DisplayName;
            selectedInfo1.text += enemy.Damage;
            selectedInfo2.text += enemy.Speed;
        }
    }

    public void SetTowerInfo(Tower tower)
    {
        if (selectionIndicator != null)
            SetSelectionIndicator(tower.gameObject);

        selectedDesc.text = "";
        selectedInfo1.text = $"Kills: ";
        selectedInfo2.text = $"Level: ";
        selectedInfo3.text = $"Damage: ";
        selectedInfo4.text = $"Speed: ";
        selectedInfo5.text = $"Range: ";

        if (tower)
        {
            selectedDesc.text = tower.DisplayName;
            selectedInfo1.text += tower.Kills;
            selectedInfo2.text += tower.Level;
            selectedInfo3.text += tower.Damage;
            selectedInfo4.text += tower.AttackSpeed;
            selectedInfo5.text += tower.AttackRange;
        }
    }

    public void ClearInfo()
    {
        selected = null;
        ClearSelectionIndicator();
        selectedDesc.text = "";
        selectedInfo1.text = "";
        selectedInfo2.text = "";
        selectedInfo3.text = "";
        selectedInfo4.text = "";
        selectedInfo5.text = "";
    }
}
