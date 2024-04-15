using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ACDisplay : MonoBehaviour
{
    public TextMeshProUGUI cardName, cardDescription;
    public Card card;
    public Image backgroundImage;
    public AuctionHouseController auctionHouseController;

    public void Start()
    {
        gameObject.SetActive(false);
        
        GameObject objWithTag = GameObject.FindWithTag("Map Manager");

        // Check if the GameObject was found
        if (objWithTag != null)
        {
            // Get the component attached to the GameObject
            auctionHouseController = objWithTag.GetComponent<AuctionHouseController>();
        }
        else
        {
            Debug.LogWarning("Map Manager not found");
        }
    }

    public void PutOnDisplay(Card data)
    {
        Debug.Log(data + "Passed to AC Display for auction house");

        gameObject.SetActive(true);

        card = data;

        cardName.text = data.cardName;
        cardDescription.text = data.description;
        backgroundImage.color = data.cardColor;
    }

    //remove the card once purchased
    public void PurchaseCard()
    {
        card = null;

        cardName.text= null;
        cardDescription.text = null;

        gameObject.SetActive(false);
    }

    public void PreviewCard()
    {
        if(auctionHouseController != null)
        {
            auctionHouseController.viewCard(GetCard(), this);
            Debug.Log("Attempting auction house preview");

        } else 
        {
            Debug.Log("Auction House Controller Script not connected");
        }
    }

    public Card GetCard()
    {
        return card;
    }
}
