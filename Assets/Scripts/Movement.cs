using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    private Piece _piece;
    private Vector2 _pos;
    private Moves _moves;
    [SerializeField] private BoardCreator boardCreator;

    private void Start()
    {
        _moves = gameObject.GetComponent<Moves>();
    }

    public void UpdatePiece()
    {
        _piece = _moves.Piece;
        _pos = _moves.Pos;
    }

    public Vector2 GETPosFromIndex(int index)
    {
        return new Vector2(index % 8, Convert.ToInt32(Math.Floor(index / 8f)));
    }

    public int GETIndex(Vector2 pos)
    {
        return Convert.ToInt32(pos.x + 8 * pos.y);
    }
    
    public List<int> KnightMovement()
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


        foreach (var move in moves)
        {
            if (!(move.x >= 0) || !(move.x < 8) || !(move.y >= 0) || !(move.y < 8)) continue;
            var index = GETIndex(move);
            if (boardCreator.pieceBoard[index] == "")
            {
                possibleMoves.Add(index);
            }
            else
            {
                if(char.IsUpper(Convert.ToChar(boardCreator.pieceBoard[index])) != _piece.IsWhite())
                {
                    possibleMoves.Add(index);
                }
            }
        }

        return possibleMoves;
    }

    public List<int> PawnMovement()
    {
        var possibleMoves = new List<int>();
        
        float dir = _piece.IsWhite() ? -1 : 1;
        var startY = _piece.IsWhite() ? 6 : 1;

        
        
        var index = GETIndex(new Vector2(_pos.x, _pos.y + dir));
        
        
        if (boardCreator.pieceBoard[index] == "")
        {
            possibleMoves.Add(index);
        }

        if (Math.Abs(_pos.y - startY) < 0.1)
        {
            index = GETIndex(new Vector2(_pos.x, _pos.y + 2*dir));
            if (boardCreator.pieceBoard[index] == "")
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
            if (boardCreator.board[index].transform.childCount == 0) continue;
            var capture = boardCreator.board[index].transform.GetChild(0).GetComponent<Piece>();
            if (capture.IsWhite() != _piece.IsWhite())
            {
                possibleMoves.Add(index);
            }
        }

        return possibleMoves;
    }

    public List<int> RookMovement()
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

    public List<int> DiagonalMovement()
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

    public List<int> KingMove()
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
                var illegal = false;
                foreach (var illegalIndex in _moves.IllegalKingMoves)
                {
                    if (index == illegalIndex)
                    {
                        
                        illegal = true;
                    }
                }

                
                if (GetCellWithCheck(newPos, _piece.piece) != -1 && !illegal)
                {
                    possibleMoves.Add(GetCellWithCheck(newPos, _piece.piece));
                }
            }
        }

        return possibleMoves;
    }

    public List<int> Castle()
    {
        var possibleMoves = new List<int>();
        if (!_piece.FirstMove) return null;
        if (GetComponent<GameManager>().InCheck1) return null;
        //king castle
    
        var board = boardCreator.pieceBoard;
        if (board[GETIndex(new Vector2(_pos.x + 1, _pos.y))] == ""
            && board[GETIndex(new Vector2(_pos.x + 2, _pos.y))] == "")
        {
            if (board[GETIndex(new Vector2(_pos.x + 3, _pos.y))] != "")
            {
                if (!boardCreator.board[GETIndex(new Vector2(_pos.x + 3, _pos.y))].transform.GetChild(0).GetComponent<Piece>().hasMoved)
                {
                    possibleMoves.Add(GETIndex(new Vector2(_pos.x + 2, _pos.y)));
                }
            }
        }

        if (board[GETIndex(new Vector2(_pos.x - 1, _pos.y))] != "" ||
            board[GETIndex(new Vector2(_pos.x - 2, _pos.y))] != "" ||
            board[GETIndex(new Vector2(_pos.x - 3, _pos.y))] != "") return possibleMoves;
        if (board[GETIndex(new Vector2(_pos.x - 4, _pos.y))] == "") return possibleMoves;
        if (!boardCreator.board[GETIndex(new Vector2(_pos.x - 4, _pos.y))].transform.GetChild(0).GetComponent<Piece>().hasMoved)
        {
            possibleMoves.Add(GETIndex(new Vector2(_pos.x - 2, _pos.y)));
        }

        return possibleMoves;
    }
    
    public int GetCellWithCheck(Vector2 pos, string piece)
    {
        var index = GETIndex(pos);
        
        if (boardCreator.pieceBoard[index] == "" || char.IsUpper(Convert.ToChar(boardCreator.pieceBoard[index])) != _piece.IsWhite())
        {
            return index;
        }

        return -1;
    }
    
    public bool MoveWithCheck(Vector2 pos, string piece)
    {
        var index = GETIndex(pos);
            
        return boardCreator.pieceBoard[index]  == "";
    }
}
