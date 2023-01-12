using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using UnityEngine.UI;
using System.Linq;
using System;
using TMPro;

public class Abilities : MonoBehaviour 
{
    private GameManager game_manager;
    private Board board;
    private GameObject CardPrfab;
    private TweenManager tweens;
    private List<string> undead_mobs = new() { "008", "009", "011" };
    private List<CardClass> AllCards;
    public GameObject Previous;

    void Start()
    {
        
        game_manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        board = GameObject.Find("Board").GetComponent<Board>();
        tweens = GameObject.Find("TweenManager").GetComponent<TweenManager>();
        AllCards = game_manager.AllCards;
        CardPrfab = game_manager.card;
    }
    public void Heal(List<GameObject> affected_cards, int heal, string phase)
    {
        foreach (GameObject card in affected_cards)
        {
            int Default = int.Parse(AllCards.Find(x => x.ID == card.GetComponent<CardClass>().ID).Health);
            card.GetComponent<CardClass>().Health = (int.Parse(card.GetComponent<CardClass>().Health) + heal).ToString();
            if(int.Parse(card.GetComponent<CardClass>().Health) > Default) 
            {
                card.GetComponent<CardClass>().Health = Default.ToString();
            }
        }
    }
    public void GainHealth(List<GameObject> affected_cards, int heal)
    {
        foreach (GameObject card in affected_cards)
        {
            card.GetComponent<CardClass>().Health = (int.Parse(card.GetComponent<CardClass>().Health) + heal).ToString();
        }
    }
    public void GainSpeed(List<GameObject> affected_cards, int speed, string Seqence)
    {
        foreach (GameObject card in affected_cards)
        {
            int Default = int.Parse(AllCards.Find(x => x.ID == card.GetComponent<CardClass>().ID).Speed);
            card.GetComponent<CardClass>().Speed = (int.Parse(card.GetComponent<CardClass>().Speed) + speed).ToString();
        }
    }
     
    public void GainDamage(List<GameObject> affected_cards, int damage)
    {
        foreach (GameObject card in affected_cards)
        {
            card.GetComponent<CardClass>().Damage = (int.Parse(card.GetComponent<CardClass>().Damage) + damage).ToString();
        }
    }
    public void BuffSpeed(List<GameObject> affected_cards, int speed)
    {
        foreach (GameObject card in affected_cards)
        {
            if (!card.GetComponent<Effects>().EffectsList.ContainsKey("Speed"))
            {
                card.GetComponent<CardClass>().Speed = (int.Parse(card.GetComponent<CardClass>().Speed) + speed).ToString();
                card.GetComponent<Effects>().EffectsList.Add("Speed", speed.ToString());
            }
        }
    }
    public void BuffDamage(List<GameObject> affected_cards, int damage)
    {
        foreach (GameObject card in affected_cards)
        {
            if (!card.GetComponent<Effects>().EffectsList.ContainsKey("Damage"))
            {
                card.GetComponent<CardClass>().Damage = (int.Parse(card.GetComponent<CardClass>().Damage) + damage).ToString();
                card.GetComponent<Effects>().EffectsList.Add("Damage", damage.ToString());
            }

        }
    }
    public void BuffHealth(List<GameObject> affected_cards, int health)
    {
        foreach (GameObject card in affected_cards)
        {
            if (!card.GetComponent<Effects>().EffectsList.ContainsKey("Health")) 
            {
                card.GetComponent<CardClass>().Health = (int.Parse(card.GetComponent<CardClass>().Health) + health).ToString();
                card.GetComponent<Effects>().EffectsList.Add("Health", health.ToString());
            }
        }
    }

    public void Spawn(string id, Transform Position)
    {
        GameObject new_card = Instantiate(CardPrfab, Vector3.zero, Quaternion.identity);
        CardClass card_details = AllCards.Find(x => x.ID == id);
        new_card.GetComponent<CardClass>().SetDetails(card_details);
        new_card.GetComponent<CardClass>().InDeckView = false;
        if (gameObject.GetComponent<CardClass>().player_number == 1) 
        {
            new_card.GetComponent<CardClass>().hidden = true;
            new_card.GetComponent<CardClass>().player_number = 1;
        }
        else if (gameObject.GetComponent<CardClass>().player_number == 2)
        {
            new_card.GetComponent<CardClass>().hidden = true;
            new_card.GetComponent<CardClass>().player_number = 2;
            new_card.transform.Rotate(00.0f, 0.0f, 180.0f, Space.Self);
        }
        new_card.transform.SetParent(Position);
        new_card.transform.localScale = Vector3.zero;
        new_card.transform.localPosition = Vector3.zero;
        new_card.transform.Find("Grayed Out").GetComponent<Image>().enabled = false;
        new_card.GetComponent<CardClass>().placed = true;
        tweens.tweens.Summon(new_card);
        board.UpdateBoard();
    }

