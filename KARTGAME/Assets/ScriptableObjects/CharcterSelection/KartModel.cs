using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "KartModel", menuName = "KART DEMO/CharacterSelection/KartModel", order = 1)]
public class KartModel : ScriptableObject
{
    public Sprite image;
    public string driverName;
    public string kartName;
    public GameObject kart;
}
