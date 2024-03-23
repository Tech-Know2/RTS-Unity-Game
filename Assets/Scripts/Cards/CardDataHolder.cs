using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CardDataHolder : MonoBehaviour
{
    public Card attachedCard;
    public TextMeshProUGUI cardTitle, cardCategory, cardEra, cardDescription;
    public UIController uiController;
    public Player player;
    public Image backgroundImage;

    public void Start()
    {
        GetPlayerScript();
    }

    private void GetPlayerScript()
    {
        if(uiController.playerScript != null)
        {
            player = uiController.playerScript;
        }
    } 

    //Card Data Retrevial
    public Card GetAttachedCard()
    {
        if(attachedCard != null)
        {
            return attachedCard;
        }

        return null;
    }

    //Initiate the card placements
    public void GetCard()
    {
        if(player != null)
        {
            Card currentCard = GetAttachedCard();

            //Start the card placement process
            player.cardPlacer.CardClicked(gameObject);
        } else 
        {
            GetPlayerScript();
            GetCard();
        }
    }

    public void SetColor(Color color)
    {
        if (backgroundImage != null)
        {
            backgroundImage.color = color;
        }
    }
}
