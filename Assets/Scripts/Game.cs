using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    public string Name;
    public int health = 100;
    public long gold = 0;
    public int goldBonus = 0;
    public int damageReduction = 0;
    public int healingBonus = 0;

    private TextMeshProUGUI _hpText;
    private TextMeshProUGUI _goldText;
    private TextMeshProUGUI _hpBonusText;
    private TextMeshProUGUI _goldBonusText;
    private TextMeshProUGUI _healingBonusText;
    private TextMeshProUGUI _nameText;

    private int _maxHealth = 100;
    private SwitchScene _sceneSwitcher;

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
    }

    private void Start()
    {
        FindInitialValues();
        InitializeVariables();
        InitializeText();
    }

    private void FindInitialValues()
    {
        health = PersistentData.Instance.HP;
        gold = PersistentData.Instance.Gold;
        _hpText ??= GameObject.Find("HPText").GetComponent<TextMeshProUGUI>();
        _goldText ??= GameObject.Find("GoldText").GetComponent<TextMeshProUGUI>();
        _hpBonusText ??= GameObject.Find("HPBonusText").GetComponent<TextMeshProUGUI>();
        _goldBonusText ??= GameObject.Find("GoldBonusText").GetComponent<TextMeshProUGUI>();
        _healingBonusText ??= GameObject.Find("HealingBonusText").GetComponent<TextMeshProUGUI>();
        _nameText ??= GameObject.Find("NameText").GetComponent<TextMeshProUGUI>();
        _sceneSwitcher = GetComponent<SwitchScene>();
    }

    private void InitializeVariables()
    {
        PlayerStats stats = PersistentData.Instance.Stats;
        damageReduction = stats.Str / 5;
        goldBonus = stats.Dex;
        healingBonus = stats.Int;
        Name = PersistentData.Instance.Name;
    }

    private void InitializeText()
    {
        _nameText.text = "Name: " + Name;
        _hpText.text = "HP: " + health;
        _goldText.text = "Gold: " + gold;
        _hpBonusText.text = "Damage reduction: " + damageReduction;
        _goldBonusText.text = "Gold bonus: " + goldBonus;
        _healingBonusText.text = "Healing bonus: " + healingBonus;
    }

    public void Damage(bool heal)
    {
        if (heal)
        {
            int healAmount = Random.Range(1 + healingBonus, 6 + healingBonus);

            if (health + healAmount > _maxHealth)
            {
                health = _maxHealth;
            }
            else
                health += healAmount;
        }
        else
        {
            int damage = Random.Range(1, 6);
            int earned = Random.Range(1 + goldBonus, 10 + goldBonus);
            damage -= damageReduction;

            if (health - damage <= 0)
            {
                health = 0;
                PersistentData.Instance.ClearJsonSave();
                //PersistentData.Instance.DeleteStoreData();
                _sceneSwitcher.SceneByOffset(1);
            }
            else
            {
                if (damage > 0)
                {
                    health -= damage;
                }
            }
            gold += earned;
        }
        _hpText.text = "HP: " + health;
        _goldText.text = "Gold: " + gold;
    }
}
