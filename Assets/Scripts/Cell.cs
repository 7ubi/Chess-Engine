using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using UnityEngine.Serialization;
public class Cell : MonoBehaviour
{
    public BoardCreator Board;
    public PieceManager pieceManager;

    public Vector2 Pos;

    public bool IsDark;

    SpriteRenderer _sp;

    public int NumField;

    public bool mouseOnCell = false;

    public bool possibleMove = false;
    public bool illegalMove = false;
    public bool check = false;
    
    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = new Vector2(Pos.x + Board.xOff, Pos.y + Board.yOff);
        _sp.color = IsDark ? Board.DarkColor : Board.LightColor;
        if (possibleMove)
        {
            _sp.color = IsDark ? Board.DarkSelectedColor : Board.LightSelectedColor;
        }

        var color = _sp.color;
        color = check ? Board.checkColor : color;
        color = illegalMove ? Board.IllegalMoveColor : color;
        _sp.color = color;
    }

    void OnMouseOver()
    {
        mouseOnCell = true; 
    }

    //void OnMouseDown()
    //{
       // Debug.Log(NumField + " " + new Vector2(NumField % 8, Convert.ToInt32(Math.Floor(NumField / 8f))));
    //}
    
    void OnMouseExit()
    {
        mouseOnCell = false;
    }
}
