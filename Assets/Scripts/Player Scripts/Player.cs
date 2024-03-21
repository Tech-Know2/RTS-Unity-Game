using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using FishNet;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

public class Player : NetworkBehaviour
{
    //Script Connections
    public Player player;
    public GameObject UIObject;
    public UIController UIController;
    public CardDealer cardDealer;
    public GameManager gameManager;
    public GameObject mapObject;
    public CardEffectManager cardEffectManager;
    public BuildPlacer buildPlacer;
    public BuildingEffectManager buildingEffectManager;
    public EmpireManager empireManager;
    public ReligionEffectManager religionEffectManager;
    public NotificationController notificationController;
    public CardPlacer cardPlacer;

    // Scene Names
    public string multiplayerScene;
    public string singlePlayer;

    [SerializeField]
    private Camera playerCamera;

    //Camera Controller Vars
    public Vector3 newPosition;
    public Quaternion newRotation;
    public Vector3 newZoom;
    public Transform cameraTransform;
    public bool cameraPanningAllowed = true;
    public bool cameraScrollAllowed = true;
    public bool cameraMovementAllowed = true;
    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotateStartPosition;
    public Vector3 rotateCurrentPosition;
    public float normalSpeed;
    public float fastSpeed; 
    public float moveSpeed;
    public float moveTime;
    public float rotationAmount;
    public Vector3 zoomAmount;
    public int maxZoomDistance = 150;
    public int minZoomDistance = 0;

    //Player Civilization
    public Empire playerEmpire;
    public Color playerColor;
    public Government playerGovernment;
    public List<Tech> playerTechs = new List<Tech>();
    public bool hasGov = false;
    public bool createdAReligion = false;
    //units
    //buildings
    //peace treaties
    //active wars
    //sanctions
    //etc

    //Networking Vars
    [SyncVar]
    public int interval = 0;
    private int currentInterval = 0;
    public float intervalSeconds = 30f;

    [SyncObject]
    public readonly SyncList<Player> playerList = new SyncList<Player>();

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (base.IsOwner)
        {
            StartCoroutine(MoveToDestinationScene(multiplayerScene));
        }
        else
        {
            gameObject.GetComponent<Player>().enabled = false;
        }

