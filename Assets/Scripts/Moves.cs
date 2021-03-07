using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moves : MonoBehaviour
{
    [SerializeField] BoardCreator _boardCreator;
    
    public void createPossibleMoves(Transform parent, string piece)
    {
        if (piece.ToLower() == "n")
        {
            int cell = parent.GetComponent<Cell>().NumField;

            int x = cell % 8;
            int y = Convert.ToInt32(Math.Floor(cell / 8f));      
            
            Vector2[] moves =
            {
                new Vector2(x + 2, y + 1),
                new Vector2(x - 2, y + 1),
                new Vector2(x + 2, y - 1),
                new Vector2(x - 2, y - 1),
                new Vector2(x + 1, y + 2),
                new Vector2(x - 1, y + 2),
                new Vector2(x + 1, y - 2),
                new Vector2(x - 1, y - 2),
            };


            foreach (Vector2 move in moves)
            {
                if (move.x >= 0 && move.x < 8 && move.y >= 0 && move.y < 8)
                {
                    int index = Convert.ToInt32(move.x + 8 * move.y);
                    Cell moveCell = _boardCreator.board[index].GetComponent<Cell>();
                    if (moveCell.transform.childCount == 0)
                    {
                        moveCell.possibleMove = true;
                    }
                    else
                    {
                        Piece _p = moveCell.transform.GetChild(0).GetComponent<Piece>();
                        if(Char.IsUpper(Convert.ToChar(_p.piece)) && Char.IsUpper(Convert.ToChar(piece))
                           || !Char.IsUpper(Convert.ToChar(_p.piece)) && !Char.IsUpper(Convert.ToChar(piece)))
                        {
                            moveCell.possibleMove = false;
                        }
                        else
                        {
                            moveCell.possibleMove = true;
                        }
                    }
                }
            }
        }
    }

    public void removePossibleMoves()
    {
        foreach (var cell in _boardCreator.board)
        {
            cell.GetComponent<Cell>().possibleMove = false;
        }
    }
}
