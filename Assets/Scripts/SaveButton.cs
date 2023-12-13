using UnityEngine;

public class SaveButton : MonoBehaviour
{
    public void QuickSave()
    {
        PersistentData.Instance.QuickSave();
    }
}
