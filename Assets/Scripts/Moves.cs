using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moves : MonoBehaviour
{
    [SerializeField] BoardCreator _boardCreator;
    
    private Transform _parent;
    private string _pieceName;
    private int _cell;
    private Vector2 _pos;
    private Piece _piece;
    
    public void CreatePossibleMoves(Transform _parent, string _pieceName, Piece _piece)
    {
        this._parent = _parent;
        this._pieceName = _pieceName;
        this._piece = _piece;
        _cell = this._parent.GetComponent<Cell>().NumField;
        _pos = GETPosFromIndex(_cell);
        
        if (this._pieceName.ToLower() == "n")
        {
            KnightMovement();
        }

        if (this._pieceName.ToLower() == "p")
        {
            PawnMovement();
        }

        if (this._pieceName.ToLower() == "q" || this._pieceName.ToLower() == "r")
        {
            RookMovement();
        }

        if (this._pieceName.ToLower() == "q" || this._pieceName.ToLower() == "b")
        {
            DiagonalMovement();
        }

        if (this._pieceName.ToLower() == "k")
        {
            KingMove();
            Castle();
        }
    }

    public void RemovePossibleMoves()
    {
        foreach (var cell in _boardCreator.board)
        {
            cell.GetComponent<Cell>().possibleMove = false;
        }
    }

    void KnightMovement()
    { 
        Vector2[] moves =
        {
            new Vector2(_pos.x + 2, _pos.y + 1),
            new Vector2(_pos.x - 2, _pos.y + 1),
            new Vector2(_pos.x + 2, _pos.y - 1),
            new Vector2(_pos.x - 2, _pos.y - 1),
            new Vector2(_pos.x + 1, _pos.y + 2),
            new Vector2(_pos.x - 1, _pos.y + 2),
            new Vector2(_pos.x + 1, _pos.y - 2),
            new Vector2(_pos.x - 1, _pos.y - 2),
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
                    if(char.IsUpper(Convert.ToChar(p.piece)) == char.IsUpper(Convert.ToChar(_pieceName)))
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

    void PawnMovement()
    {
        float dir = char.IsUpper(Convert.ToChar(_pieceName)) ? -1 : 1;
        var startY = char.IsUpper(Convert.ToChar(_pieceName)) ? 6 : 1;

        var index = GETIndex(new Vector2(_pos.x, _pos.y + dir));
        if (_boardCreator.board[index].transform.childCount == 0)
        {
            _boardCreator.board[index].GetComponent<Cell>().possibleMove = true;
        }

        if (Math.Abs(_pos.y - startY) < 0.1)
        {
            index = GETIndex(new Vector2(_pos.x, _pos.y + 2*dir));
            if (_boardCreator.board[index].transform.childCount == 0)
            {
                _boardCreator.board[index].GetComponent<Cell>().possibleMove = true;
            }
        }

        Vector2[] takesPos =
        {
            new Vector2(_pos.x + 1, _pos.y + dir),
            new Vector2(_pos.x - 1, _pos.y + dir)
        };

        foreach (var p in takesPos)
        {
            index = GETIndex(p);
            if (_boardCreator.board[index].transform.childCount != 0)
            {
                var capture = _boardCreator.board[index].transform.GetChild(0).GetComponent<Piece>();
                if (char.IsUpper(Convert.ToChar(capture.piece)) != char.IsUpper(Convert.ToChar(_pieceName)))
                {
                    _boardCreator.board[index].GetComponent<Cell>().possibleMove = true;
                }
            }
        }
    }

    void RookMovement()
    {
        for (var dir = -1; dir <= 1; dir += 2)
        {
            var xOff = dir;
            while (true)
            {
                if (_pos.x + xOff > 7 || _pos.x + xOff < 0)
                {
                    break;
                }
                var newPos = new Vector2(_pos.x + xOff, _pos.y);
                if (!MoveWithCheck(newPos, _pieceName))
                {
                    break;
                }

                xOff += dir;
            }
        }
        
        for (var dir = -1; dir <= 1; dir += 2)
        {
            var yOff = dir;
            while (true)
            {
                if (_pos.y + yOff > 7 || _pos.y + yOff < 0)
                {
                    break;
                }
                var newPos = new Vector2(_pos.x, _pos.y + yOff);

                if (!MoveWithCheck(newPos, _pieceName))
                {
                    break;
                }

                yOff += dir;
            }
        }
    }

    void DiagonalMovement()
    {
        for (var xDir = -1; xDir <= 1; xDir += 2)
        {
            for (var yDir = -1; yDir <= 1; yDir += 2)
            {
                for(var offSet = 1; offSet < 7; offSet++)
                {
                    if (_pos.x + offSet * xDir > 7 || _pos.x + offSet * xDir < 0 || _pos.y + offSet * yDir > 7 || _pos.y + offSet * yDir < 0)
                    {
                        break;
                    }
                    
                    var newPos = new Vector2(_pos.x + offSet * xDir, _pos.y + offSet * yDir);
                    
                    if (!MoveWithCheck(newPos, _pieceName))
                    {
                        break;
                    }
                }
            }
        }
    }

    void KingMove()
    {
        for (var xDir = -1; xDir <= 1; xDir++)
        {
            for (var yDir = -1; yDir <= 1; yDir++)
            {
                if (_pos.x + xDir > 7 || _pos.x + xDir < 0 || _pos.y + yDir > 7 || _pos.y + yDir < 0)
                {
                    continue;
                }
                if(yDir == -0 && xDir == 0){continue;}
                
                var newPos = new Vector2(_pos.x + xDir, _pos.y + yDir);
                var index = GETIndex(newPos);
                MoveWithCheck(newPos, _pieceName);

            }
        }
    }

    void Castle()
    {
        if (_piece.hasMoved) return;
        //king castle

        GameObject[] board = _boardCreator.board;
        
        if (board[GETIndex(new Vector2(_pos.x + 1, _pos.y))].transform.childCount == 0 
            && board[GETIndex(new Vector2(_pos.x + 2, _pos.y))].transform.childCount == 0)
        {
            if (board[GETIndex(new Vector2(_pos.x + 3, _pos.y))].transform.childCount != 0)
            {
                if (!board[GETIndex(new Vector2(_pos.x + 3, _pos.y))].transform.GetChild(0).GetComponent<Piece>().hasMoved)
                {
                    _boardCreator.board[GETIndex(new Vector2(_pos.x + 2, _pos.y))].GetComponent<Cell>().possibleMove = true;
                }
            }
        }
        
        if (board[GETIndex(new Vector2(_pos.x - 1, _pos.y))].transform.childCount == 0 
            && board[GETIndex(new Vector2(_pos.x - 2, _pos.y))].transform.childCount == 0
            && board[GETIndex(new Vector2(_pos.x - 3, _pos.y))].transform.childCount == 0)
        {
            if (board[GETIndex(new Vector2(_pos.x - 4, _pos.y))].transform.childCount != 0)
            {
                if (!board[GETIndex(new Vector2(_pos.x - 4, _pos.y))].transform.GetChild(0).GetComponent<Piece>().hasMoved)
                {
                    _boardCreator.board[GETIndex(new Vector2(_pos.x - 2, _pos.y))].GetComponent<Cell>().possibleMove = true;
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

    bool MoveWithCheck(Vector2 pos, string piece)
    {
        
        var index = GETIndex(pos);
                    
        if (_boardCreator.board[index].transform.childCount == 0)
        {
            _boardCreator.board[index].GetComponent<Cell>().possibleMove = true;
        }
        else
        {
            var p = _boardCreator.board[index].transform.GetChild(0).GetComponent<Piece>();

            if (char.IsUpper(Convert.ToChar(p.piece)) != char.IsUpper(Convert.ToChar(piece)))
            {
                _boardCreator.board[index].GetComponent<Cell>().possibleMove = true;
            }

            return false;
        }

        return true;
    }
}
