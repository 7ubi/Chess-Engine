using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    void OnMouseOver()
    {
        mouseOnCell = true;
    }

    void OnMouseExit()
    {
        mouseOnCell = false;
    }
}
