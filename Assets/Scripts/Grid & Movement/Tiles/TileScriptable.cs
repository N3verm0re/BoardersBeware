using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Tile##", menuName = "ScriptableObjects/Tiles", order = 1)]
public class TileScriptable : ScriptableObject
{
    [Header("Info")]
    public new string name;
    
    [Header("Walls")]
    public bool north;
    public bool west;
    public bool south;
    public bool east;

    [Header("Tile Graphics")]
    public Sprite TileSprite;
    public float tileSizeX;
    public float tileSizeY;
}
