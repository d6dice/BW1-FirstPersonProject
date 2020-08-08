using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishMainMenu : MonoBehaviour
{

    void OnLevelWasLoaded(int level)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void finishMain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
