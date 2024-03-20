using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TechTreeController : MonoBehaviour
{
    public UIController uiController;
    public GameObject mainUI;
    public GameObject techTreeUI;
    private Image buttonImage;
    public List<Button> branchButtons = new List<Button>();
    public List<GameObject> branchObjects = new List<GameObject>();
    public bool allowedToSwitchBranches = false;
    public Tech requiredTech;

    public void Start()
    {
        uiController = GetComponent<UIController>();

        CloseTechTree();
    }

    public void OpenTechTree()
    {
        mainUI.SetActive(false);
        techTreeUI.SetActive(true);

        if(canSwitch() == false)
        {
            CloseAllBranches();
            branchObjects[0].SetActive(true);
        } else 
        {
            SwitchBranches();
        }

        uiController.playerScript.cameraPanningAllowed = false;
        uiController.playerScript.cameraMovementAllowed = false;
    }

    public void CloseTechTree()
    {
        mainUI.SetActive(true);
        techTreeUI.SetActive(false);

        CloseAllBranches();

        uiController.playerScript.cameraPanningAllowed = true;
        uiController.playerScript.cameraMovementAllowed = true;
    }

    public void CloseAllBranches()
    {
        for(int i = 0; i < branchObjects.Count; i++)
        {
            branchObjects[i].SetActive(false);
        }
    }

    public void SwitchBranches()
    {
        if(canSwitch() == true)
        {
            CloseAllBranches();

            // The clicked button's GameObject can be obtained from the current event
            GameObject buttonObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            int branch = 0;

            for (int i = 0; i < branchButtons.Count; i++)
            {
                if (buttonObject == branchButtons[i].gameObject)
                {
                    branch = i;
                }
            }

            branchObjects[branch].SetActive(true);
            Debug.Log(branchObjects[branch]);
        }else
        {
            uiController.playerScript.notificationController.CreateNotification("Can't Change Branch", "You must first research the settlement tech in order to switch to other branches");
        }
    }

    private bool canSwitch()
    {
        if(uiController.playerScript.playerTechs.Contains(requiredTech))
        {
            return true;
        } else 
        {
            return false;
        }
    }
}
