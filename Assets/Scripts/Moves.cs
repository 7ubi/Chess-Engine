using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moves : MonoBehaviour
{
    [SerializeField] BoardCreator _boardCreator;
    
    public void CreatePossibleMoves(Transform parent, string piece)
    {
        if (piece.ToLower() == "n")
        {
            KnightMovement(parent, piece);
        }

        if (piece.ToLower() == "p")
        {
            PawnMovement(parent, piece);
        }
    }

    public void RemovePossibleMoves()
    {
        foreach (var cell in _boardCreator.board)
        {
            cell.GetComponent<Cell>().possibleMove = false;
        }
    }

    void KnightMovement(Transform parent, string piece)
    {
        var cell = parent.GetComponent<Cell>().NumField;

        var pos = GETPosFromIndex(cell);   
            
        Vector2[] moves =
        {
            new Vector2(pos.x + 2, pos.y + 1),
            new Vector2(pos.x - 2, pos.y + 1),
            new Vector2(pos.x + 2, pos.y - 1),
            new Vector2(pos.x - 2, pos.y - 1),
            new Vector2(pos.x + 1, pos.y + 2),
            new Vector2(pos.x - 1, pos.y + 2),
            new Vector2(pos.x + 1, pos.y - 2),
            new Vector2(pos.x - 1, pos.y - 2),
        };


        foreach (Vector2 move in moves)
        {
            if (move.x >= 0 && move.x < 8 && move.y >= 0 && move.y < 8)
            {
                var index = GETIndex(move);
                var moveCell = _boardCreator.board[index].GetComponent<Cell>();
                if (moveCell.transform.childCount == 0)
                {
                    moveCell.possibleMove = true;
                }
                else
                {
                    var p = moveCell.transform.GetChild(0).GetComponent<Piece>();
                    if(char.IsUpper(Convert.ToChar(p.piece)) && char.IsUpper(Convert.ToChar(piece))
                       || !char.IsUpper(Convert.ToChar(p.piece)) && !char.IsUpper(Convert.ToChar(piece)))
                    {
                        moveCell.possibleMove = false;
                    }
                    else
                    {
                        moveCell.possibleMove = true;
                    }
                }
            }
        }
    }

    void PawnMovement(Transform parent, string piece)
    {
        var cell = parent.GetComponent<Cell>().NumField;
        
        float dir = char.IsUpper(Convert.ToChar(piece)) ? -1 : 1;
        var startY = char.IsUpper(Convert.ToChar(piece)) ? 6 : 1;

        var pos = GETPosFromIndex(cell);
        
        var index = GETIndex(new Vector2(pos.x, pos.y + dir));
        if (_boardCreator.board[index].transform.childCount == 0)
        {
            _boardCreator.board[index].GetComponent<Cell>().possibleMove = true;
        }

        if (Math.Abs(pos.y - startY) < 0.1)
        {
            index = GETIndex(new Vector2(pos.x, pos.y + 2*dir));
            if (_boardCreator.board[index].transform.childCount == 0)
            {
                _boardCreator.board[index].GetComponent<Cell>().possibleMove = true;
            }
        }

        Vector2[] takesPos =
        {
            new Vector2(pos.x + 1, pos.y + dir),
            new Vector2(pos.x - 1, pos.y + dir)
        };

        foreach (var p in takesPos)
        {
            index = GETIndex(p);
            if (_boardCreator.board[index].transform.childCount != 0)
            {
                var capture = _boardCreator.board[index].transform.GetChild(0).GetComponent<Piece>();
                if (char.IsUpper(Convert.ToChar(capture.piece)) != char.IsUpper(Convert.ToChar(piece)))
                {
                    _boardCreator.board[index].GetComponent<Cell>().possibleMove = true;
                }
            }
        }
    }

    Vector2 GETPosFromIndex(int index)
    {
        return new Vector2(index % 8, Convert.ToInt32(Math.Floor(index / 8f)));
    }

    int GETIndex(Vector2 pos)
    {
        return Convert.ToInt32(pos.x + 8 * pos.y);
    }
}
