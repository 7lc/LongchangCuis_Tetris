/*
 * Developer Name: Longchang Cui
 * Date: May-23-2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoBlockController : MonoBehaviour
{
    // Blocks Variable
    public Vector2Int coordinates;
    public int blockIdx;

    // Assign Components
    public GridSystem grid;
    public TetrominoSystem tetroSystem;
    public TetrominoController tetroController;

    void Start()
    {
        grid = GameObject.FindObjectOfType<GridSystem>();
        tetroSystem = GameObject.FindObjectOfType<TetrominoSystem>();
    }

    /// <summary>
    /// Initialize current tetromino blocks variables.
    /// </summary>
    /// <param name="tetroContrl">Reference to the TetrominoController that the blocks are associated with.</param>
    /// <param name="idx">Index of the blocks that attched to the current tetromino.</param>
    public void InitializeBlocks(TetrominoController tetroContrl, int idx)
    {
        tetroController = tetroContrl;
        blockIdx = idx;
    }

    /// <summary>
    /// Check to see if the blocks can be moved to the specified location.
    /// </summary>
    /// <param name="endPos">Vector2Int X, Y Coordinates of the position that the block moves to</param>
    /// <returns>Return true if the block can be moved to the specified location.</returns>
    public bool IsBlockMovable(Vector2Int endPos)
    {
        // If the grid is not occupied and the specified location is not out of the boundary, return true
        if (GridSystem.gridInstance.IsInBounds(endPos) == false)
        {
            return false;
        }
        if (GridSystem.gridInstance.IsPositionEmpty(endPos) == false)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Move the block by the specified amount.
    /// </summary>
    /// <param name="moveVec">Vector2Int amount of the movement that the block needs to move.</param>
    public void MoveBlock(Vector2Int moveVec)
    {
        Vector2Int endPos = coordinates + moveVec;
        UpdatePosition(endPos);
    }

    /// <summary>
    /// Set the blocks coordinates and the position by the new position vectors.
    /// </summary>
    /// <param name="newPos">The new position that the block needs to be updated.</param>
    public void UpdatePosition(Vector2Int newPos)
    {
        coordinates = newPos;
        Vector3 newV3Pos = new Vector3(newPos.x, newPos.y);

        if (gameObject != null)
            gameObject.transform.position = newV3Pos;
    }

    
    /// <summary>
    /// Set the block in it's current position.
    /// </summary>
    /// <returns>Return true if the block can be positioned on the grid. False, if the block is above playing field.</returns>
    public bool SetBlock()
    {
        // If the block is out of the boundary, return false. Game Over!
        if (coordinates.y >= 20)
        {
            return false;
        }

        GridSystem.gridInstance.OccupyPos(coordinates, gameObject);
        return true;
    }
    

    /// <summary>
    /// Rotate the block by 90 degrees at the specific direction.
    /// </summary>
    /// <param name="originPos">Coordinates of the block that will be rotating to.</param>
    /// <param name="clockwise">Set true if the block rotates clockwise.</param>
    public void RotateBlock(Vector2Int originPos, bool clockwise)
    {

        Vector2Int relativePos = coordinates - originPos;
        Vector2Int[] rotMatrix = clockwise ? new Vector2Int[2] { new Vector2Int(0, -1), new Vector2Int(1, 0) }
                                           : new Vector2Int[2] { new Vector2Int(0, 1), new Vector2Int(-1, 0) };
        int newXPos = (rotMatrix[0].x * relativePos.x) + (rotMatrix[1].x * relativePos.y);
        int newYPos = (rotMatrix[0].y * relativePos.x) + (rotMatrix[1].y * relativePos.y);
        Vector2Int newPos = new Vector2Int(newXPos, newYPos);

        newPos += originPos;
        UpdatePosition(newPos);
    }


}
