using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public GameObject piece;
    
    //black
    [SerializeField] private Sprite p;
    [SerializeField] private Sprite n;
    [SerializeField] private Sprite b;
    [SerializeField] private Sprite r;
    [SerializeField] private Sprite q;
    [SerializeField] private Sprite k;
    //white
    [SerializeField] private Sprite P;
    [SerializeField] private Sprite N;
    [SerializeField] private Sprite B;
    [SerializeField] private Sprite R;
    [SerializeField] private Sprite Q;
    [SerializeField] private Sprite K;

    public readonly Dictionary<string, Sprite> pieces = new Dictionary<string, Sprite>();

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

    public void CreatePiece(string _piece, Transform parent)
    {
        var p = Instantiate(this.piece, parent);
        p.GetComponent<SpriteRenderer>().sprite = pieces[_piece];
        p.GetComponent<Piece>().piece = _piece;
    }
}
