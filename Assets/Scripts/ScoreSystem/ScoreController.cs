/*
 * Developer Name: Longchang Cui
 * Date: May-25-2020
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ScoreController : MonoBehaviour
{
    // Level numnber
    public int curLevel = 0;

    // Scoring dictionary
    public Dictionary<string, int> scoreDict;

    // Points
    public int points = 0;

    private void Awake()
    {

        // Initialize the dictionary for various score reward
        scoreDict = new Dictionary<string, int>()
        {
            { "OneLine", 40 * (curLevel + 1)},
            { "TwoLines", 100 * (curLevel + 1)},
            { "ThreeLines", 300 * (curLevel + 1)},
            { "FourLines", 1200 * (curLevel + 1)},
            { "TSpin", 400 * (curLevel + 1)}
        };
    }


    /// <summary>
    /// Add TSpin reward to current points.
    /// </summary>
    public void AddTSpinScore()
    {
        points += scoreDict["TSpin"];
    }

    /// <summary>
    /// Add line clear reward to current points.
    /// </summary>
    public void AddLineClearScore(string linesCleared)
    {
        points += scoreDict[linesCleared];
    }
}
