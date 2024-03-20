using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TechDisplayController : MonoBehaviour
{
    public UIController uiController;
    public Button techButton;
    private Image buttonImage;
    public Tech buttonTech;
    public GameObject previousTechButton;
    private TechDisplayController previousTechController;
    public bool hasBeenResearched = false;
    private TextMeshProUGUI techLabel;

    public void Start()
    {
        // Check if buttonTech is not null before accessing its properties
        if (buttonTech != null)
        {
            // Access the Image component of the Button
            buttonImage = techButton.image;

            // Access the TextMeshProUGUI component of the Button
            techLabel = techButton.GetComponentInChildren<TextMeshProUGUI>();

            // Set the color of the button
            buttonImage.color = buttonTech.techColor;

            // Set the text of the button
            techLabel.text = buttonTech.techName;

            //Set if To Interactable
            techButton.interactable = true;

            //Add a button event
            techButton.onClick.AddListener(ResearchIt);

            //Get the previous tech, if one
            if(previousTechButton != null)
            {
                previousTechController = previousTechButton.GetComponent<TechDisplayController>();
            }

            //Set has been researched to false
            hasBeenResearched = false;
        }
        else
        {
            Debug.LogError("ButtonTech is not assigned!");
        }
    }

    public void ResearchIt()
    {
        //If it has a previous tech
        if(uiController != null && uiController.gameManagerScript.playerTechPoints > 0 && previousTechController == null && uiController.gameManagerScript.gameEra == buttonTech.techEra)
        {
            foreach (Card card in buttonTech.techCards)
            {
                uiController.playerScript.cardDealer.allActionCards.Add(card);
            }

            uiController.gameManagerScript.playerTechPoints -= 1;
            techButton.interactable = false;

            uiController.playerScript.playerTechs.Add(buttonTech);

            //Modify has been researched
            hasBeenResearched = true;

            uiController.notificationController.CreateNotification("Tech Researched", buttonTech.techName + " has been researched in the tech tree");

        } else if (uiController != null && uiController.gameManagerScript.playerTechPoints > 0 && previousTechController.hasBeenResearched == true && uiController.gameManagerScript.gameEra == buttonTech.techEra)
        {
            foreach (Card card in buttonTech.techCards)
            {
                uiController.playerScript.cardDealer.allActionCards.Add(card);
            }

            uiController.gameManagerScript.playerTechPoints -= 1;
            techButton.interactable = false;

            uiController.playerScript.playerTechs.Add(buttonTech);

            //Modify has been researched
            hasBeenResearched = true;

            uiController.notificationController.CreateNotification("Tech Researched", buttonTech.techName + " has been researched in the tech tree");

        } else 
        {
            Debug.Log("Cant research it yet");

            if(uiController.gameManagerScript.gameEra != buttonTech.techEra)
            {
                uiController.notificationController.CreateNotification("Wrong Era for Tech", "You are in the wrong era for " + buttonTech.techName + " to be researched.");

            } else if (uiController.gameManagerScript.playerTechPoints == 0)
            {
                uiController.notificationController.CreateNotification("Not Enough Tech Points", "You are unable to research the " + buttonTech.techName + " due to lack of tech points.");

            } else if (previousTechController.hasBeenResearched == false)
            {
                uiController.notificationController.CreateNotification("Previous Tech Not Researched", "You must research " + previousTechController.buttonTech.techName + " before this tech.");

            } else 
            {
                uiController.notificationController.CreateNotification("Research Update", "You are unable to research " + buttonTech.techName + " at this time.");
            }
        }
    }
}
