/*
 * Developer Name: Longchang Cui
 * Date: May-23-2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    public static GameSystem gsInstance;

    // System Controller
    private GridSystem _gridSystem;
    private SpawnController _spawnContrl;
    private ScoreController _scoreContrl;
    private TetrominoSystem _tetroSystem;
    public SoundController soundContrl;

    // GameOver variable
    public bool isGameOver = false;

    // Current game level
    public int gameLevel;
    private int _nextLevelThreshold;

    // Game UI
    // Update points UI
    public TextMeshProUGUI pointsText;
    public TextMeshProUGUI levelText;
    // Level up UI
    public RectTransform levelUpText;

    // Pause game UI
    public GameObject pausePanel;

    // GameOver UI 
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverScoreText;

    // Start is called before the first frame update
    void Start()
    {
        gsInstance = this;

        // Assign system components
        _gridSystem = GameObject.FindObjectOfType<GridSystem>();
        _spawnContrl = GameObject.FindObjectOfType<SpawnController>();
        _scoreContrl = GameObject.FindObjectOfType<ScoreController>();
        soundContrl = GameObject.FindObjectOfType<SoundController>();
        _tetroSystem = GameObject.FindObjectOfType<TetrominoSystem>();

        // Assign value from score controller
        gameLevel = _scoreContrl.curLevel;

        // Set first level threshold
        _nextLevelThreshold = 1000;
    }

    /// <summary>
    /// Update the current points in the game.
    /// </summary>
    /// <param name="lines">Number of lines that are cleared</param>
    /// <param name="isTSpin">True, if the player performed T-Spin</param>
    public void UpdateScore(int lines, bool isTSpin)
    {
        // Add score for T-Spin
        if (isTSpin == true)
        {
            _scoreContrl.AddTSpinScore();
        }

        // Add score for line clear
        switch (lines)
        {
            case 1:
                _scoreContrl.AddLineClearScore("OneLine");
                break;

            case 2:
                _scoreContrl.AddLineClearScore("TwoLines");
                break;

            case 3:
                _scoreContrl.AddLineClearScore("ThreeLines");
                break;

            case 4:
                _scoreContrl.AddLineClearScore("FourLines");
                break;

            default:
                break;
        }

        // Check if the points reach the level up threshold
        // If true, upgrade level
        if (_scoreContrl.points >= _nextLevelThreshold)
        {
            LevelUp();
        }

        pointsText.text = _scoreContrl.points.ToString();
    }


    /// <summary>
    /// When the points greater than certain threshold, upgrade the level.
    /// </summary>
    public void LevelUp()
    {
        gameLevel++;
        _scoreContrl.curLevel = gameLevel;

        // Set next level threshold
        _nextLevelThreshold = _nextLevelThreshold * 2;

        // Show level up text and play level up sound
        // Stop other sound first
        soundContrl.audioSource.Stop();
        // Play level up sound
        soundContrl.audioSource.PlayOneShot(soundContrl.levelUpSound);

        // Update current level Text
        levelText.text = gameLevel.ToString();

        PlayLevelUpAnimation();
    }

    /// <summary>
    /// Trigger to play level up UI animation.
    /// </summary>
    public void PlayLevelUpAnimation()
    {
        Sequence levelUpTweenSeq = DOTween.Sequence();
        // Add a movement tween at the beginning
        levelUpTweenSeq.Append(levelUpText.DOAnchorPos(new Vector2(0, 35), 0.3f));
        levelUpTweenSeq.AppendInterval(3);
        // Add a rotation tween as soon as the previous one is finished
        levelUpTweenSeq.Append(levelUpText.DOAnchorPos(new Vector2(0, -200), 0.3f));
    }

    /// <summary>
    /// Set game over and stop tetromino drop coroutine
    /// </summary>
    public void GameOver()
    {
        isGameOver = true;

        // Stop other sound first
        soundContrl.audioSource.Stop();
        // Play game over sound
        soundContrl.audioSource.PlayOneShot(soundContrl.gameOverSound);

        // Show Game Over UI
        // Turn off the pause panel if it is open
        if(pausePanel.activeSelf == true)
            pausePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        // Set game over score text
        gameOverScoreText.text = "Score: " + pointsText.text;
    }


    /// <summary>
    /// Trigger the function to pause the game.
    /// </summary>
    public void GameIsPaused()
    {
        Time.timeScale = 0;
        _tetroSystem.tetroController.isMovable = false;
        pausePanel.SetActive(true);
    }

    /// <summary>
    /// Trigger the function to continue the game.
    /// </summary>
    public void GameIsContinued()
    {
        Time.timeScale = 1;
        _tetroSystem.tetroController.isMovable = true;
        pausePanel.SetActive(false);
    }

    /// <summary>
    /// Trigger the function to quit the game.
    /// </summary>
    public void QuitGame(string sceneName)
    {
        Time.timeScale = 1;

        // Trun off the current tetromino and blocks movement before scene loading to handle error message.
        _tetroSystem.tetroController.StopTetrominoDrop();

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

    }

    /// <summary>
    /// Reload the scene if the reload button is clicked.
    /// </summary>
    /// <param name="sceneName">The name of the game scene.</param>
    public void ReloadGameScene(string sceneName)
    {
        Time.timeScale = 1;

        // Trun off the current tetromino and blocks movement before scene loading to handle error message.
        _tetroSystem.tetroController.StopTetrominoDrop();

        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

}
