using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCreationManager : MonoBehaviour
{
    [SerializeField] private HouseUIManager _uiManager;
    [SerializeField] Sprite guitarSprite;


    public void HitBoxEntered(ObjectTypes hitboxAreaType)
    {
        _uiManager.SetbuttonText("Create " + hitboxAreaType.ToString());
        _uiManager.SetButtonActive(true);

    }

    int i = 0;
    public void HitBoxClicked(GameObject gameObjectHit)
    {
        // Debug.Log("Creating " + hitboxAreaType.ToString());

        //Add the item to the hitbox 
        //Reference to HouseUIManager
        _uiManager.ChangeHitBoxSprite(ObjectDatabase.Instance.ItemsInInventory[i].ItemImage, gameObjectHit.transform.GetChild(0).GetComponent<SpriteRenderer>());
        i++;
        //-> ChangeHitBoxSprite 
    }

    public void HitBoxExited()
    {
        _uiManager.SetButtonActive(false);
    }
}
