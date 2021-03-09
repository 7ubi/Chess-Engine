using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    private bool _isWhiteTurn = true;

    [SerializeField] private Moves moves;

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
            if (_isWhiteTurn)
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
        _isWhiteTurn = !_isWhiteTurn;
        _firstMoveMade = true;
    }

    public void Check()
    {
        moves.removeCheck();
        var pieces = GameObject.FindObjectsOfType<Piece>();
        foreach (var piece in pieces)
        {
            moves.Check(piece);
        }
    }

    public bool IsWhiteTurn => _isWhiteTurn;
}
