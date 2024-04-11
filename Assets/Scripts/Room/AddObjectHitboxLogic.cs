using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectHitboxLogic : MonoBehaviour
{
    public ObjectTypes AreaType;
    private Color startColor;
    private Coroutine _changeColorBackCoroutine;
    [SerializeField] private float changeColorBackTime = 0.02f;
    void Start()
    {
        startColor = GetComponent<SpriteRenderer>().color;
    }
    public void ChangeColorOnHover()
    {
        GetComponent<SpriteRenderer>().color = Color.green;

        // If the coroutine is already running, stop it
        if (_changeColorBackCoroutine != null)
        {
            StopCoroutine(_changeColorBackCoroutine);
        }

        // Start the coroutine
        _changeColorBackCoroutine = StartCoroutine(ChangeColorBack());
    }

    IEnumerator ChangeColorBack()
    {
        // Wait for half a second
        yield return new WaitForSeconds(changeColorBackTime);
        GetComponent<SpriteRenderer>().color = startColor;
    }
}