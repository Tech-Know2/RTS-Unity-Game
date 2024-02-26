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
    public Color activeBranchColor;
    public Color defaultBranchColor;
    public List<Button> branchButtons = new List<Button>();
    public List<GameObject> branchObjects = new List<GameObject>();

    public void Start()
    {
        uiController = GetComponent<UIController>();

        CloseTechTree();
    }

    public void OpenTechTree()
    {
        mainUI.SetActive(false);
        techTreeUI.SetActive(true);

        SwitchBranches();

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

        for (int i = 0; i < branchButtons.Count; i++)
        {
            branchButtons[i].image.color =  defaultBranchColor;
        }
    }

    public void SwitchBranches()
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

        // Cast the GameObject to Button and set the color
        Button buttonComponent = buttonObject.GetComponent<Button>();

        if (buttonComponent != null)
        {
            buttonComponent.image.color = activeBranchColor;
        }
    }
}
