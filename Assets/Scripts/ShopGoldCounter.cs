using TMPro;
using UnityEngine;

// Class to be attached to the GoldText
public class ShopGoldCounter : MonoBehaviour
{
    private TextMeshProUGUI _goldText;
    private void Start()
    {
        _goldText = GetComponent<TextMeshProUGUI>();
        UpdateGoldText();
    }

    private void OnEnable()
    {
        Store.OnItemBought += UpdateGoldText;
    }

    private void OnDisable()
    {
        Store.OnItemBought -= UpdateGoldText;
    }

    private void UpdateGoldText()
    {
        _goldText.text = "Gold: " + PersistentData.Instance.Gold;
    }
}