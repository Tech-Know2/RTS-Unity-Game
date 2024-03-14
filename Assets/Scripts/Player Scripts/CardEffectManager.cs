using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEffectManager : MonoBehaviour
{
    //Script Connections
    public Player playerScript;
    public ReligionEffectManager religionEffectManager;

    //Effect Lists
    private List<EffectManagerList> singleUseCards = new List<EffectManagerList>();
    private List<EffectManagerList> multiUseCards = new List<EffectManagerList>();
    private List<EffectManagerList> foreverCards = new List<EffectManagerList>();

    //Effects that take place every interval
    public void EffectsPerInterval()
    {
        //Forevercards
        //Call these effect cards, then remove the "dead" ones
        UpdateCardLists();

        foreach (EffectManagerList effect in multiUseCards)
        {
            CardEffectExecuter(effect, null);
        }

        foreach (EffectManagerList effect in foreverCards)
        {
            CardEffectExecuter(effect, null);
        }
    }

    //Effects from a single card (sorting, effecting, saving, etc)
    public void NewCardEffectSorter(Card passedCard, GameObject tile)
    {
        if(passedCard.isReligiousCard == false) //Cards is not religious
        {
            List<EffectManagerList> effectList = new List<EffectManagerList>();
            effectList = passedCard.effectManagerList;

            if(passedCard.governmentRelated == true)
            {
                ManageGovernmentEffects(passedCard);
            }

            if(passedCard.createAReligion == true)
            {
                CreateAReligion();
            }

            //Determine list for interval placement (forever, once, multi)
            for(int i = 0; i < effectList.Count; i++)
            {
                EffectManagerList effect = effectList[i];

                if(effect.goesForever == true)
                {
                    foreverCards.Add(effect);
                    //Call effect executer
                    CardEffectExecuter(effect, passedCard);

                } else if (effect.requiresMultipleTurns == true)
                {
                    //Effect for multiple turns
                    multiUseCards.Add(effect);
                    //Call effect executer
                    CardEffectExecuter(effect, passedCard);

                } else //Single turn
                {
                    //Call effect executer
                    CardEffectExecuter(effect, passedCard);
                }
            }
        } else //Card is religious
        {
            List<RelgiousEffectsList> religiousEffectList = new List<RelgiousEffectsList>();
            religiousEffectList = passedCard.religoiusEffectLists;

            for (int i = 0; i < religiousEffectList.Count; i++)
            {
                RelgiousEffectsList effect = religiousEffectList[i];

                ManageReligiousEffects(effect);
            }
        }
    }

    public void CardEffectExecuter(EffectManagerList effect, Card passedCard)
    {
        string costType = effect.costType;
        effect.turnsActive++;

        if(costType == "Gold Cost")
        {
            GoldCost(effect);

        } else if (costType == "Silver Cost")
        {
            SilverCost(effect);

        } else if (costType == "Loyalty Cost")
        {
            LoyaltyCost(effect);

        } else if (costType == "Food Cost")
        {
            FoodCost(effect);

        } else if (costType == "People Cost")
        {
            PeopleCost(effect);

        } else if (costType == "War Propaganda Cost")
        {
            WarPropagandaCost(effect);

        } else if (costType == "Religion Cost")
        {
            ReligionCost(effect);

        } else if (costType == "Weariness Cost")
        {
            WearinessCost(effect);

        } else if (costType == "Spread Distrust Cost")
        {
            DistrustCost(effect);

        } else if (costType == "Tech Cost")
        {
            TechCost(effect);
        }
    }

    // Check to see if current multiuse cards should continue to take effect, or should be removed
    public void UpdateCardLists()
    {
        for (int i = multiUseCards.Count - 1; i >= 0; i--)
        {
            if (multiUseCards[i].turnsActive > multiUseCards[i].turnEffectLength)
            {
                // Remove the card
                multiUseCards.RemoveAt(i);
            }
            else
            {
                break;
            }
        }
    }

    //Create A religion
    public void CreateAReligion()
    {
        Religion religion = new Religion();

        Religion newReligion = religionEffectManager.CreateAReligion(religion);

        playerScript.createdAReligion = true;

        //Assign the religion to the player's empire based on the tile the card was played on
    }

    //List of Standard Effects
    public void GoldCost(EffectManagerList effect)
    {
        if(effect.usesChance == true)
        {
            if(RNG() == true)
            {
                playerScript.gameManager.playerGold += effect.effectCost;
            } else 
            {
                playerScript.gameManager.playerGold -= effect.effectCost;
            }

        } else 
        {
            playerScript.gameManager.playerGold += effect.effectCost;
        }        
    }

    public void SilverCost(EffectManagerList effect)
    {
        
    }

    public void LoyaltyCost(EffectManagerList effect)
    {
        
    }

    public void FoodCost(EffectManagerList effect)
    {
        //Needs a city
    }

    public void PeopleCost(EffectManagerList effect)
    {
        //Needs a city
    }

    public void WarPropagandaCost(EffectManagerList effect)
    {
        
    }

    public void ReligionCost(EffectManagerList effect)
    {

    }

    public void WearinessCost(EffectManagerList effect)
    {
        //Needs a city
    }

    public void DistrustCost(EffectManagerList effect)
    {
        //Needs an opposing civ
    }

    public void TechCost(EffectManagerList effect)
    {

    }

    //List of Government Effects
    public void ManageGovernmentEffects(Card passedCard)
    {
        if(passedCard.assignsGovernment == true)
        {
            AssignsGovernment();

        } else if (passedCard.isGovernmentWarSupportCard == true)
        {
            GovernmentWarSupportEffect();

        } else if (passedCard.isAllianceCard == true)
        {
            AllianceCard();

        } else if (passedCard.isDeclareWarCard == true)
        {
            AllianceCard();

        } else if (passedCard.isPeaceTreatyCard == true)
        {
            PeaceTreaty();
        }
    }

    public void AssignsGovernment()
    {
        //Needs a government
    }

    public void GovernmentWarSupportEffect()
    {
        //Needs an opposing civ
    }

    public void AllianceCard()
    {
        //Needs an opposing civ
    }

    public void WarDecleration()
    {
        //Needs an opposing civ
    }

    public void PeaceTreaty()
    {
        //Needs an opposing civ
    }

    //Manage Religious Effects
    public void ManageReligiousEffects(RelgiousEffectsList effect)
    {
        if(effect.isPlayersReligion == true)
        {
            effect.religion = religionEffectManager.playerReligion;

            if(effect.isDedication == true && religionEffectManager.CanDedicate() == true)
            {
                DedicationManager(effect);

            } else if (effect.enablesSuzerain == true)
            {
                SuzerainTypeController(effect);

            } else if (effect.suzerainEffect == true)
            {
                SuzerainEffectManager(effect);
                
            }else if (effect.needsModifierSetup)
            {
                //Do later

            } else if (effect.religiousEffect == "Mono or Poly" && religionEffectManager.CanBecomeGods() == true)
            {
                MonoOrPoly(effect);

            } else if (effect.religiousEffect == "Educated Freedoms")
            {
                EducatedFreedoms(effect);
                
            } else if (effect.religiousEffect == "Religious Freedoms")
            {
                ReligousFreedoms(effect);
                
            } else if (effect.religiousEffect == "Holy War")
            {
                HolyWar(effect);
                
            } else if (effect.religiousEffect == "Economic Yield")
            {
                EconomicYield(effect);
                
            } else if (effect.religiousEffect == "Religious Expansion")
            {
                ReligiousExpansion(effect);
                
            } else if (effect.religiousEffect == "Religious Safety")
            {
                ReligiousSafety(effect);
            }
        }

        
    }

    public void SuzerainTypeController(RelgiousEffectsList effect)
    {
        Religion religion = effect.religion;

        if(effect.religiousEffect == "Allow Religious Suzerain")
        {
            if(RNG() == true && religionEffectManager.CanBecomeASuzerain() == true)
            {
                religionEffectManager.playerReligion.canReligiousSuzerain = true;
                SetUpSuzerainEffectModifier();

            } else 
            {
                religionEffectManager.playerReligion.canReligiousSuzerain = false;
            }
            

        } else if (effect.religiousEffect == "Allow Economic Suzerain" && religionEffectManager.CanDoSuzerains() == true)
        {
            if(RNG() == true)
            {
                religionEffectManager.playerReligion.canEconomicSuzerain = true;

            } else 
            {
                religionEffectManager.playerReligion.canEconomicSuzerain = false;
            }

        } else if (effect.religiousEffect == "Allow Academic Suzerain" && religionEffectManager.CanDoSuzerains() == true)
        {
            if(RNG() == true)
            {
                religionEffectManager.playerReligion.canAcademicSuzerain = true;
            } else 
            {
                religionEffectManager.playerReligion.canAcademicSuzerain = false;
            }

        } else if (effect.religiousEffect == "Allow Politcal Suzerain" && religionEffectManager.CanDoSuzerains() == true)
        {
            if(RNG() == true)
            {
                religionEffectManager.playerReligion.canPoliticallySuzerain = true;
            } else 
            {
                religionEffectManager.playerReligion.canPoliticallySuzerain = false;
            }            
        }
    }

    public void SuzerainEffectManager(RelgiousEffectsList effect)
    {
        if(effect.suzerainEffectType == "Academic")
        {
            //Effects here

        } else if (effect.suzerainEffectType == "Economic")
        {
            //Effects here

        } else if (effect.suzerainEffectType == "Political")
        {
            //Effects here
        }
    }

    public void DedicationManager(RelgiousEffectsList effect)
    {
        if(effect.religiousEffect == "Stable Dedication")
        {
            

        } else if (effect.religiousEffect == "Stable Dedication")
        {


        } else if (effect.religiousEffect == "Economic Dedication")
        {


        } else if (effect.religiousEffect == "Harvest Dedication")
        {


        } else if (effect.religiousEffect == "Religious Dedication")
        {


        } else if (effect.religiousEffect == "Academic Dedication")
        {

        }
    }

    //Religious Effects
    public void MonoOrPoly(RelgiousEffectsList effect)
    {
        Religion religion = effect.religion;

        if(RNG() == true) //Mono
        {
            religion.monotheism = true;
            religion.polytheism = false;

        } else //Poly
        {
            religion.polytheism = true;
            religion.monotheism = false;
        }
    }

    public void EducatedFreedoms(RelgiousEffectsList effect)
    {
        Religion religion = effect.religion;

        if(RNG() == true)
        {
            religion.educatedFreedoms = true;

        } else
        {
            religion.educatedFreedoms = false;
            religion.canHolyWars = true;
        }
    }

    public void ReligousFreedoms(RelgiousEffectsList effect)
    {
        Religion religion = effect.religion;

        if(RNG() == true)
        {
            religion.religionFreedoms = true;

        } else
        {
            religion.religionFreedoms = false;
        }
    }

    public void HolyWar(RelgiousEffectsList effect)
    {
        Religion religion = effect.religion;
    }

    public void ReligiousExpansion(RelgiousEffectsList effect)
    {
        Religion religion = effect.religion;
    }

    public void ReligiousSafety(RelgiousEffectsList effect)
    {
        Religion religion = effect.religion;
    }

    public void EconomicYield(RelgiousEffectsList effect)
    {
        Religion religion = effect.religion;
    }

    //Setup for chance and math related things
    public bool RNG()
    {
        int rng = Random.Range(0, 10);
        return rng < 5;
    }

    public void SetUpSuzerainEffectModifier()
    {
        if(religionEffectManager.playerReligion.suzerainEffectModifier == 0)
        {
            religionEffectManager.suzerainEffectModifierCalculator();
        }
    }
}
