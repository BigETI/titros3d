using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class HighscoreManager : MonoBehaviour {

    [SerializeField]
    private PlayerScoreManager entryPanel;

    [SerializeField]
    private Transform parentEntryPanel;

    [SerializeField]
    private Animator[] animators;

    private Highscore highscore = null;

    private bool running = true;

    private float time = 0.0f;

    private bool still_running = true;

    public Highscore Highscore
    {
        get
        {
            if (highscore == null)
                highscore = new Highscore("titros3d.db");
            return highscore;
        }
    }

    void Start()
    {
        if (entryPanel != null)
        {
            int count = 1;
            foreach (PlayerScore ps in Highscore.BestPlayers)
            {
                PlayerScoreManager psm = Instantiate(entryPanel);
                if (psm != null)
                {
                    if (count >= 8)
                        break;
                    RectTransform rt = psm.GetComponent<RectTransform>();
                    if (rt != null)
                    {
                        rt.SetParent(parentEntryPanel);
                        rt.anchoredPosition = new Vector3(0, -80 * count);
                        rt.sizeDelta = new Vector3(0, 0, 0);
                    }
                    psm.PlayerScore = ps;
                }
                ++count;
            }
        }
        AudioSource _as = GetComponent<AudioSource>();
        if (_as != null)
            _as.Play();
    }

    void Update()
    {
        if (running)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                if (animators != null)
                {
                    foreach (Animator a in animators)
                    {
                        if (a != null)
                            a.Play("Destroy");
                    }
                }
                running = false;
            }
        }
        else if (still_running)
        {
            time += Time.deltaTime;
            if (time >= 1.0f)
            {
                still_running = false;
                SceneManager.LoadScene("Menu");
            }
        }
    }
}
