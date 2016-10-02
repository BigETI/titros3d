using UnityEngine;
using System.Collections;
using System;

[Serializable]
public abstract class ATetrimino {

    public class RotatedTetrimino : ATetrimino
    {
        public RotatedTetrimino(ATetrimino tetrimino, bool left_rotate) : base(tetrimino.x, tetrimino.y)
        {
            for (int i = 0, j; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    if (left_rotate)
                        pieces[4 - j, i] = tetrimino.pieces[i, j];
                    else
                        pieces[j, 4 - i] = tetrimino.pieces[i, j];
                }
            }
        }
    }

    public class MovedTetrimino : ATetrimino
    {
        public MovedTetrimino(ATetrimino tetrimino, int move_x, int move_y) : base(tetrimino.x + move_x, tetrimino.y + move_y)
        {
            for (int i = 0, j; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                    pieces[i, j] = tetrimino.pieces[i, j];
            }
        }
    }

    private int x = 0;

    private int y = 0;

    protected ETetriminos[,] pieces = new ETetriminos[5,5];

    public int X
    {
        get
        {
            return x;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }
    }

    public ETetriminos[,] Pieces
    {
        get
        {
            return pieces;
        }
    }

    public ATetrimino(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public ATetrimino rotateLeft()
    {
        return new RotatedTetrimino(this, true);
    }

    public ATetrimino rotateRight()
    {
        return new RotatedTetrimino(this, false);
    }

    public ATetrimino move(int move_x, int move_y)
    {
        return new MovedTetrimino(this, move_x, move_y);
    }
}
