using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State State;
    private Coroutine _currentCoroutine;

    public void SetState(State state)
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }

        State = state;
        _currentCoroutine = StartCoroutine(State.Start());
    }

    public State GetState()
    {
        return State;
    }
}
