using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.Serialization;

public class LegalMoves : MonoBehaviour
{
    private Piece _piece;
    private Vector2 _pos;
    private Moves _moves;
    private Movement _movement;
    [SerializeField] private BoardCreator boardCreator;

    private void Start()
    {
        _moves = gameObject.GetComponent<Moves>();
        _movement = gameObject.GetComponent<Movement>();
    }

    public void UpdatePiece()
    {
        _piece = _moves.Piece;
        _pos = _moves.Pos;
    }


    public List<int> IllegalKingMoves()
    {
        var king = boardCreator.board[KingIndex()].transform.GetChild(0).GetComponent<Piece>();
        _moves.SetPiece(king);
        var kingMoves = GetComponent<Movement>().KingMove();
        var illegalMoves = new List<int>();
        
        
        for (var i = 0; i < boardCreator.pieceBoard.Length; i++)
        {
            if(boardCreator.pieceBoard[i] == "") continue;
            if (char.IsUpper(Convert.ToChar(boardCreator.pieceBoard[i])) == king.IsWhite()) continue;
            var p = boardCreator.board[i].transform.GetChild(0).GetComponent<Piece>();
            _moves.SetPiece(p);
            
            var moves = _moves.PossibleMoves();
            if (p.piece.ToLower() == "p")
            {
                moves = GetComponent<Movement>().PawnTakes();
            }

            if (p.piece.ToLower() == "k")
            {
                moves = GetComponent<Movement>().KnightTakes();
            }

            if (moves == null)
                return illegalMoves;
            foreach (var move in moves)
            {
                for (var j = kingMoves.Count - 1; j >= 0; j--)
                {
                    if (move != kingMoves[j]) continue;
                    kingMoves.Remove(move);
                    illegalMoves.Add(move);
                }
            }
        }
        return illegalMoves;
    }

    public int PinDiagonal(Vector2 pos)
    {
        var kingPos = _movement.GETPosFromIndex(KingIndex());
        var dx = kingPos.x - pos.x;
        var dy = kingPos.y - pos.y;
        
        
        if(Math.Abs(Math.Abs(dx) - Math.Abs(dy)) > 0.1)
            return -1;
        var dist = Math.Abs(dx);
        dx = dx < 0 ? -1 : 1;
        dy = dy < 0 ? -1 : 1;

        var canBePin = false;
        var pinIndex = -1;
        
        for (var i = 1; i < dist; i++)
        {
            var newPos = new Vector2(pos.x + i * dx, pos.y + i * dy);
            var index = _movement.GETIndex(newPos);
            
            if(boardCreator.pieceBoard[index] == "")
                continue;
            if (boardCreator.board[index].transform.GetChild(0).GetComponent<Piece>().IsWhite() ==
                _moves.Piece.IsWhite())
                return -1;
            if (!canBePin)
            {
                canBePin = true;
                pinIndex = index;
            }
            else
            {
                return -1;
            }
        }
        
        return pinIndex;
    }
    
    private int KingIndex()
    {
        UpdatePiece();
        var kingIndex = 0;
        for (var i = 0; i < boardCreator.pieceBoard.Length; i++)
        {
            var p = boardCreator.pieceBoard[i];
            if(p == "") continue;
            if (p.ToLower() != "k" || _piece.IsWhite() == char.IsUpper(Convert.ToChar(p))) continue;
            kingIndex = i;
            break;
        }

        return kingIndex;
    }
}
