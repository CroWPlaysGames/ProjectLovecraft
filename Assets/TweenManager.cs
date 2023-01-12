using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenManager : MonoBehaviour
{
    [Header("Tween Manager Settings")]
    [HideInInspector]public GameManager game_manager;
    [HideInInspector]public Board board;
    [HideInInspector]public HUD hud;
    [HideInInspector]public Tweens tweens;
    void Awake()
    {
        hud = GameObject.Find("HUD").GetComponent<HUD>();
        board = GameObject.Find("Board").GetComponent<Board>();
        game_manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        tweens = gameObject.GetComponent<Tweens>();
    }
    
}
