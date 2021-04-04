using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Moves moves;
    [SerializeField] private BoardCreator boardCreator;

    [SerializeField] private TMP_Text _whiteText;
    [SerializeField] private TMP_Text _blackText;

    [SerializeField] float startTime;
    private float _whiteTime;
    private float _blackTime;
    private bool _firstMoveMade = false;

    private void Start()
    {
        _whiteTime = startTime;
        _blackTime = startTime;
    }

    private void Update()
    {
        if (_firstMoveMade)
        {
            if (IsWhiteTurn)
            {
                _whiteTime -= Time.deltaTime;
            }
            else
            {
                _blackTime -= Time.deltaTime;
            }
        }
        UpdateText();
    }

    void UpdateText()
    {
        _whiteText.text = TimeSpan.FromSeconds(Convert.ToDouble(_whiteTime)).ToString(@"mm\:ss");
        _blackText.text = TimeSpan.FromSeconds(Convert.ToDouble(_blackTime)).ToString(@"mm\:ss");
    }

    public void Move()
    {
        IsWhiteTurn = !IsWhiteTurn;
        _firstMoveMade = true;
    }

    public bool Check()
    {
        var check = false;
        moves.RemoveCheck();
        var pieces = GameObject.FindObjectsOfType<Piece>();
        foreach (var piece in pieces)
        {
            if (piece.IsWhite() == IsWhiteTurn) continue;
            if (moves.Check(piece))
                check = true;
        }

        InCheck1 = check;
        return check;
    }

    public bool IsWhiteTurn { get; private set; } = true;

    public bool InCheck1 { get; set; }
}
