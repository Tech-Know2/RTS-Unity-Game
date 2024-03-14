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
    private Player player;

    public void Start()
    {
        if(uiController.playerLink() != null)
        {
            player = uiController.playerLink();
        }
    }

    public Card GetAttachedCard()
    {
        if(attachedCard != null)
        {
            return attachedCard;
        }

        return null;
    }

    public void GetCard()
    {
        if(player != null)
        {
            //Increase card object size
            Card currentCard = GetAttachedCard();
        }
    }
}
