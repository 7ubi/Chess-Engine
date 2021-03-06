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
                createSquare(i + xOff, j + yOff, (i + j) % 2 == 0);
            }
        }
    }

    void createSquare(float x, float y, bool isLight)
    {
        GameObject s = Instantiate(square, this.gameObject.transform);
        s.transform.position = new Vector2(x, y);
        if (isLight)
        {
            s.GetComponent<SpriteRenderer>().color = LightColor;
        }
        else
        {
            s.GetComponent<SpriteRenderer>().color = DarkColor;
        }
    }
}
