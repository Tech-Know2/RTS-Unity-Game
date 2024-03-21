using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ACDisplay : MonoBehaviour
{
    public TextMeshProUGUI cardName, cardDescription;
    public Card card;
    public Player originalPlayer;

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void PutOnDisplay(Card data)
    {
        cardName.text = data.cardName;
        cardDescription.text = data.description;
        originalPlayer = data.originalPlayer;

        gameObject.SetActive(true);
    }

    //remove the card once purchased
    public void PurchaseCard()
    {
        card = null;
        originalPlayer = null;

        cardName.text= null;
        cardDescription.text = null;

        gameObject.SetActive(false);
    }
}
