using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BakeryScript : MonoBehaviour
{
    //Reference
    [SerializeField] private GameObject[] _breads;
    private float[] _breadPrice = { 25f, 17f, 21f, 14f }; //Baguette Croissant CrossBun Donut
    [SerializeField] private Transform _breadSpawnPos;

    public void CreateBread(int breadType)
    {
        if (MoneyManager.I_MoneyManager.DecreaseMoney(_breadPrice[breadType]))
            Instantiate(_breads[breadType], _breadSpawnPos.position, Quaternion.identity);
    }
}
