using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    void Awake()
    {
        Application.targetFrameRate = 30;
        SceneManager.LoadScene(1);
    }
}
