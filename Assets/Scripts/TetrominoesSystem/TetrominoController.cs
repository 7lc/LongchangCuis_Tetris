/*
 * Developer Name: Longchang Cui
 * Date: May-23-2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;
using UniRx;
using System;

public enum TouchDirection{ NONE, LEFT, RIGHT, UP, DOWN }
public class TetrominoController : MonoBehaviour
{
    // Boolean to check the current tetromino's movement condition
    public bool isMovable = true;

    // Blocks of the current tetromino
    public TetrominoBlockController[] blocks;

    // Tetromino System Component
    public TetrominoSystem tetroSystem;

    // Super Rotation System rotation index
    public int rotationIndex { get; private set; }

    // Current Tetromino GameObject and Components
    GameObject curBlocks = null;
    TetrominoController curTetroController;
    public TetrominoType curType;

    // Droptime needs to be updated based on the current level
    public float dropTime;

    // UniRx coroutine handler
    public IDisposable dropCoroutine;

    // Hold varaible
    public bool isHoldable = true;

    // T-Spin check
    public bool isLastRotation = false;

    // Swipe Control
    private TouchDirection _swipeDirection = TouchDirection.NONE;
    private TouchDirection _swipeEndDirection = TouchDirection.NONE;
    private float _timeToNextSwipe = 1;
    private float _timeToSwipe = 0.2f;

    // Tap control
    private bool _isTapped = false;
    private bool _isLeftTap = false;
    private bool _isBottomTap = false;

    void OnEnable()
    {
        // Drop time reduces when the game level goes up
        dropTime = 1 - GameSystem.gsInstance.gameLevel * 0.1f;

        // Assign TetrominoSystem Component
        tetroSystem = GameObject.FindObjectOfType<TetrominoSystem>();

        // Assign attached blocks to blocks list
        blocks = new TetrominoBlockController[4];
        for (int i = 0; i < blocks.Length; i++)
        {
            TetrominoBlockController newBlock = transform.GetChild(0).GetChild(i).GetComponent<TetrominoBlockController>();
            blocks[i] = newBlock;
        }

        // Assign TetrominoController to current tetromino variable
        curTetroController = gameObject.GetComponent<TetrominoController>();
        curBlocks = gameObject;

        // Assign touch controller event
        TouchController.swipeEvent += SwipeHandler;
        TouchController.swipeEndEvent += SwipeEndHandler;
        TouchController.tapEvent += TapHandler;
    }

    void OnDisable()
    {
        // Dereference the events from touch controller
        TouchController.swipeEvent -= SwipeHandler;
        TouchController.swipeEndEvent -= SwipeEndHandler;
        TouchController.tapEvent += TapHandler;
    }

    void Start()
    {
        // Start drop coroutine
        // UniRx - Coroutine
        StartTetrominoDrop();
    }

    void Update()
    {
        // If the current tetromino is movable, perform movement and rotation operation based on the player's input.
        if (isMovable == true)
            PlayerInputMovement();
    }

    /// <summary>
    /// Track player's key input or touch input for tetromino movement.
    /// </summary>
    void PlayerInputMovement()
    {
        // Keyboard Input
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Hard Drop
            curTetroController.HardDropTetromino();
            isLastRotation = false;
        }

        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {        
            // Move down one block
            MoveTetromino(Vector2Int.down);
            isLastRotation = false;
            PlayTetrominoSound();
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Move left one block
            MoveTetromino(Vector2Int.left);
            isLastRotation = false;
            PlayTetrominoSound();
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Move right one block
            MoveTetromino(Vector2Int.right);
            isLastRotation = false;
            PlayTetrominoSound();
        }

        // Perform SRS Rotation
        else if (Input.GetKeyDown(KeyCode.C))
        {
            // Clockwise operation
            curTetroController.RotateTetromino(true);
            isLastRotation = true;
            PlayTetrominoSound();
        }

        else if (Input.GetKeyDown(KeyCode.X))
        {
            // Counter-clockwise operation
            curTetroController.RotateTetromino(false);
            isLastRotation = true;
            PlayTetrominoSound();
        }

        else if (Input.GetKeyDown(KeyCode.Z))
        {
            // Hold tetromino
            tetroSystem.HoldCurrentTetromino();
            PlayTetrominoSound();
        }

        // Touch Movement
        else if(_swipeEndDirection == TouchDirection.RIGHT && Time.time > _timeToNextSwipe)
        {
            // Rotate right
            // Clockwise operation
            curTetroController.RotateTetromino(true);
            isLastRotation = true;
            PlayTetrominoSound();
            _timeToNextSwipe = Time.time + _timeToSwipe;
        }

        else if (_swipeEndDirection == TouchDirection.LEFT && Time.time > _timeToNextSwipe)
        {
            // Rotate left
            // Counter-clockwise operation
            curTetroController.RotateTetromino(false);
            isLastRotation = true;
            PlayTetrominoSound();
            _timeToNextSwipe = Time.time + _timeToSwipe;
        }

        else if (_swipeEndDirection == TouchDirection.UP && Time.time > _timeToNextSwipe)
        {
            // Hold tetromino
            tetroSystem.HoldCurrentTetromino();
            PlayTetrominoSound();
            _timeToNextSwipe = Time.time + _timeToSwipe;
        }

        else if (_swipeEndDirection == TouchDirection.DOWN && Time.time > _timeToNextSwipe)
        {
            // Hard drop tetromino
            curTetroController.HardDropTetromino();
            isLastRotation = false;
            _timeToNextSwipe = Time.time + _timeToSwipe;
        }


        else if (_isTapped == true && _isLeftTap == true && _isBottomTap == false)
        {
            // Move left
            MoveTetromino(Vector2Int.left);
            PlayTetrominoSound();
        }

        else if (_isTapped == true && _isLeftTap == false && _isBottomTap == false)
        {   
            // Move right
            MoveTetromino(Vector2Int.right);
            PlayTetrominoSound();
        }

        else if (_isTapped == true && _isBottomTap == true)
        {
            // Move down
            MoveTetromino(Vector2Int.down);
            PlayTetrominoSound();
        }

        // Reset touch control
        _swipeDirection = TouchDirection.NONE;
        _swipeEndDirection = TouchDirection.NONE;
        _isTapped = false;
        _isBottomTap = false;
        _isLeftTap = false;
    }

    /// <summary>
    /// Get the coordinates of all the blocks of the current tetromino.
    /// </summary>
    /// <returns>Return Vector2Int array of coordinates for current tetromino blocks.</returns>
    public Vector2Int[] GetBlocksCoordinates()
    {
        List<Vector2Int> curBlocksCoor = new List<Vector2Int>();

        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] == null)
            {
                continue;
            }

            // Add blocks to the coordinates list
            curBlocksCoor.Add(blocks[i].coordinates);
        }

        // Order the coordinates
        curBlocksCoor = curBlocksCoor.OrderBy(x => x.x).ThenByDescending(x => x.y).ToList();

        Vector2Int[] curCoor = curBlocksCoor.ToArray();
        return curCoor;
    }

    /// <summary>
    /// Check if the tetromino is able to move by the specific amount of vectors.
    /// The current tetromino cannot be moved if attempted position of the grid is already occupied.
    /// </summary>
    /// <param name="moveVector">X,Y amount to move the piece</param>
    /// <returns>Return true if the attempted movement is possible</returns>
    public bool IsTetrominoMovable(Vector2Int moveVector)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            if (!blocks[i].IsBlockMovable(moveVector + blocks[i].coordinates))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Check if any blocks of the current tromino is left.
    /// </summary>
    /// <returns>Return true if there are blocks left.</returns>
    public bool AnyBlocksLeft()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            if (blocks[i] != null)
            {
                return true;
            }
        }
        
        tetroSystem.RemoveTetromino(gameObject);
        return false;
    }

    /// <summary>
    /// Move the tetromino by the amount of the specified vectors.
    /// </summary>
    /// <param name="moveVector"> Vector2Int amount of movement</param>
    /// <returns>Return true if the tetromino is able to move.</returns>
    public bool MoveTetromino(Vector2Int moveVector)
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            if (!blocks[i].IsBlockMovable(moveVector + blocks[i].coordinates))
            {
                if (moveVector == Vector2Int.down)
                {
                    StoreTetrominoToGrid();
                }
                return false;
            }
        }

        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].MoveBlock(moveVector);
        }

        return true;
    }

    /// <summary>
    /// Rotate the piece by 90 degrees in specified direction, clockwise or counter-clockwise. 
    /// </summary>
    /// <param name="clockwise">Set to true for rotating the current tetromino clockwise, false for rotating counter-clockwise.</param>
    public void RotateTetromino(bool clockwise)
    {
        int oldRotIdx = rotationIndex;
        rotationIndex += clockwise ? 1 : -1;

        // Perform modulo operation
        rotationIndex = (rotationIndex % 4 + 4) % 4;

        // Rotate the blocks of the current tetromino
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].RotateBlock(blocks[0].coordinates, clockwise);
        }

        // Perform offset tests
        bool isOffsetTest = OffsetTest(oldRotIdx, rotationIndex);

        // If offset test failed, rotate back to the original position
        if (!isOffsetTest)
        {
            RotateTetromino(!clockwise);
        }
    }

    /// <summary>
    /// Perform 5 tests on the current tetromino to find a valid final position.
    /// </summary>
    /// <param name="oldRotIdx">Original rotation index of the current tetromino.</param>
    /// <param name="newRotIdx">New rotation index the current tetromino will be rotating to</param>
    /// <returns>Return true if any of the tests passed and a final location was found.</returns>
    bool OffsetTest(int oldRotIdx, int newRotIdx)
    {
        Vector2Int offsetVal1, offsetVal2, endOffset;
        Vector2Int[,] curOffsetData;

        // Assign offset data from SuperRotationSystemData gridInstance
        if (curType == TetrominoType.O)
        {
            curOffsetData = SuperRotationSystemData.srsInstance.OTetroOffset;
        }
        else if (curType == TetrominoType.I)
        {
            curOffsetData = SuperRotationSystemData.srsInstance.ITetroOffset;
        }
        else
        {
            curOffsetData = SuperRotationSystemData.srsInstance.OtherTetroOffset;
        }

        endOffset = Vector2Int.zero;

        bool isMovable = false;

        for (int testIndex = 0; testIndex < 5; testIndex++)
        {
            offsetVal1 = curOffsetData[testIndex, oldRotIdx];
            offsetVal2 = curOffsetData[testIndex, newRotIdx];
            endOffset = offsetVal1 - offsetVal2;

            if (IsTetrominoMovable(endOffset))
            {
                isMovable = true;
                break;
            }
        }

        // If the final location is found, move the tetromino to the position
        if (isMovable)
        {
            MoveTetromino(endOffset);
        }

        return isMovable;
    }

    /// <summary>
    /// Store the tetromino to the grid. 
    /// Then, check if the tetromino is out of the boundary.
    ///     if the tetromino is out of the boundary, set game over to true.
    ///     otherwise, check if the line is full and perform clear line operation.
    ///     Also, check if the T-Spin is performed.
    /// Spawn next tetromino.
    /// </summary>
    public void StoreTetrominoToGrid()
    {
        // Stop other sound first
        GameSystem.gsInstance.soundContrl.audioSource.Stop();

        // Play drop sound
        GameSystem.gsInstance.soundContrl.audioSource.PlayOneShot(GameSystem.gsInstance.soundContrl.dropSound);

        // Check if any of the current tetromino blocks is out of the grid boundary.
        for (int i = 0; i < blocks.Length; i++)
        {
            // If the block is out of the boundary, set gameOver to true.
            if (!blocks[i].SetBlock())
            {
                GameSystem.gsInstance.GameOver();
                tetroSystem.tetroController.enabled = false;
                return;
            }
        }

        // If not Game Over
        if (GameSystem.gsInstance.isGameOver == false)
        {
            // Stop drop coroutine
            StopTetrominoDrop();

            // Check if the current tetromino is a T-type and the last movement is rotation
            Debug.Log("The last movement is rotation: " + isLastRotation);

            if (curType == TetrominoType.T && isLastRotation == true)
            {
                Debug.Log("Check T-Spin");

                bool isValidTSpinPoints = GridSystem.gridInstance.IsValidTSpinPoint(curTetroController);

                // Check if the line needs to be clear
                GridSystem.gridInstance.CheckLines();

                // T-Spin is true if and only if the T-Spin points are valid and any of the T blocks have cleared the line.
                if (isValidTSpinPoints == true && GridSystem.gridInstance.isLineCleared == true)
                {
                    // If T-Spin is true, update the score
                    GameSystem.gsInstance.UpdateScore(0, true);
                };
            }
            else
            {
                // Check if the line needs to be clear for other types of tetrominoes
                GridSystem.gridInstance.CheckLines();
            }

            gameObject.GetComponent<TetrominoController>().isMovable = false;

            // Spawn new tetromino
            SpawnController.instance.SpawnTetromino();
        }
    }

    /// <summary>
    /// Update the position of the current tetromino blocks.
    /// </summary>
    /// <param name="location">Tetromino block position that needs to be updated.</param>
    public void UpdateBlocksPosition(Vector2Int location)
    {
        // Update position by the tetromino type
        switch (curType)
        {
            case TetrominoType.I:
                tetroSystem.tetroController.blocks[1].UpdatePosition(location + Vector2Int.left);
                tetroSystem.tetroController.blocks[2].UpdatePosition(location + (Vector2Int.right * 2));
                tetroSystem.tetroController.blocks[3].UpdatePosition(location + Vector2Int.right);
                break;

            case TetrominoType.J:
                tetroSystem.tetroController.blocks[1].UpdatePosition(location + Vector2Int.left);
                tetroSystem.tetroController.blocks[2].UpdatePosition(location + new Vector2Int(-1, 1));
                tetroSystem.tetroController.blocks[3].UpdatePosition(location + Vector2Int.right);
                break;

            case TetrominoType.L:
                tetroSystem.tetroController.blocks[1].UpdatePosition(location + Vector2Int.left);
                tetroSystem.tetroController.blocks[2].UpdatePosition(location + new Vector2Int(1, 1));
                tetroSystem.tetroController.blocks[3].UpdatePosition(location + Vector2Int.right);
                break;

            case TetrominoType.O:
                tetroSystem.tetroController.blocks[1].UpdatePosition(location + Vector2Int.right);
                tetroSystem.tetroController.blocks[2].UpdatePosition(location + new Vector2Int(1, 1));
                tetroSystem.tetroController.blocks[3].UpdatePosition(location + Vector2Int.up);
                break;

            case TetrominoType.S:
                tetroSystem.tetroController.blocks[1].UpdatePosition(location + Vector2Int.left);
                tetroSystem.tetroController.blocks[2].UpdatePosition(location + new Vector2Int(1, 1));
                tetroSystem.tetroController.blocks[3].UpdatePosition(location + Vector2Int.up);
                break;

            case TetrominoType.T:
                tetroSystem.tetroController.blocks[1].UpdatePosition(location + Vector2Int.left);
                tetroSystem.tetroController.blocks[2].UpdatePosition(location + Vector2Int.up);
                tetroSystem.tetroController.blocks[3].UpdatePosition(location + Vector2Int.right);
                break;

            case TetrominoType.Z:
                tetroSystem.tetroController.blocks[1].UpdatePosition(location + Vector2Int.up);
                tetroSystem.tetroController.blocks[2].UpdatePosition(location + new Vector2Int(-1, 1));
                tetroSystem.tetroController.blocks[3].UpdatePosition(location + Vector2Int.right);
                break;

            default:
                break;
        }
    }


    /// <summary>
    /// Hard Drop current tetromino.
    /// </summary>
    public void HardDropTetromino()
    {
        while (MoveTetromino(Vector2Int.down)) { }
    }

    /// <summary>
    /// Stop drop coroutine with UniRx.
    /// </summary>
    public void StopTetrominoDrop()
    {
        if (dropCoroutine == null)
            return;

        // Stop the coroutine of current tetromino
        dropCoroutine.Dispose();
    }

    /// <summary>
    /// Start the drop coroutine with UniRx
    /// </summary>
    public void StartTetrominoDrop()
    {
        dropCoroutine = Observable.FromCoroutine(DropEnumerator).Subscribe();
    }

    /// <summary>
    /// IEnumerator for tetromino auto drop.
    /// </summary>
    public IEnumerator DropEnumerator()
    {
        while (GameSystem.gsInstance.isGameOver == false)
        {
            MoveTetromino(Vector2Int.down);
            isLastRotation = false;
            yield return new WaitForSeconds(dropTime);
        }
    }

    /// <summary>
    /// Trigger to play tetromino movement sound.
    /// </summary>
    private void PlayTetrominoSound()
    {
        // Play Move Sound
        GameSystem.gsInstance.soundContrl.audioSource.PlayOneShot(GameSystem.gsInstance.soundContrl.moveSound);
    }

    #region Touch Controller
    /// <summary>
    /// Handles swipe control movement for mobile devices.
    /// </summary>
    private void SwipeHandler(Vector2 swipeMovement)
    {
        _swipeDirection = GetTouchDirection(swipeMovement);
    }

    /// <summary>
    /// Handles swipe control end event movement for mobile devices.
    /// </summary>
    private void SwipeEndHandler(Vector2 swipeMovement)
    {
        _swipeEndDirection = GetTouchDirection(swipeMovement);
    }

    /// <summary>
    /// Handles tap control movement for mobile devices.
    /// </summary>
    private void TapHandler(Vector2 tapPosition)
    {
        _isTapped = true;

        // Tapped left half side of the screen
        if(tapPosition.x < Screen.width / 2 && tapPosition.y > Screen.height / 4)
        {
            _isLeftTap = true;
        }
        // Tapped right half side of the screen
        else if(tapPosition.x > Screen.width / 2 && tapPosition.y > Screen.height / 4)
        {
            _isLeftTap = false;
        }

        // Tapped bottom half side of the screen
        else if (tapPosition.y < Screen.height / 4)
        {
            _isBottomTap = true;
        }
    }

    /// <summary>
    /// Get touch control swipe direction for mobile devices.
    /// </summary>
    TouchDirection GetTouchDirection(Vector2 swipeMovement)
    {
        TouchDirection swipeDir = TouchDirection.NONE;

        // Horizontal Swipe
        if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
        {
            swipeDir = (swipeMovement.x >= 0) ? TouchDirection.RIGHT : TouchDirection.LEFT;
        }

        // Vertical Swipe
        else
        {
            swipeDir = (swipeMovement.y >= 0) ? TouchDirection.UP : TouchDirection.DOWN;
        }

        return swipeDir;
    }
    #endregion Touch Controller
}
