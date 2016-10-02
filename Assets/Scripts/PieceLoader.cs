using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PieceLoader {

    private static Dictionary<string, GameObject> piece_cache = new Dictionary<string, GameObject>();

    private Transform parent;

    private ETetriminos piece = ETetriminos.None;

    private GameObject loaded_piece = null;

    private bool suggest_mode;

    public ETetriminos Piece
    {
        get
        {
            return piece;
        }
        set
        {
            //if (piece != value)
            //{
                piece = value;
                if (loaded_piece != null)
                    GameObject.Destroy(loaded_piece);
                loaded_piece = null;
                switch (piece)
                {
                    case ETetriminos.I:
                        loaded_piece = loadPiece("LightBlue");
                        break;
                    case ETetriminos.J:
                        loaded_piece = loadPiece("Blue");
                        break;
                    case ETetriminos.L:
                        loaded_piece = loadPiece("Orange");
                        break;
                    case ETetriminos.O:
                        loaded_piece = loadPiece("Yellow");
                        break;
                    case ETetriminos.S:
                        loaded_piece = loadPiece("Green");
                        break;
                    case ETetriminos.T:
                        loaded_piece = loadPiece("Purple");
                        break;
                    case ETetriminos.Z:
                        loaded_piece = loadPiece("Red");
                        break;
                }
            //}
        }
    }

    public GameObject LoadedPiece
    {
        get
        {
            return loaded_piece;
        }
    }

    public PieceLoader(Transform parent, bool suggest_mode)
    {
        this.parent = parent;
        this.suggest_mode = suggest_mode;
    }

    public GameObject loadPiece(string name)
    {
        GameObject ret = null;
        if (suggest_mode)
            name = "Suggest";
        if (piece_cache.ContainsKey(name))
            ret = GameObject.Instantiate<GameObject>(piece_cache[name]);
        else
        {
            GameObject go = Resources.Load<GameObject>("Pieces/" + name + " Piece");
            if (go != null)
            {
                piece_cache.Add(name, go);
                ret = GameObject.Instantiate<GameObject>(go);
                
            }
        }
        if (ret != null)
        {
            ret.transform.parent = parent;
            ret.name = name + " Piece";
        }
        return ret;
    }
}
