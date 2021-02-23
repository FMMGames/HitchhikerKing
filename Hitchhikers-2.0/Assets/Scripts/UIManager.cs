using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] GameObject mainScreen, inGameScreen, resultScreen, loadingScreen;
    [SerializeField] RectTransform scoreBoard;
    [SerializeField] GameObject scorePrefab;
    [SerializeField] Image levelProgressionBar;
    [SerializeField] TextMeshProUGUI resultTitle, playerScoreDisplay, mainLevelDisplay, inGameLevelDisplay, progressCurrentLevelDisplay, progressNextLevelDisplay;
    [SerializeField] TextMeshProUGUI tempRankDisplay, tempScoreDisplay;

    float temp;

    private void Awake()
    {
        if (!instance)
            instance = this;
    }

    private void Start()
    {
        UpdateScreens();

        EnableLoadingScreen();
    }

    public void EnableLoadingScreen()
    {
        loadingScreen.SetActive(true);
        Invoke("DisableLoadingScreen", 5f);
    }

    void DisableLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }

    private void Update()
    {
        if (GameManager.instance.currentGameState == GameState.InGame)
        {
            UpdateLevelProgressionBar();
        }
            UpdateScoreDisplay();
    }

    public void UpdateScreens()
    {
        if (GameManager.instance.currentGameState == GameState.MainScreen)
        {
            mainScreen.SetActive(true);
            inGameScreen.SetActive(false);
            resultScreen.SetActive(false);
        }
        else if (GameManager.instance.currentGameState == GameState.InGame)
        {
            mainScreen.SetActive(false);
            inGameScreen.SetActive(true);
            resultScreen.SetActive(false);
        }
        else if (GameManager.instance.currentGameState == GameState.EndScreen)
        {
            mainScreen.SetActive(false);
            inGameScreen.SetActive(false);
            resultScreen.SetActive(true);
        }

        mainLevelDisplay.text = "Level "+ GameManager.instance.currentLevel;
        inGameLevelDisplay.text = "Lv. " + GameManager.instance.currentLevel;
        progressCurrentLevelDisplay.text = GameManager.instance.currentLevel.ToString();
        progressNextLevelDisplay.text = (GameManager.instance.currentLevel+1).ToString();

        tempRankDisplay.text = " ";
        tempScoreDisplay.text = "0";
    }

    private void UpdateLevelProgressionBar()
    {
        levelProgressionBar.fillAmount = GameManager.instance.player.transform.position.z / GameManager.instance.generator.levelSize;
    }

    void UpdateScoreDisplay()
    {
        temp = Mathf.Lerp(temp, GameManager.instance.playerMoney, 5 * Time.deltaTime);
        temp = Mathf.CeilToInt(temp);

        playerScoreDisplay.text = temp.ToString();
    }

    public void UpdateResultScreen(bool result)
    {
        foreach (Transform child in scoreBoard)
        {
            Destroy(child.gameObject);
        }

        if (result)
            resultTitle.text = "VICTORY!!";
        else
            resultTitle.text = "DEFEAT!";
    }

    public void SpawnScoreElement(int rank, string name, int score)
    {
        GameObject s = Instantiate(scorePrefab, scoreBoard);
        s.GetComponent<ScoreUI>().SetupScore(rank, name, score);
    }

    public void ShowTemporaryRank()
    {      
        for (int i = 0; i < GameManager.instance.racers.Length; i++)
        {
            if (GameManager.instance.racers[i].name == GameManager.instance.playerName)
            {
                tempScoreDisplay.text = GameManager.instance.racerScores[i].ToString();
                tempRankDisplay.text = (i+1).ToString("0");
            }
        }
    }
}
