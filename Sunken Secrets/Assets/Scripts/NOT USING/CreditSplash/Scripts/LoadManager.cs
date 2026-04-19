using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : MonoBehaviour {

    public float timeToWait;
    public int sceneToLoad = 1;
	
    // Use this for initialization
	void Start () {
        Invoke("LoadNextScene", timeToWait);
	}
	
    /// <summary>
    /// Load the scene that was inputted.
    /// </summary>
	private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
