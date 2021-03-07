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
    private Transform lastParent;

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
            
            getParent();
        }
    }
    
    void OnMouseDown()
    {
        lastParent = transform.parent;
        transform.parent = null;
        IsMoving = true;
        offSet = cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    void OnMouseUp()
    {
        if (nextParent.childCount != 0)
        {
            if (Char.IsUpper(Convert.ToChar(nextParent.GetChild(0).GetComponent<Piece>().piece)) &&
                Char.IsUpper(Convert.ToChar(piece))
                ||
                !Char.IsUpper(Convert.ToChar(nextParent.GetChild(0).GetComponent<Piece>().piece)) &&
                !Char.IsUpper(Convert.ToChar(piece)))
            {
                transform.position = new Vector3(lastParent.position.x, lastParent.position.y, -0.1f);
                transform.parent = lastParent;
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
        
        
        IsMoving = false;
    }

    void getParent()
    {
        Cell[] cells = (Cell[]) GameObject.FindObjectsOfType<Cell>();
        
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