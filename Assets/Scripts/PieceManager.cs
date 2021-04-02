using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public GameObject piece;
    
    //black
    [SerializeField] Sprite p;
    [SerializeField] Sprite n;
    [SerializeField] Sprite b;
    [SerializeField] Sprite r;
    [SerializeField] Sprite q;
    [SerializeField] Sprite k;
    //white
    [SerializeField] Sprite P;
    [SerializeField] Sprite N;
    [SerializeField] Sprite B;
    [SerializeField] Sprite R;
    [SerializeField] Sprite Q;
    [SerializeField] Sprite K;

    public Dictionary<string, Sprite> pieces = new Dictionary<string, Sprite>();

    void Start()
    {
        //black
        pieces.Add("p", p);
        pieces.Add("n", n);
        pieces.Add("b", b);
        pieces.Add("r", r);
        pieces.Add("q", q);
        pieces.Add("k", k);
        //white
        pieces.Add("P", P);
        pieces.Add("N", N);
        pieces.Add("B", B);
        pieces.Add("R", R);
        pieces.Add("Q", Q);
        pieces.Add("K", K);
    }

    public void CreatePiece(string piece, Transform parent)
    {
        GameObject _p = Instantiate(this.piece, parent);
        _p.GetComponent<SpriteRenderer>().sprite = pieces[piece];
        _p.GetComponent<Piece>().piece = piece;
    }
}
