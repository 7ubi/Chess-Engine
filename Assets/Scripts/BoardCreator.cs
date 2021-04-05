using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using UnityEngine.Serialization;

public class BoardCreator : MonoBehaviour
{
    public string FEN;
    
    public Color LightColor;
    public Color DarkColor;
    public Color LightSelectedColor;
    public Color DarkSelectedColor;
    public Color checkColor;
    public Color IllegalMoveColor;

    public float xOff;
    public float yOff;
    public float size;

    public GameObject square;

    private int _n = 63;

    public PieceManager pieceManager;

    public GameObject[] board = new GameObject[64];

    public string[] pieceBoard = new string[64];

    private void Start()
    {
        
        for(var i = 0; i < 8; i++)
        {
            for(var j = 7; j >= 0; j--)
            {
                CreateSquare(j, i, (i + j) % 2 == 0);
                _n--;
            }
        }

        if (FEN != "")
        {
            var subFen = FEN.Split('/');
            var n = 0; 
            for (var i = 0; i < 8; i++)
            {
                var sub = subFen[i].ToCharArray();
                foreach (var t in sub)
                {
                    if (int.TryParse(char.ToString(t), NumberStyles.Integer, new CultureInfo("en-US"), out var number))
                    {
                        n += Convert.ToInt32(char.GetNumericValue(t));
                    }
                    else
                    {
                        pieceManager.CreatePiece(t.ToString(), board[n].transform);
                        n++;
                    }
                }
            }
            for(var i = 0; i < board.Length; i++)
            {
                pieceBoard[i] = "";
                var b = board[i];
                if(b.transform.childCount == 0) continue;
                pieceBoard[i] = b.transform.GetChild(0).GetComponent<Piece>().piece;
            }
        }
    }

    public void UpdateBoard()
    {
        for(var i = 0; i < board.Length; i++)
        {
            pieceBoard[i] = "";
            var b = board[i];
            if(b.transform.childCount == 0) continue;
            pieceBoard[i] = b.transform.GetChild(0).GetComponent<Piece>().piece;
            
        }
        
    }

    private void CreateSquare(int x, int y, bool isDark)
    {
        var s = Instantiate(square, this.gameObject.transform);
        board[_n] = s; 
        s.GetComponent<Cell>().Pos = new Vector2(x, y);
        s.GetComponent<Cell>().Board = gameObject.GetComponent<BoardCreator>();
        s.GetComponent<Cell>().IsDark = isDark;
        s.GetComponent<Cell>().NumField = _n;
        s.GetComponent<Cell>().pieceManager = pieceManager;
    }
}
