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
