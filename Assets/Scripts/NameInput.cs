using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NameInput : MonoBehaviour
{
    public readonly int nameCharacterLimit = 10;
    [SerializeField]
    private Button _startButton;
    private TMP_InputField _input;

    private void Awake()
    {
        _input = GetComponent<TMP_InputField>();
    }

    public void GetInputName()
    {
        PersistentData.Instance.Name = _input.text;
    }

    public bool IsNameNotEmpty()
    {
        return _input.text.Length > 0;
    }

    public bool IsNameWithinLimit()
    {
        return _input.text.Length <= nameCharacterLimit;
    }

    public void ValidateNameAndEnableButton()
    {
        _startButton.interactable = IsNameNotEmpty() && IsNameWithinLimit();
    }
}
