using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorsSO", menuName = "ColorsSO", order = 0)]
public class ColorsSO : ScriptableObject
{
    public Enums.Color color;
    public Texture2D cellTexture;
    public Texture2D frogTexture;
    public Texture2D grapeTexture;
    public Texture2D arrowTexture;
}
