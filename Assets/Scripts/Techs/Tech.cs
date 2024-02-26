using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tech", menuName = "Table Top Data/Tech")]
public class Tech : ScriptableObject
{
    public string techName;
    public string description;
    public string techType;
    public Color techColor;
    public int techEra;

    public bool isResearched = false;

    //public List<Button> techButtons = new List<Button>();
    public List<Card> techCards = new List<Card>();
}
