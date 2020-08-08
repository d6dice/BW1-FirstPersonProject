using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishMenu : MonoBehaviour{

    void OnLevelWasLoaded(int level)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    public void FinishGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 3);
    }

}
