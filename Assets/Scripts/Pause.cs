using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    private ActionMap _actionMap;
    private InputAction _pause;

    private void Awake()
    {
        _actionMap = new ActionMap();
    }

    private void OnEnable()
    {
        _pause = _actionMap.Player.Pause;
        _pause.Enable();
        _pause.performed += TogglePause;
    }

    private void OnDisable()
    {
        _pause.performed -= TogglePause;
        _pause.Disable();
    }

    private void TogglePause(InputAction.CallbackContext context)
    {
        bool paused = Time.timeScale == 0;
        // Resume if paused and pause otherwise
        Time.timeScale = paused ? 1 : 0;
        _pauseMenu.SetActive(!paused);
    }
}
