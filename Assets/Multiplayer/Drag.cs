using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cards;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameManager game_manager; 
    private HUD hud;
    private Board board;
    private Tweens tweens;
    Transform parent_origin;

    void Awake()
    {
        game_manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        hud = GameObject.Find("HUD").GetComponent<HUD>();
        board = GameObject.Find("Board").GetComponent<Board>();
        tweens = GameObject.Find("TweenManager").GetComponent<Tweens>();
        if (game_manager.player.ToString().Equals("Player_1"))
        {
            try
            { parent_origin = GameObject.Find("Player 1 Hand").GetComponent<Transform>(); }
            catch
            { }
        }

        else if (game_manager.player.ToString().Equals("Player_2"))
        {
            try
            { parent_origin = GameObject.Find("Player 2 Hand").GetComponent<Transform>(); }
            catch
            { }
        }
    }
    void Update() 
    {
        if (game_manager.player_1_turn)
        {
            try
            { parent_origin = GameObject.Find("Player 1 Hand").GetComponent<Transform>(); }
            catch
            { }
        }

        else if (game_manager.player_2_turn)
        {
            try
            { parent_origin = GameObject.Find("Player 2 Hand").GetComponent<Transform>(); }
            catch
            { }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        

            this.transform.localScale = Vector3.one;
            this.transform.SetParent(this.transform.parent.parent);
            GameObject.Find("HUD").GetComponent<HUD>().blur.enabled = false;
            GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Add Card");
            GetComponent<CanvasGroup>().blocksRaycasts = false;
            if (game_manager.is_viewing)
            {
                game_manager.ViewCards();
            }
        
    }

    public void OnDrag(PointerEventData eventData)
    {
       
            this.transform.position = new Vector3(eventData.position.x, eventData.position.y + 144, 0);
        
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        
            hud.end_turn_button.interactable = true;
            if (eventData.pointerCurrentRaycast.gameObject == null)
            {

                this.transform.SetParent(parent_origin);
                this.GetComponent<CardClass>().viewing = false;

            }
            else if (eventData.pointerCurrentRaycast.gameObject.CompareTag("Tile"))
            {
                if (((eventData.pointerCurrentRaycast.gameObject.GetComponent<FieldSpellMarker>().FieldSpellSlot == true && !(this.gameObject.GetComponent<CardClass>().Type == "Field Spell")) || (eventData.pointerCurrentRaycast.gameObject.GetComponent<FieldSpellMarker>().FieldSpellSlot == false && this.gameObject.GetComponent<CardClass>().Type == "Field Spell")) || (game_manager.player_1_turn && (int.Parse(this.gameObject.GetComponent<CardClass>().Mana_Cost) > game_manager.player_1_mana)) || ((!game_manager.player_1_turn) && (int.Parse(this.gameObject.GetComponent<CardClass>().Mana_Cost) > game_manager.player_2_mana)))
                {
                    this.transform.SetParent(parent_origin);
                    this.GetComponent<CardClass>().viewing = false;
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }
                if (gameObject.transform.Find("Grayed Out").GetComponent<Image>().enabled)
                {
                    this.transform.SetParent(parent_origin);
                    this.GetComponent<CardClass>().viewing = false;

                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }
                else
                {
                    int sacrifices = 0;
                    int circles = 0;
                    List<GameObject> CardsToConsume = new List<GameObject>();
                    switch (gameObject.GetComponent<CardClass>().ID)
                    {

                        case "021":
                            Debug.Log("Eldar one Consume");
                            sacrifices = 0;
                            circles = 0;
                            foreach (GameObject pos in board.CardPositions2)
                            {
                                if (!(pos.transform.childCount == 0))
                                {
                                    if (pos.transform.GetChild(0).gameObject.GetComponent<CardClass>().ID == "018" && !(sacrifices == 2))
                                    {

                                        CardsToConsume.Add(pos.transform.GetChild(0).gameObject);
                                        sacrifices++;
                                    }
                                    else if (pos.transform.GetChild(0).gameObject.GetComponent<CardClass>().ID == "019" && !(circles == 1))
                                    {
                                        CardsToConsume.Add(pos.transform.GetChild(0).gameObject);
                                        circles++;
                                    }
                                }
                                if (sacrifices == 2 && circles == 1)
                                {

                                    foreach (GameObject card in CardsToConsume)
                                    {
                                        LeanTween.scale(card.GetComponent<RectTransform>(), new Vector3(0f, 0f, 0f), 0.5f).setOnComplete(() => { Destroy(card); });
                                    }
                                    CardsToConsume.Clear();
                                }
                            }
                            break;
                        case "020":

                            sacrifices = 0;
                            circles = 0;
                            foreach (GameObject pos in board.CardPositions2)
                            {
                                if (!(pos.transform.childCount == 0))
                                {
                                    if (pos.transform.GetChild(0).gameObject.GetComponent<CardClass>().ID == "018" && !(sacrifices == 0))
                                    {

                                        CardsToConsume.Add(pos.transform.GetChild(0).gameObject);
                                        sacrifices++;
                                    }
                                    else if (pos.transform.GetChild(0).gameObject.GetComponent<CardClass>().ID == "019" && !(circles == 1))
                                    {
                                        CardsToConsume.Add(pos.transform.GetChild(0).gameObject);
                                        circles++;
                                    }
                                }
                                if (sacrifices == 0 && circles == 1)
                                {

                                    foreach (GameObject card in CardsToConsume)
                                    {
                                        LeanTween.scale(card.GetComponent<RectTransform>(), new Vector3(0f, 0f, 0f), 0.5f).setOnComplete(() => { Destroy(card); });
                                    }
                                    CardsToConsume.Clear();
                                }
                            }
                            break;
                    }


                }
                //Destroys placed card if it already exists on tile
                if (eventData.pointerCurrentRaycast.gameObject.transform.childCount > 0)
                {
                    if (game_manager.player_1_turn)
                    {
                        game_manager.player_1_handcards.Remove(eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).gameObject);
                    }
                    else if (game_manager.player_2_turn)
                    {
                        game_manager.player_2_handcards.Remove(eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).gameObject);
                    }
                    Destroy(eventData.pointerCurrentRaycast.gameObject.transform.GetChild(0).gameObject);
                    eventData.pointerCurrentRaycast.gameObject.transform.DetachChildren();

                }

                this.transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                this.transform.localPosition = Vector3.zero;
                GetComponent<Image>().raycastTarget = false;
                GetComponent<CardClass>().placed = true;

                game_manager.has_placed_card = true;

                if (game_manager.player_1_turn)
                {
                    game_manager.player_1_mana = game_manager.player_1_mana - int.Parse(this.gameObject.GetComponent<CardClass>().Mana_Cost);
                    game_manager.player_1_handcards.Remove(gameObject);
                }
                if (!game_manager.player_1_turn)
                {
                    game_manager.player_2_mana = game_manager.player_2_mana - int.Parse(this.gameObject.GetComponent<CardClass>().Mana_Cost);
                    game_manager.player_2_handcards.Remove(gameObject);
                }
                List<GameObject> card_slotted = new List<GameObject>();
                List<GameObject> card_effects = new List<GameObject>();
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
                    if (card != null && card.GetComponent<CardClass>().Ability_Type.Contains("Place"))
                    {
                        Debug.Log(card + " place");
                        //adds card to the list of cards to have their abilities executed
                        card.GetComponent<CardClass>().ExecuteAblity();
                    }
                }
                card_slotted.Clear();
                hud.ManaUpdate(game_manager.player_1_mana, game_manager.player_2_mana);
                GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Add Card");
                gameObject.GetComponent<CanvasGroup>().blocksRaycasts = false;
                game_manager.EndTurn();
                enabled = false;
            }

            else
            {
                this.transform.SetParent(parent_origin);
            }

        
    }
}