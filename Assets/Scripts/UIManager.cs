using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public PlayerController player;
    public Text ammoText;
    public Text winText;
    public Text nextLevelText;
    public Text quitText;
    public Text loseText;
    public Text retryText;
    public Text reloadText;

    private bool levelClear;
    public bool LevelClear
    {
        get
        {
            return levelClear;
        }

        set
        {
            levelClear = value;
        }
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;

        player = GameObject.FindObjectOfType<PlayerController>();

        if (player == null)
            return;

        if (winText == null && nextLevelText == null && quitText == null &&loseText == null && retryText == null)
            return; 

        winText.enabled = false;
        nextLevelText.enabled = false;
        quitText.enabled = false;
        ammoText.enabled = true;
        loseText.enabled = false;
        retryText.enabled = false;
        reloadText.enabled = false;
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        if(player != null)
            DisplayAmmo(player.bulletCount, player.ammoCount);
            

        if (levelClear)
            DisplayLevelClearText();
        else if((player.IsDead && !levelClear) || (player.bulletCount < 1 && !levelClear))
            DisplayLevelLoseText();
    }

    void DisplayAmmo(int ammo, int ammoCount)
    {
        ammoText.text = "Ammo: " + ammo + "/" + ammoCount;
    }

    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }

    public void DisplayLevelClearText()
    {
        winText.enabled = true;
        quitText.enabled = true;
        nextLevelText.enabled = true;
        ammoText.enabled = false;
        reloadText.enabled = false;
    }

    public void DisplayLevelLoseText()
    {
        loseText.enabled = true;
        retryText.enabled = true;
        quitText.enabled = true;
        ammoText.enabled = false;
        reloadText.enabled = false;
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void EnableReloadText()
    {
        reloadText.enabled = !reloadText.enabled;
    }
}
