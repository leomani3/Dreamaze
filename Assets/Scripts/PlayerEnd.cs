using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GameManager.currentLevelIndex++;
            if(GameManager.currentLevelIndex > GameManager.nbLevel)
            {
                GameManager.currentLevelIndex = 1;
                SceneManager.LoadScene(0);
            }
            else
            {
                SceneManager.LoadScene(GameManager.currentLevelIndex);
            }
        }
    }
}
