using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AnyKeyTo : MonoBehaviour {

    public string SceneName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.anyKeyDown || Input.touchCount > 0)
        {
            SceneManager.LoadScene(SceneName);
        }
	}
}
