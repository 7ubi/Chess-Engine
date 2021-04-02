using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Moves : MonoBehaviour
{
    [SerializeField] BoardCreator _boardCreator;
    
    private Vector2 _pos;
    private Piece _piece;
    
    public void CreatePossibleMoves(Piece _piece)
    {
        this._piece = _piece;
        _pos = GETPosFromIndex(this._piece._parent.GetComponent<Cell>().NumField);

        var moves = PossibleMoves();
        ShowPossibleMoves(moves);
    }

    public void RemovePossibleMoves()
    {
        foreach (var cell in _boardCreator.board)
        {
            cell.GetComponent<Cell>().possibleMove = false;
        }
    }

    private List<int> KnightMovement()
    {
        var possibleMoves = new List<int>();
        
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
            if (!(move.x >= 0) || !(move.x < 8) || !(move.y >= 0) || !(move.y < 8)) continue;
            var index = GETIndex(move);
            if (_boardCreator.pieceBoard[index] == "")
            {
                possibleMoves.Add(index);
            }
            else
            {
                if(char.IsUpper(Convert.ToChar(_boardCreator.pieceBoard[index])) != _piece.IsWhite())
                {
                    possibleMoves.Add(index);
                }
            }
        }

        return possibleMoves;
    }

    private List<int> PawnMovement()
    {
        var possibleMoves = new List<int>();
        
        float dir = _piece.IsWhite() ? -1 : 1;
        var startY = _piece.IsWhite() ? 6 : 1;

        
        
        var index = GETIndex(new Vector2(_pos.x, _pos.y + dir));
        
        try
        {
            if (_boardCreator.pieceBoard[index] == "")
            {
                possibleMoves.Add(index);
            }
        }
        catch (IndexOutOfRangeException)
        {
            Debug.Log(index + " "+ new Vector2(_pos.x, _pos.y) + " " +  _piece.piece + " " + dir);
        }

        if (Math.Abs(_pos.y - startY) < 0.1)
        {
            index = GETIndex(new Vector2(_pos.x, _pos.y + 2*dir));
            if (_boardCreator.pieceBoard[index] == "")
            {
                possibleMoves.Add(index);
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
            if (_boardCreator.board[index].transform.childCount == 0) continue;
            var capture = _boardCreator.board[index].transform.GetChild(0).GetComponent<Piece>();
            if (capture.IsWhite() != _piece.IsWhite())
            {
                possibleMoves.Add(index);
            }
        }

        return possibleMoves;
    }

    private List<int> RookMovement()
    {
        var possibleMoves = new List<int>();
        
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
                if (GetCellWithCheck(newPos, _piece.piece) != -1)
                {
                    possibleMoves.Add(GetCellWithCheck(newPos, _piece.piece));
                }
                if (!MoveWithCheck(newPos, _piece.piece))
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
                if (GetCellWithCheck(newPos, _piece.piece) != -1)
                {
                    possibleMoves.Add(GetCellWithCheck(newPos, _piece.piece));
                }
                if (!MoveWithCheck(newPos, _piece.piece))
                {
                    break;
                }

                yOff += dir;
            }
        }

        return possibleMoves;
    }

    private List<int> DiagonalMovement()
    {
        var possibleMoves = new List<int>();
        
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
                    if (GetCellWithCheck(newPos, _piece.piece) != -1)
                    {
                        possibleMoves.Add(GetCellWithCheck(newPos, _piece.piece));
                    }

                    if (!MoveWithCheck(newPos, _piece.piece))
                    {
                        break;
                    }
                }
            }
        }

        return possibleMoves;
    }

    private List<int> KingMove()
    {
        var possibleMoves = new List<int>();
        
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
                if (GetCellWithCheck(newPos, _piece.piece) != -1)
                {
                    possibleMoves.Add(GetCellWithCheck(newPos, _piece.piece));
                }
            }
        }

        return possibleMoves;
    }

    private List<int> Castle()
    {
        var possibleMoves = new List<int>();
        if (!_piece.FirstMove) return null;
        //king castle
    
        var board = _boardCreator.pieceBoard;
        if (board[GETIndex(new Vector2(_pos.x + 1, _pos.y))] == ""
            && board[GETIndex(new Vector2(_pos.x + 2, _pos.y))] == "")
        {
            if (board[GETIndex(new Vector2(_pos.x + 3, _pos.y))] != "")
            {
                if (!_boardCreator.board[GETIndex(new Vector2(_pos.x + 3, _pos.y))].transform.GetChild(0).GetComponent<Piece>().hasMoved)
                {
                    possibleMoves.Add(GETIndex(new Vector2(_pos.x + 2, _pos.y)));
                }
            }
        }

        if (board[GETIndex(new Vector2(_pos.x - 1, _pos.y))] != "" ||
            board[GETIndex(new Vector2(_pos.x - 2, _pos.y))] != "" ||
            board[GETIndex(new Vector2(_pos.x - 3, _pos.y))] != "") return possibleMoves;
        if (board[GETIndex(new Vector2(_pos.x - 4, _pos.y))] == "") return possibleMoves;
        if (!_boardCreator.board[GETIndex(new Vector2(_pos.x - 4, _pos.y))].transform.GetChild(0).GetComponent<Piece>().hasMoved)
        {
            possibleMoves.Add(GETIndex(new Vector2(_pos.x - 2, _pos.y)));
        }

        return possibleMoves;
    }
    
    Vector2 GETPosFromIndex(int index)
    {
        return new Vector2(index % 8, Convert.ToInt32(Math.Floor(index / 8f)));
    }

    int GETIndex(Vector2 pos)
    {
        return Convert.ToInt32(pos.x + 8 * pos.y);
    }

    private bool MoveWithCheck(Vector2 pos, string piece)
    {
        var index = GETIndex(pos);
            
        return _boardCreator.pieceBoard[index]  == "";
    }

    private int GetCellWithCheck(Vector2 pos, string piece)
    {
        var index = GETIndex(pos);
                    
        if (_boardCreator.pieceBoard[index] == "")
        {
            return index;
        }

        if (char.IsUpper(Convert.ToChar(_boardCreator.pieceBoard[index])) != _piece.IsWhite())
        {
            return index;
        }

        return -1;
    }

    private void ShowPossibleMoves(IReadOnlyCollection<int> moves)
    {
        if (moves.Count == 0) return;
        foreach (var move in moves)
        {
            _boardCreator.board[move].GetComponent<Cell>().possibleMove = true;
        }
    }

    public bool Check(Piece piece)
    {
        this._piece = piece;
        _pos = GETPosFromIndex(this._piece.transform.parent.GetComponent<Cell>().NumField);
        var madeCheck = false;

        var moves = PossibleMoves();
        
        foreach (var m in moves)
        {
            if (_boardCreator.pieceBoard[m].ToLower() != "k") continue;
            _boardCreator.board[m].GetComponent<Cell>().check = true;
            madeCheck = true;
        }

        return madeCheck;
    }

    private List<int> PossibleMoves()
    {
        var moves = new List<int>();
        
        if (_piece.piece.ToLower() == "n")
        {
            if (KnightMovement() != null)
            {
                moves.AddRange(KnightMovement());
            }
        }

        if (_piece.piece.ToLower() == "p")
        {
            if (PawnMovement() != null)
            {
                moves.AddRange(PawnMovement());
            }
        }

        if (_piece.piece.ToLower() == "q" || _piece.piece.ToLower() == "r")
        {
            if (RookMovement() != null)
            {
                moves.AddRange(RookMovement());
            }
        }
        
        if (_piece.piece.ToLower() == "q" || _piece.piece.ToLower() == "b")
        {
            if (DiagonalMovement() != null)
            {
                moves.AddRange(DiagonalMovement());
            }
        }

        if (_piece.piece.ToLower() == "k")
        {
            if (KingMove() != null)
            {
                moves.AddRange(KingMove());
            }

            if (Castle() != null)
            {
                moves.AddRange(Castle());
            }
        }

        return moves;
    } 

    public void RemoveCheck()
    {
        foreach(var c in _boardCreator.board)
        {
            c.GetComponent<Cell>().check = false;
        }
    }

}
