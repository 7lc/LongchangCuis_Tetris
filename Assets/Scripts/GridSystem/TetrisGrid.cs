/*
 * Developer Name: Longchang Cui
 * Date: May-23-2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Grid Property
/// </summary>
public class TetrisGrid
{
    // Single unit grid square properties
    public GameObject gameObject { get; private set; }
    public GameObject blockOnGrid;
    public Vector2Int location { get; private set; }
    public bool isOccupied;

    public TetrisGrid(GameObject newGameObject, Transform objectParent, int x, int y)
    {
        gameObject = GameObject.Instantiate(newGameObject, objectParent);

        location = new Vector2Int(x, y);
        gameObject.transform.position = new Vector3(location.x, location.y);

        isOccupied = false;
    }
}
