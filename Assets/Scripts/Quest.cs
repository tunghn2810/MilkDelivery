using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Quest
{
	private Sprite _bread1;
    private string _name1;
    private int _num1;

    private Sprite _bread2;
    private string _name2;
    private int _num2;

    private QuestCircle _questCircle;
    private Sprite _building;
    private string _location;
    private Vector3 _position;
    
    private bool _isDone;

    private float _money;
    private float _bonusTime;

    public Sprite Bread1 { get => _bread1; set => _bread1 = value; }
    public string Name1 { get => _name1; set => _name1 = value; }
    public int Num1 { get => _num1; set => _num1 = value; }
    public string Name2 { get => _name2; set => _name2 = value; }
    public Sprite Bread2 { get => _bread2; set => _bread2 = value; }
    public int Num2 { get => _num2; set => _num2 = value; }
    public QuestCircle QuestCircle { get => _questCircle; set => _questCircle = value; }
    public Sprite Building { get => _building; }
    public string Location { get => _location; }
    public Vector3 Position { get => _position; }
    public bool IsDone { get => _isDone; set => _isDone = value; }
    public float Money { get => _money; }
    public float BonusTime { get => _bonusTime; }

    public void Setup()
    {
        _building = _questCircle.BuildingSprite;
        _location = _questCircle.LocationName;
        _position = _questCircle.transform.position;

        _money = QuestManager.I_QuestManager.CalculateRewards(this);
        _bonusTime = _money / 3f; //Arbitrary multiplier
    }
}
