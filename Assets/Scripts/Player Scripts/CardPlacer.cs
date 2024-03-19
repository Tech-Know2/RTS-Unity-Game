using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPlacer : MonoBehaviour
{
    //Cards vars
    public Card clickedCard;
    public GameObject currentCardObject, previousCardObject, tileClicked;

    public List<string> tileTags = new List<string>();

    //Display Vars
    public Vector3 scalingFactor;
    private Vector3 normalScaling;

    //Script Connections
    public Player playerScript;
    public UIController uiController;
    public CardEffectManager cardEffectManager;
    public GameManager gameManager;
    public BuildPlacer buildPlacer;
    public BuildingEffectManager buildingEffectManager;

    public void Start()
    {
        uiController = playerScript.UIController;
    }

    public void CardClicked(GameObject cardObject)
    {
        Card cardData;

        currentCardObject = cardObject;

        if(normalScaling == null)
        {
            normalScaling = cardObject.transform.localScale;
        }

        if(previousCardObject == null)
        {
            //If this is the first thing clicked, scale it up
            previousCardObject = currentCardObject;

            ScaleUp(currentCardObject);

        } else if (previousCardObject != null && previousCardObject != currentCardObject) //If something before it was already scaled up, scale the previous down, and then scale the new one up
        {
            ScaleDown(previousCardObject);
            ScaleUp(currentCardObject);

            previousCardObject = currentCardObject;

        } else if (previousCardObject == currentCardObject)
        {
            ScaleDown(currentCardObject);

            previousCardObject = null;
            currentCardObject = null;
        }

        if(cardObject != null)
        {
            //Get data from object
            CardDataHolder cardDataHolder = cardObject.GetComponent<CardDataHolder>();
            cardData = cardDataHolder.GetAttachedCard();
        }
    }

    private void ScaleUp(GameObject obj)
    {
        //Scale up the newly clicked object
        Vector3 currentScale = obj.transform.localScale;
        Vector3 newScale = currentScale + scalingFactor;
        obj.transform.localScale = newScale;

        Debug.Log("Scaled up");
    }

    private void ScaleDown(GameObject obj)
    {
        Vector3 currentScale = obj.transform.localScale;
        Vector3 newScale = currentScale - scalingFactor;
        obj.transform.localScale = newScale;

        Debug.Log("Scaled down");
    }

    public void Update()
    {
        // Check if the left mouse button is clicked and a card has been clicked
        if (Input.GetMouseButtonDown(0) && currentCardObject != null)
        {
            // Cast a ray from the mouse position into the scene
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Check if the ray hits an object
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the hit object's tag is contained in the tileTags list
                if (tileTags.Contains(hit.collider.tag))
                {
                    // Do something with the clicked object (e.g., call a method)
                    OnTileClicked(hit.collider.gameObject);
                }
            }
        }
    }

    void OnTileClicked(GameObject tileObject)
    {
        CardDataHolder cardDataHolder = currentCardObject.GetComponent<CardDataHolder>();
        Card card = cardDataHolder.attachedCard;

        if(currentCardObject != null && (card.desiredTilesList.Contains(tileObject.tag) || card.desiredTilesList.Count == 0))
        {
            //Remove that card from the data in the UI controller
            Debug.Log(currentCardObject);
            playerScript.UIController.RemoveCardElement(currentCardObject);

            ScaleDown(currentCardObject);
            currentCardObject = null;
            previousCardObject = null;

            if(card.effectManagerList.Count != 0 || card.religoiusEffectLists.Count != 0)
            {
                //Deal with the effects of the card (Religion/Standard)
                cardEffectManager.NewCardEffectSorter(card, tileObject);
            }

            if(card.buildsSomething == true)
            {
                //Pass to build system
                playerScript.UIController.SetUpBuildingDisplay(card);
            }
            
        } else 
        {
            playerScript.notificationController.CreateNotification("Card Placement Error", "That card can't be player on that tile, try another tile please");
        }
    }
}
