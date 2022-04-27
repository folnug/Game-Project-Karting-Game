using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PositionUI : MonoBehaviour
{
    [SerializeField] Text position;
    [SerializeField] Text racername;

    public void SetValues(string pos, string name) {
        position.text = pos;
        racername.text = name;
    }

}
