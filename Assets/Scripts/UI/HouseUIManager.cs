using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HouseUIManager : MonoBehaviour
{

    [SerializeField] private Button _button;

    private void Awake()
    {
        _button.gameObject.SetActive(false);
    }
    
    public void SetButtonActive(bool active)
    {
        _button.gameObject.SetActive(active);
    }
    
    public void SetbuttonText(string text)
    {
        _button.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public void ChangeHitBoxSprite(Sprite spriteToChangeTo, SpriteRenderer objectSpriteRenderer)
    {
        objectSpriteRenderer.transform.localScale = new Vector3(2, 2, 2);
        objectSpriteRenderer.sprite = spriteToChangeTo;
    }
}
