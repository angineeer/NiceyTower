using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject platformPrefab;
    public GameObject player;

    private const float MIN_PLATFORM_WIDTH = 3f;
    private const float MAX_PLATFORM_WIDTH = 10f;
    private float lastPlatformY = -10;

    void Update()
    {
        if (player.transform.position.y > lastPlatformY - 20)
        {
            float platformWidth = Random.Range(
                Mathf.Max(MIN_PLATFORM_WIDTH, MIN_PLATFORM_WIDTH + 2 - player.transform.position.y * 0.001f),
                Mathf.Max(MIN_PLATFORM_WIDTH, MAX_PLATFORM_WIDTH - player.transform.position.y * 0.001f));
            GameObject platform = Instantiate(platformPrefab, new Vector2(Random.Range(-8 + platformWidth / 2, 8 - platformWidth / 2), lastPlatformY + 5), Quaternion.identity);
            Vector3 platformScale = platform.transform.localScale;
            platformScale.x = platformWidth;
            platform.transform.localScale = platformScale;
            lastPlatformY += 5;
        }
    }
}
