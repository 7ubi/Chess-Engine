using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
    public Color LightColor;
    public Color DarkColor;

    public float xOff;
    public float yOff;
    public float size;

    public GameObject square;


    private void Start()
    {
        for(int i = 0; i < 8; i++)
        {
            for(int j = 0; j < 8; j++)
            {
                createSquare(i, j, (i + j) % 2 == 0);
            }
        }
    }

    void createSquare(float x, float y, bool isDark)
    {
        GameObject s = Instantiate(square, this.gameObject.transform);
        s.GetComponent<Cell>().pos = new Vector2(x, y);
        s.GetComponent<Cell>().board = gameObject.GetComponent<BoardCreator>();
        s.GetComponent<Cell>().isDark = isDark;
    }
}
