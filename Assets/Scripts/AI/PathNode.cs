using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathNode : MonoBehaviour
{
    [SerializeField] private PathNode[] _connectedNodes;
    public PathNode[] ConnectedNodes { get { return _connectedNodes; } }
    [SerializeField] private float[] _distanceToAdjacents;

    private Dictionary<PathNode, float> _distanceDict = new Dictionary<PathNode, float>();

    [SerializeField] private float _distanceToEnd;

    private void Awake()
    {
        _distanceToAdjacents = new float[_connectedNodes.Length];
        CalculateDist();
    }

    private void CalculateDist()
    {
        for (int i = 0; i < _connectedNodes.Length; i++)
        {
            float distance = Vector3.Distance(transform.position, _connectedNodes[i].transform.position);
            _distanceToAdjacents[i] = distance;
            _distanceDict.Add(_connectedNodes[i], distance);
        }
        Array.Sort(_distanceToAdjacents);
    }

    public PathNode GetShortestPath(int index)
    {
        foreach (var pair in _distanceDict)
        {
            if (pair.Value == _distanceToAdjacents[index])
            {
                return pair.Key;
            }
        }
        return null;
    }

    public float GetDistanceToNode(PathNode node)
    {
        return Vector3.Distance(transform.position, node.transform.position);
    }
}
