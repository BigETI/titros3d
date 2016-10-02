using UnityEngine;
using System.Collections;
using System.Timers;

public class PointsManager : MonoBehaviour {

    public TextMesh textMesh;

    public ParticleSystem _particleSystem;

    private bool use_timer = false;

    private float time = 0.0f;

    public void updatePoints(long points)
    {
        if (textMesh != null)
            textMesh.text = "" + points;
        if (_particleSystem != null)
            _particleSystem.Play();
        use_timer = true;
    }

    void Awake()
    {
        if (_particleSystem != null)
            _particleSystem.Stop();
    }

    // Use this for initialization
    void Start () {
        if (_particleSystem != null)
            _particleSystem.Stop();
    }
	
	// Update is called once per frame
	void Update () {
        if (use_timer)
        {
            time += Time.deltaTime;
            if (time >= 0.1f)
            {
                if (_particleSystem != null)
                    _particleSystem.Stop();
                use_timer = false;
                time = 0.0f;
            }
        }
	}
}
