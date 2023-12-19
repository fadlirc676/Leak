using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    private static GameCanvas instance;

    private void Awake()
    {
        // Check if an instance of GameCanvas already exists
        if (instance == null)
        {
            // If not, set this as the instance
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy this object when loading a new scene
        }
        else
        {
            // If an instance already exists, destroy this one
            Destroy(gameObject);
        }
    }



    // You can add other methods or functionalities for your GameCanvas here
}
