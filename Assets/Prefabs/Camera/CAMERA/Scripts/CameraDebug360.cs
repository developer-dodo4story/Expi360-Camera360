using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDebug360 : MonoBehaviour
{
    public static Transform targetToFollow;

    private void Update()
    {
        transform.LookAt(targetToFollow);
        transform.rotation = Quaternion.Euler(0f, transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
