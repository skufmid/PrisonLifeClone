using System;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private int money = 10;
    public int Money
    {
        get => money;
        private set
        {
            if (money == value) return;
            money = Mathf.Max(0, value);
            OnMoneyChanged?.Invoke(money);
            if (isFirstGetMoney && money > 0)
            {
                isFirstGetMoney = false;
                onFirstGetMoney?.Invoke();
            }
        }
    }

    bool isFirstGetMoney = true;
    [SerializeField] private UnityEvent onFirstGetMoney;
    public event Action<int> OnMoneyChanged;

    public const int MoneyPerCarriable = 5;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Init();
    }

    private void Init()
    {
        Money = 0;
    }

    public void PlusMoney(int value)
    {
        Money += value;
    }

    public void UseMoney(int value)
    {
        Money -= value;
    }

    public void AddMoneyCarriable(int count = 1)
    {
        if (count <= 0) return;
        Money += count * MoneyPerCarriable;
    }

    public void RemoveMoneyCarriable(int count = 1)
    {
        if (count <= 0) return;
        Money -= count * MoneyPerCarriable;
    }
}