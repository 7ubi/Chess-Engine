using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public string piece;

    public bool hasMoved = false;
    
    private bool _isMoving = false;
    private Vector2 _offSet;
    
    private Transform _nextParent;
    private Transform _parent;

    private Camera _cam;
    private GameManager _gameManager;
    private BoardCreator _board;
    
    void Start()
    {
        _cam = Camera.main;
        _gameManager = GameObject.FindObjectOfType<GameManager>().GetComponent<GameManager>();
         _board = GameObject.FindObjectOfType<BoardCreator>().GetComponent<BoardCreator>();
    }
    
    void Update()
    {
        if (_isMoving)
        {
            Vector2 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos = mousePos - _offSet;
            transform.position = new Vector3(mousePos.x, mousePos.y, -0.1f);
            
            GETParent();
        }
    }
    
    void OnMouseDown()
    {
        if(char.IsUpper(Convert.ToChar(piece)) == !_gameManager.IsWhiteTurn)
            return;
        _parent = transform.parent;
        transform.parent = null;
        _isMoving = true;
        _offSet = _cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        GameObject.FindObjectOfType<Moves>().CreatePossibleMoves(_parent, piece, GetComponent<Piece>());
    }

    void OnMouseUp()
    {
        if(char.IsUpper(Convert.ToChar(piece)) == !_gameManager.IsWhiteTurn)
            return;
        var firstMove = !hasMoved;
        if (_nextParent.GetComponent<Cell>().possibleMove)
        {
            if (_nextParent.childCount != 0)
            {
                if (Char.IsUpper(Convert.ToChar(_nextParent.GetChild(0).GetComponent<Piece>().piece)) ==
                    Char.IsUpper(Convert.ToChar(piece)))
                {
                    transform.position = new Vector3(_parent.position.x, _parent.position.y, -0.1f);
                    transform.parent = _parent;
                }
                else
                {
                    hasMoved = true;
                    transform.position = new Vector3(_nextParent.position.x, _nextParent.position.y, -0.1f);
                    transform.parent = _nextParent;
                    _gameManager.Move();
                    Destroy(_nextParent.GetChild(0).gameObject);
                }
            }
            else
            {
                hasMoved = true;
                transform.parent = _nextParent;
                transform.position = new Vector3(_nextParent.position.x, _nextParent.position.y, -0.1f);
                _gameManager.Move();
            }
        }
        else
        {
            transform.position = new Vector3(_parent.position.x, _parent.position.y, -0.1f);
            transform.parent = _parent;
        }

        if (piece.ToLower() == "k")
        {
            if (firstMove)
            {
                var cell = transform.parent.GetComponent<Cell>().NumField;
                if (Math.Abs(cell - _parent.GetComponent<Cell>().NumField) ==
                    2)
                {
                    if(cell % 8 == 6)
                    {
                        if (_board.board[cell + 1].transform.childCount != 0)
                        {
                            if (_board.board[cell + 1].transform.GetChild(0).GetComponent<Piece>().piece.ToLower() == "r"
                                && !_board.board[cell + 1].transform.GetChild(0).GetComponent<Piece>().hasMoved
                            )
                            {
                                var rook = _board.board[cell + 1].transform.GetChild(0);
                                rook.parent = _board.board[cell - 1].transform;
                                rook.GetComponent<Piece>()._parent = _board.board[cell - 1].transform;
                                rook.position = _board.board[cell - 1].transform.position;
                            }
                        }
                    }else if (cell % 8 == 2)
                    {
                        if (_board.board[cell - 2].transform.childCount != 0)
                        {
                            if (_board.board[cell - 2].transform.GetChild(0).GetComponent<Piece>().piece.ToLower() == "r"
                                && !_board.board[cell - 2].transform.GetChild(0).GetComponent<Piece>().hasMoved
                            )
                            {
                                var rook = _board.board[cell - 2].transform.GetChild(0);
                                rook.parent = _board.board[cell + 1].transform;
                                rook.GetComponent<Piece>()._parent = _board.board[cell + 1].transform;
                                rook.position = _board.board[cell + 1].transform.position;
                            }
                        }
                    }
                }
            }
        }

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
        
        //TODO en passant
    }

    void GETParent()
    {
        foreach (GameObject b in _board.board)
        {
            Cell cell = b.GetComponent<Cell>();
            if (cell.mouseOnCell)
            {
                _nextParent = cell.transform;
                return;
            }
        }
    }
}
