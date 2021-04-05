using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Moves : MonoBehaviour
{
    [SerializeField] BoardCreator _boardCreator;
    private GameManager _gameManager;
    private Movement _movement;
    private LegalMoves _legalMoves;
    private List<int> illegalKingMoves = new List<int>();

    private Vector2 _pos;
    private Piece _piece;

    private void Start()
    {
        _gameManager = GetComponent<GameManager>();
        _movement = GetComponent<Movement>();
        _legalMoves = GetComponent<LegalMoves>();
    }

    public Piece Piece
    {
        get => _piece;
        set => _piece = value;
    }

    public Vector2 Pos
    {
        get => _pos;
        set => _pos = value;
    }


    public void CreatePossibleMoves(Piece _piece)
    {
        this._piece = _piece;
        _pos = _movement.GETPosFromIndex(this._piece.parent.GetComponent<Cell>().NumField);
        _movement.UpdatePiece();
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
        _pos = _movement.GETPosFromIndex(this._piece.transform.parent.GetComponent<Cell>().NumField);
        _movement.UpdatePiece();
        var madeCheck = false;

        var moves = PossibleMoves();
        
        foreach (var m in moves.Where(m => _boardCreator.pieceBoard[m].ToLower() == "k"))
        {
            _boardCreator.board[m].GetComponent<Cell>().check = true;
            
            madeCheck = true;
        }
        
        illegalKingMoves.AddRange(_legalMoves.IllegalKingMoves());

        foreach (var illegalKingMove in IllegalKingMoves)
        {
            _boardCreator.board[illegalKingMove].GetComponent<Cell>().illegalMove = true;
        }
        
        return madeCheck;
    }

    public List<int> PossibleMoves()
    {
        var moves = new List<int>();
        if (_piece.piece.ToLower() == "n")
        {
            if (_movement.KnightMovement() != null)
            {
                moves.AddRange(_movement.KnightMovement());
            }
        }

        if (_piece.piece.ToLower() == "p")
        {
            if (_movement.PawnMovement() != null)
            {
                moves.AddRange(_movement.PawnMovement());
            }
        }

        if (_piece.piece.ToLower() == "q" || _piece.piece.ToLower() == "r")
        {
            if (_movement.RookMovement() != null)
            {
                moves.AddRange(_movement.RookMovement());
            }
        }
        
        if (_piece.piece.ToLower() == "q" || _piece.piece.ToLower() == "b")
        {
            if (_movement.DiagonalMovement() != null)
            {
                moves.AddRange(_movement.DiagonalMovement());
            }
        }

        if (_piece.piece.ToLower() == "k")
        {
            if (_movement.KingMove() != null)
            {
                moves.AddRange(_movement.KingMove());
            }

            if (_movement.Castle() != null)
            {
                moves.AddRange(_movement.Castle());
            }
        }

        moves = LegalMoves(moves);
        return moves;
    }

    private static List<int> LegalMoves(List<int> moves)
    {
        var legalMoves = moves;
        

        return legalMoves;
    }

    public void SetPiece(Piece p)
    {
        this._piece = p;
        _pos = _movement.GETPosFromIndex(this._piece.parent.GetComponent<Cell>().NumField);
        _movement.UpdatePiece();
    }

    public void RemoveCheck()
    {
        foreach(var c in _boardCreator.board)
        {
            c.GetComponent<Cell>().check = false;
            c.GetComponent<Cell>().illegalMove = false;
        }
        
        illegalKingMoves = new List<int>();
    }
    

    public List<int> IllegalKingMoves
    {
        get => illegalKingMoves;
        set => illegalKingMoves = value;
    }
}
