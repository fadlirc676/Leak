using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CutSceneController : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.LoadScene("Scene 1", LoadSceneMode.Single);
    }
}
