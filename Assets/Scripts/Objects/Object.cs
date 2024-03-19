using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "Table Top Data/Object")]
public class Object : ScriptableObject
{
    //General Information
    public string objectName;
    public int turnsToProduce;

    //Is Religious
    public bool isReligiousFollower;
    public string religionColor;
    public Religion followerReligion;

}