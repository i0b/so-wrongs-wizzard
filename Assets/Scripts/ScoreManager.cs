using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreItem : IComparable
{
    protected string name;
    protected int score;

    public ScoreItem(string name, int score)
    {
        this.name = name;
        this.score = score;
    }

    public int CompareTo(object obj)
    {
        if (obj == null) return 1;

        ScoreItem otherItem = obj as ScoreItem;
        if (otherItem != null)
            return this.score.CompareTo(otherItem.score);
        else
            throw new ArgumentException("Object is not a ScoreItem");
    }

    public string Name
    {
        get { return this.name; }
        set { this.name = value; }
    }

    public int Score
    {
        get { return this.score; }
        set { this.score = value; }
    }
}

public class ScoreManager : MonoBehaviour
{
    public Text firstPlayerScore;
    public Text firstPlayerName;
    public Text secondPlayerScore;
    public Text secondPlayerName;
    public Text thirdPlayerScore;
    public Text thirdPlayerName;
    public Text fourthPlayerScore;
    public Text fourthPlayerName;
    public Text fifthPlayerScore;
    public Text fifthPlayerName;

    public GameManager gameManager;
    public Text NameInput;

    ScoreItem[] scores;

    void Start()
    {
        scores = new ScoreItem[5];

        scores[0] = new ScoreItem(PlayerPrefs.GetString("FirstPlayerName", ""), PlayerPrefs.GetInt("FirstPlayerScore", 0));
        scores[1] = new ScoreItem(PlayerPrefs.GetString("SecondPlayerName", ""), PlayerPrefs.GetInt("SecondPlayerScore", 0));
        scores[2] = new ScoreItem(PlayerPrefs.GetString("ThirdPlayerName", ""), PlayerPrefs.GetInt("ThirdPlayerScore", 0));
        scores[3] = new ScoreItem(PlayerPrefs.GetString("FourthPlayerName", ""), PlayerPrefs.GetInt("FourthPlayerScore", 0));
        scores[4] = new ScoreItem(PlayerPrefs.GetString("FifthPlayerName", ""), PlayerPrefs.GetInt("FifthPlayerScore", 0));
    }

    public void UpdateScoreboard()
    {
        firstPlayerScore.text = scores[0].Score.ToString();
        firstPlayerName.text = scores[0].Name;

        secondPlayerScore.text = scores[1].Score.ToString();
        secondPlayerName.text = scores[1].Name;

        thirdPlayerScore.text = scores[2].Score.ToString();
        thirdPlayerName.text = scores[2].Name;

        fourthPlayerScore.text = scores[3].Score.ToString();
        fourthPlayerName.text = scores[3].Name;

        fifthPlayerScore.text = scores[4].Score.ToString();
        fifthPlayerName.text = scores[4].Name;
    }

    public void NewScore(string name, int score)
    {
        Array.Sort(this.scores);
        Array.Reverse(this.scores);

        scores[4].Name = name;
        scores[4].Score = score;

        Array.Sort(this.scores);
        Array.Reverse(this.scores);

        SaveScore();
    }

    public void SaveScore()
    {
        PlayerPrefs.SetString("FirstPlayerName", scores[0].Name);
        PlayerPrefs.SetInt("FirstPlayerScore", scores[0].Score);

        PlayerPrefs.SetString("SecondPlayerName", scores[1].Name);
        PlayerPrefs.SetInt("SecondPlayerScore", scores[1].Score);

        PlayerPrefs.SetString("ThirdPlayerName", scores[2].Name);
        PlayerPrefs.SetInt("ThirdPlayerScore", scores[2].Score);

        PlayerPrefs.SetString("FourthPlayerName", scores[3].Name);
        PlayerPrefs.SetInt("FourthPlayerScore", scores[3].Score);

        PlayerPrefs.SetString("FifthPlayerName", scores[4].Name);
        PlayerPrefs.SetInt("FifthPlayerScore", scores[4].Score);
    }

    public int GetMinScore()
    {
        return scores[4].Score;
    }
}