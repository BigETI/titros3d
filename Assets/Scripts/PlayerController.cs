using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private GridManager mainPiecesGrid;

    [SerializeField]
    private GridManager nextPiecesGrid;

    [SerializeField]
    private GridManager holdingPiecesGrid;

    [SerializeField]
    private GridManager suggestingPiecesGrid;

    [SerializeField]
    private PointsManager scorePointsManager;

    [SerializeField]
    private PointsManager levelPointsManager;

    [SerializeField]
    private AudioClip clearLinesAudioClip;

    [SerializeField]
    private AudioClip singleAudioClip;

    [SerializeField]
    private AudioClip doubleAudioClip;

    [SerializeField]
    private AudioClip tripleAudioClip;

    [SerializeField]
    private AudioClip unbelievableAudioClip;

    [SerializeField]
    private AudioClip moveAudioClip;

    [SerializeField]
    private AudioClip dropAudioClip;

    [SerializeField]
    private AudioClip dropFastAudioClip;

    [SerializeField]
    private AudioClip rotateAudioClip;

    [SerializeField]
    private AudioClip gameAudioClip;

    [SerializeField]
    private AudioClip gameOverAudioClip;

    [SerializeField]
    private AudioClip pauseAudioClip;

    [SerializeField]
    private AudioSource[] soundEffectsAudioSources;

    [SerializeField]
    private RectTransform pausePanel;

    [SerializeField]
    private RectTransform enterNamePanel;

    [SerializeField]
    private Text nameInputText;

    private uint current_audio_source = 0U;

    private long score = 0;

    private ulong cleared_lines = 0;

    private uint last_level = 0;
    
    private float deltaTime = 0.0f;

    private float time = 2.0f;

    private float clear_pause = 0.0f;

    private ATetrimino next_tetrimino = null;

    private ATetrimino holding_tetrimino = null;

    private bool moved = false;

    private bool smashed = false;

    private bool moved_down = false;

    private bool running = true;

    private bool paused = false;

    public long Score
    {
        get
        {
            return score;
        }
        set
        {
            if (score != value)
            {
                score = value;
                if (scorePointsManager != null)
                    scorePointsManager.updatePoints(score);
            }
        }
    }

    public ulong ClearedLines
    {
        get
        {
            return cleared_lines;
        }
        set
        {
            ulong delta_cleared = value - cleared_lines;
            cleared_lines = value;
            uint l = Level;
            if (last_level != l)
            {
                last_level = l;
                if (levelPointsManager != null)
                    levelPointsManager.updatePoints(Level);
            }
            switch (delta_cleared)
            {
                case 1:
                    Score += 40 * (Level + 1);
                    break;
                case 2:
                    Score += 100 * (Level + 1);
                    break;
                case 3:
                    Score += 300 * (Level + 1);
                    break;
                case 4:
                    Score += 1200 * (Level + 1);
                    break;
            }
        }
    }

    public uint Level
    {
        get
        {
            uint ret = 0;
            ulong cleared_lines = this.cleared_lines;
            while (cleared_lines >= 10UL)
            {
                cleared_lines -= 10;
                ++ret;
            }
            time = 2.0f - (ret * 0.2f);
            if (time <= 0.0f)
                time = 0.1f;
            return ret;
        }
    }

    public ATetrimino NextTetrimino
    {
        get
        {
            return next_tetrimino;
        }
        set
        {
            int im = value.Pieces.GetUpperBound(0);
            int jm = value.Pieces.GetUpperBound(1);
            if ((im == nextPiecesGrid.PiecesField.GetUpperBound(0)) && (jm == nextPiecesGrid.PiecesField.GetUpperBound(1)))
            {
                for (int i = 0, j; i <= im; i++)
                {
                    for (j = 0; j <= jm; j++)
                    {
                        PieceLoader pl = nextPiecesGrid.PiecesField[i, j];
                        pl.Piece = value.Pieces[i, j];
                        if (pl.LoadedPiece != null)
                            pl.LoadedPiece.transform.localPosition = new Vector3(i, j, 0);
                    }
                }
            }
            next_tetrimino = value;
        }
    }

    public ATetrimino HoldingTetrimino
    {
        get
        {
            return holding_tetrimino;
        }
        set
        {
            int im = value.Pieces.GetUpperBound(0);
            int jm = value.Pieces.GetUpperBound(1);
            if ((im <= holdingPiecesGrid.PiecesField.GetUpperBound(0)) && (jm <= holdingPiecesGrid.PiecesField.GetUpperBound(1)))
            {
                for (int i = 0, j; i <= im; i++)
                {
                    for (j = 0; j <= jm; j++)
                    {
                        PieceLoader pl = holdingPiecesGrid.PiecesField[i, j];
                        pl.Piece = value.Pieces[i, j];
                        if (pl.LoadedPiece != null)
                            pl.LoadedPiece.transform.localPosition = new Vector3(i, j, 0);
                    }
                }
            }
            holding_tetrimino = value;
            if (holding_tetrimino != null)
            {
                holdingPiecesGrid.transform.localPosition = new Vector3(holding_tetrimino.X, holding_tetrimino.Y, 0);
                for (int i = 0, j; i <= im; i++)
                {
                    for (j = 0; j <= jm; j++)
                    {
                        PieceLoader pl = suggestingPiecesGrid.PiecesField[i, j];
                        pl.Piece = ETetriminos.None;
                        pl.Piece = holdingPiecesGrid.PiecesField[i, j].Piece;
                        if (pl.LoadedPiece != null)
                            pl.LoadedPiece.transform.localPosition = new Vector3(i, j, 0);
                    }
                }
                if (!(holding_tetrimino is Tetriminos.NoTetrimino))
                {
                    ATetrimino suggesting_tetrimino = holding_tetrimino;
                    ATetrimino t = suggesting_tetrimino;
                    while (validateHold(t))
                    {
                        suggesting_tetrimino = t;
                        t = t.move(0, -1);
                    }
                    suggestingPiecesGrid.transform.localPosition = new Vector3(suggesting_tetrimino.X, suggesting_tetrimino.Y, 0);
                }
            }
        }
    }

    public void respawnHoldingTetrimino(ATetrimino next_tetrimino)
    {
        int x = (mainPiecesGrid.PiecesField.GetUpperBound(0) - holdingPiecesGrid.PiecesField.GetUpperBound(0) - 1) / 2;
        int y = mainPiecesGrid.PiecesField.GetUpperBound(1) - 2;
        HoldingTetrimino = next_tetrimino.move(x, y);
    }


    public bool validateHold(ATetrimino holding_tetrimino)
    {
        bool ret = true;
        int im = holdingPiecesGrid.PiecesField.GetUpperBound(0);
        int jm = holdingPiecesGrid.PiecesField.GetUpperBound(1);
        int xm = mainPiecesGrid.PiecesField.GetUpperBound(0);
        int ym = mainPiecesGrid.PiecesField.GetUpperBound(1);
        for (int i = 0, j, x, y; i <= im; i++)
        {
            x = holding_tetrimino.X + i;
            for (j = 0; j <= jm; j++)
            {
                y = holding_tetrimino.Y + j;
                if (holding_tetrimino.Pieces[i, j] != ETetriminos.None)
                {
                    if ((x >= 0) && (x <= xm) && (y >= 0))
                    {
                        if (y <= ym)
                            ret = (mainPiecesGrid.PiecesField[x, y].Piece == ETetriminos.None);
                    }
                    else
                    {
                        ret = false;
                        break;
                    }
                }
                if (!ret)
                    break;
            }
        }
        return ret;
    }

    public void dropHold(ATetrimino holding_tetrimino, bool fast_drop)
    {
        bool loose = false;
        int im = holdingPiecesGrid.PiecesField.GetUpperBound(0);
        int jm = holdingPiecesGrid.PiecesField.GetUpperBound(1);
        int xm = mainPiecesGrid.PiecesField.GetUpperBound(0);
        int ym = mainPiecesGrid.PiecesField.GetUpperBound(1);
        for (int i = 0, j, x, y; i <= im; i++)
        {
            x = holding_tetrimino.X + i;
            for (j = 0; j <= jm; j++)
            {
                y = holding_tetrimino.Y + j;
                if (holding_tetrimino.Pieces[i, j] != ETetriminos.None)
                {
                    if ((x >= 0) && (x <= xm) && (y >= 0) && (y <= ym))
                    {
                        PieceLoader pl = mainPiecesGrid.PiecesField[x, y];
                        if (pl.Piece != ETetriminos.None)
                            loose = true;
                        pl.Piece = holding_tetrimino.Pieces[i, j];
                        if (pl.LoadedPiece != null)
                            pl.LoadedPiece.transform.localPosition = new Vector3(x, y);
                    }
                }
            }
        }
        respawnHoldingTetrimino(NextTetrimino);
        NextTetrimino = Tetriminos.RandomTetrimino;
        ClearedLines += clearLines(true);
        if (fast_drop)
            playSoundEffect(dropFastAudioClip);
        else
            playSoundEffect(dropAudioClip);
        if (loose)
            looseRound();
    }

    public uint clearLines(bool animate_only)
    {
        uint ret = 0U;
        int im = mainPiecesGrid.PiecesField.GetUpperBound(1);
        int jm = mainPiecesGrid.PiecesField.GetUpperBound(0);
        bool can_clear;
        for (int i = im, j; i >= 0; i--)
        {
            can_clear = true;
            for (j = 0; j <= jm; j++)
            {
                if (mainPiecesGrid.PiecesField[j, i].Piece == ETetriminos.None)
                {
                    can_clear = false;
                    break;
                }
            }
            if (can_clear)
            {
                ++ret;
                clearLine((uint)i, animate_only);
            }
        }
        if (animate_only)
        {
            if (ret > 0)
            {
                clear_pause = 0.5f;
                playSoundEffect(clearLinesAudioClip);
            }
        }
        else
        {
            HoldingTetrimino = HoldingTetrimino;
            if (ret > 0)
            {
                switch (ret)
                {
                    case 1:
                        playSoundEffect(singleAudioClip);
                        break;
                    case 2:
                        playSoundEffect(doubleAudioClip);
                        break;
                    case 3:
                        playSoundEffect(tripleAudioClip);
                        break;
                    default:
                        playSoundEffect(unbelievableAudioClip);
                        break;
                }
            }
        }
        return ret;
    }

    private void clearLine(uint line, bool animate_only)
    {
        int im = mainPiecesGrid.PiecesField.GetUpperBound(0);
        int jm = mainPiecesGrid.PiecesField.GetUpperBound(1);
        if (animate_only)
        {
            for (int i = 0; i <= im; i++)
            {
                GameObject go = mainPiecesGrid.PiecesField[i, line].LoadedPiece;
                if (go != null)
                {
                    Animator a = go.GetComponent<Animator>();
                    if (a != null)
                    {
                        a.Play("Destroy");
                    }
                }
            }
        }
        else
        {
            for (int j = (int)line, i; j <= jm; j++)
            {
                for (i = 0; i <= im; i++)
                {
                    PieceLoader pl = mainPiecesGrid.PiecesField[i, j];
                    pl.Piece = (j == jm) ? ETetriminos.None : mainPiecesGrid.PiecesField[i, j + 1].Piece;
                    if (pl.LoadedPiece != null)
                        pl.LoadedPiece.transform.localPosition = new Vector3(i, j, 0);
                }
            }
        }
    }

    private void applyDestroyAnimation(GridManager grid_manager)
    {
        int im = grid_manager.PiecesField.GetUpperBound(0);
        int jm = grid_manager.PiecesField.GetUpperBound(1);
        for (int i = 0, j; i <= im; i++)
        {
            for (j = 0; j <= jm; j++)
            {
                GameObject go = grid_manager.PiecesField[i, j].LoadedPiece;
                if (go != null)
                {
                    Animator a = go.GetComponent<Animator>();
                    if (a != null)
                        a.Play("Destroy");
                }
            }
        }
    }

    public void looseRound()
    {
        running = false;
        playMusic(gameOverAudioClip, false);
        applyDestroyAnimation(mainPiecesGrid);
        applyDestroyAnimation(holdingPiecesGrid);
        applyDestroyAnimation(nextPiecesGrid);
        applyDestroyAnimation(suggestingPiecesGrid);
        if ((enterNamePanel == null) || (nameInputText == null))
            onCloseInput();
        else
            enterNamePanel.localPosition = Vector3.zero;
    }

    public void updateText()
    {
        if (levelPointsManager != null)
            levelPointsManager.updatePoints(Level);
    }

    public bool moveDown(bool fast_drop = false)
    {
        bool ret = false;
        ATetrimino t = HoldingTetrimino.move(0, -1);
        if (validateHold(t))
        {
            HoldingTetrimino = t;
            ret = true;
        }
        else
            dropHold(HoldingTetrimino, fast_drop);
        return ret;
    }

    public void smashDown()
    {
        if (!smashed)
        {
            while (moveDown(true))
            {
                //
            }
            smashed = true;
        }
    }

    public void moveLeft()
    {
        if (!moved)
        {
            ATetrimino t = HoldingTetrimino.move(-1, 0);
            if (validateHold(t))
            {
                HoldingTetrimino = t;
                playSoundEffect(moveAudioClip);
            }
            moved = true;
        }
    }

    public void moveRight()
    {
        if (!moved)
        {
            ATetrimino t = HoldingTetrimino.move(1, 0);
            if (validateHold(t))
            {
                HoldingTetrimino = t;
                playSoundEffect(moveAudioClip);
            }
            moved = true;
        }
    }

    public void rotateLeft()
    {
        ATetrimino t = HoldingTetrimino.rotateLeft();
        if (validateHold(t))
        {
            HoldingTetrimino = t;
            playSoundEffect(rotateAudioClip);
        }
    }

    public void rotateRight()
    {
        ATetrimino t = HoldingTetrimino.rotateRight();
        if (validateHold(t))
        {
            HoldingTetrimino = t;
            playSoundEffect(rotateAudioClip);
        }
    }

    public void playMusic(AudioClip music_clip, bool loop)
    {
        AudioSource[] _as = GetComponents<AudioSource>();
        if (_as != null)
        {
            if (_as.Length > 0)
            {
                _as[0].clip = music_clip;
                _as[0].loop = loop;
                _as[0].Play();
            }
        }
    }

    public void playSoundEffect(AudioClip audio_clip)
    {
        current_audio_source++;
        if (current_audio_source >= soundEffectsAudioSources.Length)
            current_audio_source = 0U;
        if ((audio_clip != null) && (soundEffectsAudioSources[current_audio_source] != null))
        {
            soundEffectsAudioSources[current_audio_source].clip = audio_clip;
            soundEffectsAudioSources[current_audio_source].Play();
        }
    }

    // Use this for initialization
    void Start () {
        NextTetrimino = Tetriminos.RandomTetrimino;
        respawnHoldingTetrimino(Tetriminos.RandomTetrimino);
        playMusic(gameAudioClip, true);
    }
	
	// Update is called once per frame
	void Update () {
        if (running)
        {
            if (paused)
            {
                if (Input.GetButtonDown("Cancel"))
                    onResumeGame();
            }
            else
            {
                bool c = true;
                if (clear_pause > 0.0f)
                {
                    c = false;
                    clear_pause -= Time.deltaTime;
                    if (clear_pause <= 0.0f)
                    {
                        c = true;
                        clear_pause = 0.0f;
                        clearLines(false);
                    }
                    deltaTime = 0.0f;
                }
                if (c)
                {
                    deltaTime += Time.deltaTime;
                    float v_axis = Input.GetAxis("Vertical");
                    float h_axis = Input.GetAxis("Horizontal");
                    float mult = (v_axis <= -0.8f) ? 0.25f : 1.0f;
                    float t = (time * mult);
                    if (v_axis <= -0.8f)
                    {
                        if (!moved_down)
                        {
                            moveDown();
                            moved_down = true;
                            deltaTime = 0.0f;
                        }
                        else if (deltaTime >= t)
                        {
                            deltaTime -= t;
                            moveDown();
                        }
                    }
                    else
                    {
                        moved_down = false;
                        if (deltaTime >= t)
                        {
                            deltaTime -= t;
                            moveDown();
                        }
                        else if (v_axis >= 0.8f)
                            smashDown();
                        else
                            smashed = false;
                    }
                    if (Input.GetButtonDown("Fire1"))
                        rotateLeft();
                    if (Input.GetButtonDown("Fire2"))
                        rotateRight();
                    if (h_axis <= -0.2f)
                        moveLeft();
                    else if (h_axis >= 0.8f)
                        moveRight();
                    else
                        moved = false;
                    if (Input.GetButtonDown("Cancel"))
                    {
                        paused = true;
                        if (pausePanel != null)
                            pausePanel.localPosition = Vector3.zero;
                        playSoundEffect(pauseAudioClip);
                    }
                }
            }
        }
    }

    public void onResumeGame()
    {
        if (paused)
        {
            if (pausePanel != null)
                pausePanel.localPosition = new Vector3(0, -9000, 0);
            paused = false;
            playSoundEffect(pauseAudioClip);
        }
    }

    public void onRestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void onExitGame()
    {
        SceneManager.LoadScene("Menu");
    }

    public void onSaveStats()
    {
        Highscore highscore = new Highscore("titros3d.db");
        highscore.NewScore = new PlayerScore(nameInputText.text, Score, Level);
        onCloseInput();
    }

    public void onCloseInput()
    {
        SceneManager.LoadScene("Highscore");
    }
}
