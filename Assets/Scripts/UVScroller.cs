using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Scrolling Background Code derived from: 
/// Unity 2D Space Shooter Game
/// https://noobtuts.com/unity/2d-space-shooter-game
/// </summary>
public class UVScroller : MonoBehaviour
{
    public Vector2 speed;

    void LateUpdate()
    {
        GetComponent<Renderer>().material.mainTextureOffset = speed * Time.time;
    }
}
