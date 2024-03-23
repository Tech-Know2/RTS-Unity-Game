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
    public int currentPage = 0;
    public bool houseOpen = false;
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
        totalPages = 1 + cards.Count / cardsPerPage; //So that it still shows there being one page when there areno cards, instead of starting at 0 it starts at 1

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

        CardDataHolder cardDataHolder = playerScript.cardPlacer.currentCardObject.GetComponent<CardDataHolder>();
        Card card = cardDataHolder.attachedCard;

        if(playerScript.cardPlacer.cardActive() == true && uiController.playerScript.gameManager.playerGold >= 25 && card.isPurchasable == true) //Card can be purchased
        {
            uiController.playerScript.gameManager.playerGold -= 25;

            card.originalPlayer = playerScript;

            cards.Add(card);

            uiController.RemoveCardElement(playerScript.cardPlacer.currentCardObject);
            playerScript.cardPlacer.HandleDiscard();

            uiController.notificationController.CreateNotification("Card Moved to Auction", "The card " + card.cardName + " was moved to the auction house to be sold");

        } else if (playerScript.cardPlacer.cardActive() == true && uiController.playerScript.gameManager.playerGold >= 25 && card.isPurchasable == true) //Card can't be purchased
        {
            uiController.playerScript.gameManager.playerGold -= 25;

            uiController.RemoveCardElement(playerScript.cardPlacer.currentCardObject);
            playerScript.cardPlacer.HandleDiscard();

        }else
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
        if(playerScript == null)
        {
            ConnectToPlayer();
        }

        if(houseOpen == false)
        {
            CloseAll();
            OpenHouse();

            UpdateDisplays();

            houseOpen = true;

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
        playerScript.MovementAllowedSetter(false);
    }

    private void CloseHouse()
    {
        auctionHouse.SetActive(false);
        playerScript.MovementAllowedSetter(true);
    }

    public void PurchaseCard()
    {
        int purchaseCost = 75 /* ( sanctions + alliances, etc) */; //Work on later

        uiController.playerScript.gameManager.playerGold -= purchaseCost;

        //work on the puchase methods
        //setactive(false), add to hand, etc
    }
}
