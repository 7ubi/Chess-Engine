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
