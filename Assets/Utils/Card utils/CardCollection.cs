using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace Cards
{
    public class CardCollection
    {
        //Fields
        private List<CardClass> Deck = new List<CardClass>();
        private System.Random rng = new System.Random();

        void Awake()
        {

        }
        //constructor
        public CardCollection()
        {

        }

        public CardCollection(List<CardClass> cards)
        {
            Deck = cards;
        }

        //methods
        public void Add(CardClass Card)
        {
            Deck.Add(Card);
        }

        public CardClass FindWithID(int temp)
        {
            return Deck.Find(x => Convert.ToInt32(x.ID) == temp);
        }

        public List<CardClass> ToList()
        {
            return Deck; 
        }


        public List<CardClass> RandomList()
        {
            var _deck = Deck;
            int n = _deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = _deck[k];
                _deck[k] = _deck[n];
                _deck[n] = value;
            }
            return _deck;
        }
    }
}
