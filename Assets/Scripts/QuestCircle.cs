using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestCircle : MonoBehaviour
{
    //Reference
    private MeshRenderer _circleMesh;


    //Properties
    [SerializeField] private Sprite _buildingSprite;
    public Sprite BuildingSprite { get => _buildingSprite; }
	[SerializeField] private string _locationName;
    public string LocationName { get => _locationName; }


    //Quest requirements
    private Quest _currentQuest;
    public Quest CurrentQuest { get => _currentQuest; set => _currentQuest = value; }
    private int _remaining1;
    private int _remaining2;


    //For testing only
    [SerializeField] private float _money;
    [SerializeField] private float _time;


    private void Awake()
    {
        _circleMesh = GetComponent<MeshRenderer>();
        DeactivateQuest();
    }

    private void Update()
    {
        if (_currentQuest != null)
        {
            _money = _currentQuest.Money;
            _time = _currentQuest.BonusTime;
        }
    }

    public void ActivateQuest(Quest quest)
    {
        _circleMesh.enabled = true;
        _currentQuest = quest;
    }

    public void DeactivateQuest()
    {
        _circleMesh.enabled = false;
        _currentQuest = null;
    }

    private void QuestCheck()
    {
        if (_remaining1 <= 0)
        {
            _currentQuest.Num1 = 0;
            if (_currentQuest.Bread2 != null)
            {
                if (_remaining2 <= 0)
                {
                    _currentQuest.Num2 = 0;
                    _currentQuest.IsDone = true;
                    //Successfully submitted quest
                }
                else
                {
                    _currentQuest.Num2 = _remaining2;
                    //Incomplete quest
                }

            }
            else
            {
                _currentQuest.IsDone = true;
                //Successfully submitted quest
            }
        }
        else
        {
            _currentQuest.Num1 = _remaining1;
            //Incomplete quest
        }
    }


    private void OnTriggerStay(Collider collider)
    {
        if (collider.GetComponent<BreadContainer>().QuestReady && _currentQuest != null)
        {
            _remaining1 = collider.GetComponent<BreadContainer>().InventoryCheck(_currentQuest.Name1, _currentQuest.Num1);

            if (_currentQuest.Bread2 != null)
            {
                _remaining2 = collider.GetComponent<BreadContainer>().InventoryCheck(_currentQuest.Name2, _currentQuest.Num2);
            }

            QuestCheck();
        }
    }
}
