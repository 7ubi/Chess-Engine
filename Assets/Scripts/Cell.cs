using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public BoardCreator Board;
    public Piece pieceManager;

    public Vector2 Pos;

    public bool IsDark;

    SpriteRenderer _sp;

    public int NumField;

    private void Start()
    {
        _sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = new Vector2(Pos.x + Board.xOff, Pos.y + Board.yOff);
        _sp.color = IsDark ? Board.DarkColor : Board.LightColor;
    }
    
    void OnMouseDown()
    {
        if (transform.childCount == 0)
        {
            pieceManager.createPiece("b", this.gameObject.transform);
        }
        else
        {
            Destroy(transform.GetChild(0).gameObject);
        }

    }
}
