using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class GUIManager : MonoBehaviour 
{
	public static GUIManager Instance;

	public Animator Anim;
	public Text MyScore;

	public Text ScoreText;
	public Text MovesCounterText;
	public int MovesCounter = 50;

	private int score;

	public int Score
    {
		get
        {
			return score;
        }
        set
        {
			score = value;
			ScoreText.text = score.ToString();
        }
    }

	void Awake() 
	{
		Instance = GetComponent<GUIManager>();
		MovesCounterText.text = MovesCounter.ToString();
	}
	private void Update()
    {
		MovesCounterText.text = MovesCounter.ToString();
		if(MovesCounter <= 0)
        {
			StartCoroutine(WaitForShiftOver());
        }
	}

	// Show the game over panel
	public void GameOver() 
	{
        //GameOverPanel.SetActive(true);


        if (score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
		PlayerPrefs.SetInt("Score", score);
        

		MyScore.text = score.ToString();
		Anim.GetComponent<LevelChanger>().FadeToLevel("GameOver");
	}

	private IEnumerator WaitForShiftOver()
    {
		yield return new WaitUntil(() => !BoardManager.Instance.IsShifting);
		yield return new WaitForSeconds(0.3f);
		GameOver();
    }
}
