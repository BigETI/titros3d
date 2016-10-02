using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerScoreManager : MonoBehaviour {

    [SerializeField]
    private Text nameText;

    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Text levelText;

    public PlayerScore PlayerScore
    {
        set
        {
            if (value != null)
            {
                if (nameText != null)
                    nameText.text = value.Name;
                if (scoreText != null)
                    scoreText.text = value.Score.ToString();
                if (levelText != null)
                    levelText.text = value.Level.ToString();
            }
        }
    }
}
