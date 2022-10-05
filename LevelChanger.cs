using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelChanger : MonoBehaviour
{
    public Animator Anim;
    private string sceneName;

    public void FadeToLevel(string InSceneName)
    {
        sceneName = InSceneName;
        Anim.SetTrigger("FadeOut");
    }
    public void OnFadeComplete()
    {
        if(sceneName == null)
        {
            ExitGame();
        }
        SceneManager.LoadScene(sceneName);
    }
    public void ExitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
