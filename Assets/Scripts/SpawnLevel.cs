using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLevel : MonoBehaviour
{

    // This is more like a GameManager
    // I should change the name of it

    public Transform player;
    public Transform playerSpawn;
    public GameObject playerPrefab;

    public GameObject[] levelPieces;
    private Queue<GameObject> spawnedLevelPieces;

    public float groundHeightOfLevel = -1f;
    private Vector3 levelSpawnPos;

    // This tells us which piece should be spawned
    private int currentPiece = 0;

    // These are used to keep track of when a new piece should be spawned
    private float positionOnCurrentPiece;
    private float lengthOfPiece = 20f; // All of the pieces have been designed to have an x-value of 20

    public static SpawnLevel instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        // Instantiate queue
        spawnedLevelPieces = new Queue<GameObject>();

        // Set-up level
        BeginLevel();
    }

    void Update()
    {

        if (player.position.x > 0 && 
            player.position.x % lengthOfPiece < positionOnCurrentPiece)
        {
            SpawnNextPiece();
        }

        positionOnCurrentPiece = player.position.x % lengthOfPiece;
    }

    void SpawnNextPiece()
    {
        // Find out where to spawn new piece
        int distanceTravelled = Mathf.FloorToInt(player.position.x / lengthOfPiece) + 1;
        levelSpawnPos.x = distanceTravelled * lengthOfPiece;

        // Spawn new piece and destroy piece off screen
        spawnedLevelPieces.Enqueue(Instantiate(levelPieces[currentPiece], levelSpawnPos, Quaternion.identity));
        Destroy(spawnedLevelPieces.Dequeue());

        // Increment through level pieces
        // Wrap around to zero if the end of the array has been reached
        currentPiece++;
        if (currentPiece == levelPieces.Length)
        {
            currentPiece = 0;
        }
    }

    public void BeginLevel()
    {
        Debug.Log("Spawning");

        // Clear queue
        int numOfPieces = spawnedLevelPieces.Count;
        for (int i = 0; i < numOfPieces; i++)
        {
            Destroy(spawnedLevelPieces.Dequeue());
        }

        // Respawn player
        if (player != null)
        {
            Destroy(player.gameObject);
        }

        GameObject p = Instantiate(playerPrefab, playerSpawn.position, Quaternion.identity);
        player = p.transform;

        // Spawn two starting pieces
        levelSpawnPos = new Vector3(0f, groundHeightOfLevel, 0f);
        spawnedLevelPieces.Enqueue(Instantiate(levelPieces[0], levelSpawnPos, Quaternion.identity));

        levelSpawnPos.x = lengthOfPiece;
        spawnedLevelPieces.Enqueue(Instantiate(levelPieces[1], levelSpawnPos, Quaternion.identity));
    }

    // This is my very hacky solution for mobile controls
    public void ButtonJump()
    {
        player.gameObject.GetComponent<PlayerMovement>().ButtonJump();
    }

    public void ButtonSlide()
    {
        player.gameObject.GetComponent<PlayerMovement>().ButtonSlide();
    }

    public void ButtonAttack()
    {
        player.gameObject.GetComponent<PlayerMovement>().ButtonAttack();
    }
}