    public void Splitshot(GameObject card, string phase)
    {
        for(int i = 0; i < 5; i++) 
        {
            if (card.GetComponent<CardClass>().player_number == 1)
            {
                Debug.Log(i);
                if(!(board.CardPositions2[i].transform.childCount == 0)) 
                {
                    if (card.transform.parent.gameObject == board.CardPositions2[i])
                    {
                        List<GameObject> Cards = new List<GameObject>(2);
                        if (i == 4)
                        {
                            if (!(board.CardPositions[i-1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions[i-1].transform.GetChild(0).gameObject);
                            }
                        }
                        else if (i == 0)
                        {
                            if (!(board.CardPositions[i+1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions[i+1].transform.GetChild(0).gameObject);
                            }
                        }
                        else
                        {
                            if (!(board.CardPositions[i-1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions[i-1].transform.GetChild(0).gameObject);
                            }
                            if (!(board.CardPositions[i+1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions[i+1].transform.GetChild(0).gameObject);
                            }

                        }
                        Heal(Cards, -1, "Battle");
                        
                    }
                }
                
            }

            else if (card.GetComponent<CardClass>().player_number == 2)
            {
                if (!(board.CardPositions[i].transform.childCount == 0))
                {
                    if (card.transform.parent.gameObject == board.CardPositions[i])
                    {
                        List<GameObject> Cards = new List<GameObject>(2);
                        if (i == 4)
                        {
                            if (!(board.CardPositions2[i-1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions2[i-1].transform.GetChild(0).gameObject);
                            }
                        }
                        else if (i == 0)
                        {
                            if (!(board.CardPositions2[i+1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions2[i+1].transform.GetChild(0).gameObject);
                            }
                        }
                        else
                        {
                            if(!(board.CardPositions2[i-1].transform.childCount == 0)) 
                            {
                                Cards.Add(board.CardPositions2[i-1].transform.GetChild(0).gameObject);
                            }
                            if(board.CardPositions2[i+1].transform.childCount == 1) 
                            {
                                Cards.Add(board.CardPositions2[i+1].transform.GetChild(0).gameObject);
                            }
                            
                        }
                        Heal(Cards, -1, "Battle");
                    }
                }
  
            }
        }
    }

    public void StealCard(GameObject card)
    {
        if (card.GetComponent<CardClass>().player_number == 1)
        {
            game_manager.AddCard(game_manager.player_1_hand, game_manager.player_2_deck);
        }
        if (card.GetComponent<CardClass>().player_number == 2)
        {
            game_manager.AddCard(game_manager.player_2_hand, game_manager.player_1_deck);
        }
    }

    public void Poison(List<GameObject> affected_cards, int amount)
    {
        foreach (GameObject card in affected_cards)
        {
            if (!card.GetComponent<Effects>().EffectsList.ContainsKey("Poison"))
            {
                card.GetComponent<Effects>().EffectsList.Add("Poison", amount.ToString());
            }
        }
    }

    public void DeathTouch(List<GameObject> affected_cards)
    {
            foreach (GameObject card in affected_cards)
            {
                if (!card.GetComponent<Effects>().EffectsList.ContainsKey("DeathTouch"))
                {
                card.GetComponent<Effects>().EffectsList.Add("DeathTouch", "0");
                }
            }
    }

    public void SpawnToHand(string id, Transform hand)
    {
        if (hand.childCount <= 7) 
        {
            GameObject new_card = Instantiate(CardPrfab, Vector3.zero, Quaternion.identity);
            CardClass card_details = AllCards.Find(x => x.ID == id);
            new_card.GetComponent<CardClass>().SetDetails(card_details);
            new_card.GetComponent<CardClass>().InDeckView = false;
            new_card.GetComponent<CardClass>().hidden = false;
            if (gameObject.GetComponent<CardClass>().player_number == 1)
            {
                new_card.GetComponent<CardClass>().player_number = 1;
                game_manager.player_1_handcards.Add(new_card);
            }
            else if (gameObject.GetComponent<CardClass>().player_number == 2)
            {
                new_card.GetComponent<CardClass>().player_number = 2;
                game_manager.player_2_handcards.Add(new_card);
            }
            new_card.transform.SetParent(hand);
            new_card.transform.localScale = Vector3.one;
            new_card.transform.localPosition = Vector3.zero;
            board.UpdateBoard();
        }
        
    }

    public void AdjustCardDeck(string id, List<CardClass> Deck)
    {
        if (Deck.Exists(x => x.ID == id)) 
        {
            System.Random rnd = new System.Random();
            CardClass Temp = Deck.Find(x => x.ID == id);
            Deck.RemoveAt(Deck.FindIndex(x => x.ID == id));
            Deck.Insert(rnd.Next(2), Temp);
        }
       
    }
}