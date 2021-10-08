using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Example : MonoBehaviour
{
    void Start()
    {
        // Sets the rotation so that the transform's y-axis goes along the z-axis
    }

    private void Update()
    {
        transform.rotation = Quaternion.FromToRotation(Vector3.up, transform.forward);
    }
}