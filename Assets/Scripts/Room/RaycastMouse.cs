using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RaycastMouse : MonoBehaviour
{
    private ObjectCreationManager _objectCreationManager;

    void Start()
    {
        _objectCreationManager = GetComponent<ObjectCreationManager>();
    }
    void Update()
    {
        DetectMouseHover();
    }


    private void DetectMouseHover()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.GetComponent<AddObjectHitboxLogic>() != null)
            {

                GameObject gameObjectHit = hit.collider.gameObject;
                hit.collider.GetComponent<AddObjectHitboxLogic>().ChangeColorOnHover();
                if(Input.GetMouseButtonDown(0))
                {
                    _objectCreationManager.HitBoxClicked(gameObjectHit);
                }
            }
        }
        else
        {
            _objectCreationManager.HitBoxExited();
        }
    }

    private void UpdateObjectionCreationManagerOnHover(ObjectTypes hitboxAreaType)
    {
        _objectCreationManager.HitBoxEntered(hitboxAreaType);
        Debug.Log("Hovering over " + hitboxAreaType);
    }
}
