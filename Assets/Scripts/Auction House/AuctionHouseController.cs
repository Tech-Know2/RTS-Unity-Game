using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using TMPro;
using UnityEngine.UI;

public class AuctionHouseController : MonoBehaviour
{
    //Script Connections
    public UIController uiController;
    public Player playerScript;

    //Nonmain Viewing Vars

    //Main Viewing Vars
    public List<GameObject> viewSlots = new List<GameObject>();
    [SyncObject]
    public readonly SyncList<Card> cards = new SyncList<Card>();
    public int cardsPerPage = 16;
    public int discardCost = 25;
    public int totalPages;
    public int currentPage;
    public bool houseOpen = false;
    public bool firstTime = true;
    public TextMeshProUGUI currentPageText, totalPagesText;

    //Other UI Objs
    public GameObject mainUI;
    public GameObject auctionHouse;

    public void ConnectToPlayer()
    {
        playerScript = uiController.playerScript;
    }

    public void Update()
    {
        totalPages = cards.Count / cardsPerPage;

        int current = currentPage + 1;

        currentPageText.text = current.ToString();
        totalPagesText.text = totalPages.ToString();
    }

    public void Discard() //Adds card from player hand to the auction house
    {
        if(playerScript == null)
        {
            ConnectToPlayer();
        }

        if(playerScript != null && playerScript.cardPlacer.cardActive() == true && uiController.playerScript.gameManager.playerGold >= 25)
        {
            uiController.playerScript.gameManager.playerGold -= 25;

            CardDataHolder cardDataHolder = playerScript.cardPlacer.currentCardObject.GetComponent<CardDataHolder>();
            Card card = cardDataHolder.attachedCard;

            cards.Add(card);

            uiController.RemoveElement(card);
            playerScript.cardPlacer.HandleDiscard();

            uiController.notificationController.CreateNotification("Card Moved to Auction", "The card " + card.cardName + " was moved to the auction house to be sold");
        } else 
        {
            uiController.notificationController.CreateNotification("Error in Discard", "Unable to discard for some reason");
        }
    }

    public void UpdateDisplays()
    {
        int startIndex = currentPage * cardsPerPage;
        int endIndex = Mathf.Min(startIndex + cardsPerPage, cards.Count);

        for (int i = startIndex; i < endIndex; i++)
        {
            GameObject slot = viewSlots[i - startIndex];
            ACDisplay display = slot.GetComponent<ACDisplay>();

            display.PutOnDisplay(cards[i]);
        }
    }


    public void pageLeft()
    {
        if(currentPage > 0)
        {
            currentPage--;
            UpdateDisplays();
        }
    }

    public void pageRight()
    {
        if(currentPage < totalPages && cards.Count >= (currentPage * 16 + 1))
        {
            currentPage++;
            UpdateDisplays();

        } else if (currentPage >= totalPages)
        {
            currentPage = 0;
            UpdateDisplays();
        }
    }

    public void AuctionHouseButton()
    {
        if(houseOpen == false)
        {
            //Adds the player to the list of players
            if(firstTime == true && playerScript != null)
            {
                //players.Add(playerScript);
                firstTime = false;
            }

            houseOpen = true;

            CloseAll();
            OpenHouse();

        } else 
        {
            CloseHouse();
            OpenAll();

            houseOpen = false;
        }
    }

    private void CloseAll()
    {
        mainUI.SetActive(false);
        auctionHouse.SetActive(false);
    }

    private void OpenAll()
    {
        mainUI.SetActive(true);
    }

    private void OpenHouse()
    {
        auctionHouse.SetActive(true);
    }

    private void CloseHouse()
    {
        auctionHouse.SetActive(false);
    }

    public void PurchaseCard()
    {
        int purchaseCost = 75 /* ( sanctions + alliances, etc) */; //Work on later

        uiController.playerScript.gameManager.playerGold -= purchaseCost;
    }
}
