using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    //Reference
    private PathNode[] _pathNodes;

    public static PathManager I_PathManager { get; set; }
    private void Awake()
    {
        if (I_PathManager == null)
        {
            I_PathManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameObject[] _pathNodeObjects = GameObject.FindGameObjectsWithTag("PathNode");
        _pathNodes = new PathNode[_pathNodeObjects.Length];
        for (int i = 0; i < _pathNodeObjects.Length; i++)
        {
            _pathNodes[i] = _pathNodeObjects[i].GetComponent<PathNode>();
        }
    }

    public PathNode GetRandomNode()
    {
        int rnd = Random.Range(0, _pathNodes.Length);
        return _pathNodes[rnd];
    }
}
