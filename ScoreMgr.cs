using UnityEngine;
using UnityEngine.UI;


public class ScoreMgr : MonoBehaviour
{
    public static ScoreMgr Instance;

    public int HighScore = 0;
    public int YourScore = 0;
    public Text HighScoreText;
    public Text YourScoreText;

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = GetComponent<ScoreMgr>();
    }
    private void Update()
    {
        YourScoreText.text = PlayerPrefs.GetInt("Score").ToString();
        HighScoreText.text = "Best: " + PlayerPrefs.GetInt("HighScore").ToString();
    }
}