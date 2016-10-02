using UnityEngine;
using System.Collections;

public class RandomSphereSpectrumManager : MonoBehaviour {

    [SerializeField]
    private GameObject[] selection;

    [SerializeField]
    private float radius;

    [SerializeField]
    private uint randomCount;

    [SerializeField]
    private float visualizationStrength = 0.1f;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private FFTWindow fftWindow;

    private Transform[] loadedSpectrum;

    private float[] samples = new float[64];

    private float last_size = 1.0f;

    // Use this for initialization
    void Start () {
        if ((selection != null) && (randomCount > 0))
        {
            loadedSpectrum = new Transform[randomCount];
            for (int i = 0; i < loadedSpectrum.Length; i++)
            {
                GameObject g = Instantiate(selection[Random.Range(0, selection.Length)]);
                g.transform.parent = transform;
                Vector3 rot = new Vector3(Random.Range(-360.0f, 360.0f), Random.Range(-360.0f, 360.0f), Random.Range(-360.0f, 360.0f));
                g.transform.localPosition = rot.normalized * radius;
                g.transform.localEulerAngles = rot;
                loadedSpectrum[i] = g.transform;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if ((audioSource != null) && (samples != null) && (loadedSpectrum != null))
        {
            audioSource.GetSpectrumData(samples, 0, fftWindow);
            last_size = 0.0f;
            for (int i = 0; i < samples.Length; i++)
                last_size += samples[i];
            last_size = (last_size * visualizationStrength) + 1.0f;
            //for (int i = 0; i < loadedSpectrum.Length; i++)
            //loadedSpectrum[i].localScale = new Vector3(1.0f, last_size, 1.0f);
            transform.localScale = new Vector3(last_size, last_size, last_size);
        }
    }

    void OnDrawGizmos()
    {
        //
    }
}
