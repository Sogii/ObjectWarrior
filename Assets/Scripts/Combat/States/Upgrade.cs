using System.Collections;
using UnityEngine;

internal class Upgrade : State
{
    public Upgrade(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    public override IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
    }
}


