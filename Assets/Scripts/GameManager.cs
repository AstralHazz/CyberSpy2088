using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float respawnTimer = 3f;

    public void PlayerRespawn()
    {
        StartCoroutine(RespawnTimer());
    }

    IEnumerator RespawnTimer()
    {
        yield return new WaitForSeconds(respawnTimer);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
