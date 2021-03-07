using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public string piece;

    private bool IsMoving = false;
    private Vector2 offSet;
    
    private Transform nextParent;
    private Transform parent;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }
    
    void Update()
    {
        if (IsMoving)
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
            mousePos = mousePos - offSet;
            transform.position = new Vector3(mousePos.x, mousePos.y, -0.1f);
            
            GETParent();
        }
    }
    
    void OnMouseDown()
    {
        parent = transform.parent;
        transform.parent = null;
        IsMoving = true;
        offSet = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        GameObject.FindObjectOfType<Moves>().CreatePossibleMoves(parent, piece);
    }

    void OnMouseUp()
    {
        if (nextParent.GetComponent<Cell>().possibleMove)
        {
            if (nextParent.childCount != 0)
            {
                if (Char.IsUpper(Convert.ToChar(nextParent.GetChild(0).GetComponent<Piece>().piece)) &&
                    Char.IsUpper(Convert.ToChar(piece))
                    ||
                    !Char.IsUpper(Convert.ToChar(nextParent.GetChild(0).GetComponent<Piece>().piece)) &&
                    !Char.IsUpper(Convert.ToChar(piece)))
                {
                    transform.position = new Vector3(parent.position.x, parent.position.y, -0.1f);
                    transform.parent = parent;
                }
                else
                {
                    transform.position = new Vector3(nextParent.position.x, nextParent.position.y, -0.1f);
                    transform.parent = nextParent;
                    Destroy(nextParent.GetChild(0).gameObject);
                }
            }
            else
            {
                transform.parent = nextParent;
                transform.position = new Vector3(nextParent.position.x, nextParent.position.y, -0.1f);
            }
        }
        else
        {
            transform.position = new Vector3(parent.position.x, parent.position.y, -0.1f);
            transform.parent = parent;
        }

        GameObject.FindObjectOfType<Moves>().RemovePossibleMoves();
        IsMoving = false;
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
        var cells = (Cell[]) GameObject.FindObjectsOfType<Cell>();
        
        foreach (Cell cell in cells)
        {
            if (cell.mouseOnCell)
            {
                nextParent = cell.transform;
                return;
            }
        }
    }
}
