using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class BaseGameLogic : MonoBehaviour
{
    public Animator Anim;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Anim.GetComponent<LevelChanger>().FadeToLevel("Menu");
        }
    }

}
