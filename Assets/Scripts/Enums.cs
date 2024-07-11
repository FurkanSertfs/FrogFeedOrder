using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums
{
    public enum CellType
    {
        Empty,
        Frog,
        Berry,
        Arrow,
    }

    public enum Direction
    {
        No = -1,
        Up = 2,
        Down = 0,
        Left = 1,
        Right = 3,
    }

    public enum Color
    {
        Blue,
        Green,
        Red,
        Purple,
        Yellow,
    }
}
