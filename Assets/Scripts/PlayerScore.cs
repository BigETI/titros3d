using UnityEngine;
using System.Collections;

public class PlayerScore {

    private string name = "";

    private long score = 0L;

    private uint level = 0U;

    public string Name
    {
        get
        {
            return name;
        }
    }

    public long Score
    {
        get
        {
            return score;
        }
    }

    public uint Level
    {
        get
        {
            return level;
        }
    }

    public PlayerScore(string name, long score, uint level)
    {
        this.name = name;
        this.score = score;
        this.level = level;
    }
}
