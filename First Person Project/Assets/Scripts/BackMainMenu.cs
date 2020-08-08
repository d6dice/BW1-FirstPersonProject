using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackMainMenu : MonoBehaviour
{

    void OnLevelWasLoaded(int level)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void backMain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }

}
