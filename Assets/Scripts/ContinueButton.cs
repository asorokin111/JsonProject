using UnityEngine;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour
{
    private Button _continueButton;
    private void Start()
    {
        _continueButton = GetComponent<Button>();
        if (!PersistentData.Instance.FileExistsAndNotEmpty())
        {
            _continueButton.interactable = false;
        }
        else
        {
            _continueButton.interactable = true;
        }
    }
}
