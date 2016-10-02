using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
        AudioSource _as = GetComponent<AudioSource>();
        if (_as != null)
            _as.Play();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClickGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OnClickHighscore()
    {
        SceneManager.LoadScene("Highscore");
    }

    public void OnClickExit()
    {
        Application.Quit();
    }

    public void OnHoverButton(Button button)
    {
        Animator a = button.GetComponent<Animator>();
        if (a != null)
        {
            a.Play("Hover");
        }
    }
}
