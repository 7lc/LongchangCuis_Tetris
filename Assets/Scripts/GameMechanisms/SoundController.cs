/*
 * Developer Name: Longchang Cui
 * Date: May-25-2020
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public AudioClip clearLineSound; // Sound triggered at line 205 in GridSystem.cs 
    public AudioClip moveSound;      // Sound triggered at line 81 in TetrominoController.cs 
    public AudioClip dropSound;      // Sound triggered at line 314 in TetrominoController.cs 

    public AudioClip gameOverSound;  // Sound triggered at line 113 in GameSystem.cs 
    public AudioClip levelUpSound;   // Sound triggered at line 104 in GameSystem.cs 
    public AudioClip backgroundMusic;

    public AudioSource audioSource;
}
