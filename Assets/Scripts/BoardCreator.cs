using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class BoardCreator : MonoBehaviour
{
    public string FEN;
    
    public Color LightColor;
    public Color DarkColor;
    public Color LightSelectedColor;
    public Color DarkSelectedColor;

    public float xOff;
    public float yOff;
    public float size;

    public GameObject square;

    private int _n = 63;

    public PieceManager pieceManager;

    public GameObject[] board = new GameObject[64];
    
    private void Start()
    {
        
        for(int i = 0; i < 8; i++)
        {
            for(int j = 7; j >= 0; j--)
            {
                createSquare(j, i, (i + j) % 2 == 0);
                _n--;
            }
        }

        if (FEN != "")
        {
            string[] subFEN = FEN.Split('/');
            int n = 0; 
            for (int i = 0; i < 8; i++)
            {
                char[] sub = subFEN[i].ToCharArray();
                foreach (char t in sub)
                {
                    if (Int32.TryParse(Char.ToString(t), NumberStyles.Integer, new CultureInfo("en-US"), out int number))
                    {
                        n += Convert.ToInt32(Char.GetNumericValue(t));
                    }
                    else
                    {
                        pieceManager.createPiece(t.ToString(), board[n].transform);
                        n++;
                    }
                }
            }
        }
    }

    public void createSquare(int x, int y, bool isDark)
    {
        GameObject s = Instantiate(square, this.gameObject.transform);
        board[_n] = s; 
        s.GetComponent<Cell>().Pos = new Vector2(x, y);
        s.GetComponent<Cell>().Board = gameObject.GetComponent<BoardCreator>();
        s.GetComponent<Cell>().IsDark = isDark;
        s.GetComponent<Cell>().NumField = _n;
        s.GetComponent<Cell>().pieceManager = pieceManager;
    }
}
