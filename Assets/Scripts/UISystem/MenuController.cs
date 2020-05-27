/*
 * Developer Name: Longchang Cui
 * Date: May-25-2020
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    public static MenuController instance;

    public RectTransform creditMenu;
    public RectTransform gameMenu;
    public RectTransform loadMenu;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    /// <summary>
    /// Trigger to load the specified scene.
    /// </summary>
    /// <param name="sceneName">The scene name that needs to be loaded in string.</param>
    public void LoadGame(string sceneName)
    {
        // Load Game Scene
        SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
    }

    /// <summary>
    /// Show the developer's name. - Longchang Cui if the Credit button is clicked.
    /// </summary>
    public void ShowCredit()
    {
        creditMenu.DOAnchorPos(new Vector2(0,0), 0.3f);
        // Slide right animation
        gameMenu.DOAnchorPos(new Vector2(1500, 0), 0.3f);
    }

    /// <summary>
    /// Show the game menu if the back button is clicked from the Credit menu.
    /// </summary>
    public void ShowGameMenu()
    {
        gameMenu.DOAnchorPos(new Vector2(0,0), 0.3f);

        // Slide left animation
        creditMenu.DOAnchorPos(new Vector2(-1500, 0), 0.3f);
    }

}
