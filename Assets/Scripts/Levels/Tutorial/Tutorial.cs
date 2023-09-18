using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{

    private GameManager gameManager;

    [Header("References")]
    public GameObject[] tutorialLevelPieces;
    public GameObject[] tutorialScreens;
    public GameObject introScreen, victoryScreen, failScreen;

    [Header("Variables")]
    public int positionInTutorial;
    public float spawnXPosition = 20f;
    private bool isReading;
    private float readTimer;
    public float timeToReadScreen = 5f;

    private List<GameObject> spawnedLevelPieces;

    public static Tutorial instance;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameManager = GameManager.instance;

        // Make sure character is stopped
        gameManager.player.GetComponent<PlayerMovement>().runEndlessly = false;

        positionInTutorial = 0;
        spawnedLevelPieces = new List<GameObject>();
        readTimer = 0;
        isReading = false;

        introScreen.SetActive(true);
    }

    void Update()
    {

        if (isReading)
        {
            readTimer += Time.deltaTime;

            if (readTimer > timeToReadScreen)
            {
                ExitTutorialScreen();
            }
        }

        // Tutorial begins with the basic rules of the game
        // When that screen is tapped out of, the player's character will begin running
        // A series of screens will appear. Each one will explain one of the following mechanics, and an obstacle will appear for the player to practice the mechanic.
        // The player will be sent back to the level select menu.

        // Mechanics to be taught:
        // Jump
        // Slide 
        // Attack 
        // Shooting
        // Dodging bullets
        // Deflecting bullets

        // Functions needed:
        // Spawn the next practice piece after screen is exited
        // Display the next tutorial screen after the level piece is successfully completed
        // If piece is not successfully completed, the level piece will be spawned again.
    }

    public void BeginTutorial()
    {
        // Start moving the character
        gameManager.player.GetComponent<PlayerMovement>().runEndlessly = true;

        // Switch screen
        introScreen.SetActive(false);
        ShowTutorialScreen();
    }

    void ShowTutorialScreen()
    {
        tutorialScreens[positionInTutorial].SetActive(true);

        // After a certain amount of time, hide the screen and move on
        isReading = true;
    }

    public void ExitTutorialScreen()
    {
        tutorialScreens[positionInTutorial].SetActive(false);
        SpawnPracticePiece();
        readTimer = 0f;
        isReading = false;
    }

    void SpawnPracticePiece()
    {
        // Reset failure variable in game manager
        gameManager.playerFailed = false;

        // Get appropriate spawn position and create piece
        Vector3 playerPosition = gameManager.player.transform.position;
        Vector3 levelSpawnPosition = new Vector3(playerPosition.x + spawnXPosition, 
            gameManager.groundHeightOfLevel, 
            playerPosition.z);

        spawnedLevelPieces.Add(Instantiate(tutorialLevelPieces[positionInTutorial], levelSpawnPosition, Quaternion.identity));
    }

    public void CompleteLevelPiece()
    {
        if (gameManager.playerFailed)
        {
            RedoLevelPiece();
            return;
        }

        // Deactivate the completed level piece
        spawnedLevelPieces[positionInTutorial].SetActive(false);

        // Advance the players position in the tutorial
        positionInTutorial++;

        // End the level if necessary
        if (positionInTutorial == tutorialLevelPieces.Length)
        {
            ShowVictoryMessage();
            gameManager.EndLevel();
            return;
        }

        // Advace to the next tutorial scene
        ShowTutorialScreen();
    }

    void RedoLevelPiece()
    {
        ShowFailMessage();

        // Reset failure variable in game manager
        gameManager.playerFailed = false;

        // Place the failed level piece in front of the player again
        Vector3 playerPosition = gameManager.player.transform.position;
        Vector3 levelSpawnPosition = new Vector3(playerPosition.x + spawnXPosition + 5f,
            playerPosition.y,
            playerPosition.z);

        spawnedLevelPieces[positionInTutorial].transform.position = levelSpawnPosition;
    }

    void ShowFailMessage()
    {
        failScreen.SetActive(true);
        StartCoroutine(HideFailScreen());
    }

    IEnumerator HideFailScreen()
    {
        yield return new WaitForSeconds(timeToReadScreen * 0.5f);

        failScreen.SetActive(false);
    }

    void ShowVictoryMessage()
    {
        victoryScreen.SetActive(true);
    }
}
