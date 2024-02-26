using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CardDealer : MonoBehaviour
{
    //Data Collection
    public List<Card> cards = new List<Card>();
    public List<Tech> techs = new List<Tech>();

    //Script Attachments
    private Player player;
    private UIController uiController;
    private GameManager gameManager;

    //Player Information
    private Government government;
    private bool hasGov = false;
    public List<string> researchTechs = new List<string>();
    public List<Card> dealtCards = new List<Card>(); //Cards dealt by dealer and now in the hands of the player

    //Card Lists
    public List<Card> actionCard = new List<Card>();
    public List<Card> allActionCards = new List<Card>(); //A list of the action cards as you get them (pre sorting)
    public List<Card> eventCard = new List<Card>();
    public List<Card> allEventCards = new List<Card>(); //Same thing here
    public List<Card> queuedCards = new List<Card>();

    //Pull the Data from The Government of The Player
    private int settlementModifier;
    private int agrarianModifier;
    private int militaryModifier;
    private int industrialModifier;
    private int academicAndReligiousModifier;
    private int navalModifier;
    private int transportationAndEconomyModifier;
    private int defenseModifier;
    private int scoutingModifier;
    private int mediaAndSocialModifier;

    //Religion Lists
    public List<Card> religionCards = new List<Card>();

    //List of all of your cards sorted into each category
    private List<Card> settlementCards = new List<Card>();
    private List<Card> agrarianCards = new List<Card>();
    private List<Card> militaryCards = new List<Card>();
    private List<Card> industrialCards = new List<Card>();
    private List<Card> academicAndReligiousCards = new List<Card>();
    private List<Card> navalCards = new List<Card>();
    private List<Card> transportationAndEconomyCards = new List<Card>();
    private List<Card> defenseCards = new List<Card>();
    private List<Card> scoutingCards = new List<Card>();
    private List<Card> mediaAndSocialCards = new List<Card>();

    //Dealer Related Event Card Arrays
    private List<Card> eraOneEventCards = new List<Card>();
    private List<Card> eraTwoEventCards = new List<Card>();
    private List<Card> eraThreeEventCards = new List<Card>();

    public void Connect(UIController UIController, Player player)
    {
        uiController = UIController;
        this.player = player;
        gameManager = player.gameManager;
    }

    public void DealCards()
    {
        government = player.playerGovernment;

        //queueCardDealing(); When the auction house is set up

        DetermineCardType();
    }

    public void DetermineCardType()
    {
        int cardTypeInt = Random.Range(0, 10);

        if(gameManager.GetOpenCardSlotCount() != -1)
        {
            if(cardTypeInt < 6) //Action Cards
            {
                Debug.Log("Deal Action Card");

                DealActionCards();

            } else if (cardTypeInt >= 6 && cardTypeInt < 10) // Event Cards
            {
                Debug.Log("Deal Event Card. Actually deals acton cards until, event cards added to game");

                DealActionCards();
                //DealEventCards();

            } else //Religious Cards
            {
                Debug.Log("Deal Religion Card. Actually deals acton cards until, event cards added to game");

                if(gameManager.GetEra() > 1 && player.createdAReligion == true)
                {
                    DealActionCards();
                    //DealReligionCards();
                } else 
                {
                    Debug.Log("Wrong era, or has not created a religion yet");

                    DetermineCardType();
                }
            }
        }  
    }

    public void FilterCards(bool govState) //Sort Cards into lists to make random card selection, and make it be influenced by the players government
    {
        List<Card> cardsToRemove = new List<Card>(); //Add the cards to this list to remove them after they have bee sorted

        hasGov = govState;

        for (int a  = 0; a < allActionCards.Count; a++)
        {
            Card card = allActionCards[a];
            string currentCardCat = card.cardCategory;

            if (currentCardCat == "Settlement")
            {
                settlementCards.Add(card);
                cardsToRemove.Add(card);
            }else if (currentCardCat == "Agrarian")
            {
                agrarianCards.Add(card);
                cardsToRemove.Add(card);
            }
            else if (currentCardCat == "Military")
            {
                militaryCards.Add(card);
                cardsToRemove.Add(card);
            }
            else if (currentCardCat == "Industrial")
            {
                industrialCards.Add(card);
                cardsToRemove.Add(card);
            }
            else if (currentCardCat == "Academic and Religious")
            {
                academicAndReligiousCards.Add(card);
                cardsToRemove.Add(card);
            }
            else if (currentCardCat == "Naval")
            {
                navalCards.Add(card);
            }
            else if (currentCardCat == "Transportation and Economy")
            {
                transportationAndEconomyCards.Add(card);
                cardsToRemove.Add(card);
            }
            else if (currentCardCat == "Defense")
            {
                defenseCards.Add(card);
                cardsToRemove.Add(card);
            }
            else if (currentCardCat == "Scouting")
            {
                scoutingCards.Add(card);
                cardsToRemove.Add(card);
            }
            else if (currentCardCat == "Media and Social")
            {
                mediaAndSocialCards.Add(card);
                cardsToRemove.Add(card);
            }
            else
            {
                Debug.LogError("Card with wrong identifier: " + card.name);
            }
        }

        for (int i = 0; i < allEventCards.Count; i++)
        {
            Card currentCard = allEventCards[i];

            if(currentCard.cardEra == 1)
            {
                eraOneEventCards.Add(currentCard);
            } else if (currentCard.cardEra == 2)
            {
                eraTwoEventCards.Add(currentCard);
            } else 
            {
                eraThreeEventCards.Add(currentCard);
            }
        }

        foreach (Card cardToRemove in cardsToRemove)
        {
            allActionCards.Remove(cardToRemove);
        }
    }

    public void DealEventCards()
    {
        if (gameManager.gameEra == 1)
        {
            int randCard = Random.Range(0, eraOneEventCards.Count);

            Card selectedCard = eraOneEventCards[randCard];

            Card duplicateCard = ScriptableObject.CreateInstance<Card>();
            duplicateCard = selectedCard;

            uiController.DisplayCard(duplicateCard, uiController.GetOpenSlot());

        } else if (gameManager.gameEra == 2)
        {
            int randCard = Random.Range(0, eraOneEventCards.Count);

            Card selectedCard = eraOneEventCards[randCard];

            Card duplicateCard = ScriptableObject.CreateInstance<Card>();
            duplicateCard = selectedCard;

            uiController.DisplayCard(duplicateCard, uiController.GetOpenSlot());

        } else if (gameManager.gameEra == 3)
        {
            int randCard = Random.Range(0, eraOneEventCards.Count);

            Card selectedCard = eraOneEventCards[randCard];

            Card duplicateCard = ScriptableObject.CreateInstance<Card>();
            duplicateCard = selectedCard;

            uiController.DisplayCard(duplicateCard, uiController.GetOpenSlot());
        }
    }

    public void DealReligionCards()
    {
        int randCard = Random.Range(0, religionCards.Count);

        Card selectedCard = religionCards[randCard];

        Card duplicateCard = ScriptableObject.CreateInstance<Card>();
        duplicateCard = selectedCard;

        uiController.DisplayCard(duplicateCard, uiController.GetOpenSlot());
    }

    public void DealActionCards()
    {
        Card currentCard = null;

        if (government != null && GovernmentModifiers(government))
        {
            int cardCat = Random.Range(0, 951);

            if (cardCat >= 0 && cardCat <= 100 + agrarianModifier)
            {
                if(DealCardFromCategory(agrarianCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 100 + agrarianModifier && cardCat <= 200 + militaryModifier + agrarianModifier)
            {
                if(DealCardFromCategory(militaryCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 200 + militaryModifier + agrarianModifier && cardCat <= 300 + militaryModifier + agrarianModifier + industrialModifier)
            {
                if(DealCardFromCategory(industrialCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 300 + agrarianModifier + militaryModifier + industrialModifier && cardCat <= 400 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier)
            {
                if (hasGov)
                {
                    academicAndReligiousCards.RemoveAll(card => card.cardTech == "Government");
                }
                
                if(DealCardFromCategory(academicAndReligiousCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 400 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier && cardCat <= 500 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier + navalModifier)
            {
                if(DealCardFromCategory(navalCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 500 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier + navalModifier && cardCat <= 600 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier + navalModifier + transportationAndEconomyModifier)
            {
                if(DealCardFromCategory(transportationAndEconomyCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 600 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier + navalModifier + transportationAndEconomyModifier && cardCat <= 700 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier + navalModifier + transportationAndEconomyModifier + defenseModifier)
            {
                if(DealCardFromCategory(defenseCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 700 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier + navalModifier + transportationAndEconomyModifier + defenseModifier && cardCat <= 800 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier + navalModifier + transportationAndEconomyModifier + defenseModifier + scoutingModifier)
            {
                if(DealCardFromCategory(scoutingCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 800 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier + navalModifier + transportationAndEconomyModifier + defenseModifier + scoutingModifier && cardCat <= 900 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier + navalModifier + transportationAndEconomyModifier + defenseModifier + scoutingModifier + mediaAndSocialModifier)
            {
                if(DealCardFromCategory(mediaAndSocialCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 900 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier + navalModifier + transportationAndEconomyModifier + defenseModifier + scoutingModifier + mediaAndSocialModifier && cardCat <= 950 + militaryModifier + agrarianModifier + industrialModifier + academicAndReligiousModifier + navalModifier + transportationAndEconomyModifier + defenseModifier + scoutingModifier + mediaAndSocialModifier + settlementModifier)
            {
                if(DealCardFromCategory(settlementCards) == false)
                {
                    DealActionCards();
                }
            }
        }
        else
        {
            int cardCat = Random.Range(0, 951);

            if (cardCat >= 0 && cardCat <= 100)
            {
                if(DealCardFromCategory(agrarianCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 100 && cardCat <= 200)
            {
                if(DealCardFromCategory(militaryCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 200 && cardCat <= 300)
            {
                if(DealCardFromCategory(industrialCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 300 && cardCat <= 400)
            {
                if (hasGov)
                {
                    academicAndReligiousCards.RemoveAll(card => card.cardTech == "Government");
                }
                
                if(DealCardFromCategory(academicAndReligiousCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 400 && cardCat <= 500)
            {
                if(DealCardFromCategory(navalCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 500 && cardCat <= 600)
            {
                if(DealCardFromCategory(transportationAndEconomyCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 600 && cardCat <= 700)
            {
                if(DealCardFromCategory(defenseCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 700 && cardCat <= 800)
            {
                if(DealCardFromCategory(scoutingCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 800 && cardCat <= 900)
            {
                if(DealCardFromCategory(mediaAndSocialCards) == false)
                {
                    DealActionCards();
                }
            }
            else if (cardCat > 900 && cardCat <= 950)
            {
                if(DealCardFromCategory(settlementCards) == false)
                {
                    DealActionCards();
                }
            }
        }
    }

    private bool DealCardFromCategory(List<Card> categoryCards)
    {
        if(categoryCards.Count != 0)
        {
            int cardCount = categoryCards.Count;
            int cardSelect = Random.Range(0, cardCount);

            Card selectedCard;

            if (cardSelect >= 0 && cardSelect < cardCount)
            {
                selectedCard = categoryCards[cardSelect];

                Card duplicateCard = ScriptableObject.CreateInstance<Card>();
                duplicateCard = selectedCard;

                Debug.Log("Card Data Duplicated" + duplicateCard);

                uiController.DisplayCard(duplicateCard, uiController.GetOpenSlot());

                return true;
            }

            return false;
        }

        return false;
    }

    private bool GovernmentModifiers(Government gov)
    {
        settlementModifier = gov.settlement;
        agrarianModifier = gov.agrarian;
        militaryModifier = gov.military;
        industrialModifier = gov.industrial;
        academicAndReligiousModifier = gov.academicAndReligious;
        navalModifier = gov.naval;
        transportationAndEconomyModifier = gov.transportationAndEconomy;
        defenseModifier = gov.defense;
        scoutingModifier = gov.scouting;
        mediaAndSocialModifier = gov.mediaAndSocial;

        return true;
    }

    public void queueCardDealing()
    {
        if(queuedCards != null)
        {
            for(int i = 0; i < queuedCards.Count; i++)
            {
                Card currentCard = queuedCards[i];

                if(currentCard.actionCard == true)
                {
                    actionCard.Add(currentCard);
                }else 
                {
                    eventCard.Add(currentCard);
                }
            }
        }
    }
}
