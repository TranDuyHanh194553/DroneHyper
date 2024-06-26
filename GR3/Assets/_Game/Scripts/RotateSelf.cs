using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSelf : MonoBehaviour
{
    [SerializeField] Transform tf;
    [SerializeField] float angle = -6f;
    [SerializeField] private bool ver, hor;

    private void LateUpdate()
    {
        if (ver && !hor)
        {
            tf.Rotate(Vector3.up * angle, Space.Self);
        }
        else if (!ver && hor)
        {
            tf.Rotate(Vector3.right * angle, Space.Self);
        }
    }
}
