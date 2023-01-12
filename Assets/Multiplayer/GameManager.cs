using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using System;
using System.Threading.Tasks;
using Cards;
using System.Linq;
using TMPro;


public class GameManager : MonoBehaviour
{
    public enum player_number
    {
        Player_1,
        Player_2
    }

    [Header("Player Management")]
    public bool Player1RoundStart = false;
    public bool Flippped = false;
    public bool manaflipped = false;
    public bool Healthflipped = false;
    public bool handflipped = false;
    public bool BoardIsFlipped = false;
    [SerializeField] public player_number player;

    private List<GameObject> SavedCards = new List<GameObject>(6);
    
    public List<CardClass> player_1_deck;
    public List<CardClass> player_2_deck;

    public GameObject player_1_hand;
    public GameObject player_2_hand;

    public List<GameObject> player_1_handcards;
    public List<GameObject> player_2_handcards;

    public bool player_1_turn;
    public bool player_2_turn;

    public bool EndButtonPressed;

    [HideInInspector] public bool player_1_confirmed;
    [HideInInspector] public bool player_2_confirmed;

    [HideInInspector] public bool player_1_token = true;

    [Header ("Game Settings")]
    public bool Goblindeck;
    public int round_count = 1;
    public int max_health;
    public int current_health = 0;
    public bool TurnScreen;
    [Header("Mana Settings")]
    public int max_mana = 5;
    public int mana_pool = 0;
    public int player_1_mana = 0;
    public int player_2_mana = 0;

    public int time_per_turn;

    [Header ("Card Management")]
    [HideInInspector] public List<CardClass> all_cards = new List<CardClass>();
    public GameObject card;
    [HideInInspector] public bool has_placed_card = false;
    public int skipped_turn = 0;
    public bool player1canplay = true;
    public bool player2canplay = true;
    [HideInInspector] public bool is_viewing = false;

