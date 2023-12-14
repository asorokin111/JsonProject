using UnityEngine;
using UnityEngine.InputSystem;

// UI-related inputs
public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameObject _shopMenu;
    [SerializeField]
    private GameObject _pauseMenu;

    private bool _paused;

    private ActionMap _actionMap;
    private InputAction _interact;
    private InputAction _pause;

    private void Awake()
    {
        _actionMap = new ActionMap();
    }

    private void OnEnable()
    {
        _interact = _actionMap.Player.Interact;
        _interact.Enable();
        _interact.performed += InteractWithTrader;

        _pause = _actionMap.Player.Pause;
        _pause.Enable();
        _pause.performed += Pause;
    }

    private void OnDisable()
    {
        _interact.Disable();
        _interact.performed -= InteractWithTrader;

        _pause.Disable();
        _pause.performed -= Pause;
    }

    private void InteractWithTrader(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.zero);
        if (hit.collider != null && hit.collider.CompareTag("Trader"))
        {
            OpenShopMenu();
        }
    }

    private void OpenShopMenu()
    {

    }

    private void Pause(InputAction.CallbackContext context)
    {
        if (_paused)
        {
            Time.timeScale = 1;
            _paused = false;
        }
        else
        {
            Time.timeScale = 0;
            _paused = true;
        }
    }
}
