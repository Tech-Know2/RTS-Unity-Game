using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReligionEffectManager : MonoBehaviour
{
    public Religion playerReligion;
    public Player playerScript;
    public NameCreator nameCreator;

    public Religion CreateAReligion(Religion newReligion)
    {
        playerReligion = newReligion;
        playerReligion.religionName = playerScript.empireManager.nameCreator.GenerateReligionName();
        playerReligion.religionColor = playerScript.playerColor;

        playerScript.createdAReligion = true;

        return playerReligion;
    }

    //Check to see if the empire can make dedications
    public bool CanDedicate()
    {
        if(playerReligion.dedicationStatusNotSet == true)
        {
            playerReligion.dedicationStatusNotSet = false;

            return true;
        } else 
        {
            return false;
        }
    }

    //Check to see if Suzerain is possible
    public bool CanDoSuzerains()
    {
        return playerReligion.canReligiousSuzerain;
    }

    public bool CanBecomeASuzerain()
    {
        if(playerReligion.suzerainStatusNotSet == true)
        {
            playerReligion.suzerainStatusNotSet = false;

            return true;
        } else 
        {
            return false;
        }
        
    }

    //Check if religion already has a type
    public bool CanBecomeGods()
    {
        if(playerReligion.monotheism == true || playerReligion.polytheism == true)
        {
            return false;

        } else 
        {
            return true;
        }
    }

    //Work on later
    public float DiploCalc()
    {
        if(playerReligion)
        {
            return 1f;
        }

        return 1;
    }

    public void suzerainEffectModifierCalculator()
    {
        playerReligion.suzerainEffectModifier = UnityEngine.Random.Range(0.01f, 0.5f);
    }
}
