using UnityEngine;
using UnityEngine.EventSystems;

public class CardPlacer : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Create a pointer event
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);

            // Set the pointer event position to the mouse position
            pointerEventData.position = Input.mousePosition;

            // Check if the mouse is over a UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                // Get the GameObject that was clicked on
                GameObject clickedObject = pointerEventData.pointerCurrentRaycast.gameObject;

                // Check if the clicked UI element has the desired component
                if (clickedObject != null)
                {
                    CardDataHolder cardData = clickedObject.GetComponent<CardDataHolder>();

                    // Check if the component exists before using it
                    if (cardData != null)
                    {
                        // Do something with the UI component
                        Debug.Log(cardData.GetAttachedCard() + " Card Selected");

                        if (cardData.GetAttachedCard() != null)
                        {
                            // Execute card functions
                        }
                    }
                    else
                    {
                        Debug.Log("CardDataHolder component not found on the clicked UI element.");
                    }
                }
                else
                {
                    Debug.Log("No UI element selected.");
                }
            }
            else
            {
                Debug.Log("No Card Selected");
            }
        }
    }
}
