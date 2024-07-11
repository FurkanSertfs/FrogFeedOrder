using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunAnim : MonoBehaviour
{
    [SerializeField]
    float speed = 1.0f;

    void Update()
    {
        transform.Rotate(Vector3.forward * speed * Time.deltaTime);
    }
}
