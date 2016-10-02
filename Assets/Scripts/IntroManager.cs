using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour {

    [SerializeField]
    private Animator[] pieces;

    private bool no_start_game = true;

    private float time = 0.0f;

    private bool no_start = true;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	    if (no_start_game)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                no_start_game = false;
                foreach (Animator i in pieces)
                    i.Play("Destroy");
                AudioSource _as = GetComponent<AudioSource>();
                if (_as != null)
                    _as.Play();
            }
        }
        else
        {
            if (no_start)
            {
                time += Time.deltaTime;
                if (time >= 1.0f)
                {
                    no_start = false;
                    SceneManager.LoadSceneAsync("Menu");
                }
            }
        }
	}
}
