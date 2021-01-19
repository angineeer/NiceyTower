using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("player");
    }
    void Update()
    {
        if (player.transform.position.y > 30)
        {
            Destroy(gameObject, 5f);
        }
    }
}
