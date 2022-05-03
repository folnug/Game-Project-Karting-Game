using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSelection", menuName = "KART DEMO/CharacterSelection/CharacterSelection", order = 2)]
public class CharacterSelection : ScriptableObject
{
    public KartModel[] characters;
    public int playerCharacterIndex;
    public int maxlaps = 3;
    public KartController.KartDriftModes playerDriftMode = KartController.KartDriftModes.Normal;
    public TrackManager.GameModes gameMode = TrackManager.GameModes.Arcade;

}
