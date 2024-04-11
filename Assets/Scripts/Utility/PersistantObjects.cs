using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantObjects : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
