using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BreadContainer : MonoBehaviour
{
    [SerializeField] private RectTransform[] _inventory;

    [SerializeField] private bool _questReady = false;
    public bool QuestReady { get => _questReady; set => _questReady = value; }

    private string[] _breadTypes = { "Baguette", "Croissant", "CrossBun", "Donut" };

    //For testing only
    [SerializeField] private List<GameObject> _baguette = new List<GameObject>();
    [SerializeField] private List<GameObject> _croissant = new List<GameObject>();
    [SerializeField] private List<GameObject> _crossBun = new List<GameObject>();
    [SerializeField] private List<GameObject> _donut = new List<GameObject>();

    private List<GameObject>[] _breads = new List<GameObject>[4];

    private List<GameObject> _toBeDestroyed = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < _breads.Length; i++)
        {
            _breads[i] = new List<GameObject>();
        }
    }

    private void Update()
    {
        _baguette = _breads[0];
        _croissant = _breads[1];
        _crossBun = _breads[2];
        _donut = _breads[3];

        for (int i = 0; i < _inventory.Length; i++)
        {
            _inventory[i].GetComponent<TMP_Text>().text = "x" + _breads[i].Count.ToString();
        }
    }

    public int InventoryCheck(string bread, int num)
    {
        for (int i = 0; i < _breadTypes.Length; i++)
        {
            if (_breadTypes[i] == bread)
            {
                int questRemaining = num - _breads[i].Count;
                if (questRemaining <= 0) //Enough in container, remove exact amount
                {
                    for (int j = 0; j < num; j++)
                    {
                        //_breads[i].Remove(_breads[i][j]);
                        EndCardStats.I_EndCardStats.AddBread(i);
                        _toBeDestroyed.Add(_breads[i][j]);
                    }
                }
                else //Not enough, remove all
                {
                    for (int j = 0; j < _breads[i].Count; j++)
                    {
                        //_breads[i].Remove(_breads[i][j]);
                        EndCardStats.I_EndCardStats.AddBread(i);
                        _toBeDestroyed.Add(_breads[i][j]);
                    }
                }

                int index = _toBeDestroyed.Count - 1;
                while (_toBeDestroyed.Count > 0)
                {
                    GameObject destroyedObj = _toBeDestroyed[index];
                    _breads[i].Remove(destroyedObj);
                    _toBeDestroyed.Remove(destroyedObj);
                    Destroy(destroyedObj);
                    index--;
                }

                return questRemaining;
            }
        }

        Debug.Log("Something wrong!");
        return 0;
    }

    private void OnTriggerEnter(Collider collider)
    {
        for (int i = 0; i < _breadTypes.Length; i++)
        {
            if (_breadTypes[i] == collider.tag)
            {
                _breads[i].Add(collider.transform.root.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        for (int i = 0; i < _breadTypes.Length; i++)
        {
            if (_breadTypes[i] == collider.tag)
            {
                _breads[i].Remove(collider.transform.root.gameObject);
            }
        }
    }
}
