using UnityEngine;
using System.Collections;

public class GridManager : MonoBehaviour {

    [SerializeField]
    private Transform pieces;

    [SerializeField]
    private uint fieldWidth;

    [SerializeField]
    private uint fieldHeight;

    [SerializeField]
    private bool suggestField = false;

    private PieceLoader[,] pieces_field;

    public PieceLoader[,] PiecesField
    {
        get
        {
            if (pieces_field == null)
            {
                pieces_field = new PieceLoader[fieldWidth, fieldHeight];
                for (int i = 0, im = pieces_field.GetUpperBound(0), j, jm = pieces_field.GetUpperBound(1); i <= im; i++)
                {
                    for (j = 0; j <= jm; j++)
                    {
                        pieces_field[i, j] = new PieceLoader(pieces, suggestField);
                    }
                }
            }
            return pieces_field;
        }
    }
    
	void Start ()
    {
        //transform.localScale = new Vector3(fieldWidth * 0.5f, 0.5f, fieldHeight * 0.5f);
    }
	
	void Update ()
    {
        
	}
}
