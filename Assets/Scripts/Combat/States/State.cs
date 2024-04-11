using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    protected BattleSystem BattleSystem;

    protected State(BattleSystem battleSystem)
    {
        BattleSystem = battleSystem;
    }
    public virtual IEnumerator Start()
    {
        yield break;
    }
}

