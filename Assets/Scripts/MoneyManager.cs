using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private GameObject _moneyText;

    private const float INITIAL_MONEY = 200f;
    private float _currentMoney = INITIAL_MONEY;
    public float CurrentMoney { get => _currentMoney; }

    //Singleton
    public static MoneyManager I_MoneyManager { get; set; }
    private void Awake()
    {
        if (I_MoneyManager == null)
        {
            I_MoneyManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        DisplayMoney();
    }

    public void DisplayMoney()
    {
        _moneyText.GetComponent<TMP_Text>().text = _currentMoney.ToString("n0");
    }

    public void IncreaseMoney(float amount)
    {
        _currentMoney += amount;
    }

    public bool DecreaseMoney(float amount)
    {
        if (_currentMoney < amount)
        {
            Debug.Log("Not enough money");
            return false;
        }
        _currentMoney -= amount;
        return true;
    }

    public void ResetMoney()
    {
        _currentMoney = INITIAL_MONEY;
    }
}
