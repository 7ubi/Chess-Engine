using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Piece : MonoBehaviour
{
    public string piece;

    public bool hasMoved = false;
    private bool _firstMove = true;
    
    private bool _isMoving = false;
    private Vector2 _offSet;
    
    private Transform _nextParent;
    public Transform parent;

    private Camera _cam;
    private GameManager _gameManager;
    private BoardCreator _board;

    private AudioSource _moveSound;
    private AudioSource _takeSound;
    
    void Start()
    {
        _cam = Camera.main;
        _gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
         _board = GameObject.FindObjectOfType<BoardCreator>().GetComponent<BoardCreator>();
         _moveSound = GameObject.Find("Move").GetComponent<AudioSource>();
         _takeSound = GameObject.Find("Capture").GetComponent<AudioSource>();
         parent = transform.parent;
    }
    
    void Update()
    {
        if (!_isMoving) return;
        Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mousePos = mousePos - _offSet;
        transform.position = new Vector3(mousePos.x, mousePos.y, -0.2f);
            
        GETParent();
    }
    
    void OnMouseDown()
    {
        if(IsWhite() == !_gameManager.IsWhiteTurn)
            return;
        parent = transform.parent;
        transform.parent = null;
        _isMoving = true;
        _offSet = _cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        GameObject.FindObjectOfType<Moves>().CreatePossibleMoves(GetComponent<Piece>());
    }

    void OnMouseUp()
    {
        if(char.IsUpper(Convert.ToChar(piece)) == !_gameManager.IsWhiteTurn)
            return;
        var playedSound = false;
        if (_nextParent.GetComponent<Cell>().possibleMove)
        {
            playedSound = Move(_nextParent);
        }
        else
        {
            transform.position = new Vector3(parent.position.x, parent.position.y, -0.1f);
            transform.parent = parent;
        }
        Castles();
        GameObject.FindObjectOfType<Moves>().RemovePossibleMoves();
        _isMoving = false;
        if (piece.ToLower() == "p")
        {
            var y = Convert.ToInt32(Math.Floor(transform.parent.GetComponent<Cell>().NumField / 8f));
            if (y == 0 || y == 7)
            {
                piece = char.IsUpper(Convert.ToChar(piece)) ? "Q" : "q";
                gameObject.GetComponent<SpriteRenderer>().sprite = GameObject.FindObjectOfType<PieceManager>().pieces[piece];
            }
        }
        
        if (hasMoved && !playedSound)
        {
            _moveSound.Play();
        }

        if (hasMoved)
        {
            _firstMove = false;
        }

        hasMoved = false;
        _board.UpdateBoard();
        _gameManager.Check();

        //TODO en passant
    }

    public bool Move(Transform nextParent)
    {
        var playedSound = false;
        if (nextParent.childCount != 0)
        {
            if (char.IsUpper(Convert.ToChar(nextParent.GetChild(0).GetComponent<Piece>().piece)) ==
                char.IsUpper(Convert.ToChar(piece)))
            {
                var transform1 = transform;
                transform1.position = new Vector3(parent.position.x, parent.position.y, -0.1f);
                transform1.parent = parent;
            }
            else
            {
                hasMoved = true;
                var position = nextParent.position;
                var transform1 = transform;
                transform1.position = new Vector3(position.x, position.y, -0.1f);
                transform1.parent = nextParent;
                parent = nextParent;
                _gameManager.Move();
                
                _takeSound.Play();
                playedSound = true;
                Destroy(nextParent.GetChild(0).gameObject);
            }
        }
        else
        {
            hasMoved = true;
            var transform1 = transform;
            transform1.parent = nextParent;
            var position = nextParent.position;
            parent = nextParent;
            transform1.position = new Vector3(position.x, position.y, -0.1f);
            _gameManager.Move();
        }

        GameObject.FindObjectOfType<Moves>().RemovePossibleMoves();
        return playedSound;
    }

    private void Castles()
    {
        if (piece.ToLower() != "k") return;
        if (!_firstMove) return;
        var cell = transform.parent.GetComponent<Cell>().NumField;
        if (Math.Abs(cell - parent.GetComponent<Cell>().NumField) != 2) return;
        switch (cell % 8)
        {
            case 6:
            {
                var rook = _board.board[cell + 1].transform.GetChild(0);
                rook.parent = _board.board[cell - 1].transform;
                rook.GetComponent<Piece>().parent = _board.board[cell - 1].transform;
                rook.position = new Vector3(_board.board[cell - 1].transform.position.x, _board.board[cell - 1].transform.position.y, -0.1f);
                break;
            }
            case 2:
            {
                var rook = _board.board[cell - 2].transform.GetChild(0);
                rook.parent = _board.board[cell + 1].transform;
                rook.GetComponent<Piece>().parent = _board.board[cell + 1].transform;
                rook.position = new Vector3(_board.board[cell + 1].transform.position.x, _board.board[cell + 1].transform.position.y, -0.1f);
                break;
            }
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void GETParent()
    {
        foreach (var b in _board.board)
        {
            if (!b.GetComponent<Cell>().mouseOnCell) continue;
            _nextParent = b.transform;
            return;
        }
    }

    public bool IsWhite()
    {
        return char.IsUpper(Convert.ToChar(piece));
    }
    

    public bool FirstMove
    {
        get => _firstMove;
        set => _firstMove = value;
    }
}
