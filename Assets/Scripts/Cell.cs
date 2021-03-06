using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public BoardCreator board;

    public Vector2 pos;

    public bool isDark;

    SpriteRenderer sp;



    private void Start()
    {
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.position = new Vector2(pos.x + board.xOff, pos.y + board.yOff);
        if (isDark)
        {
            sp.color = board.DarkColor;
        }
        else
        {
            sp.color = board.LightColor;
        }
    }
}
