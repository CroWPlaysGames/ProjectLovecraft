using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
public static class ApplicationModel
{
    public static string Player_1_name;
    public static string Player_2_name;
    public static List<CardClass> Player_1_Deck = new List<CardClass>();
    public static List<CardClass> Player_2_Deck = new List<CardClass>();
    public static List<CardClass> SelectedDeck;
    public static List<CardClass> AllCardsDeck;
}
