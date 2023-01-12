using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using UnityEngine.UI;
using TMPro;

public class Effects : MonoBehaviour
    {
        public GameManager game_manager;
        public Board board;
    public bool CanPlace = true;
        public Dictionary<string, string> EffectsList = new Dictionary<string, string>();
    private void Start()
    {
        board = GameObject.Find("Board").GetComponent<Board>();
        game_manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }
    public void SummonCheck(Dictionary<string, int> requirments)
    {
        foreach (KeyValuePair<string, int> kvp in requirments)
        {
            int count = 0;
            int index = 0;
            foreach (GameObject card in board.CardPositions2)
            {
                if (!(card.transform.childCount == 0))
                {
                    if (card.transform.GetChild(0).GetComponent<CardClass>().ID == kvp.Key)
                    {
                        count++;
                    }
                }
            }
            if (count >= kvp.Value)
            {
                index++;
                if (index == requirments.Count)
                {
                    CanPlace = true;
                    gameObject.transform.Find("Grayed Out").GetComponent<Image>().enabled = false;
                }
            }
            else 
            {
                if (!gameObject.GetComponent<CardClass>().placed == true)
                {
                    CanPlace = false;
                    gameObject.transform.Find("Grayed Out").GetComponent<Image>().enabled = true;
                }

            }
        }
    }

}

