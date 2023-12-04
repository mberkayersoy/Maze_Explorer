using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{
    [SerializeField] private Transform player;

    private void LateUpdate()
    {
        Vector3 newPosition =  new Vector3 (player.position.x, transform.position.y, player.position.z);

        transform.position = newPosition;
    }
}
