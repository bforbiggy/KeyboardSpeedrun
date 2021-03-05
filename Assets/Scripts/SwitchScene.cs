using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void SwitchGame()
    {
        DataHub.Clear();
        SceneManager.LoadScene("Game");
    }

    public void SwitchOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void SwitchMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
