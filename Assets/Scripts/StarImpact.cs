using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarImpact : MonoBehaviour
{
    private Vector3 _toLookAt;
    private Vector3 _lookAtOffset = new Vector3(0, 6f, 0);

    public void PointAtPlayer(Transform player)
    {
        _toLookAt = player.position + _lookAtOffset;
        transform.LookAt(_toLookAt);
        //transform.rotation = new Quaternion(transform.rotation.x + _angleOffset, transform.rotation.y, transform.rotation.z, transform.rotation.w);
    }
}
