using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour
{


    void Update()
    {
        transform.Rotate(Vector3.left / Random.Range(5, 15));
    }
}
