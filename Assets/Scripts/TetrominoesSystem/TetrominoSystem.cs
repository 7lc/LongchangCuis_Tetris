/*
 * Developer Name: Longchang Cui
 * Date: May-23-2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Tetromino Types
public enum TetrominoType { I, O, J, L, S, T, Z }
public class TetrominoSystem: MonoBehaviour
{
    // Current tetromino object
    public GameObject currentTetromino;
    public TetrominoController tetroController;

    // List to track of the spawned tetromino objects
    public List<GameObject> tetrominoesInGame;

    // Next tetromino object
    public Sprite[] tetrominoSprite;
    public Image nextImage;

    // Hold tetromino object
    public GameObject holdTetromino;
    public Image holdImage;

    // Tetromino type dictionary
    public Dictionary<TetrominoType, Sprite> typeDictionary;

    private void OnEnable()
    {
        // Initialize Tetromino Type dictionary to manage "Hold Tetromino" and "Next Tetromino" operation
        typeDictionary = new Dictionary<TetrominoType, Sprite>()
        {
            { TetrominoType.I, tetrominoSprite[0] },
            { TetrominoType.J, tetrominoSprite[1] },
            { TetrominoType.L, tetrominoSprite[2] },
            { TetrominoType.O, tetrominoSprite[3] },
            { TetrominoType.S, tetrominoSprite[4] },
            { TetrominoType.T, tetrominoSprite[5] },
            { TetrominoType.Z, tetrominoSprite[6] },
        };
    }

    /// <summary>
    /// Add Next tetromino sprite to the "Next" box UI.
    /// </summary> 
    /// <param name="tetrominoType"> Tetromino type input for the "Next Tetromino" operation</param>
    public void AddNextTetrominoSprite(TetrominoType tetrominoType)
    {
        nextImage.sprite = typeDictionary[tetrominoType];
    }

    /// <summary>
    /// Hold the current tetromino in the game scene for later use.
    /// </summary>
    public void HoldCurrentTetromino()
    {
        // Hold is only possible when the tetromino location is above the middle line of the play field
        if(tetroController.isHoldable == true && tetroController.blocks[0].coordinates.y >= 11)
        {
            // Initial hold
            if (holdTetromino == null)
            {
                tetroController.isHoldable = false;

                // Stop the current tetromino coroutine
                tetroController.StopTetrominoDrop();

                // Assign current tetromino
                holdTetromino = currentTetromino;

                // Turn off the tetromino controller that on hold
                holdTetromino.GetComponent<TetrominoController>().enabled = false;

                // Move the hold tetromino out of the grid
                holdTetromino.transform.position = new Vector3(10, 25, 0);

                // Add tetromino sprite to hold box
                AddHoldTetrominoSprite(holdTetromino.GetComponent<TetrominoController>().curType);

                // Spawn the next tetromino
                SpawnController.instance.SpawnTetromino();
            }
            // Subsequent hold - Swtich the current tetromino and hold tetromino
            else
            {
                tetroController.isHoldable = false;

                // Assign temporaly game object before switch
                GameObject tmpTetromino = currentTetromino;
                Vector2Int[] blocksCoordinates = tmpTetromino.GetComponent<TetrominoController>().GetBlocksCoordinates();

                // Swap is possible if the surrounding grid of the current block is empty
                bool isGridEmpty = false;
                Vector2Int[] surroundVec = new Vector2Int[] {
                    new Vector2Int(0, -1),
                    new Vector2Int(0, 1),
                    new Vector2Int(-1, 0),
                    new Vector2Int(1, 0) };

                // If the blocks in bound and 
                for (int i = 0; i < blocksCoordinates.Length; i++)
                {
                    foreach (var item in surroundVec)
                    {
                        // Check if the current block surrounding grid are empty
                        int newX = blocksCoordinates[i].x + item.x;
                        int newY = blocksCoordinates[i].y + item.y;

                        if(GridSystem.gridInstance.IsInBounds(new Vector2Int(newX, newY)))
                            isGridEmpty = GridSystem.gridInstance.IsPositionEmpty(new Vector2Int(newX, newY));

                        if (!isGridEmpty)
                            break;
                    }
                }

                // If surrounding are empty. Perform the swap operation
                if(isGridEmpty)
                {
                    // Turn off the current tetromino coroutine
                    tetroController.StopTetrominoDrop();

                    // Assign current tetromino the hold with tmp tetromino
                    holdTetromino.SetActive(true);
                    currentTetromino = holdTetromino;
                    currentTetromino.GetComponent<TetrominoController>().enabled = true;
                    tetroController = currentTetromino.GetComponent<TetrominoController>();
                    currentTetromino.transform.position = tmpTetromino.transform.position;

                    // Update the current blocks position after swap
                    Vector2Int newPos = new Vector2Int((int)blocksCoordinates[1].x, (int)blocksCoordinates[1].y);

                    // Check if updating the current blocks position after swap would out of the grid boundary.
                    int leftBound = Mathf.Abs(newPos.x - 0);
                    int rightBound = Mathf.Abs(newPos.x - 10);

                    bool isLeftBound = leftBound < 2 ? true : false;
                    bool isRightBound = rightBound <= 2 ? true : false;

                    // If none of the blocks is out of the boundary, update the tetromino at the new position.
                    if (isLeftBound == false && isRightBound == false)
                    {
                        tetroController.blocks[0].UpdatePosition(newPos);
                        tetroController.UpdateBlocksPosition(newPos);
                    }

                    // If any of the blocks position is outside of the right boundary, move the new position X coordinate left 1 unit
                    else if (isLeftBound == false && isRightBound == true)
                    {
                        newPos.x -= 1;

                        // Edge case: if the current tetromino type is I type
                        // Then move the new position X coordinate 1 unit further
                        if (tetroController.curType == TetrominoType.I)
                            newPos.x -= 1;

                        tetroController.blocks[0].UpdatePosition(newPos);
                        tetroController.UpdateBlocksPosition(newPos);
                    }

                    // If any of the blocks position is outside of the left boundary, move the new position X coordinate right 1 unit
                    else if (isLeftBound == true && isRightBound == false)
                    {
                        newPos.x += 1;
                        tetroController.blocks[0].UpdatePosition(newPos);
                        tetroController.UpdateBlocksPosition(newPos);
                    }

                    // Switch the current tetromino and on hold tetromino
                    holdTetromino = tmpTetromino;

                    // Update hold tetromino position
                    holdTetromino.GetComponent<TetrominoController>().enabled = false;
                    AddHoldTetrominoSprite(holdTetromino.GetComponent<TetrominoController>().curType);
                    holdTetromino.transform.position = new Vector3(10, 25, 0);

                    // Temporarily turn off the hold object
                    holdTetromino.SetActive(false);

                    // Start current tetromino drop after switch
                    tetroController.StartTetrominoDrop();
                }
                
            }
        }
        
    }

    /// <summary>
    /// Add Hold tetromino sprite to the "Hold" box UI.
    /// </summary> 
    /// <param name="tetrominoType"> Tetromino type input for the "Next Tetromino" operation</param>
    public void AddHoldTetrominoSprite(TetrominoType tetrominoType)
    {
        holdImage.sprite = typeDictionary[tetrominoType];
    }

    /// <summary>
    /// Remove the specified tetromino from the list of current tetrominoes in the game.
    /// </summary>
    /// <param name="tetrominoToRemove">Game Object of the Tetris piece to be removed.</param>
    public void RemoveTetromino(GameObject tetrominoToRemove)
    {
        tetrominoesInGame.Remove(tetrominoToRemove);
    }

}
