/*
 * Developer Name: Longchang Cui
 * Date: May-23-2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnController : MonoBehaviour
{
    // Static Instance
    public static SpawnController instance;

    // TetrominoSystem Component
    public TetrominoSystem tetroSystem;

    // Spawn Points
    //  - Middle: Spawn I, O in the middle columns
    //  - Left-Middle: Spawn J, L, T, Z, S in the left-middle columns
    private Vector2Int _spawnMiddle;
    private Vector2Int _spawnLeftMiddle;

    // Tetromino Spawn Location
    private Vector2Int _spawnLocation;

    // Tetromino Spawn Prefabs
    public GameObject[] tetrominoSpawnList;

    // 7-bag Random Generator
    //private List<GameObject> _bag = new List<GameObject>();
    private List<int> _bagIdxList = new List<int>();
    private int shuffleCounter = 0;
    private GameObject _tmpTetromino;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize instance
        instance = this;

        // Middle: Spawn I, O in the middle columns
        // Left-Middle: Spawn J, L, T, Z, S in the left-middle columns
        _spawnMiddle = new Vector2Int(4,21);
        _spawnLeftMiddle = new Vector2Int(3,21);

        // Assign Tetromino System Component
        tetroSystem = GameObject.FindObjectOfType<TetrominoSystem>();

        // Initial bag shuffule
        _bagIdxList = ShuffleIndex();

        // Spawn Intial Tetromino
        SpawnTetromino();
    }


    /// <summary>
    /// Randomly select a tetromino object from prefabs and spawn the tetromino piece.
    /// Also sets the correct color of tile sprite.
    /// </summary>
    public void SpawnTetromino()
    {
        // Randomly Pick one piece of Tetromino
        GameObject localGO = PickTetromino(); 

        // Assign new tetromino to Tetromino System
        tetroSystem.currentTetromino = localGO;
        tetroSystem.tetroController = tetroSystem.currentTetromino.GetComponent<TetrominoController>();
        //tetroSystem.tetroMove = tetroSystem.currentTetromino.GetComponent<TetrominoMovementController>();

        // Assign new tetromino type and spawn the 4 blocks of the current tetromino type 
        TetrominoType newType = tetroSystem.tetroController.curType;
        SpawnTetrominoBlocks(newType);

        // Add new tetromino to TetrominoesInGame list
        tetroSystem.tetrominoesInGame.Add(localGO);

        // For T-Spin check initialization
        GridSystem.gridInstance.isLineCleared = false;
    }

    /// <summary>
    /// Spawn a tetromino and position the blocks of the spawned tetromino by type.
    /// </summary>
    /// <param name="tetroType"> Type of tetromino that needs to be spawned.</param>
    public void SpawnTetrominoBlocks(TetrominoType tetroType)
    {
        tetroSystem.tetroController.curType = tetroType;

        // Assign spawn location by the type
        if (tetroType == TetrominoType.I || tetroType == TetrominoType.O)
            _spawnLocation = _spawnMiddle;
        else
            _spawnLocation = _spawnLeftMiddle;

        // Update the blocks position
        tetroSystem.tetroController.blocks[0].UpdatePosition(_spawnLocation);
        tetroSystem.tetroController.UpdateBlocksPosition(_spawnLocation);

        // Assign Initial index of each block
        int index = 0;
        foreach (TetrominoBlockController ti in tetroSystem.tetroController.blocks)
        {
            ti.InitializeBlocks(tetroSystem.tetroController, index);
            index++;
        }
    }

    /// <summary>
    /// 7-Bag Random Generator. Randomly pick tetromino from a _bag of tetrminoes.
    /// </summary>
    /// <returns>Retrun a tetromino gameObject.</returns>
    public GameObject PickTetromino()
    {
        // If the shuffule number is greater than the index length, then shuffule the bag index.
        
        TetrominoType newType = new TetrominoType();

        // For first round shuffule
        
        if (shuffleCounter >= 6)
        {
            //_bag.AddRange(tetrominoSpawnList);
            _bagIdxList = ShuffleIndex();
            shuffleCounter = 0;

            // Assign the last tetromino in the bag
            GameObject newLocalTetro = GameObject.Instantiate(_tmpTetromino, transform);
            newType = tetrominoSpawnList[_bagIdxList[shuffleCounter]].GetComponent<TetrominoController>().curType;

            // Add sprite to the "Next Tetromino" box
            tetroSystem.AddNextTetrominoSprite(newType);

            return newLocalTetro;
        }

        // Pick the tetromino by index and spawn it
        GameObject newTetro = GameObject.Instantiate(tetrominoSpawnList[_bagIdxList[shuffleCounter]], transform);

        // For Next Box operation
        newType = tetrominoSpawnList[_bagIdxList[shuffleCounter + 1]].GetComponent<TetrominoController>().curType;
        // Add sprite to the "Next Tetromino" box
        tetroSystem.AddNextTetrominoSprite(newType);

        // Assign next object in the bag
        _tmpTetromino = tetrominoSpawnList[_bagIdxList[shuffleCounter + 1]];


        // Increase shuffule counter
        shuffleCounter++;

        // Return new tetromino object
        return newTetro;
    }

    /// <summary>
    /// Shuffle the index of the 7-Bag Random Generator
    /// </summary>
    /// <returns>Returns a list of the 7 index numbers</returns>
    public List<int> ShuffleIndex()
    {
        List<int> idxList = new List<int>() {0, 1, 2, 3, 4, 5, 6};

        for (int i = 0; i < idxList.Count; i++)
        {
            int rnd = Random.Range(0, idxList.Count);
            int tmpIdx = idxList[rnd];
            idxList[rnd] = idxList[i];
            idxList[i] = tmpIdx;
        }
        return idxList;
    }
}
