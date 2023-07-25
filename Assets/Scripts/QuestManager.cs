using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestManager : MonoBehaviour
{
	//Reference
	[SerializeField] private Sprite[] _breadSprites;
    [SerializeField] private string[] _breadTypes;
    private float[] _breadPrice = { 25f, 17f, 21f, 14f }; //Baguette Croissant CrossBun Donut
    [SerializeField] private QuestComplete _questCompletePanel;
    [SerializeField] private GameObject _bakery;
    [SerializeField] private GameObject _player;


    //Quest reference
    [SerializeField] private Transform[] _questBreads;
    [SerializeField] private Transform[] _questBuildings;
    [SerializeField] private Transform[] _questLocations;


    //Quest numbers
    private List<Quest> _questList = new List<Quest>();
	private int _maxQuest = 3;
	[SerializeField] private int _minBread;
    [SerializeField] private int _maxBread;
	[SerializeField] private List<QuestCircle> _buildingList;
    private List<QuestCircle> _currentBuildings = new List<QuestCircle>();


    //Singleton
	public static QuestManager I_QuestManager { get; set; }
    private void Awake()
    {
        if (I_QuestManager == null)
        {
            I_QuestManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateQuestVisuals();

        //Check for finished quests
        for (int i = 0; i < _questList.Count; i++)
        {
            if (_questList[i].IsDone)
            {
                //Rewards
                MoneyManager.I_MoneyManager.IncreaseMoney(_questList[i].Money);
                GameTimer.I_GameTimer.IncreaseTimer(_questList[i].BonusTime);
                GameUIManager.I_GameUIManager.QuestComplete();

                _currentBuildings.Remove(_questList[i].QuestCircle);
                RemoveQuest(_questList[i]);
                _questList[i] = GenerateQuest();
            }
        }
    }

    private int GetBreadType()
	{
		return Random.Range(0, _breadSprites.Length);
    }

	private int GetBreadNum()
	{
        return Random.Range(_minBread, _maxBread);
    }

	private int GetBuilding()
	{
        return Random.Range(0, _buildingList.Count);
    }

    private Quest GenerateQuest()
    {
        int bread1Type = GetBreadType();
        Sprite bread1 = _breadSprites[bread1Type];
        string name1 = _breadTypes[bread1Type];
        int num1 = GetBreadNum();

        Sprite bread2 = null;
        string name2 = "";
        int num2 = 0;
        int twoBreads = Random.Range(0, 100);
        if (twoBreads < 30)
        {
            int bread2Type = GetBreadType();
            bread2 = _breadSprites[bread2Type];
            while (bread2 == bread1)
            {
                bread2 = _breadSprites[GetBreadType()];
            }
            name2 = _breadTypes[bread2Type];
            num2 = GetBreadNum();
        }

        Quest newQuest = new Quest();

        newQuest.Bread1 = bread1;
        newQuest.Name1 = name1;
        newQuest.Num1 = num1;

        newQuest.Bread2 = bread2;
        newQuest.Name2 = name2;
        newQuest.Num2 = num2;

        newQuest.QuestCircle = _buildingList[GetBuilding()];
        while (_currentBuildings.Contains(newQuest.QuestCircle))
        {
            newQuest.QuestCircle = _buildingList[GetBuilding()];
        }
        _currentBuildings.Add(newQuest.QuestCircle);

        newQuest.Setup();

        newQuest.IsDone = false;

        newQuest.QuestCircle.ActivateQuest(newQuest);
        return newQuest;
	}

	private void UpdateQuestVisuals()
	{
		for (int i = 0; i < _questList.Count; i++)
		{
			Transform breadObject1 = _questBreads[i].GetChild(0);
			breadObject1.GetComponentInChildren<Image>().sprite = _questList[i].Bread1;
			breadObject1.GetComponentInChildren<TMP_Text>().text = "x" + _questList[i].Num1.ToString();

            Transform breadObject2 = _questBreads[i].GetChild(1);
            if (_questList[i].Bread2 == null)
			{
				breadObject2.gameObject.SetActive(false);
			}
			else
			{
                breadObject2.GetComponentInChildren<Image>().sprite = _questList[i].Bread2;
                breadObject2.GetComponentInChildren<TMP_Text>().text = "x" + _questList[i].Num2.ToString();
            }

			_questBuildings[i].GetComponent<Image>().sprite = _questList[i].Building;
			_questLocations[i].GetComponent<TMP_Text>().text = _questList[i].Location;
        }
	}

    private void RemoveQuest(Quest quest)
    {
        quest.QuestCircle.DeactivateQuest();
    }

    public float CalculateRewards(Quest quest)
    {
        float breadPrice1 = 0;
        float breadPrice2 = 0;
        for (int i = 0; i < _breadTypes.Length; i++)
        {
            if (quest.Name1 == _breadTypes[i])
            {
                breadPrice1 = _breadPrice[i] * quest.Num1;
            }
            else if (quest.Name2 == _breadTypes[i])
            {
                breadPrice2 = _breadPrice[i] * quest.Num2;
            }
        }

        float distanceFromBakery = (_bakery.transform.position - quest.Position).magnitude;
        float distanceFromPlayer = (_player.transform.position - quest.Position).magnitude;

        return breadPrice1 + breadPrice2 + (distanceFromBakery + distanceFromPlayer) / 5f; //Arbitrary multiplier
    }

    public void ResetQuests()
    {
        for (int i = 0; i < _questList.Count; i++)
        {
            RemoveQuest(_questList[i]);
        }
        _currentBuildings.Clear();
        _questList.Clear();


        for (int i = 0; i < _maxQuest; i++)
        {
            _questList.Add(GenerateQuest());
        }
    }
}
