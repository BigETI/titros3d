using UnityEngine;
using System.Collections;

public class SpectrumManager : MonoBehaviour {

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private Transform[] sprectumTransforms;

    [SerializeField]
    FFTWindow fftWindow;

    private float[] samples;

	// Use this for initialization
	void Start () {
        if (sprectumTransforms != null)
            samples = new float[sprectumTransforms.Length * 8];
	}
	
	// Update is called once per frame
	void Update () {
	    if ((audioSource != null) && (samples != null))
        {
            audioSource.GetSpectrumData(samples, 1, fftWindow);
            for (int i = 0, j; i < sprectumTransforms.Length; i++)
            {
                float size = 0.0f;
                for (j = 0; j < 8; j++)
                {
                    size += samples[(i * 8) + j];
                }
                size *= 100.0f;
                sprectumTransforms[i].localScale = new Vector3(1.0f, size + 1.0f, 1.0f);
            }
        }
	}
}
