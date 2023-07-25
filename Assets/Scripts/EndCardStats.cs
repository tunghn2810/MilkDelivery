using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndCardStats : MonoBehaviour
{
    //Text reference
    [SerializeField] private TMP_Text[] _breadText;
    [SerializeField] private TMP_Text _moneyText;
    [SerializeField] private TMP_Text _timeText;


    //Numbers
    private int[] _breadNum = { 0, 0, 0, 0 }; //Baguette Croissant CrossBun Donut
    private float _moneyNum = 0f;
    private float _timeNum = 0f;


    //Singleton
    public static EndCardStats I_EndCardStats { get; set; }
    private void Awake()
    {
        if (I_EndCardStats == null)
        {
            I_EndCardStats = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateStats()
    {
        for (int i = 0; i < _breadText.Length; i++)
        {
            _breadText[i].text = "x" + _breadNum[i].ToString();
        }

        _moneyNum = MoneyManager.I_MoneyManager.CurrentMoney;
        _moneyText.text = "Money: " + _moneyNum.ToString();

        _timeNum += GameTimer.I_GameTimer.TotalTime;
        _timeText.text = "Time Played: " + DisplayTime(_timeNum);
    }

    private string DisplayTime(float totalTime)
    {
        float minutes = Mathf.FloorToInt(totalTime / 60f);
        float seconds = Mathf.FloorToInt(totalTime % 60f);
        float ticks = Mathf.FloorToInt((totalTime * 100f) % 100f);

       return string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, ticks);
    }

    public void AddBread(int breadType)
    {
        _breadNum[breadType]++;
    }

    public void AddTime(float time)
    {
        _timeNum += time;
    }

    public void ResetEndStats()
    {
        for (int i = 0; i < _breadNum.Length; i++)
        {
            _breadNum[i] = 0;
        }
        _moneyNum = 0f;
        _timeNum = 0f;
    }
}
