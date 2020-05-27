/*
 * Developer Name: Longchang Cui
 * Date: May-23-2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperRotationSystemData : MonoBehaviour
{
    public static SuperRotationSystemData srsInstance;

    // Tetromino block offset data is referenced from the following website
    // https://tetris.wiki/Super_Rotation_System
    // https://tetris.fandom.com/wiki/SRS

    public Vector2Int[,] OtherTetroOffset { get; private set; }
    public Vector2Int[,] ITetroOffset { get; private set; }
    public Vector2Int[,] OTetroOffset { get; private set; }

    // Start is called before the first frame update
    void Awake()
    {
        srsInstance = this;

        // Tetromino I Blocks Data
        #region Tetromino_I_Blocks_Data
        ITetroOffset = new Vector2Int[5, 4];
        ITetroOffset[0, 0] = Vector2Int.zero;
        ITetroOffset[0, 1] = new Vector2Int(-1, 0);
        ITetroOffset[0, 2] = new Vector2Int(-1, 1);
        ITetroOffset[0, 3] = new Vector2Int(0, 1);

        ITetroOffset[1, 0] = new Vector2Int(-1, 0);
        ITetroOffset[1, 1] = Vector2Int.zero;
        ITetroOffset[1, 2] = new Vector2Int(1, 1);
        ITetroOffset[1, 3] = new Vector2Int(0, 1);

        ITetroOffset[2, 0] = new Vector2Int(2, 0);
        ITetroOffset[2, 1] = Vector2Int.zero;
        ITetroOffset[2, 2] = new Vector2Int(-2, 1);
        ITetroOffset[2, 3] = new Vector2Int(0, 1);

        ITetroOffset[3, 0] = new Vector2Int(-1, 0);
        ITetroOffset[3, 1] = new Vector2Int(0, 1);
        ITetroOffset[3, 2] = new Vector2Int(1, 0);
        ITetroOffset[3, 3] = new Vector2Int(0, -1);

        ITetroOffset[4, 0] = new Vector2Int(2, 0);
        ITetroOffset[4, 1] = new Vector2Int(0, -2);
        ITetroOffset[4, 2] = new Vector2Int(-2, 0);
        ITetroOffset[4, 3] = new Vector2Int(0, 2);
        #endregion Tetromino_I_Blocks_Data

        // Tetromino O Blocks Data
        #region Tetromino_O_Blocks_Data
        OTetroOffset = new Vector2Int[1, 4];
        OTetroOffset[0, 0] = Vector2Int.zero;
        OTetroOffset[0, 1] = Vector2Int.down;
        OTetroOffset[0, 2] = new Vector2Int(-1, -1);
        OTetroOffset[0, 3] = Vector2Int.left;
        #endregion Tetromino_O_Blocks_Data

        // Tetromino J, L, T, S, Z Blocks Data
        #region Other_Tetromino_Blocks_Data
        OtherTetroOffset = new Vector2Int[5, 4];
        OtherTetroOffset[0, 0] = Vector2Int.zero;
        OtherTetroOffset[0, 1] = Vector2Int.zero;
        OtherTetroOffset[0, 2] = Vector2Int.zero;
        OtherTetroOffset[0, 3] = Vector2Int.zero;

        OtherTetroOffset[1, 0] = Vector2Int.zero;
        OtherTetroOffset[1, 1] = new Vector2Int(1, 0);
        OtherTetroOffset[1, 2] = Vector2Int.zero;
        OtherTetroOffset[1, 3] = new Vector2Int(-1, 0);

        OtherTetroOffset[2, 0] = Vector2Int.zero;
        OtherTetroOffset[2, 1] = new Vector2Int(1, -1);
        OtherTetroOffset[2, 2] = Vector2Int.zero;
        OtherTetroOffset[2, 3] = new Vector2Int(-1, -1);

        OtherTetroOffset[3, 0] = Vector2Int.zero;
        OtherTetroOffset[3, 1] = new Vector2Int(0, 2);
        OtherTetroOffset[3, 2] = Vector2Int.zero;
        OtherTetroOffset[3, 3] = new Vector2Int(0, 2);

        OtherTetroOffset[4, 0] = Vector2Int.zero;
        OtherTetroOffset[4, 1] = new Vector2Int(1, 2);
        OtherTetroOffset[4, 2] = Vector2Int.zero;
        OtherTetroOffset[4, 3] = new Vector2Int(-1, 2);
        #endregion Other_Tetromino_Block_Data
    }

}
