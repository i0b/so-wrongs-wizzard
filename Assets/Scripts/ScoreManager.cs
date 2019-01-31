using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ScoreItem : IComparable
{
    protected string name;
    protected int score;

    // ScoreItem class
    // sorting it sorts by ascending score value
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

// the score class hold five score values and
// has a reference to the text field representations
// allowing update and insertion operations

public class ScoreManager : MonoBehaviour
{
    public Text FirstPlayerScore;
    public Text FirstPlayerName;
    public Text SecondPlayerScore;
    public Text SecondPlayerName;
    public Text ThirdPlayerScore;
    public Text ThirdPlayerName;
    public Text FourthPlayerScore;
    public Text FourthPlayerName;
    public Text FifthPlayerScore;
    public Text FifthPlayerName;

    public Text NameInput;

    public static ScoreManager Instance;

    private GameManager gameManager;
    private ScoreItem[] scores;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        scores = new ScoreItem[5];

        scores[0] = new ScoreItem(PlayerPrefs.GetString("FirstPlayerName", ""), PlayerPrefs.GetInt("FirstPlayerScore", 0));
        scores[1] = new ScoreItem(PlayerPrefs.GetString("SecondPlayerName", ""), PlayerPrefs.GetInt("SecondPlayerScore", 0));
        scores[2] = new ScoreItem(PlayerPrefs.GetString("ThirdPlayerName", ""), PlayerPrefs.GetInt("ThirdPlayerScore", 0));
        scores[3] = new ScoreItem(PlayerPrefs.GetString("FourthPlayerName", ""), PlayerPrefs.GetInt("FourthPlayerScore", 0));
        scores[4] = new ScoreItem(PlayerPrefs.GetString("FifthPlayerName", ""), PlayerPrefs.GetInt("FifthPlayerScore", 0));
    }

    // sets text field values to internal score data set
    public void UpdateScoreboard()
    {
        FirstPlayerScore.text = scores[0].Score.ToString();
        FirstPlayerName.text = scores[0].Name;

        SecondPlayerScore.text = scores[1].Score.ToString();
        SecondPlayerName.text = scores[1].Name;

        ThirdPlayerScore.text = scores[2].Score.ToString();
        ThirdPlayerName.text = scores[2].Name;

        FourthPlayerScore.text = scores[3].Score.ToString();
        FourthPlayerName.text = scores[3].Name;

        FifthPlayerScore.text = scores[4].Score.ToString();
        FifthPlayerName.text = scores[4].Name;
    }

    // adds new score by removing the smallest valued score keeping the score sorted
    public void NewScore(string name, int score)
    {
        if (name.Equals(""))
        {
            return;
        }

        Array.Sort(this.scores);
        Array.Reverse(this.scores);

        scores[4].Name = name;
        scores[4].Score = score;

        Array.Sort(this.scores);
        Array.Reverse(this.scores);

        SaveScore();
    }

    // save score persistanly
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

    // delete all score entries / resets score
    public void ResetScore()
    {
        PlayerPrefs.DeleteAll();
    }
}