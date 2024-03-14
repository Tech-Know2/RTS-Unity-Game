using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject mainUI;
    [SerializeField] private TextMeshProUGUI goldDisplay, techPointDisplay, eraDisplay;

    // Script Connection Vars
    public Player playerScript;
    public GameObject managerObject;
    public GameManager gameManagerScript;
    public CardDealer dealer;
    public NotificationController notificationController;

    //Client vars
    private bool playerConnected = false;

    //Card Slots
    public List<GameObject> cardSlots = new List<GameObject>();
    public Card[] playerCards = {null, null, null, null, null};

    public void ConnectToPlayer(Player player, GameManager manager, CardDealer dealerScript)
    {
        this.playerScript = player;
        this.gameManagerScript = manager;
        this.dealer = dealerScript;
    }

    public Player playerLink()
    {
        if(playerScript != null)
        {
            return playerScript;

        } else 
        {
            return null;
        }
    }

    public void DealCard()
    {
        if(playerScript.playerTechs.Count >= 1)
        {
            if(gameManagerScript.playerGold >= 50 && GetOpenSlot() != -1)
            {
                //Remove the gold from player balance
                gameManagerScript.playerGold -= 50;

                //Call the dealer to get a card for the player
                playerScript.GetCards();

            } else if (gameManagerScript.playerGold < 50)
            {
                Debug.Log("Not Enough Gold");
                
                notificationController.CreateNotification("Can't Deal", "You do not have enough gold to be dealt a new card.");

            } else if (GetOpenSlot() == -1)
            {
                Debug.Log("No Space");

                notificationController.CreateNotification("Can't Deal", "Not enough space in your hand. Please clear some cards before getting new ones.");
            }
        } else 
        {
            Debug.Log("You need to research something first");

            notificationController.CreateNotification("Can't Deal", "In order to get a card you must research something first.");
        }
    }

    void Update()
    {
        if(gameManagerScript != null)
        {
            goldDisplay.text = gameManagerScript.playerGold.ToString();
            eraDisplay.text = gameManagerScript.gameEra.ToString();
            techPointDisplay.text = gameManagerScript.playerTechPoints.ToString();

        } else 
        {
            Debug.Log("No Game Manager Script Attached");
        }
    }

    public int GetCardSlotCount()
    {
        return cardSlots.Count;
    }

    public int GetOpenSlot()
    {
        for (int i = 0; i < playerCards.Length; i++)
        {
            if(playerCards[i] == null)
            {
                return i;
            }
        }

        // No divergence found, return -1.
        return -1;
    }

    public void DisplayCard(Card card, int slotNum)
    {
        // Get the open slot index
        int openSlot = slotNum;

        Debug.Log("Display Card method called" + card);
        
        playerCards[openSlot] = card;

        // Set the cardValues at the open slot index
        playerCards[openSlot] = card;

        //Gte access to the right digital diplay
        GameObject cardSlot = cardSlots[openSlot];

        //Get the CardDataHolder component a reference to it
        CardDataHolder cardDataHolder = cardSlot.GetComponent<CardDataHolder>();
        cardDataHolder.attachedCard = card;

        //Set the values
        cardDataHolder.cardTitle.text = card.cardName;
        cardDataHolder.cardCategory.text = card.cardCategory;
        cardDataHolder.cardEra.text = card.cardEra.ToString();
        cardDataHolder.cardDescription.text = card.description;
    }
}