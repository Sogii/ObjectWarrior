using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatMenuButtonLogic : MonoBehaviour
{
    public CombatUIManager combatUIManager;
    public void RunButton()
    {
        BattleSystem.Instance.SetState(new Run(BattleSystem.Instance));
    }

    public void SelectItemButton(int itemIndex)
    {
        //Add item logic
    }

    public void UseItemForCombatButton()
    {
        //Add item logic
    }

    public void ChangePannelButton(int panelIndex)
    {
        combatUIManager.ChangeCombatMenuPanel(panelIndex);
    }

    public void GoToUpgradeState()
    {
        combatUIManager.ChangeCombatMenuPanel(1);
        BattleSystem.Instance.SetState(new Upgrade(BattleSystem.Instance));
    }

    public void GoToChangeItemState()
    {
        combatUIManager.ChangeCombatMenuPanel(1);
        BattleSystem.Instance.SetState(new AddItem(BattleSystem.Instance));
    }

    public void UseItemButton()
    {
        if (BattleSystem.Instance.GetState() is PlayerTurn)
        {
            BattleSystem.Instance.SetState(new PlayerCombat(BattleSystem.Instance, combatUIManager.selectedItem));
        }
        else if (BattleSystem.Instance.GetState() is Upgrade)
        {
            combatUIManager.ChangeCombatMenuPanel(5);
            // ObjectDetection.instance.is_upgrade_generation_done = false;
            // BattleSystem.Instance.SetState(new UpgradeSelected(BattleSystem.Instance, combatUIManager.selectedItem));
        }
    }

    public void UpgradeItemButton()
    {
        StartCoroutine(ObjectDetection.instance.getObjectUpgradeBig(combatUIManager.selectedItem.UpgradeText, combatUIManager.selectedItem.ItemName, combatUIManager.upgradeInputField.text));
        BattleSystem.Instance.SetState(new UpgradeSelected(BattleSystem.Instance, combatUIManager.selectedItem, combatUIManager.upgradeInputField.text));
    }

    public void SimpleUpgradeItemButton()
    {
        RandomGenerationUtility.UpgradeItem(combatUIManager.selectedItem);
        BattleSystem.Instance.SetState(new NewCombatRound(BattleSystem.Instance));
    }

    public void Run()
    {
       BattleSystem.Instance.level = 10;
       BattleSystem.Instance.SetState(new PlayerVictory(BattleSystem.Instance));
    }

}
