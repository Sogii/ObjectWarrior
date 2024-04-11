using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatUIManager : MonoBehaviour
{

    #region Healthbars
    //buttons


    //Healthbars
    [SerializeField] private HealthBarData _playerHealthBarData;
    [SerializeField] private HealthBarData _enemyHealthBarData;

    void Start()
    {
        BattleEventSystem.current.OnPlayerHealthChanged += UpdatePlayerHealthBar;
        BattleEventSystem.current.OnEnemyHealthChanged += UpdateEnemyHealthBar;
    }
    public void UpdatePlayerHealthBar(float currentHealth, float maxHealth)
    {
        _playerHealthBarData.healthBarNumber.text = currentHealth + "/" + maxHealth;
        _playerHealthBarData.healthBarSlider.value = currentHealth / maxHealth;
    }

    public void UpdateHealthBarNames()
    {
        _playerHealthBarData.healthBarName.text = BattleSystem.Instance._playerData.PlayerName;
        _enemyHealthBarData.healthBarName.text = BattleSystem.Instance._enemyData.EnemyName;
    }

    public void UpdateEnemyHealthBar(float currentHealth, float maxHealth)
    {
        _enemyHealthBarData.healthBarNumber.text = currentHealth + "/" + maxHealth;
        _enemyHealthBarData.healthBarSlider.value = currentHealth / maxHealth;
    }
    #endregion

    #region CombatMenu
    [SerializeField] private GameObject[] _combatMenuPanels;
    [SerializeField] private string _selectedButtonName;
    private int _currentPanelIndex = 0;

    //ItemPanel
    public TextMeshProUGUI itemPanelTitle;
    public TextMeshProUGUI itemPanelDescription;
    public Button useItemButton;
    public Item selectedItem;

    public Image[] weaponImages;
    public TextMeshProUGUI itemUpgradeWording;

    //Post-combat panel
    public TextMeshProUGUI postCombatPanelText;
    public TextMeshProUGUI postCombatPanelTitle;

    //UpgradeInputPanel
    public TMP_InputField upgradeInputField;

    // Item upgrade panel
    public GameObject OpjectGenerationPanel; 

    public void ToggleObjectGenerationPanel(bool toggle)
    {
        OpjectGenerationPanel.SetActive(toggle);
    }
    public void ChangeCombatMenuPanel(int index, string buttonName = null)
    {
        if (index == 2 && buttonName != null)
        {
            PopulateItemInfoPanel(buttonName);
        }
        else if (index == 1)
        {
            PopulatePickItemPanel();
        }

        foreach (GameObject panel in _combatMenuPanels)
        {
            panel.SetActive(false);
        }
        _combatMenuPanels[index].SetActive(true);
        _currentPanelIndex = index;
    }

    private void PopulateItemInfoPanel(string buttonName)
    {
        selectedItem = ObjectDatabase.Instance.GetItemByName(buttonName);
        itemPanelTitle.text = selectedItem.ItemName;
        itemPanelDescription.text = selectedItem.ItemDescription;
        foreach(Image weaponImage in weaponImages)
        {
            weaponImage.sprite = selectedItem.ItemImage;
        }
        if (BattleSystem.Instance.GetState() is PlayerTurn)
        {
            itemUpgradeWording.text = "Use Item";
        }
        else if (BattleSystem.Instance.GetState() is Upgrade)
        {
            itemUpgradeWording.text = "Upgrade Item";
        }
        else if (BattleSystem.Instance.GetState() is AddItem)
        {

        }
    }

    public void PopulatePickItemPanel()
    {
        GameObject itemOptionsHolder = _combatMenuPanels[1].transform.Find("ItemOptions").gameObject;
        foreach (Transform buttonTransform in itemOptionsHolder.transform)
        {

            Item itemForButton = ObjectDatabase.Instance.ItemsInInventory[buttonTransform.GetSiblingIndex()];

            if (itemForButton.ItemName == null || itemForButton.ItemName == "")
            {
                buttonTransform.gameObject.SetActive(false);
                continue;
            }
            else
            {
                buttonTransform.gameObject.SetActive(true);
                GameObject button = buttonTransform.gameObject;

                button.GetComponentInChildren<TextMeshProUGUI>().text = itemForButton.ItemName;
                button.GetComponent<Button>().onClick.AddListener(() => ChangeCombatMenuPanel(2, itemForButton.ItemName));
            }
        }
    }

    public void PopulatePostCombatPanel(string title, string text)
    {
        postCombatPanelTitle.text = title;
        postCombatPanelText.text = text;
    }

    public void InnitiatePlayerCombat()
    {
        BattleSystem.Instance.SetState(new PlayerCombat(BattleSystem.Instance, selectedItem));
    }

    #endregion

}

[System.Serializable]
public class HealthBarData
{
    public GameObject healthBarHolder;
    public TextMeshProUGUI healthBarNumber;
    public TextMeshProUGUI healthBarName;
    public Slider healthBarSlider;
}

// public struct MenuPage
// {
//     public GameObject menuPanel;
//     public Button[] buttons;
// }