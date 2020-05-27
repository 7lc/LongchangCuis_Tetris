/*
 * Developer Name: Longchang Cui
 * Date: May-23-2020
 */

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    // Static field
    public static GridSystem gridInstance;

    // One unit grid prefab
    public GameObject gridPrefab;

    // TetrisGrid size
    public int gridWidth;
    public int gridHeight;

    // TetrisGrid object
    private TetrisGrid[,] _grid;

    // Clear line effect
    public ParticleController[] clearLineFX;
    public bool isLineCleared = false;

    private void Awake()
    {
        gridInstance = this;
        CreateGrid();
    }

    /// <summary>
    /// Create a grid by the specific width and height
    /// </summary>
    private void CreateGrid()
    {
        _grid = new TetrisGrid[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                TetrisGrid newGridUnit = new TetrisGrid(gridPrefab, transform, x, y);
                _grid[x, y] = newGridUnit;
            }
        }
    }

    /// <summary>
    /// Check if the coorinate is a valid coordinate.
    /// </summary>
    /// <param name="coordinate">The x,y coordinate to test.</param>
    /// <returns>Return true if the coordinate parameter is a vaild coordinate on the grid.</returns>
    public bool IsInBounds(Vector2Int coordinate)
    {
        if (coordinate.x < 0 || coordinate.x >= gridWidth || coordinate.y < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Check if a given coordinate is occupied.
    /// </summary>
    /// <param name="coordinate">The x,y coordinate to test.</param>
    /// <returns>Return true if the coordinate is not occupied.</returns>
    public bool IsPositionEmpty(Vector2Int coordinate)
    {
        if (coordinate.y >= 20)
        {
            return true;
        }

        if (_grid[coordinate.x, coordinate.y].isOccupied)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    /// <summary>
    /// Set the grid location to be occupied if the tetromino is on the grid coordinates.
    /// </summary>
    /// <param name="coordinates">The x,y coordinates to be occupied.</param>
    /// <param name="block">GameObject of the specific block on this grid location.</param>
    public void OccupyPos(Vector2Int coordinates, GameObject block)
    {
        _grid[coordinates.x, coordinates.y].isOccupied = true;
        _grid[coordinates.x, coordinates.y].blockOnGrid = block;
    }

    /// <summary>
    /// Check lines from bottom to top. If the line is full, then clear the line.
    /// </summary>
    public void CheckLines()
    {
        // Index list of the lines that need to be cleared.
        List<int> linesToClear = new List<int>();

        // Counts how many lines next to each other will be cleared.
        int lineClearNum = 0;
        int lineScoreNum = 0;

        for (int y = 0; y < gridHeight; y++)
        {
            bool lineClear = true;
            for (int x = 0; x < gridWidth; x++)
            {
                if (!_grid[x, y].isOccupied)
                {
                    lineClear = false;
                    lineClearNum = 0;
                }
            }
            if (lineClear)
            {
                linesToClear.Add(y);
                ClearLine(y);
                isLineCleared = true;

                // Play particle effect after clear line
                ClearLineVFX(lineClearNum, y);
                lineClearNum++;
            }

            // Get the maximum line clear number for score update
            lineScoreNum = lineScoreNum > lineClearNum ? lineScoreNum : lineClearNum;
        }

        // Update Score for line clears
        GameSystem.gsInstance.UpdateScore(lineScoreNum, false);



        // When the lines have been cleared, the lines above those will drop to fill in the empty space
        if (linesToClear.Count > 0)
        {
            for (int i = 0; i < linesToClear.Count; i++)
            {
                for (int lineToDrop = linesToClear[i] + 1 - i; lineToDrop < gridHeight; lineToDrop++)
                {
                    for (int x = 0; x < gridWidth; x++)
                    {
                        TetrisGrid curGrid = _grid[x, lineToDrop];
                        if (curGrid.isOccupied)
                        {
                            MoveBlockDown(curGrid);
                        }
                    }
                }

            }
        }
    }

    /// <summary>
    /// Move a block down one unit.
    /// </summary>
    /// <param name="curGrid">The grid that contains the blocks to be moved down</param>
    void MoveBlockDown(TetrisGrid curGrid)
    {
        TetrominoBlockController curTile = curGrid.blockOnGrid.GetComponent<TetrominoBlockController>();
        curTile.MoveBlock(Vector2Int.down);
        curTile.SetBlock();

        // Set coordinate of the grid empty and set occupied false
        curGrid.blockOnGrid = null;
        curGrid.isOccupied = false;
    }

    /// <summary>
    /// Clear all blocks from a specified line.
    /// </summary>
    /// <param name="lineToClear">Index of the line to be cleared.</param>
    void ClearLine(int lineToClear)
    {
        if (lineToClear < 0 || lineToClear > gridHeight)
        {
            return;
        }

        for (int x = 0; x < gridWidth; x++)
        {
            TetrominoController curTetroContrl = _grid[x, lineToClear].blockOnGrid.GetComponent<TetrominoBlockController>().tetroController;
            curTetroContrl.blocks[_grid[x, lineToClear].blockOnGrid.GetComponent<TetrominoBlockController>().blockIdx] = null;
            Destroy(_grid[x, lineToClear].blockOnGrid);

            if (!curTetroContrl.AnyBlocksLeft())
                Destroy(curTetroContrl.gameObject);

            _grid[x, lineToClear].blockOnGrid = null;
            _grid[x, lineToClear].isOccupied = false;
        }

        // Play Clear line sound
        GameSystem.gsInstance.soundContrl.audioSource.PlayOneShot(GameSystem.gsInstance.soundContrl.clearLineSound);
    }


    /// <summary>
    /// Check if the last movement is rotation.
    /// Then, if any of the three points of the adjacent 3x3 T shape is occupied 
    /// </summary>
    public bool IsValidTSpinPoint(TetrominoController curTetroContrl)
    {
        // How T-Spin is determined?
        // Todo:
        // 1. Check if the current type is T shape and last movement is rotation
        // 2. If Three of the 4 squares diagonally adjacent to the T's center are occupied, return true as a valid TSpin; 
        //    In Tetris DS, the walls and floor surrounding the playfield are considered "occupied", 
        // 3. For each point, use IsInBound and IsOuccupied (IsInBound() == false || IsPositionEmpty() == false) to mark the point as valid point.
        // 4. Otherwise, return false;

        // More info:
        // https://tetris.fandom.com/wiki/T-Spin

        // Create vectors for diagonal check
        // There are two cases we need to consider.

        // Case 1: The flat side of the T surface facing left or right.
        // Case 2: The flat side of the T surface facing up or down.   

        // Case 1 Example:  (X is the point that needs to be checked.)
        //        X || X                    X || X
        //          ||====                ====||       
        //        X || X                    X || X

        // Case 2 Example: 
        //             X || X              X      X
        //            ========             ======== 
        //             X    X              X  ||  X

        // For Case 1:
        Vector2Int[] pointVecHorizontal = new Vector2Int[]
        {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0)
        };

        // For Case 2:
        Vector2Int[] pointVecVertical = new Vector2Int[]
        {
            new Vector2Int(0, 1),
            new Vector2Int(0,-1)
        };

        // Points count - If valid points are greater or equal to three, then the T-Spin is valid.
        int pointsCount = 0;

        // Case 1: Loop through blocks and check both side of the block 2 and block 4 points.
        // Case 2: Loop through blocks and check the points above block 2 and block 4, then check the points below block 2 and block 4
        for (int i = 1; i < 4; i += 2)
        {
            // Inside block 2 and block 4
            // Check points next 
            for (int j = 0; j < 2; j++)
            {
                // If block2.coordinates.x equals to block4.coordinates.x, then it's case 1
                if (curTetroContrl.blocks[1].coordinates.x == curTetroContrl.blocks[3].coordinates.x)
                {
                    Debug.Log("Inside T-Spin case 1 condition!!!");
                    Vector2Int pointToCheck = curTetroContrl.blocks[i].coordinates + pointVecHorizontal[j];

                    // If IsInBounds is false, the diagonal points are kicking the wall.
                    if (IsInBounds(pointToCheck) == false)
                    {
                        pointsCount++;
                    }
                    else
                    {
                        // If IsPositionEmpty is false, the diagonal points are occupied by other tetrominoes.
                        if (IsPositionEmpty(pointToCheck) == false)
                            pointsCount++;
                    }
                }

                // If block2.coordinates.x not equals to block4.coordinates.x, then it's case 2
                else
                {
                    Debug.Log("Inside T-Spin case 2 condition!!!");
                    Vector2Int pointToCheck = curTetroContrl.blocks[i].coordinates + pointVecVertical[j];

                    // If IsInBounds is false, the diagonal points are kicking the wall.
                    if (IsInBounds(pointToCheck) == false)
                    {
                        pointsCount++;
                    }
                    else
                    {
                        // If IsPositionEmpty is false, the diagonal points are occupied by other tetrominoes.
                        if (IsPositionEmpty(pointToCheck) == false)
                            pointsCount++;
                    }
                }
            }
        }

        Debug.Log("The T-Spin points count is " + pointsCount);

        // If three diagonal points are valid and any of the line is cleared by the T tetromino, then T-Spin is valid
        // The line cleared check is in TetrominoController.cs at line 350

        if (pointsCount >= 3)
        {
            // Set line cleared false for next round
            isLineCleared = false;
            return true;
        }
        else
        {
            // Set line cleared false for next round
            isLineCleared = false;
            return false;
        }
    }

    public void ClearLineVFX(int idx, int yVal)
    {
        if(clearLineFX[idx])
        {
            clearLineFX[idx].transform.position = new Vector3(0, yVal, -3);
            clearLineFX[idx].PlayParticle();
        }
    }

}
