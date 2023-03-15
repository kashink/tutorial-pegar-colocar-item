using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    void Update()
    {
        Vector3 playerPos = GameObject.Find("Player").transform.position;
        this.transform.position = new Vector3(playerPos.x, playerPos.y, -10);
    }
}
