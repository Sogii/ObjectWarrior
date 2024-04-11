using System.Collections;
using UnityEngine;
public class PlayerTurn : State
{
    public PlayerTurn(BattleSystem battleSystem) : base(battleSystem)
    {


    }

    public override IEnumerator Start()
    {
        Debug.Log("Player Turn");
        BattleSystem.combatUIManager.ChangeCombatMenuPanel(0);
        //Populate button array with the proper item buttons 
        yield return new WaitForSeconds(.1f);
    }
}
