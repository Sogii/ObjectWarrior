using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    private Camera mainCamera;
    public Transform player;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.15f;
    void Start()
    {
        mainCamera = this.GetComponent<Camera>();
    }

    // Update is called once per frames
    void LateUpdate()
    {
        Vector3 targetCameraPosition = new Vector3(player.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = Vector3.SmoothDamp(mainCamera.transform.position, targetCameraPosition, ref velocity, smoothTime);
    }


}
