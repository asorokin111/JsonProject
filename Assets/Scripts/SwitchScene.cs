using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void NextScene()
    {
        SceneByOffset(1);
    }

    public void SceneByOffset(int offset)
    {
        SavePreviousIndex();
        SaveBeforeLoad();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + offset);
    }

    public void SceneByIndex(int index)
    {
        SavePreviousIndex();
        SaveBeforeLoad();
        SceneManager.LoadScene(index);
    }

    public void ReturnToPreviousScene()
    {
        if (!PersistentData.Instance.previousScenes.TryPop(out int previousScene))
            return;
        SaveBeforeLoad();
        SceneManager.LoadScene(previousScene);
    }

    // Pause toggle that's not tied to the player
    // TODO: make this entire class into a singleton and delete the pause that's tied to the player
    public void GlobalPause(GameObject pauseMenu)
    {
        bool paused = Time.timeScale == 0.0f;
        Time.timeScale = paused ? 1 : 0;
        pauseMenu.SetActive(!paused);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void SavePreviousIndex()
    {
        PersistentData.Instance.previousScenes.Push(SceneManager.GetActiveScene().buildIndex);
    }

    private void SaveBeforeLoad()
    {
        if (PersistentData.Instance.FileExistsAndNotEmpty() && SceneManager.GetActiveScene().buildIndex != 0)
            PersistentData.Instance.QuickSave();
    }

}
