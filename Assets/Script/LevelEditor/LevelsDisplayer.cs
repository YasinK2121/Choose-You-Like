using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "LevelEditor", order = 1)]
public class LevelsDisplayer : ScriptableObject 
{
	public List<LevelData> LevelAndStepList =  new List<LevelData>(5);
}
