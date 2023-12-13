using TMPro;
using UnityEngine;

public class StatHandler : MonoBehaviour
{
    public static StatHandler Instance;
    [SerializeField]
    private TextMeshProUGUI _pointText;
    [SerializeField]
    private TextMeshProUGUI _strText;
    [SerializeField]
    private TextMeshProUGUI _dexText;
    [SerializeField]
    private TextMeshProUGUI _intText;
    private PlayerStats _stats;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        _pointText ??= GameObject.Find("PointText").GetComponent<TextMeshProUGUI>();
        _strText ??= GameObject.Find("STRText").GetComponent<TextMeshProUGUI>();
        _dexText ??= GameObject.Find("DEXText").GetComponent<TextMeshProUGUI>();
        _intText ??= GameObject.Find("INTText").GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _stats = PersistentData.Instance.Stats;
        updatePointText();
        updateStatText();
    }

    public enum PossibleStats
    {
        Str,
        Dex,
        Int,
    };

    public void AddToStat(int stat)
    {
        if (_stats.LeftoverPoints <= 0) return;
        switch ((PossibleStats)stat)
        {
            case PossibleStats.Str:
                ++_stats.Str;
                break;
            case PossibleStats.Dex:
                ++_stats.Dex;
                break;
            case PossibleStats.Int:
                ++_stats.Int;
                break;
        }
        --_stats.LeftoverPoints;
        updatePointText();
        updateStatText();
        updatePersistentStats();
    }

    public void SubtractFromStat(int stat)
    {
        switch ((PossibleStats)stat)
        {
            case PossibleStats.Str:
                if (_stats.Str <= 0) return;
                --_stats.Str;
                break;
            case PossibleStats.Dex:
                if (_stats.Dex <= 0) return;
                --_stats.Dex;
                break;
            case PossibleStats.Int:
                if (_stats.Int <= 0) return;
                --_stats.Int;
                break;
        }
        ++_stats.LeftoverPoints;
        updatePointText();
        updateStatText();
        updatePersistentStats();
    }

    public void updatePointText()
    {
        _pointText.text = "Points left: " + _stats.LeftoverPoints;
    }

    public void updateStatText()
    {
        _strText.text = "Strength: " + _stats.Str;
        _dexText.text = "Dexterity: " + _stats.Dex;
        _intText.text = "Intelligence: " + _stats.Int;
    }

    public void updatePersistentStats()
    {
        PersistentData.Instance.Stats = _stats;
    }
}
