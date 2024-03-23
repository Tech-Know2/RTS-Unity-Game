using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class BranchData
{
    public string branchName;
    public List<GameObject> eraDisplays = new List<GameObject>();
}

public class TechTreeController : MonoBehaviour
{
    public UIController uiController;
    public GameObject mainUI;
    public GameObject techTreeUI;
    private Image buttonImage;
    public List<Button> branchButtons = new List<Button>();
    public List<BranchData> branchObjects = new List<BranchData>();
    public bool allowedToSwitchBranches = false;
    public Tech requiredTech;
    public int branch = 0;
    public TextMeshProUGUI currentEraDisplay; //Era on scroll
    public TextMeshProUGUI gameEraDisplay; //Era of the game

    public int currentEra = 1;

    public void Start()
    {
        uiController = GetComponent<UIController>();

        CloseTechTree();
    }

    public void Update()
    {
        gameEraDisplay.text = uiController.playerScript.gameManager.gameEra.ToString();
    }

    public void OpenTechTree()
    {
        mainUI.SetActive(false);
        techTreeUI.SetActive(true);

        currentEra = 1;
        currentEraDisplay.text = currentEra.ToString();

        if(canSwitch() == false)
        {
            CloseAllBranches();
            branchObjects[0].eraDisplays[0].SetActive(true);
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
            for (int j = 0; j < branchObjects[i].eraDisplays.Count; j++)
            {
                branchObjects[i].eraDisplays[j].SetActive(false);
            }
        }

        currentEra = 1;

        currentEraDisplay.text = currentEra.ToString();
    }


    public void SwitchBranches()
    {
        if(canSwitch() == true)
        {
            CloseAllBranches();

            // The clicked button's GameObject can be obtained from the current event
            GameObject buttonObject = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            branch = 0;

            for (int i = 0; i < branchButtons.Count; i++)
            {
                if (buttonObject == branchButtons[i].gameObject)
                {
                    branch = i;
                }
            }

            branchObjects[branch].eraDisplays[currentEra - 1].SetActive(true);
        }else
        {
            uiController.playerScript.notificationController.CreateNotification("Can't Change Branch", "You must first research the settlement tech in order to switch to other branches");
        }

        currentEra = 1;

        currentEraDisplay.text = currentEra.ToString();
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

    public void UpdateEraDisplay()
    {
        for(int i = 0; i < branchObjects[branch].eraDisplays.Count; i++)
        {
            branchObjects[branch].eraDisplays[i].SetActive(false);
        }

        branchObjects[branch].eraDisplays[currentEra - 1].SetActive(true);

        currentEraDisplay.text = currentEra.ToString();
    }

    public void EraIncrease()
    {
        if(currentEra < 3 && currentEra < branchObjects[branch].eraDisplays.Count)
        {
            currentEra++;
            UpdateEraDisplay();
        }

        currentEraDisplay.text = currentEra.ToString();
    }

    public void EraDecrease()
    {
        if(currentEra > 1)
        {
            currentEra--;
            UpdateEraDisplay();
        }
    }
}
