using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPlacer : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                GameObject selectedCardHolder = EventSystem.current.currentSelectedGameObject;

                // Check if the clicked UI element has the desired component
                CardDataHolder cardData = selectedCardHolder.GetComponent<CardDataHolder>();

                // Check if the component exists before using it
                if (cardData != null)
                {
                    // Do something with the UI component
                    Debug.Log(cardData.attachedCard + " Card Selected");
                }
            }
            else
            {
                Debug.Log("No Card Selected");
            }
        }
    }
}
