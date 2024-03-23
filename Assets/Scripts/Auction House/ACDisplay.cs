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

    public void Start()
    {
        gameObject.SetActive(false);
    }

    public void PutOnDisplay(Card data)
    {
        Debug.Log(data + "Passed to AC Display for auction house");

        gameObject.SetActive(true);

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
}
