using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class PlayerDecks : MonoBehaviour
{
    public string player_1_name;
    public string player_2_name;
    public static PlayerDecks instance;
    public List<CardClass> player_1_deck;
    public List<CardClass> player_2_deck;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
