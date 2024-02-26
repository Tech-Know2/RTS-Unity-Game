using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Object", menuName = "Table Top Data/Object")]
public class Object : ScriptableObject
{
    public string objectName;
    public int turnsToProduce;

    //Is Religious
    public bool isReligious;
    public string religionColor;

}