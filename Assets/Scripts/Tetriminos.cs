using UnityEngine;
using System.Collections;
using System;

public class Tetriminos {

    public class ITetrimino : ATetrimino
    {
        public ITetrimino() : base(0, 0)
        {
            
            pieces[2, 0] = ETetriminos.I;
            pieces[2, 1] = ETetriminos.I;
            pieces[2, 2] = ETetriminos.I;
            pieces[2, 3] = ETetriminos.I;
        }
    }

    public class JTetrimino : ATetrimino
    {
        public JTetrimino() : base(0, 0)
        {
            pieces[3, 1] = ETetriminos.J;
            pieces[3, 2] = ETetriminos.J;
            pieces[2, 3] = ETetriminos.J;
            pieces[3, 3] = ETetriminos.J;
        }
    }

    public class LTetrimino : ATetrimino
    {
        public LTetrimino() : base(0, 0)
        {
            pieces[2, 1] = ETetriminos.L;
            pieces[2, 2] = ETetriminos.L;
            pieces[2, 3] = ETetriminos.L;
            pieces[3, 3] = ETetriminos.L;
        }
    }

    public class OTetrimino : ATetrimino
    {
        public OTetrimino() : base(0, 0)
        {
            pieces[2, 1] = ETetriminos.O;
            pieces[3, 1] = ETetriminos.O;
            pieces[2, 2] = ETetriminos.O;
            pieces[3, 2] = ETetriminos.O;
        }
    }

    public class STetrimino : ATetrimino
    {
        public STetrimino() : base(0, 0)
        {
            pieces[2, 1] = ETetriminos.S;
            pieces[3, 1] = ETetriminos.S;
            pieces[1, 2] = ETetriminos.S;
            pieces[2, 2] = ETetriminos.S;
        }
    }

    public class TTetrimino : ATetrimino
    {
        public TTetrimino() : base(0, 0)
        {
            pieces[1, 1] = ETetriminos.T;
            pieces[2, 1] = ETetriminos.T;
            pieces[3, 1] = ETetriminos.T;
            pieces[2, 2] = ETetriminos.T;
        }
    }

    public class ZTetrimino : ATetrimino
    {
        public ZTetrimino() : base(0, 0)
        {
            pieces[1, 1] = ETetriminos.Z;
            pieces[2, 1] = ETetriminos.Z;
            pieces[2, 2] = ETetriminos.Z;
            pieces[3, 2] = ETetriminos.Z;
        }
    }

    public class NoTetrimino : ATetrimino
    {
        public NoTetrimino() : base(0, 0)
        {
            //
        }
    }

    private static ATetrimino[] tetriminos = new ATetrimino[Enum.GetNames(typeof(ETetriminos)).Length - 1];

	static Tetriminos()
    {
        tetriminos[(int)ETetriminos.I - 1] = new ITetrimino();
        tetriminos[(int)ETetriminos.J - 1] = new JTetrimino();
        tetriminos[(int)ETetriminos.L - 1] = new LTetrimino();
        tetriminos[(int)ETetriminos.O - 1] = new OTetrimino();
        tetriminos[(int)ETetriminos.S - 1] = new STetrimino();
        tetriminos[(int)ETetriminos.T - 1] = new TTetrimino();
        tetriminos[(int)ETetriminos.Z - 1] = new ZTetrimino();
    }

    public static ATetrimino[] _Tetriminos
    {
        get
        {
            return tetriminos;
        }
    }

    public static ATetrimino RandomTetrimino
    {
        get
        {
            return tetriminos[UnityEngine.Random.Range(0, tetriminos.Length)];
        }
    }

    public static ATetrimino EmptyTetrimino
    {
        get
        {
            return new NoTetrimino();
        }
    }
}
