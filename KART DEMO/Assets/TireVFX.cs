using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireVFX : MonoBehaviour
{

    [SerializeField] Transform leftFrontTire, rightFrontTire, leftBackTire, rightBackTire;
    float horizontal, vertical;

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (horizontal < 0) {
            leftFrontTire.localEulerAngles = Vector3.Lerp(leftFrontTire.localEulerAngles, new Vector3(0, 55, 0), 5 * Time.deltaTime);
            rightFrontTire.localEulerAngles = Vector3.Lerp(rightFrontTire.localEulerAngles, new Vector3(0, 55, 0), 5 * Time.deltaTime);
        } else if (horizontal > 0) {
            leftFrontTire.localEulerAngles = Vector3.Lerp(leftFrontTire.localEulerAngles, new Vector3(0, 125, 0), 5 * Time.deltaTime);
            rightFrontTire.localEulerAngles = Vector3.Lerp(rightFrontTire.localEulerAngles, new Vector3(0, 125, 0), 5 * Time.deltaTime);
        } else {
            leftFrontTire.localEulerAngles = Vector3.Lerp(leftFrontTire.localEulerAngles, new Vector3(0, 90, 0), 5 * Time.deltaTime);
            rightFrontTire.localEulerAngles = Vector3.Lerp(rightFrontTire.localEulerAngles, new Vector3(0, 90, 0), 5 * Time.deltaTime);
        }
    }
}
