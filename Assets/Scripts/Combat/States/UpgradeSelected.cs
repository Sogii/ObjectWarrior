using System;
using System.Collections;
using UnityEngine;

internal class UpgradeSelected : State
{
    private Item item;
    private string upgradeMotivation;
    public UpgradeSelected(BattleSystem battleSystem, Item item, String upgradeMotivation) : base(battleSystem)
    {
        this.item = item;
        this.upgradeMotivation = upgradeMotivation;
    }

    public override IEnumerator Start()
    {
        // StartCoroutine(ObjectDetection.instance.GenerateUpgrade(item));

        yield return new WaitUntil(() => ObjectDetection.instance.is_upgrade_generation_done);
        // ObjectDatabase.Instance.UpgradeItem(item, upgradeMotivation, BattleSystem.level);
        ObjectDetection.instance.GetGeneratedUpgrade(item);
        Debug.Log("ÏTEM UPGRADED");
        BattleSystem.SetState(new NewCombatRound(BattleSystem));
        yield return new WaitForSeconds(1f);
    }
}


