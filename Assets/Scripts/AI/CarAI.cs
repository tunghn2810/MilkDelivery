using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using static PathManager;
using static UnityEngine.InputSystem.UI.VirtualMouseInput;
using UnityEngine.InputSystem;

public class CarAI : MonoBehaviour
{
    [SerializeField] private PathNode _startNode;
    [SerializeField] private PathNode _cursorNode;
    [SerializeField] private PathNode _endNode;
    [SerializeField] private List<PathNode> _path = new List<PathNode>();
    private List<Vector3> _pathPositions = new List<Vector3>();
    private Dictionary<PathNode, int> _pathRepeats = new Dictionary<PathNode, int>();

    private const int MAX_NODES = 30;

    private int nextNodeIndex = 1;
    [SerializeField] private Vector3 _to;

    private bool _arrived = false;
    private bool _isMoving = true;

    //AI Control
    private Rigidbody _rgbd;
    private CarController2 _carController2;
    private bool _isStuck = false;
    private float _stuckTimer = 0;
    private const float STUCK_TIME = 4.0f;
    private float _reverseTimer = 0;
    private const float REVERSE_TIME = 2.0f;

    private void Awake()
    {
        _rgbd = GetComponent<Rigidbody>();
        _carController2 = GetComponent<CarController2>();
    }

    private void Start()
    {
        _cursorNode = _startNode;
        _path.Add(_cursorNode);
        _pathRepeats.Add(_cursorNode, 0);
        GetPath();
        NextNode();
    }

    private void Update()
    {
        if (_carController2.CurrentSpeed < 1f)
        {
            _stuckTimer += Time.deltaTime;
            if (_stuckTimer >= STUCK_TIME)
            {
                _isStuck = true;
            }
        }

        Drive();
        if (!_arrived)
        {
            if (Vector3.Distance(transform.position, _to) <= 30f)
            {
                nextNodeIndex++;
                NextNode();
            }
        }

        if (!_isMoving)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                ResetPath();
        }
    }

    private void GetPath()
    {
        int nodeIteration = 0;
        while (_cursorNode != _endNode && nodeIteration < MAX_NODES)
        {
            PathNode nextNode = _cursorNode.ConnectedNodes[0];

            if (!_pathRepeats.ContainsKey(nextNode))
            {
                _pathRepeats.Add(nextNode, 0);
            }

            float lowestWeight = CalculateWeight(nextNode);

            for (int i = 1; i < _cursorNode.ConnectedNodes.Length; i++)
            {
                if (!_pathRepeats.ContainsKey(_cursorNode.ConnectedNodes[i]))
                {
                    _pathRepeats.Add(_cursorNode.ConnectedNodes[i], 0);
                }
                float tempWeight = CalculateWeight(_cursorNode.ConnectedNodes[i]);
                if (tempWeight < lowestWeight)
                {
                    lowestWeight = tempWeight;
                    nextNode = _cursorNode.ConnectedNodes[i];
                }
            }

            _path.Add(nextNode);
            _pathRepeats[nextNode]++;
            _cursorNode = nextNode;

            nodeIteration++;

            if (nodeIteration == MAX_NODES)
            {
                _endNode = _cursorNode;
            }
        }

        for (int i = 0; i < _path.Count; i++)
        {
            _pathPositions.Add(new Vector3(_path[i].transform.position.x, transform.position.y, _path[i].transform.position.z));
        }
    }

    private float CalculateWeight(PathNode node)
    {
        float distanceToCursor = node.GetDistanceToNode(_cursorNode);
        float distanceToEnd = node.GetDistanceToNode(_endNode);
        float iterationWeight = _pathRepeats[node] * 300f;
        return distanceToCursor + distanceToEnd + iterationWeight;
    }

    private void ResetPath()
    {
        _startNode = _endNode;
        _cursorNode = _startNode;
        _endNode = I_PathManager.GetRandomNode();
        while(_endNode == _cursorNode)
        {
            _endNode = I_PathManager.GetRandomNode();
        }

        _path.Clear();
        _pathPositions.Clear();
        nextNodeIndex = 1;

        _path.Add(_cursorNode);
        GetPath();
        NextNode();

        _arrived = false;
        _isMoving = true;
    }

    private void NextNode()
    {
        if (nextNodeIndex == _path.Count)
        {
            _arrived = true;
            Debug.Log("Arrived!");

            _isMoving = false;
        }
        else
        {
            _to = _pathPositions[nextNodeIndex];
        }
    }

    private void Drive()
    {
        if (_isStuck)
        {
            Accelerate(new Vector2(0, -1));
            Steer(new Vector2(-1, 0));
            _reverseTimer += Time.deltaTime;
            if (_reverseTimer >= REVERSE_TIME)
            {
                _isStuck = false;
                _stuckTimer = 0;
                _reverseTimer = 0;
            }
        }
        else
        {
            float forwardDotProd = Vector3.Dot(transform.forward, (_to - transform.position).normalized);
            float rightDotProd = Vector3.Dot(transform.right, (_to - transform.position).normalized);

            Steer(new Vector2(rightDotProd, 0));

            if (!_arrived)
            {
                Accelerate(new Vector2(0, 1));
                Brake(false);
            }
            else
            {
                Accelerate(new Vector2(0, 0));
                Brake(true);
            }
        }
    }

    public void Accelerate(Vector2 accelDirection)
    {
        _carController2?.Accelerate(accelDirection);
    }

    public void Steer(Vector2 steerDirection)
    {
        _carController2?.Steer(steerDirection);
    }

    public void Brake(bool isBraking)
    {
        _carController2?.Brake(isBraking);
    }
}