    [HideInInspector] public List<GameObject> card_slotted = new List<GameObject>();
    private List<GameObject> card_ordered;
    [Header ("GameObject References")]
    private Board board;
    private HUD hud;
    public GameObject blur;
    public GameObject AnimationTop;
    public GameObject AnimationBottom;
    public GameObject CloseCardView;
    public GameObject BoardBackgroundflipped;
    public List<CardClass> AllCards;
    public GameObject Ready;
    public GameObject Startbutton;
    public GameObject Quitbutton;
    public GameObject ReadyText;
    private Tweens tweens;
    public GameObject PhaseWidget;
    public List<int> tDescrs;
    private List<GameObject> DeleteList = new List<GameObject>();
    public LTSeq GameSeq;
    private void Awake()
    {
        GameSeq = LeanTween.sequence();
        player_1_deck = ApplicationModel.Player_1_Deck;
        player_2_deck = ApplicationModel.Player_2_Deck;
        AllCards = ApplicationModel.AllCardsDeck;
    }
    void Start()
    {
        player_1_turn = true;
        player_2_turn = false;
        LeanTween.init(1000);
        hud = GameObject.Find("HUD").GetComponent<HUD>();
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Main Menu Music");
        board = GameObject.Find("Board").GetComponent<Board>();

        tweens = GameObject.Find("TweenManager").GetComponent<Tweens>();
        hud.end_turn_button.interactable = false;
        // Draw the cards depending on what screen you see
        ManaUpdate();
        DrawFirstHand(player_1_deck);
        
    }
    //Executed when the battle phases need to be executed
    public void StartRound()
    {
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Button Click");
        RevealCards();
        Ready.SetActive(false);
        if (Flippped) { Flip(); }
        hud.end_turn_button.interactable = false;
        player_1_hand.SetActive(false);

        //Calls the Pre battle phase and executes pre battle abilities

        tweens.PhaseTransition(PhaseWidget.transform.GetChild(1).gameObject, () =>
        {
            
            PreBattle();
            tweens.Battle.append(() =>
            {
                tweens.PhaseTransition(PhaseWidget.transform.GetChild(2).gameObject, () =>
                {
                    BattlePhase();
                });
            });
        });
        //Calls the End round function
    }
    //executed before the battle phase to process all the pre battle abilities
    private void PreBattle()
    {
        tweens.preBattle = LeanTween.sequence();
        //List of cards with pre battle abilities
        List<GameObject> card_effects = new List<GameObject>();
        //updates the current cards on the board
        board.UpdateBoard();
        //iterates over all positions on the board
        for (int i = 0; i <= 5; i++)
        {
            //adds cards to a list of all cards on the board
            card_slotted.Add(board.player_side[i]);
            card_slotted.Add(board.opponent_side[i]);
        }
        //iterates over all cards that are on the board
        foreach (GameObject card in card_slotted)
        {
           
            //checks if the card is a Pre battle type
            if (card != null && card.GetComponent<CardClass>().Ability_Type.Contains("Pre Battle"))
            {
                tweens.PreAbilityAnimation(card);
                //adds card to the list of cards to have their abilities executed
                card.GetComponent<CardClass>().ExecuteAblity();
                
            }
            if (card != null)
            {
                if (int.Parse(card.GetComponent<CardClass>().Health) <= 0)
                {
                    if (!(card.GetComponent<CardClass>().Opposite == null))
                    {
                        tweens.preBattle.append(() =>
                        {
                            try { card.GetComponent<CardClass>().Opposite.GetComponent<CardClass>().Opposite = null; }
                            catch { }
                        });
                    }
                    Debug.Log(card.GetComponent<CardClass>().Name);
                    if (card.GetComponent<CardClass>().ID == "009")
                    {
                        if (!card.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                        {
                            card.GetComponent<Effects>().EffectsList.Add("Dead", "0");
                            Transform pos = card.transform.parent;
                            int playersidenumber = card.GetComponent<CardClass>().player_number;
                            tweens.preBattle.append(() =>
                            {
                                tweens.DestroycardAnimation(card);
                            });
                            tweens.preBattle.append(LeanTween.play(card.transform.Find("Animation").GetComponent<RectTransform>(), tweens.FireSprite, 0.017f));
                            tweens.preBattle.append(() =>
                            {
                                Destroy(card);
                            });
                            tweens.preBattle.append(() =>
                            {
                                Spawncard("010", pos, playersidenumber);
                            });
                        }
                    }
                    else
                    {
                        if (!card.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                        {
                            card.GetComponent<Effects>().EffectsList.Add("Dead", "0");
                            tweens.preBattle.append(() =>
                            {
                                tweens.DestroycardAnimation(card);
                            });
                            tweens.preBattle.append(LeanTween.play(card.transform.Find("Animation").GetComponent<RectTransform>(), tweens.FireSprite, 0.017f));
                            tweens.preBattle.append(() =>
                            {
                                Destroy(card);
                            });
                        }
                    }
                }
            }
        }
        //clear the list of current cards on the board
        card_slotted.Clear();
        //call the CalculateState function with the cards to have the abilities executed
        CalculateState(card_effects);
    }
    //executed when the main battle phase need to be processed and executes any on-hit or battle-phase abilities
    private void BattlePhase()
    {
        int ID = 0;
        //updates the current cards on the board
        board.UpdateBoard();
        tweens.Battle = LeanTween.sequence();
        //List of cards with battle abilities
        List<GameObject> card_effects1 = new List<GameObject>();
        //List of cards with onhit abilities
        List<GameObject> SpeedClass = new List<GameObject>();
        //create new list for ordering cards
        card_ordered = new List<GameObject>();
        //loop though all positions on the board
        for (int i = 0; i < 6; i++)
        {
            //check if posistion is not null
            if (board.player_side[i] != null)
            {
                //if not null add the card to the order list
                card_ordered.Add(board.player_side[i]);
                if (board.opponent_side[i] == null) 
                { 
                    board.player_side[i].GetComponent<CardClass>().Opposite = null; 
                } 
                else 
                {
                    board.player_side[i].GetComponent<CardClass>().Opposite = board.opponent_side[i].gameObject;
                }
                Debug.Log(board.player_side[i].GetComponent<CardClass>().Name + " Was Added");
            }
            if (board.opponent_side[i] != null)
            {
                //if not null add the card to the order list
                card_ordered.Add(board.opponent_side[i]);
                if (board.player_side[i] == null) 
                {
                    board.opponent_side[i].GetComponent<CardClass>().Opposite = null;
                } 
                else 
                {
                    board.opponent_side[i].GetComponent<CardClass>().Opposite = board.player_side[i].gameObject;
                }
                Debug.Log(board.opponent_side[i].GetComponent<CardClass>().Name + " Was Added");
            }

        }
        //Arrange cards by order of their speed
        card_ordered = card_ordered.OrderByDescending(card => int.Parse(card.GetComponent<CardClass>().Speed)).ToList();
        foreach (GameObject card in card_ordered)
        {
            Debug.Log(card.GetComponent<CardClass>().Name);
        }
        Debug.Log("-------------");
        GameObject previous = null;
        void ActiveCard(List<GameObject> ClassList)
        {
            CalculateState(card_effects1);
            foreach (GameObject Cards in ClassList)
            {
                Debug.Log("Battle STATS");
                
                if (Cards.GetComponent<CardClass>().Ability_Type.Contains("Active"))
                {
                    Cards.GetComponent<CardClass>().ExecuteAblity();
                }
                if (Cards.GetComponent<CardClass>().Opposite != null)
                {
                    if (!(Cards.GetComponent<CardClass>().Type == "Field Spell"))
                    {
                        if (!(int.Parse(Cards.GetComponent<CardClass>().Damage) == 0))
                        {
                            tDescrs.Add(tweens.moveY(Cards, int.Parse(Cards.GetComponent<CardClass>().Opposite.GetComponent<CardClass>().Health) - int.Parse(Cards.GetComponent<CardClass>().Damage)));
                        }
                    }
                    Cards.GetComponent<CardClass>().Opposite.GetComponent<CardClass>().Health = (int.Parse(Cards.GetComponent<CardClass>().Opposite.GetComponent<CardClass>().Health) - int.Parse(Cards.GetComponent<CardClass>().Damage)).ToString();
                }
                else if (Cards.GetComponent<CardClass>().Opposite == null)
                {
                    
                    if (Cards.GetComponent<CardClass>().ID == "005")
                    {
                        Cards.GetComponent<CardClass>().ExecuteAblity();
                    }
                    if (Cards.GetComponent<CardClass>().player_number == 1)
                    {
                        if (!(Cards.GetComponent<CardClass>().Type == "Field Spell"))
                        {
                            if (!(int.Parse(Cards.GetComponent<CardClass>().Damage) == 0))
                            {
                                tDescrs.Add(tweens.moveY(Cards, current_health + int.Parse(Cards.GetComponent<CardClass>().Damage)));
                            }
                        }
                        current_health += Cards.GetComponent<CardClass>().intDamage;
                    }
                    else if (Cards.GetComponent<CardClass>().player_number == 2)
                    {
                        if (!(Cards.GetComponent<CardClass>().Type == "Field Spell"))
                        {
                            if (!(int.Parse(Cards.GetComponent<CardClass>().Damage) == 0))
                            {
                                tDescrs.Add(tweens.moveY(Cards, current_health - int.Parse(Cards.GetComponent<CardClass>().Damage)));
                            }
                        }
                        current_health -= Cards.GetComponent<CardClass>().intDamage;
                    }

                }
            }
            foreach (GameObject card in card_ordered)
            {
                if (card != null)
                {
                    if (int.Parse(card.GetComponent<CardClass>().Health) <= 0)
                    {
                            
                        if (!(card.GetComponent<CardClass>().Opposite == null))
                        {
                            tweens.Battle.append(() =>
                            {
                                try { card.GetComponent<CardClass>().Opposite.GetComponent<CardClass>().Opposite = null; }
                                catch { }
                            });
                        }
                        Debug.Log(card.GetComponent<CardClass>().Name);
                        if (card.GetComponent<CardClass>().ID == "009")
                        {
                            if (!card.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                            {
                                card.GetComponent<Effects>().EffectsList.Add("Dead", "0");
                                Transform pos = card.transform.parent;
                                int playersidenumber = card.GetComponent<CardClass>().player_number;
                                tweens.Battle.append(() =>
                                {
                                    tweens.DestroycardAnimation(card);
                                });
                                tweens.Battle.append(LeanTween.play(card.transform.Find("Animation").GetComponent<RectTransform>(), tweens.FireSprite, 0.017f));
                                tweens.Battle.append(() =>
                                {
                                    Destroy(card);
                                });
                                tweens.Battle.append(() =>
                                {
                                    Spawncard("010", pos, playersidenumber);
                                });
                            }   
                        }
                        else 
                        {
                            if (!card.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                            {
                                card.GetComponent<Effects>().EffectsList.Add("Dead", "0");
                                tweens.Battle.append(() =>
                                {
                                    tweens.DestroycardAnimation(card);
                                });
                                tweens.Battle.append(LeanTween.play(card.transform.Find("Animation").GetComponent<RectTransform>(), tweens.FireSprite, 0.017f));
                                tweens.Battle.append(() =>
                                {
                                    Destroy(card);
                                });
                            }
                        }    
                    }
                }
            }
        }
        //loop over all cards in in ordered cards list
        foreach (GameObject card in card_ordered)
        {
            
            
            if (!(previous == null))
            {
                if (!(card.GetComponent<CardClass>().Speed == previous.GetComponent<CardClass>().Speed))
                {
                    ActiveCard(SpeedClass);
                    SpeedClass.Clear();

                }
            }
            else if (previous == null)
            {
                if (card_ordered.Count == 1)
                {
                    
                    ActiveCard(SpeedClass);
                    SpeedClass.Clear();
                }
            }

            if (!(int.Parse(card.GetComponent<CardClass>().Health) <= 0))
            {
                SpeedClass.Add(card);
            }

            if (card_ordered.FindIndex(a => a.Equals(card)) == (card_ordered.Count - 1))
            {
                ActiveCard(SpeedClass);
                SpeedClass.Clear();
            }
            //Find opposing card from card about to attack
            previous = card;
        }

        tweens.Battle.append(() =>
        {
            //check if player 1 has meet the health win condition
            if (current_health >= max_health)
            {
                WinScreen(1);
            }
            //check if player 2 has meet the health win condition
            else if (current_health <= -max_health)
            {
                WinScreen(2);
            }
            tweens.PhaseTransition(PhaseWidget.transform.GetChild(3).gameObject, () => { });
        });
        tweens.Battle.append(2);
        tweens.Battle.append(() =>
        {
            PostBattle();
        });

    }
    //executed after the battle phase to process all the post battle abilities
    private void PostBattle()
    {
        
        tweens.postBattle = LeanTween.sequence();
        CheckTags();
        //List of cards with post battle abilities
        List<GameObject> card_effects = new List<GameObject>();
        //updates the current cards on the board
        board.UpdateBoard();
        //iterates over all positions on the board
        for (int i = 0; i <= 5; i++)
        {
            //adds cards to a list of all cards on the board
            card_slotted.Add(board.player_side[i]);
            card_slotted.Add(board.opponent_side[i]);
        }
        //iterates over all cards that are on the board
        foreach (GameObject card in card_slotted)
        {
            //checks if the card is a Post battle type
            if (card != null && card.GetComponent<CardClass>().Ability_Type.Contains("Post Battle"))
            {
                //adds card to the list of cards to have their abilities executed

                card.GetComponent<CardClass>().ExecuteAblity();
                tweens.PostAbilityAnimation(card);
            }
            //checks if card has an effect applied
            if (card != null)
            {
                if (int.Parse(card.GetComponent<CardClass>().Health) <= 0)
                {
                    if (!(card.GetComponent<CardClass>().Opposite == null))
                    {
                        tweens.postBattle.append(() =>
                        {
                            try { card.GetComponent<CardClass>().Opposite.GetComponent<CardClass>().Opposite = null; }
                            catch { }
                        });
                    }
                    Debug.Log(card.GetComponent<CardClass>().Name);
                    if (card.GetComponent<CardClass>().ID == "009")
                    {
                        if (!card.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                        {
                            card.GetComponent<Effects>().EffectsList.Add("Dead", "0");
                            Transform pos = card.transform.parent;
                            int playersidenumber = card.GetComponent<CardClass>().player_number;
                            tweens.postBattle.append(() =>
                            {
                                tweens.DestroycardAnimation(card);
                            });
                            tweens.postBattle.append(LeanTween.play(card.transform.Find("Animation").GetComponent<RectTransform>(), tweens.FireSprite, 0.017f));
                            tweens.postBattle.append(() =>
                            {
                                Destroy(card);
                            });
                            tweens.postBattle.append(() =>
                            {
                                Spawncard("010", pos, playersidenumber);
                            });
                        }
                    }
                    else
                    {
                        if (!card.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                        {
                            card.GetComponent<Effects>().EffectsList.Add("Dead", "0");
                            tweens.postBattle.append(() =>
                            {
                                tweens.DestroycardAnimation(card);
                            });
                            tweens.postBattle.append(LeanTween.play(card.transform.Find("Animation").GetComponent<RectTransform>(), tweens.FireSprite, 0.017f));
                            tweens.postBattle.append(() =>
                            {
                                Destroy(card);
                            });
                        }
                    }
                }
            }
        }
        //clear the list of current cards on the board

        //call the CalculateState function with the cards to have the abilities executed
        CalculateState(card_effects);
        foreach (GameObject card in card_slotted)
        {
            if (card != null)
            {
                if (int.Parse(card.GetComponent<CardClass>().Health) <= 0)
                {

                    if (!(card.GetComponent<CardClass>().Opposite == null))
                    {
                        tweens.postBattle.append(() =>
                        {
                            try { card.GetComponent<CardClass>().Opposite.GetComponent<CardClass>().Opposite = null; }
                            catch { }
                        });
                    }
                    Debug.Log(card.GetComponent<CardClass>().Name);
                    if (card.GetComponent<CardClass>().ID == "009")
                    {
                        if (!card.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                        {
                            Transform pos = card.transform.parent;
                            int playersidenumber = card.GetComponent<CardClass>().player_number;
                            tweens.postBattle.append(() =>
                            {
                                tweens.DestroycardAnimation(card);
                            });
                            tweens.postBattle.append(LeanTween.play(card.transform.Find("Animation").GetComponent<RectTransform>(), tweens.FireSprite, 0.017f));
                            tweens.postBattle.append(() =>
                            {
                                Destroy(card);
                            });
                            tweens.postBattle.append(() =>
                            {
                                Spawncard("010", pos, playersidenumber);
                            });
                        }
                    }
                    else
                    {
                        if (!card.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                        { 
                            tweens.postBattle.append(() =>
                            {
                                tweens.DestroycardAnimation(card);
                            });
                            tweens.postBattle.append(LeanTween.play(card.transform.Find("Animation").GetComponent<RectTransform>(), tweens.FireSprite, 0.017f));
                            tweens.postBattle.append(() =>
                            {
                                Destroy(card);
                            });
                        }
                    }

                }
            }
        }
        card_slotted.Clear();
        tweens.postBattle.append(() =>
        {
            hud.end_turn_button.interactable = true;
            player_1_hand.SetActive(true);
            TurnReadyCheck();
            if (Flippped) { Flip(); }

            EndRound();
        });
    }
    //Executed whenever a cards ability needs to be executed and at the end of every battle phase
    public void CalculateState(List<GameObject> ability_effects)
    {
        // Execute each ability from the ability list
        foreach (GameObject card in ability_effects.ToList())
        {
            //Call the ExecuteAblity function on each of the cards
            card.GetComponent<CardClass>().ExecuteAblity();
            //remove the card from the list of cards to have their abilities executed
            ability_effects.Remove(card);
        }
        //Call the destroycards function to remove any cards at 0 health
        //update the cards on the board
        board.UpdateBoard();
    }
    //called at the end of the battle phase to check if anycards need to be removed from the board
   
    public void Spawncard(string id, Transform Position, int playerside)
    {
        GameObject new_card = Instantiate(card, Vector3.zero, Quaternion.identity);
        CardClass card_details = ApplicationModel.AllCardsDeck.Find(x => x.ID == id);
        new_card.GetComponent<CardClass>().SetDetails(card_details);
        new_card.GetComponent<CardClass>().InDeckView = false;

        if (playerside == 1)
        {
            new_card.GetComponent<CardClass>().player_number = 1;
            new_card.GetComponent<CardClass>().hidden = true;
        }
        else if (playerside == 2)
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
        tweens.Summon(new_card);
        board.UpdateBoard();
    }

    public void EndRound()
    {
        bool temp1 = player_1_turn, temp2 = player_2_turn;
        //checks if player 1 should start first this round
        if (Player1RoundStart) 
        {
            if (Flippped) { Flip(); } else {  }
            //if true make it player 1's turn and set if the player should go first next round to false
            player_1_turn = true;
            player_2_turn = false;
            Player1RoundStart = false;
        }
        else if (!Player1RoundStart) 
        {
            if (Flippped) { } else { Flip(); }
            //if false make it player 2's turn and set if the player should go first next round to true
            player_1_turn = false;
            player_2_turn = true;
            Player1RoundStart = true;
        }
        if (!(temp1 == player_1_turn) || !(temp2 == player_2_turn))
        {
            TurnReadyCheck();
        }
        if (player_2_turn) { Flippped = true; }
        if (player_1_turn) { Flippped = false; }
        //Add a card to each players hand
        AddCard(player_1_hand, player_1_deck);
        AddCard(player_2_hand, player_2_deck);
        //Add 1 to the round counter
        round_count++;
        //Update all the cards on the board

        board.UpdateBoard();
        //Updates the mana with the start of a new round
        ManaUpdate();
    }
    public void CheckTags() 
    {
        foreach (GameObject Pos in board.CardPositions)
        { 
            if(!(Pos.transform.childCount == 0)) 
            {
                RemoveBuff("Speed" , Pos.transform.GetChild(0).gameObject);
                RemoveBuff("Health", Pos.transform.GetChild(0).gameObject);
                RemoveBuff("Damage", Pos.transform.GetChild(0).gameObject);
                RemoveBuff("Poison", Pos.transform.GetChild(0).gameObject);
                RemoveBuff("DeathTouch", Pos.transform.GetChild(0).gameObject);
            }
        }
        foreach (GameObject Pos in board.CardPositions2) 
        {
            if (!(Pos.transform.childCount == 0))
            {
                RemoveBuff("Speed" , Pos.transform.GetChild(0).gameObject);
                RemoveBuff("Health", Pos.transform.GetChild(0).gameObject);
                RemoveBuff("Damage", Pos.transform.GetChild(0).gameObject);
                RemoveBuff("Poison", Pos.transform.GetChild(0).gameObject);
                RemoveBuff("DeathTouch", Pos.transform.GetChild(0).gameObject);
            }
        }
    }
    private void RemoveBuff(string KEY, GameObject Card) 
    {
        string value = "";
        if (Card.GetComponent<Effects>().EffectsList.TryGetValue(KEY, out value))
        {
            switch (KEY) 
            {
                case "Speed":
                    Card.GetComponent<CardClass>().Speed = (int.Parse(Card.GetComponent<CardClass>().Speed) - int.Parse(value)).ToString();
                    tweens.postBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatDown"); });
                    tweens.SpeedDisplay(Card.transform.GetChild(11).gameObject, "postBattle");
                    Card.GetComponent<Effects>().EffectsList.Remove(KEY);
                    break;
                case "Health":
                    Card.GetComponent<CardClass>().Health = (int.Parse(Card.GetComponent<CardClass>().Health) - int.Parse(value)).ToString();
                    tweens.postBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatDown"); });
                    tweens.HealthDisplay(Card.transform.GetChild(7).gameObject, "postBattle", 1);
                    Card.GetComponent<Effects>().EffectsList.Remove(KEY);
                    break;
                case "Damage":
                    Card.GetComponent<CardClass>().Damage = (int.Parse(Card.GetComponent<CardClass>().Damage) - int.Parse(value)).ToString();
                    tweens.postBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatDown"); });
                    tweens.DamageDisplay(Card.transform.GetChild(9).gameObject, "postBattle");
                    Card.GetComponent<Effects>().EffectsList.Remove(KEY);
                    break;
                case "Poison":
                    Card.GetComponent<CardClass>().Health = (int.Parse(Card.GetComponent<CardClass>().Health) - int.Parse(value)).ToString();
                    tweens.postBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatDown"); });
                    tweens.HealthDisplay(Card.transform.GetChild(7).gameObject, "postBattle", 1);
                    Card.transform.Find("Effect").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                    Card.GetComponent<Effects>().EffectsList.Remove(KEY);
                    break;
                case "DeathTouch":
                    Card.GetComponent<CardClass>().Health = "0";
                    Card.transform.Find("Effect").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
                    Card.GetComponent<Effects>().EffectsList.Remove(KEY);
                    break;
            }
        }
    }

    private void RevealCards() 
    {
        List<GameObject> Pos = new List<GameObject>();
        for (int i = 0; i <= 5; i++)
        {
            //adds cards to a list of all cards on the board
            Pos.Add(board.CardPositions[i]);
            Pos.Add(board.CardPositions2[i]);
        }
        foreach (GameObject card in Pos)
        {
            if (!(card.transform.childCount == 0))
            {
                card.transform.GetChild(0).GetComponent<CardClass>().revealed = true;
                card.transform.GetChild(0).GetComponent<CardClass>().hidden = false;
            }
        }
            
    }
    public void DrawFirstHand(List<CardClass> deck)
    {
        // Turn on the special UI for the first hand to confirm first hand
        hud.recycle_UI.SetActive(true);
        player_1_hand.GetComponent<Button>().interactable = false;
        
        player_2_hand.GetComponent<Button>().interactable = false;
        ShuffleCards(deck);

        for (int i = 0; i < 5; i++)
        {
            GameObject new_card = Instantiate(card, Vector3.zero, Quaternion.identity);
            CardClass card_details = deck[0];
            
            new_card.GetComponent<CardClass>().SetDetails(deck[0]);
            
            player_1_deck.Remove(deck[0]);

            if (deck.Equals(player_1_deck))
            {
                new_card.GetComponent<CardClass>().player_number = 1;
                player_1_handcards.Add(new_card);
            }

            else
            {
                new_card.GetComponent<CardClass>().player_number = 2;
                player_2_handcards.Add(new_card);
            }

            new_card.transform.SetParent(GameObject.Find("View Cards").transform);
            new_card.transform.localScale = new Vector3(1.25f, 1.25f, 1);
            new_card.GetComponent<CanvasGroup>().blocksRaycasts = false;
            new_card.GetComponent<CardClass>().InDeckView = true;
        }
    }

    public void DrawFirstHand2(List<CardClass> deck)
    {
        // Turn on the special UI for the first hand to confirm first hand
        hud.recycle_UI2.SetActive(true);

        ShuffleCards(deck);
        
        for (int i = 0; i < 5; i++)
        {
            GameObject new_card = Instantiate(card, Vector3.zero, Quaternion.identity);
            CardClass card_details = deck[0];

            new_card.GetComponent<CardClass>().SetDetails(deck[0]);
            player_2_deck.Remove(deck[0]);

            if (deck.Equals(player_1_deck))
            {
                new_card.GetComponent<CardClass>().player_number = 1;
                player_1_handcards.Add(new_card);
            }

            else
            {
                new_card.GetComponent<CardClass>().player_number = 2;
                player_2_handcards.Add(new_card);
            }

            new_card.transform.SetParent(GameObject.Find("View Cards").transform);
            new_card.transform.localScale = new Vector3(1.25f, 1.25f, 1);
            new_card.GetComponent<CanvasGroup>().blocksRaycasts = false;
            new_card.GetComponent<CardClass>().InDeckView = true;
        }
    }

    public void AddCard(GameObject hand, List<CardClass> deck)
    {
        //check if players hand is less then or equal to 7 and deck is grater then 0
        if (deck.Count > 0 && hand.transform.childCount <= 7)
        {
            GameObject new_card = Instantiate(card, Vector3.zero, Quaternion.identity);

            new_card.GetComponent<CardClass>().SetDetails(deck[0]);
            new_card.GetComponent<CardClass>().InDeckView = false;
            new_card.GetComponent<CanvasGroup>().blocksRaycasts = false;
            deck.Remove(deck[0]);

            if (hand.Equals(player_2_hand))
            {
                new_card.GetComponent<CardClass>().hidden = false;
                new_card.GetComponent<CardClass>().player_number = 2;
                new_card.transform.rotation = Quaternion.Euler(0, 0, 0);
                player_2_handcards.Add(new_card);
            }

            else
            {
                new_card.GetComponent<CardClass>().hidden = false;
                new_card.GetComponent<CardClass>().player_number = 1;
                player_1_handcards.Add(new_card);
            }

            new_card.transform.SetParent(hand.transform);
            new_card.transform.localScale = Vector3.one;
        }
        //check if players deck is less the or equal to 0
        else if (deck.Count <= 0)
        {
            if (player_1_turn)
            {
                tweens.PlayerHealth(current_health - 1);
                current_health = current_health - 1;
            }

            else if (player_2_turn)
            {
                tweens.PlayerHealth(current_health + 1);
                current_health = current_health + 1;
            }
        }
    }

    public List<CardClass> ShuffleCards(List<CardClass> cards)
    {
        // Shuffle the cards when needed
        System.Random rng = new System.Random();
        int n = cards.Count;

        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            CardClass value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
        return cards;
    }

    public void TransferCards(GameObject old_parent, GameObject new_parent, float scale, bool interact)
    {
        // Used to move the cards between the bottom and middle of the screen
        StartCoroutine(WaitForTransfer(old_parent, new_parent, scale, interact));
    }

    private IEnumerator WaitForTransfer(GameObject old_parent, GameObject new_parent, float scale, bool interact)
    {
        // Move cards to new parent
        for (int i = old_parent.transform.childCount - 1; i >= 0; --i)
        {
            Transform card = old_parent.transform.GetChild(i);

            if (!card.GetComponent<CardClass>().is_moving)
            {
                card.SetParent(new_parent.transform, false);
                card.localScale = new Vector3(scale, scale, 1);
            }
        }

        // Add a slight delay in enabling raycast function on cards
        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < new_parent.transform.childCount; i++)
        {
            if (!new_parent.transform.GetChild(i).GetComponent<CardClass>().is_moving)
            {
                new_parent.transform.GetChild(i).GetComponent<Image>().raycastTarget = interact;
                new_parent.transform.GetChild(i).GetComponent<CardClass>().viewing = interact;
            }
        }
    }
    
    // Button function at the bottom of the screen to view cards
    public void ViewCards()
    {
        Debug.Log("ViewCards");
        hud.end_turn_button.interactable = false;
        if (player_1_turn)
        {
            if (!is_viewing)
            {
                GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("ViewCards");
                TransferCards(player_1_hand, GameObject.Find("View Cards"), 1.25f, true);
                GameObject.Find("Blur").GetComponent<Image>().enabled = true;
                for (int i = 0; i < 5; i++)
                {
                    GameObject.Find("View Cards").transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
                CloseCardView.SetActive(true);
                is_viewing = true;
            }

            else
            {

                TransferCards(GameObject.Find("View Cards"), player_1_hand, 1, false);
                GameObject.Find("Blur").GetComponent<Image>().enabled = false;
                hud.end_turn_button.interactable = true;
                CloseCardView.SetActive(false);
                is_viewing = false;
            }            
        }
        else if (player_2_turn) 
        {
            if (!is_viewing)
            {
                GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("ViewCards");
                TransferCards(player_2_hand, GameObject.Find("View Cards"), 1.25f, true);
                GameObject.Find("Blur").GetComponent<Image>().enabled = true;
                for (int i = 0; i < 5; i++)
                {
                    GameObject.Find("View Cards").transform.GetChild(i).GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
                CloseCardView.SetActive(true);
                is_viewing = true;
            }

            else
            {
                TransferCards(GameObject.Find("View Cards"), player_2_hand, 1, false);
                GameObject.Find("Blur").GetComponent<Image>().enabled = false;
                hud.end_turn_button.interactable = true;
                CloseCardView.SetActive(false);
                is_viewing = false;
            }
        }

    }
    private void ManaUpdate() 
    {
        //checks if the mana is at the max allowed and if not adds 1 to it and updates the players UI and mana pools
        if (mana_pool < max_mana) { mana_pool = mana_pool + 1; }
        player_1_mana = mana_pool;
        player_2_mana = mana_pool;
        hud.ManaUpdate(mana_pool, mana_pool);
    }
    //Checks if player can play any cards in their hand
    public bool CheckHand(List<GameObject> hand) 
    {
        player_1_hand.SetActive(true);
        player_2_hand.SetActive(true);
        int X = 0;
        //loops over everycard in their hand
        for (int i = 0; i < hand.Count(); i++)
        {
            //check if card is greyed out
            if (hand[i].transform.Find("Grayed Out").GetComponent<Image>().enabled == true)
            {
                X++;
            }
            //if greyed out cards equal the number of cards in hand return false
            if (X == i) 
            {
                player_1_hand.SetActive(false);
                player_2_hand.SetActive(false); 
                return false;
            }
        }
        //if greyed out cards in hand does not equal number of cards in hand return true
        return true;
    }
    //executed when the end turn button is pressed in the game
    public void EndTurnButton() 
    {
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Button Click");
        //set the button pressed bool to true
        EndButtonPressed = true;
        //call the end turn function
        EndTurn();
    }
    //executed whenever should be triggered E.g. End turn button is pressed or card is placed
    public void EndTurn()
    {
        //call the check hand function on both players to see if they have cards they can play
        player1canplay = CheckHand(player_1_handcards);
        player2canplay = CheckHand(player_2_handcards);
        
        //checks if the call was because the end turn button was pressed
        if (EndButtonPressed) 
        {
            // add one to the skip turn counter and set the button pressed bool to false
            skipped_turn++; EndButtonPressed = false;
        }
        //checks if end turn was called from a card being placed if so return the skip counter to 0
        if (has_placed_card) 
        {
            skipped_turn = 0;
        }

        bool temp1 = player_1_turn, temp2 = player_2_turn;
        //check if it's player 1 or 2s turn
        if (player_1_turn)
        {
            //if it's players 1s turn set it to player 2
            player_1_turn = false;
            player_2_turn = true;
            //check if player 2 has mana or can play a card
            if (player2canplay || player_2_mana == 0) 
            {
                //calls the skip turn function if player 2 has not cards they can play or no mana
                SkipTurn(2);
                player1canplay = CheckHand(player_1_handcards);
                if (player1canplay || player_1_mana == 0)
                {
                    //calls the skip turn function if player 1 has not cards they can play or no mana
                    SkipTurn(1);
                }
            }
            Flip();
        }
        else if (player_2_turn)
        {
            //if it's players 2s turn set it to player 1
            player_1_turn = true;
            player_2_turn = false;
            if (player1canplay || player_1_mana == 0)
            {
                //calls the skip turn function if player 1 has not cards they can play or no mana
                SkipTurn(1);
                player2canplay = CheckHand(player_2_handcards);
                if (player2canplay || player_2_mana == 0)
                {
                    //calls the skip turn function if player 2 has not cards they can play or no mana
                    SkipTurn(2);
                }
            }
            Flip();
        }
        
        if (!(temp1 == player_1_turn) || !(temp2 == player_2_turn)) 
        {
            
            TurnReadyCheck();
        }

        //returns the placed card bool to false 
        has_placed_card = false;
        if (player_2_turn) { Flippped = true; }
        if (player_1_turn) { Flippped = false; }
        //check if the skip turn counter is equal to or grater then 2 if so calls end round functions
        if (skipped_turn >= 2)
        {

            //return the skip turn counter to 0
            skipped_turn = 0;
            if (TurnScreen)
            {
                PhaseReadyCheack();
            }
            else 
            { 
                StartRound();
            }
            
        }
        
    }
    public void FlipHand() 
    {

        if (handflipped)
        {
            handflipped = false;
            player_1_hand.SetActive(true);
            player_2_hand.SetActive(false);
        }
        else if (!handflipped)
        {
            handflipped = true;
            player_1_hand.SetActive(false);
            player_2_hand.SetActive(true);
        } 
    }
    void FlipBoard()
    {           
        
        for (int i=0; i<6; i++) 
        {
            
            if (board.CardPositions[i].GetComponent<RectTransform>().childCount == 0 && board.CardPositions2[i].GetComponent<RectTransform>().childCount == 0)
            {

            } 
            else if (board.CardPositions2[i].GetComponent<RectTransform>().childCount == 0 && !(board.CardPositions[i].GetComponent<RectTransform>().childCount == 0)) 
            {
                Cover(i, false , true);
                board.CardPositions[i].GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().SetParent(board.CardPositions2[i].GetComponent<RectTransform>().transform);
                board.CardPositions2[i].GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
                board.CardPositions2[i].GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().transform.Rotate(00.0f, 0.0f, 180.0f, Space.Self);
            } 
            else if(board.CardPositions[i].GetComponent<RectTransform>().childCount == 0 && !(board.CardPositions2[i].GetComponent<RectTransform>().childCount == 0)) 
            {
                Cover(i, true, false);
                board.CardPositions2[i].GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().SetParent(board.CardPositions[i].GetComponent<RectTransform>().transform);
                board.CardPositions[i].GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
                board.CardPositions[i].GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().transform.Rotate(00.0f, 0.0f, 180.0f, Space.Self);
            }
            else if(board.CardPositions2[i].GetComponent<RectTransform>().childCount > 0 && board.CardPositions[i].GetComponent<RectTransform>().childCount > 0)
            {
                Cover(i, true, true);
                board.CardPositions2[i].GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().SetParent(board.CardPositions[i].GetComponent<RectTransform>().transform);
                board.CardPositions[i].GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().SetParent(board.CardPositions2[i].GetComponent<RectTransform>().transform);
                board.CardPositions[i].GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().transform.Rotate(00.0f, 0.0f, 180.0f, Space.Self);
                board.CardPositions[i].GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
                board.CardPositions2[i].GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().transform.Rotate(00.0f, 0.0f, 180.0f, Space.Self);
                board.CardPositions2[i].GetComponent<RectTransform>().GetChild(0).GetComponent<RectTransform>().transform.localPosition = Vector3.zero;
                
            }
                
            
        }
        
    }
    private void Cover(int i, bool Player_1, bool Player_2) 
    {
        if (Flippped)
        {
            if (Player_1 && Player_2) 
            {
                board.CardPositions[i].GetComponent<RectTransform>().GetChild(0).GetComponent<CardClass>().hidden = false;
                board.CardPositions2[i].GetComponent<RectTransform>().GetChild(0).GetComponent<CardClass>().hidden = true;
            }
            else if (!(Player_1) && Player_2)
            {
                board.CardPositions[i].GetComponent<RectTransform>().GetChild(0).GetComponent<CardClass>().hidden = false;
            }
            else if (Player_1 && !(Player_2))
            {
                board.CardPositions2[i].GetComponent<RectTransform>().GetChild(0).GetComponent<CardClass>().hidden = true;
            }
        }
        else if(!Flippped)
        {
            if (Player_1 && Player_2) 
            {
                board.CardPositions2[i].GetComponent<RectTransform>().GetChild(0).GetComponent<CardClass>().hidden = true;
                board.CardPositions[i].GetComponent<RectTransform>().GetChild(0).GetComponent<CardClass>().hidden = false;
            }
            else if (!(Player_1) && Player_2)
            {
                board.CardPositions[i].GetComponent<RectTransform>().GetChild(0).GetComponent<CardClass>().hidden = false;
            }
            else if (Player_1 && !(Player_2))
            {
                board.CardPositions2[i].GetComponent<RectTransform>().GetChild(0).GetComponent<CardClass>().hidden = true;
            }

        }
        if (!(board.CardPositions[i].transform.childCount == 0)) 
        {
            if (board.CardPositions[i].GetComponent<RectTransform>().GetChild(0).GetComponent<CardClass>().revealed == true)
            {
                board.CardPositions[i].GetComponent<RectTransform>().GetChild(0).GetComponent<CardClass>().hidden = false;
            }
        }
        if (!(board.CardPositions2[i].transform.childCount == 0)) 
        {
            if (board.CardPositions2[i].GetComponent<RectTransform>().GetChild(0).GetComponent<CardClass>().revealed == true)
            {
                board.CardPositions2[i].GetComponent<RectTransform>().GetChild(0).GetComponent<CardClass>().hidden = false;
            }
        }
    }
    public void FlipMana() 
    {
        if (manaflipped) 
        {
            manaflipped = !manaflipped;
            hud.ManaUpdate(player_1_mana, player_2_mana);
        }
        else if (!manaflipped) 
        {
            manaflipped = !manaflipped;
            hud.ManaUpdate(player_1_mana, player_2_mana);
        }
    }
    public void FlipHealth()
    {
        if (Healthflipped)
        {
            BoardBackgroundflipped.SetActive(false);
            GameObject.Find("TurnArrow").transform.eulerAngles = new Vector3(0, 0, 90);
            Healthflipped = !Healthflipped;
            foreach (Tuple<int, int, int, int> pos in tweens.Dialpos)
            {
                if (pos.Item1 == current_health)
                {
                    tweens.Dial.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3((float)pos.Item3, (float)pos.Item4, 0);
                    tweens.Dial.transform.GetChild(0).transform.eulerAngles = new Vector3(0, 0, (float)pos.Item2);
                }
            }
        }
        else if (!Healthflipped)
        {
            BoardBackgroundflipped.SetActive(true);
            GameObject.Find("TurnArrow").transform.eulerAngles = new Vector3(0, 0, -90);
            Healthflipped = !Healthflipped;
            foreach (Tuple<int, int, int, int> pos in tweens.Dialpos)
            {
                int health = current_health * -1;
                if (pos.Item1 == health)
                {
                    tweens.Dial.transform.GetChild(0).GetComponent<RectTransform>().localPosition = new Vector3((float)pos.Item3, (float)pos.Item4, 0);
                    tweens.Dial.transform.GetChild(0).transform.eulerAngles = new Vector3(0, 0, (float)pos.Item2);
                }
            }
        }
    }
    public void Flip() 
    {
        BoardIsFlipped = !BoardIsFlipped;
        FlipBoard();
        FlipHand();
        FlipMana();
        FlipHealth();
    }
    //exectued when a players turn needs to be skipped because they don't have cards to play or mana
    public void TurnReadyCheck() 
    {
        if (TurnScreen)
        {

            if (player_1_turn)
            {
                ReadyText.transform.GetChild(0).GetComponent<TMP_Text>().text = "Hand " + ApplicationModel.Player_1_name + " the device";
            }
            else
            {
                ReadyText.transform.GetChild(0).GetComponent<TMP_Text>().text = "Hand " + ApplicationModel.Player_2_name + " the device";
            }
            Ready.SetActive(true);
            ReadyText.SetActive(true);
        }
    }
    void PhaseReadyCheack()
    {

        ReadyText.transform.GetChild(0).GetComponent<TMP_Text>().text = "Start the Battle Phase";
        Startbutton.SetActive(true);
        Ready.SetActive(false);
        ReadyText.SetActive(true);
    }

    void WinScreen(int player) 
    {
        if (player == 1) 
        {
            ReadyText.transform.GetChild(0).GetComponent<TMP_Text>().text = ApplicationModel.Player_1_name + " Wins";
            ReadyText.SetActive(true);
            Quitbutton.SetActive(true);
        } 
        else if (player == 2) 
        {
            ReadyText.transform.GetChild(0).GetComponent<TMP_Text>().text = ApplicationModel.Player_2_name + " Wins";
            ReadyText.SetActive(true);
            Quitbutton.SetActive(true);
        }
    }

    public void Readymeth() 
    {
        GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Button Click");
        Ready.SetActive(false);
        ReadyText.SetActive(false);
    }
    public void SkipTurn(int i)
    {
        //check if it's player 1 or 2 
        if (i == 1) 
        {
            Flip();
            //print a log for skipped player and if it's players 1s turn set it to player 2
            Debug.Log("Player 1 had their turn skipped");
            player_1_turn = false;
            player_2_turn = true;
            // add one to the skip turn counter
            skipped_turn++;
        }
        else if (i == 2) 
        {
            Flip();
            //print a log for skipped player and if it's players 2s turn set it to player 1
            Debug.Log("Player 2 had their turn skipped");
            player_1_turn = true;
            player_2_turn = false;
            // add one to the skip turn counter
            skipped_turn++;
        }
    }
}