        if(base.IsServer)
        {
            StartCoroutine(IncrementNumberEveryIntervalSeconds());
        }
    }

    IEnumerator IncrementNumberEveryIntervalSeconds()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalSeconds);

            interval += 1;
        }
    }

    //Control the intervals
    public void DealWithIntervalChange()
    {
        gameManager.interval = currentInterval;
        gameManager.UpdateEra();
        gameManager.playerTechPoints++;

        cardEffectManager.EffectsPerInterval();
        buildingEffectManager.IntervalEffects();

        if(playerEmpire != null)
        {
            empireManager.IntervalChanges(playerEmpire);
        }
        

        Debug.Log("Interval: " + currentInterval);
    }

    public void GetCards()
    {
        if(playerGovernment != null)
        {
            hasGov = true;
        } else 
        {
            hasGov = false;
        }

        //Deal With card dealing
        cardDealer.FilterCards(hasGov);
        cardDealer.DealCards();
    }


    public void Start()
    {
        player = GetComponent<Player>();
        cardDealer = GetComponent<CardDealer>();

        playerList.Add(this);

        if(playerList.Count > 0)
        {
            Debug.Log("Player added to player list");
        }

        currentInterval = interval;
    }

    public void PlayerEmpireSetup()
    {
        //Setup Empire
        empireManager = GetComponent<EmpireManager>();
        empireManager.ConnectToPlayer(this);
        playerEmpire = empireManager.CreateEmpire();
        empireManager.SetUpEmpire(playerEmpire);
        playerColor = playerEmpire.empireColor;
    }

    //Used to Start them once the player has gotten into the scene officially
    private void WakeUpScripts()
    {
        //Connect to the UI
        UIObject = GameObject.FindGameObjectWithTag("ClientUI");
        UIController = UIObject.GetComponent<UIController>();
        notificationController = UIController.notificationController;

        //Connect the Game Manager to the Player
        gameManager = this.GetComponent<GameManager>();

        gameManager.ConnectToPlayer(player);

        //Connect the UI Controller to the CardDealer
        cardDealer.Connect(UIController, player);

        //Get the UI Controller to make its connections
        UIController.ConnectToPlayer(player, gameManager, cardDealer);

        PlayerEmpireSetup();
    }

    IEnumerator MoveToDestinationScene(string sceneToMoveTo)
    {
        // Wait for 0.5 seconds
        yield return new WaitForSeconds(0.5f);

        // Ensure the destination scene is loaded
        if (!UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneToMoveTo).isLoaded)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneToMoveTo, LoadSceneMode.Additive);
        }

        // Find the destination scene
        Scene destinationScene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneToMoveTo);

        // Move the GameObject to the destination scene
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(gameObject, destinationScene);

        // Now that the GameObject is in the new scene, find the camera and set it up
        yield return new WaitForSeconds(0.5f);
        SetupCamera();
        WakeUpScripts();
    }

    void SetupCamera()
    {
        // Find the main camera in the current scene
        playerCamera = Camera.main;

        // If a camera is found, set its position and make it a child of the GameObject
        if (playerCamera != null)
        {
            // Check if the camera is already a child
            if (playerCamera.transform.parent != transform)
            {
                // Adjust the parent's rotation to (0, 0, 0)
                transform.rotation = Quaternion.Euler(0, 0, 0);

                // Store the current camera position and rotation
                Vector3 cameraPosition = playerCamera.transform.position;
                Quaternion cameraRotation = playerCamera.transform.rotation;

                // Set up the new parent-child relationship
                playerCamera.transform.SetParent(transform, false);

                // Reset local position, rotation, and scale after setting parent
                playerCamera.transform.localPosition = Vector3.zero;
                playerCamera.transform.localRotation = Quaternion.identity;
                playerCamera.transform.localScale = Vector3.one;

                // Set the camera rotation to 45,0,0
                playerCamera.transform.localRotation = Quaternion.Euler(45, 0, 0);
                newRotation = Quaternion.Euler(0, 0, 0);

                cameraTransform = playerCamera.transform;
                newPosition = transform.position;

                // Store the new local position for zoom adjustments
                newZoom = playerCamera.transform.localPosition;

                Debug.Log("Main camera found and set up!");
            }
            else
            {
                // Camera is already a child, nothing to do
                Debug.Log("Main camera is already a child of the player.");
            }
        }
        else
        {
            Debug.LogError("Main camera not found in the scene!");
        }
    }

    void Update()
    {
        HandelMouseInput();
        HandelMovementInput();

        if (playerCamera == null)
        {
            SetupCamera();
        }

        transform.position = cameraTransform.position;

        if (interval == (currentInterval + 1))
        {
            currentInterval = interval;

            DealWithIntervalChange();
        }
    }

    void HandelMouseInput()
    {
        /*if (cameraScrollAllowed)
        {
            // Scroll With The Mouse
            if (Input.mouseScrollDelta.y != 0)
            {
                float scrollDelta = Input.mouseScrollDelta.y;
                float newZoomY = newZoom.y - scrollDelta * zoomAmount.y;

                // Clamp the new zoom value between minZoomDistance and maxZoomDistance
                newZoomY = Mathf.Clamp(newZoomY, minZoomDistance, maxZoomDistance);

                newZoom.y = newZoomY;
            }
        }*/

        if (cameraPanningAllowed)
        {
            // Pan With the Mouse
            if (Input.GetMouseButtonDown(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero); // Adjust Vector3.up to your desired up direction

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragStartPosition = ray.GetPoint(entry);
                }
            }
            if (Input.GetMouseButton(0))
            {
                Plane plane = new Plane(Vector3.up, Vector3.zero);

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                float entry;

                if (plane.Raycast(ray, out entry))
                {
                    dragCurrentPosition = ray.GetPoint(entry);

                    // Set the vertical component to zero to restrict panning to horizontal movement
                    Vector3 horizontalOffset = new Vector3(dragStartPosition.x - dragCurrentPosition.x, 0f, dragStartPosition.z - dragCurrentPosition.z);
                    newPosition = transform.position + horizontalOffset;
                }
            }

            if (Input.GetMouseButtonDown(2))
            {
                rotateStartPosition = Input.mousePosition;
            }
            if (Input.GetMouseButton(2))
            {
                rotateCurrentPosition = Input.mousePosition;

                Vector3 difference = rotateStartPosition - rotateCurrentPosition;

                rotateStartPosition = rotateCurrentPosition;

                newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
            }
        }

    }

    void HandelMovementInput()
    {   
        if(cameraMovementAllowed == true)
        {
            // Going Faster or Slower with Shift
            if(Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = fastSpeed;
            }
            else 
            {
                moveSpeed = normalSpeed;
            }

            // Get the camera's forward and right vectors without vertical component
            Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 cameraRight = Vector3.Scale(cameraTransform.right, new Vector3(1, 0, 1)).normalized;

            // Moving the Camera in All Cardinal Directions
            if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                newPosition += (cameraForward * moveSpeed);
            }
            if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                newPosition -= (cameraForward * moveSpeed);
            }
            if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                newPosition -= (cameraRight * moveSpeed);
            }
            if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                newPosition += (cameraRight * moveSpeed);
            }
            
            // Rotation the Camera
            if(Input.GetKey(KeyCode.Q))
            {
                newRotation *= Quaternion.Euler(Vector3.up * rotationAmount);
            }
            if(Input.GetKey(KeyCode.E))
            {
                newRotation *= Quaternion.Euler(Vector3.up * -rotationAmount);
            }

            // Smoothing Out All Movement and Input
            transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * moveTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * moveTime);
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * moveTime);
        }
    }

}
