using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSelection", menuName = "KART DEMO/CharacterSelection", order = 2)]
public class CharacterSelection : ScriptableObject
{
    public KartModel[] characters;
    public int playerCharacterIndex;
}
