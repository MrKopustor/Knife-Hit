using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(GameUI))]
public class GameController : MonoBehaviour
{
    private static int score = 0;
    private static int highScore = 0;
    private static string path;
  
    public static GameController Instance { get; private set; }
    [SerializeField]
    private int knifeCount;

    [Header("Knife Spawning")]
    [SerializeField]
    private Vector2 knifeSpawnPosition;
    [SerializeField]
    private GameObject knifeObject;

    public GameUI GameUI { get; private set; }

    private void Awake()
    {
        Instance = this;
        GameUI = GetComponent<GameUI>();
    }

    private void Start()
    {
        GameUI.SetInitialDisplayedKnifeCount(knifeCount);
        SpawnKnife();
    }

    public void OnSuccessfulKnifeHit()
    {
        if (knifeCount > 0)
        {
            SpawnKnife();
        }
        else
        {
            StartGameOverSequence(true);
        }
    }

    private void SpawnKnife()
    {
        knifeCount--;
        Instantiate(knifeObject, knifeSpawnPosition, Quaternion.identity);
    }

    public void StartGameOverSequence(bool win)
    {
        StartCoroutine("GameOverSequenceCoroutine", win);
    }

    private IEnumerator GameOverSequenceCoroutine(bool win)
    {
        if (win)
        {
            //yield return new WaitForSecondsRealtime(0.3f);
            //RestartGame();
            GameObject.Find("TextWin").GetComponent<Text>().text = "YOU WIN!";
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            GameUI.ShowRestartButton();
        }
    }
    public void RestartGame()
    {
       SceneManager.LoadScene("Level1");
    }

    public static void SetScore(int newScore)
    {
        score += newScore;
        if (score > highScore)
            highScore = score;
    }

    public static void ResetScore()
    {
        score = 0;
    }

    public static int GetScore()
    {
        return score;
    }

    public static int GetHighScore()
    {
        return highScore;
    }

    public static void SaveHighScore()
    {
        if (score == highScore)
        {
            string strData = highScore.ToString();
            byte[] byteData = System.Text.Encoding.UTF8.GetBytes(strData);
            string[] data = { System.Convert.ToBase64String(byteData) };
            System.IO.File.WriteAllLines(path + "/Data.akh", data);
        }
    }

    void LoadHighScore()
    {
        try
        {
            string[] data = System.IO.File.ReadAllLines(path + "/Data.akh");

            if (data.Length > 0)
            {
                byte[] byteData = System.Convert.FromBase64String(data[0]);
                string strData = System.Text.Encoding.UTF8.GetString(byteData);
                highScore = System.Convert.ToInt32(strData);
            }
        }
        catch (System.Exception)
        {
        }
    }
}
