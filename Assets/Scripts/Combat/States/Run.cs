using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Run : State
{
    public Run(BattleSystem battleSystem) : base(battleSystem)
    {
    }
    public override IEnumerator Start()
    {
        Debug.Log("Run");
        BattleSystem.combatUIManager.PopulatePostCombatPanel("Run", "You have chosen to run!");
        yield return new WaitForSeconds(.5f);
        //Add run logic (pick 1/4 items randomly and return to base)
    }
}
