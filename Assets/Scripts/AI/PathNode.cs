using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    [SerializeField] private PathNode[] _connectedNodes;
    [SerializeField] private float[] _distanceToOthers;

    private Dictionary<PathNode, float> _distanceDict;

    private void Awake()
    {
        _distanceToOthers = new float[_connectedNodes.Length];
    }

    private void CalculateDist()
    {
        for (int i = 0; i < _connectedNodes.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, _connectedNodes[i].transform.position);
            _distanceToOthers[i] = distance;
            _distanceDict.Add(_connectedNodes[i], distance);
        }

        Array.Sort(_distanceToOthers);
    }

    private PathNode GetShortestPath(PathNode exclude)
    {
        int index = 0;
        foreach (var pair in _distanceDict)
        {
            if (pair.Value == _distanceToOthers[index])
            {
                if (pair.Key == exclude)
                {
                    index++;
                }
                else
                {
                    return pair.Key;
                }
            }
        }
        return null;
    }
}
