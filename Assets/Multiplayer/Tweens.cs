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

public class Tweens : MonoBehaviour
{
    [Header("PreBattlePhase Settings")]
    public float PreBattlePhase_Distance;
    public float PreBattlePhase_Speed;
    public float PreBattlePhase_Delay;
    public Action PreBattlePhase_OnComplete;
    [Header("BattlePhase Settings")]
    public float BattlePhase_Distance;
    public float BattlePhase_Speed;
    public float BattlePhase_Delay;
    public Action BattlePhase_OnComplete;
    [Header("PostBattlePhase Settings")]
    public float PostBattlePhase_Distance;
    public float PostBattlePhase_Speed;
    public float PostBattlePhase_Delay;
    public Action PostBattlePhase_OnComplete;
    [Header("SlashAttack Settings")]
    public float SlashAttack_Distance;
    public float SlashAttack_Speed;
    public float SlashAttack_Delay;
    public Action SlashAttack_OnComplete;
    [Header("MagicAttack Settings")]
    public float MagicAttack_Distance;
    public float MagicAttack_Speed;
    public float MagicAttack_Delay;
    public Action MagicAttack_OnComplete;
    [Header("Spawn Settings")]
    public float Spawn_Distance;
    public float Spawn_Speed;
    public float Spawn_Delay;
    public Action Spawn_OnComplete;
    [Header("Summon Settings")]
    public float Summon_Distance;
    public float Summon_Speed;
    public float Summon_Delay;
    public Action Summon_OnComplete;
    [Header("Destroy Settings")]
    public float Destroy_Distance;
    public float Destroy_Speed;
    public float Destroy_Delay;
    public Action Destroy_OnComplete;
    [Header("moveY Settings")]
    public float moveY_Distance;
    public float moveY_Speed;
    public float moveY_Delay;
    public Action moveY_OnComplete;
    TweenManager tweenManager;
    public LTSeq preBattle;
    public LTSeq Battle;
    public LTSeq postBattle; 
    public Sprite[] BloodAttack;
    public Sprite[] MagicSlash;
    public Sprite[] Shine;
    public Sprite[] PoisonSprite;
    public Sprite[] DeathTouchSprite;
    public Sprite[] FireSprite;
    public List<CardClass> AllCards;
    private Board board;
    private GameManager game_manager;
    public GameObject Dial;
    public List<Tuple<int, int, int, int>> Dialpos = new List<Tuple<int, int, int, int>>();
    private void Awake()
    {
        AllCards = ApplicationModel.AllCardsDeck;
        Dial = GameObject.Find("HealthDial");
        game_manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        tweenManager = gameObject.GetComponent<TweenManager>();
        preBattle = LeanTween.sequence();
        Battle = LeanTween.sequence();
        postBattle = LeanTween.sequence();
        board = GameObject.Find("Board").GetComponent<Board>();
        Dialpos = new List<Tuple<int, int, int, int>> 
        { 
            new Tuple<int, int, int, int>(10, 90, -665, 8), 
            new Tuple<int, int, int, int>(9, 81, -657, 7),
            new Tuple<int, int, int, int>(8, 72, -657, 5),
            new Tuple<int, int, int, int>(7, 63, -658, 3),
            new Tuple<int, int, int, int>(6, 54, -658, 1),
            new Tuple<int, int, int, int>(5, 45, -659, -1),
            new Tuple<int, int, int, int>(4, 36, -660, -2),
            new Tuple<int, int, int, int>(3, 27, -661, -1),
            new Tuple<int, int, int, int>(2, 18, -663, 0),
            new Tuple<int, int, int, int>(1, 9, -664, 0),
            new Tuple<int, int, int, int>(0, 0, -665, 0),
            new Tuple<int, int, int, int>(-1, -9, -665, 0),
            new Tuple<int, int, int, int>(-2, -18, -664, 0),
            new Tuple<int, int, int, int>(-3, -27, -664, 0),
            new Tuple<int, int, int, int>(-4, -36, -663, 0),
            new Tuple<int, int, int, int>(-5, -45, -662, -1),
            new Tuple<int, int, int, int>(-6, -54, -662, -2),
            new Tuple<int, int, int, int>(-7, -63, -662, -3),
            new Tuple<int, int, int, int>(-8, -72, -662, -4),
            new Tuple<int, int, int, int>(-9, -81, -662, -5),
            new Tuple<int, int, int, int>(-10, -90, -662, -5)
        };
    }
    //PreBattlePhase Tween Method with overload
    public void PhaseTransition(GameObject objects, Action oncomplete)
    {
        LTSeq temp;
        temp = LeanTween.sequence();
        objects.transform.parent.gameObject.SetActive(true);
        objects.SetActive(true);
        temp.append(2);
        temp.append(() =>
        {
            objects.transform.parent.gameObject.SetActive(false);
            objects.SetActive(false);
            oncomplete();
        });
    }
    public void BloodSlash(GameObject objects, int Damage)
    {
        Battle.append(() => 
        {
            objects.transform.Find("Animation").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            HealthDisplay(objects.transform.GetChild(7).gameObject, "", Damage);
        });
        Battle.append(() => 
        {
            LeanTween.play(objects.transform.Find("Animation").GetComponent<RectTransform>(), BloodAttack, 0.1f).setOnComplete(() => { objects.transform.Find("Animation").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0); }); 
        });
    }

    public void Poison(GameObject objects)
    {
        LTSeq temp;
        temp = LeanTween.sequence();
        temp.append(() => { objects.transform.Find("Animation").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100); });
        temp.append(LeanTween.play(objects.transform.Find("Animation").GetComponent<RectTransform>(), PoisonSprite, 0.1f));
        temp.append(() => { try { objects.transform.Find("Animation").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0); } catch { } });
    }
    public void DeathTouch(GameObject objects)
    {
        LTSeq temp;
        temp = LeanTween.sequence();
        temp.append(() => { objects.transform.Find("Animation").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100); });
        temp.append(LeanTween.play(objects.transform.Find("Animation").GetComponent<RectTransform>(), DeathTouchSprite, 0.1f));
        temp.append(() => { try { objects.transform.Find("Animation").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0); } catch { } });
    }
    public void DeckShine(GameObject objects)
    {
        LTSeq temp;
        temp = LeanTween.sequence();
        temp.append(() => { objects.GetComponent<RectTransform>().sizeDelta = new Vector2(140, 65); });
        temp.append(LeanTween.play(objects.GetComponent<RectTransform>(), Shine, 0.01f));
        temp.append(() => { try { objects.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0); } catch { } });
    }
    //SlashAttack Tween Method with overload
    public void PostAbilityAnimation(GameObject objects)
    {
        if (!objects.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
        {
            string[] goblins = new string[] { "000", "001", "002", "003", "004", "005", "007" };
            switch (objects.GetComponent<CardClass>().ID)
            {
                case "001":
                    postBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatDown"); });
                    HealthDisplay(objects.transform.GetChild(7).gameObject, "postBattle", 1);
                    break;
                case "006":
                    if (objects.GetComponent<CardClass>().player_number == 1)
                    {
                        foreach (GameObject tile in board.CardPositions2)
                        {
                            foreach (string goblin in goblins)
                            {
                                if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(goblin))
                                {
                                    if (!tile.transform.GetChild(0).gameObject.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                                    {
                                        postBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                                        HealthDisplay(tile.transform.GetChild(0).transform.GetChild(7).gameObject, "postBattle", 1);
                                    }

                                }
                            }
                        }
                    }
                    if (objects.GetComponent<CardClass>().player_number == 2) 
                    {
                        foreach (GameObject tile in board.CardPositions)
                        {
                            foreach (string goblin in goblins)
                            {
                                if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(goblin))
                                {
                                    if (!tile.transform.GetChild(0).gameObject.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                                    {
                                        postBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                                        HealthDisplay(tile.transform.GetChild(0).transform.GetChild(7).gameObject, "postBattle", 1);
                                    }
                                }
                            }
                        }
                    }
                    break;

                case "008":
                    postBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                    HealthDisplay(objects.transform.GetChild(7).gameObject, "postBattle", 1);
                    break;

                case "012":
                    postBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatDown"); });
                    HealthDisplay(objects.transform.GetChild(7).gameObject, "postBattle", 1);
                    break;
                case "000":
                    postBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                    SpeedDisplay(objects.transform.GetChild(11).gameObject, "postBattle");
                    break;
            }
        }
        
    }
    public void PreAbilityAnimation(GameObject objects)
    {
        if (!objects.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
        {
            string[] goblins = new string[] { "000", "001", "002", "003", "004", "005", "007" };
            string[] undead_mobs = new string[] { "008", "009", "011" };
            switch (objects.GetComponent<CardClass>().ID)
            {
                case "007":
                    if (objects.GetComponent<CardClass>().player_number == 1)
                    {
                        foreach (GameObject tile in board.CardPositions2)
                        {
                            foreach (string goblin in goblins)
                            {
                                if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(goblin))
                                {
                                    if (!(tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(goblins[6])))
                                    {
                                        if (!tile.transform.GetChild(0).GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                                        {
                                            preBattle.append(()=> { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                                            SpeedDisplay(tile.transform.GetChild(0).transform.GetChild(11).gameObject, "preBattle");
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (objects.GetComponent<CardClass>().player_number == 2)
                    {
                        foreach (GameObject tile in board.CardPositions)
                        {
                            foreach (string goblin in goblins)
                            {
                                if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(goblin))
                                {
                                    if (!(tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(goblins[6])))
                                    {
                                        if (!tile.transform.GetChild(0).GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                                        {
                                            preBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                                            SpeedDisplay(tile.transform.GetChild(0).transform.GetChild(11).gameObject, "preBattle");
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case "017":
                    if (objects.GetComponent<CardClass>().player_number == 1)
                    {
                        foreach (GameObject tile in board.CardPositions2)
                        {
                            if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals("016"))
                            {
                                preBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                                HealthDisplay(objects.transform.GetChild(7).gameObject, "preBattle", 1);
                                preBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                                SpeedDisplay(objects.transform.GetChild(11).gameObject, "preBattle");
                                preBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                                DamageDisplay(objects.transform.GetChild(9).gameObject, "preBattle");
                            }
                        }
                    }
                    if (objects.GetComponent<CardClass>().player_number == 2)
                    {
                        foreach (GameObject tile in board.CardPositions)
                        {
                            if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals("016"))
                            {
                                preBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                                HealthDisplay(objects.transform.GetChild(7).gameObject, "preBattle", 1);
                                preBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                                SpeedDisplay(objects.transform.GetChild(11).gameObject, "preBattle");
                                preBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                                DamageDisplay(objects.transform.GetChild(9).gameObject, "preBattle");
                            }
                        }
                    }
                    break;
                case "013":
                    if (objects.GetComponent<CardClass>().player_number == 1)
                    {
                        foreach (GameObject tile in board.CardPositions2)
                        {
                            foreach (string mod in undead_mobs)
                            {
                                if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(mod))
                                {
                                    if (!tile.transform.GetChild(0).GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                                    {
                                        preBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                                        SpeedDisplay(tile.transform.GetChild(0).transform.GetChild(11).gameObject, "preBattle");
                                    }
                                }
                            }
                        }
                    }
                    if (objects.GetComponent<CardClass>().player_number == 2)
                    {
                        foreach (GameObject tile in board.CardPositions)
                        {
                            foreach (string mod in undead_mobs)
                            {
                                if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(mod))
                                {
                                    if (!tile.transform.GetChild(0).GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
                                    {
                                        preBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatUp"); });
                                        SpeedDisplay(tile.transform.GetChild(0).transform.GetChild(11).gameObject, "preBattle");
                                    }
                                }
                            }
                        }
                    }
                    preBattle.append(() => { GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("StatDown"); });
                    HealthDisplay(objects.transform.GetChild(7).gameObject, "preBattle", 1);
                    break;
            }
        }
    }
    public void AbilityAnimation(GameObject objects)
    {
        if (!objects.GetComponent<Effects>().EffectsList.ContainsKey("Dead"))
        {

        }
        string[] goblins = new string[] { "000", "001", "002", "003", "004", "005", "007" };
        switch (objects.GetComponent<CardClass>().ID) 
        {
            case "005":
                if (objects.GetComponent<CardClass>().player_number == 1)
                {
                    DeckShine(game_manager.AnimationTop);
                }
                if (objects.GetComponent<CardClass>().player_number == 2)
                {
                    DeckShine(game_manager.AnimationBottom);
                }
                break;
            case "011":
                for (int i = 0; i < 6; i++)
                {
                    if (objects.GetComponent<CardClass>().player_number == 1)
                    {
                        if (!(board.CardPositions2[i].transform.childCount == 0))
                        {
                            if (board.CardPositions2[i].transform.GetChild(0) == objects.transform && !(board.CardPositions[i].transform.childCount == 0))
                            {
                                board.CardPositions[i].transform.GetChild(0).gameObject.transform.Find("Effect").GetComponent<RectTransform>().sizeDelta = new Vector2(16, 25);
                                board.CardPositions[i].transform.GetChild(0).gameObject.transform.Find("Effect").GetComponent<Image>().sprite = PoisonSprite[0];
                            }
                        }

                    }
                    else if (objects.GetComponent<CardClass>().player_number == 2)
                    {
                        if (!(board.CardPositions[i].transform.childCount == 0))
                        {
                            if (board.CardPositions[i].transform.GetChild(0) == objects.transform && !(board.CardPositions2[i].transform.childCount == 0))
                            {
                                board.CardPositions2[i].transform.GetChild(0).gameObject.transform.Find("Effect").GetComponent<RectTransform>().sizeDelta = new Vector2(16, 25);
                                board.CardPositions2[i].transform.GetChild(0).gameObject.transform.Find("Effect").GetComponent<Image>().sprite = PoisonSprite[0];
                            }
                        }

                    }

                }
                break;
            case "014":
                for (int i = 0; i < 6; i++)
                {
                    if (objects.GetComponent<CardClass>().player_number == 1)
                    {
                        if (!(board.CardPositions2[i].transform.childCount == 0))
                        {
                            if (board.CardPositions2[i].transform.GetChild(0) == objects.transform && !(board.CardPositions[i].transform.childCount == 0))
                            {
                                board.CardPositions[i].transform.GetChild(0).gameObject.transform.Find("Effect").GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
                                board.CardPositions[i].transform.GetChild(0).gameObject.transform.Find("Effect").GetComponent<Image>().sprite = DeathTouchSprite[0];
                            }
                        }

                    }
                    else if (objects.GetComponent<CardClass>().player_number == 2)
                    {
                        if (!(board.CardPositions[i].transform.childCount == 0))
                        {
                            if (board.CardPositions[i].transform.GetChild(0) == objects.transform && !(board.CardPositions2[i].transform.childCount == 0))
                            {
                                board.CardPositions2[i].transform.GetChild(0).gameObject.transform.Find("Effect").GetComponent<RectTransform>().sizeDelta = new Vector2(25, 25);
                                board.CardPositions2[i].transform.GetChild(0).gameObject.transform.Find("Effect").GetComponent<Image>().sprite = DeathTouchSprite[0];
                            }
                        }

                    }

                }
                break;
        }
    }
    public void ArchersAnimation(GameObject objects, int Damage)
    {
        Debug.Log("Archers");
        for (int i = 0; i < 5; i++)
        {
            if (objects.GetComponent<CardClass>().player_number == 1)
            {
                Debug.Log(i);
                if (!(board.CardPositions2[i].transform.childCount == 0))
                {
                    if (objects.transform.parent.gameObject == board.CardPositions2[i])
                    {
                        List<GameObject> Cards = new List<GameObject>(2);
                        if (i == 4)
                        {
                            if (!(board.CardPositions[i - 1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions[i - 1].transform.GetChild(0).gameObject);
                            }
                        }
                        else if (i == 0)
                        {
                            if (!(board.CardPositions[i + 1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions[i + 1].transform.GetChild(0).gameObject);
                            }
                        }
                        else
                        {
                            if (!(board.CardPositions[i - 1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions[i - 1].transform.GetChild(0).gameObject);
                            }
                            if (!(board.CardPositions[i + 1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions[i + 1].transform.GetChild(0).gameObject);
                            }

                        }
                        foreach (GameObject Card in Cards)
                        {

                            BloodSlash(Card, int.Parse(Card.GetComponent<CardClass>().Health));
                        }

                    }
                }

            }

            else if (objects.GetComponent<CardClass>().player_number == 2)
            {
                if (!(board.CardPositions[i].transform.childCount == 0))
                {
                    if (objects.transform.parent.gameObject == board.CardPositions[i])
                    {
                        List<GameObject> Cards = new List<GameObject>(2);
                        if (i == 4)
                        {
                            if (!(board.CardPositions2[i - 1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions2[i - 1].transform.GetChild(0).gameObject);
                            }
                        }
                        else if (i == 0)
                        {
                            if (!(board.CardPositions2[i + 1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions2[i + 1].transform.GetChild(0).gameObject);
                            }
                        }
                        else
                        {
                            if (!(board.CardPositions2[i - 1].transform.childCount == 0))
                            {
                                Cards.Add(board.CardPositions2[i - 1].transform.GetChild(0).gameObject);
                            }
                            if (board.CardPositions2[i + 1].transform.childCount == 1)
                            {
                                Cards.Add(board.CardPositions2[i + 1].transform.GetChild(0).gameObject);
                            }

                        }
                        foreach (GameObject Card in Cards)
                        {

                            BloodSlash(Card, int.Parse(Card.GetComponent<CardClass>().Health));
                        }
                    }
                }

            }
        }
    }
    //MagicAttack Tween Method with overload
    public void MagicAttack(GameObject objects) 
    {
        Battle.append(() => { objects.transform.Find("Animation").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100); });
        Battle.append(() => {
            LeanTween.play(objects.transform.Find("Animation").GetComponent<RectTransform>(), MagicSlash, 0.1f).setOnComplete(() => 
            {
                objects.transform.Find("Animation").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            });
        });
    }
    public void Summon(GameObject objects)
    { 
        LeanTween.scale(objects.GetComponent<RectTransform>(), Vector3.one, Destroy_Speed).setOnComplete(() => { });
    }
    public void PlayerHealth(int Damage) 
    {
        LTSeq temp;
        temp = LeanTween.sequence();
        foreach (Tuple<int, int, int, int> pos in Dialpos)
        {
            if (pos.Item1 == Damage)
            {
                GameObject.Find("Health_Text").GetComponent<TextMeshProUGUI>().text = (Damage).ToString();
                LeanTween.moveX(Dial.transform.GetChild(0).GetComponent<RectTransform>(), (float)pos.Item3, 1);
                LeanTween.rotateZ(Dial.transform.GetChild(0).gameObject, (float)pos.Item2, 1);
                LeanTween.moveY(Dial.transform.GetChild(0).gameObject.GetComponent<RectTransform>(), (float)pos.Item4, 1);
            }
        }
        if (Damage > 10)
        {
            LeanTween.moveX(Dial.transform.GetChild(0).GetComponent<RectTransform>(), -665, 1);
            LeanTween.rotateZ(Dial.transform.GetChild(0).gameObject, 90, 1);
            LeanTween.moveY(Dial.transform.GetChild(0).gameObject.GetComponent<RectTransform>(), 8, 1);
        }
        if (Damage < -10)
        {
            LeanTween.moveX(Dial.transform.GetChild(0).GetComponent<RectTransform>(), -662, 1);
            LeanTween.rotateZ(Dial.transform.GetChild(0).gameObject, -90, 1);
            LeanTween.moveY(Dial.transform.GetChild(0).gameObject.GetComponent<RectTransform>(), -5, 1);
        }
    }
    //Destroytween Tween Method with overload
    //Colour Tween Method with overload
    //Scale Tween Method with overload

    //moveY Tween Method with overload
    public int moveY(GameObject objects, int Damage)
    {
        System.Random rnd = new System.Random();
        Battle.append(LeanTween.scale(objects.GetComponent<RectTransform>(), new Vector3(1.25f, 1.25f, 1.25f), moveY_Speed));
        Battle.append(LeanTween.moveY(objects.GetComponent<RectTransform>(), moveY_Distance, moveY_Speed));
        if (objects.GetComponent<CardClass>().ID == "004") 
        {
            ArchersAnimation(objects, Damage);
        }
        Battle.append(() =>
        {
            
            
            
            AbilityAnimation(objects);
        });
        if (objects.GetComponent<CardClass>().Opposite != null)
        {
            if (!objects.GetComponent<CardClass>().Opposite.GetComponent<Effects>().EffectsList.ContainsKey("Dead")) 
            {
                
                Battle.append(() => { objects.GetComponent<CardClass>().Opposite.transform.Find("Animation").GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100); });
                Battle.append(()=> {
                    switch (rnd.Next(3))
                    {
                        case 0:
                            GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Hit1");
                            break;
                        case 1:
                            GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Hit2");
                            break;
                        case 2:
                            GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Hit3");
                            break;
                        case 3:
                            GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Hit4");
                            break;
                    }
                });
                Battle.append(LeanTween.play(objects.GetComponent<CardClass>().Opposite.transform.Find("Animation").GetComponent<RectTransform>(), MagicSlash, 0.1f));
                Battle.append(() => { objects.GetComponent<CardClass>().Opposite.transform.Find("Animation").GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0); });
                HealthDisplay(objects.GetComponent<CardClass>().Opposite.transform.GetChild(7).gameObject, "Battle", Damage);
            }
        }
        else 
        {
            Battle.append(() =>
            {
                GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("HealthHit1");
                PlayerHealth(Damage);
            });
            MagicAttack(GameObject.Find("PlayerHit"));
        }
        Battle.append(LeanTween.moveY(objects.GetComponent<RectTransform>(), 0f, moveY_Speed));
        return Battle.append(LeanTween.scale(objects.GetComponent<RectTransform>(), new Vector3(1, 1, 1), moveY_Speed)
            .setOnComplete(() =>
            {
            })).id;
    }
    //WaitForTweenComplete Method

    public void DestroycardAnimation(GameObject card)
    {
        try 
        { 
            if (LeanTween.isTweening(card)) 
            {
                LeanTween.cancel(card, true);
            }
            GameObject.Find("Audio Manager").GetComponent<AudioManager>().Play("Burn");
            card.transform.Find("Animation").GetComponent<RectTransform>().sizeDelta = new Vector2(135, 100);
            card.transform.Find("Animation").GetComponent<RectTransform>().localPosition = new Vector2(0, -145);
            LeanTween.moveY(card.transform.GetChild(0).GetComponent<RectTransform>(), -190, 1);
            LeanTween.moveY(card.transform.GetChild(1).GetComponent<RectTransform>(), -190, 1);
            LeanTween.moveY(card.transform.GetChild(2).GetComponent<RectTransform>(), -190, 1);
            LeanTween.moveY(card.transform.GetChild(3).GetComponent<RectTransform>(), -190, 1);
            LeanTween.moveY(card.transform.GetChild(4).GetComponent<RectTransform>(), -199, 1);
            LeanTween.moveY(card.transform.GetChild(5).GetComponent<RectTransform>(), -103, 1);
            LeanTween.moveY(card.transform.GetChild(6).GetComponent<RectTransform>(), -224, 1);
            LeanTween.moveY(card.transform.GetChild(7).GetComponent<RectTransform>(), -224, 1);
            LeanTween.moveY(card.transform.GetChild(8).GetComponent<RectTransform>(), -224, 1);
            LeanTween.moveY(card.transform.GetChild(9).GetComponent<RectTransform>(), -224, 1);
            LeanTween.moveY(card.transform.GetChild(10).GetComponent<RectTransform>(), -224, 1);
            LeanTween.moveY(card.transform.GetChild(11).GetComponent<RectTransform>(), -224, 1);
            LeanTween.moveY(card.transform.GetChild(12).GetComponent<RectTransform>(), -246.5f, 1);
            LeanTween.moveY(card.transform.GetChild(13).GetComponent<RectTransform>(), -275, 1);
            LeanTween.moveY(card.transform.GetChild(14).GetComponent<RectTransform>(), -190, 1);
            LeanTween.moveY(card.transform.GetChild(15).GetComponent<RectTransform>(), -224, 1);
            LeanTween.moveY(card.transform.GetChild(17).GetComponent<RectTransform>(), -224, 1);
            Debug.Log("Dead");
            LeanTween.moveY(card.GetComponent<RectTransform>(), 192, 1);
        }
        catch { }
    }
    public void WaitForTweenComplete(GameObject objects, Action oncomplete) 
    {
        if (LeanTween.isTweening(objects))
        {
            LTDescr[] tween = LeanTween.descriptions(objects);
            if (!(tween.Length == 0)) 
            {
                tween[tween.Length - 1]
                .setOnComplete(() =>
                {
                    oncomplete();
                });
            }
            
        }
    }
    public void HealthDisplay(GameObject Cards, string seqence, int Damage) 
    {
        GameObject Parent = Cards.transform.parent.gameObject;
        int Defult = int.Parse(AllCards.Find(x => x.ID == Parent.GetComponent<CardClass>().ID).Health);
        
        Vector3 Scale = new Vector3(2f, 2f, 2f);
        Color C = new Color();

        
        switch (seqence)
        {
            case "Battle":
                Battle.append(LeanTween.scale(Cards, Scale, 0.25f));
                Battle.append(() =>
                {
                    int Health = Damage;
                    if (Health < Defult)
                    {
                        C = Color.red;
                    }
                    else if (Health > Defult)
                    {
                        C = Color.green;
                    }
                    else if (Health == Defult)
                    {
                        C = Color.white;
                    }
                    Cards.GetComponent<TextMeshProUGUI>().text = Health.ToString();
                    foreach (Transform outline in Parent.transform.Find("Health Background"))
                    {
                        outline.GetComponent<TextMeshProUGUI>().text = Health.ToString();
                    }
                    LeanTween.value(Parent, Parent.GetComponent<CardClass>().SetHealthColour, Cards.GetComponent<TextMeshProUGUI>().color, C, 0.01f);
                });
                Battle.append(LeanTween.scale(Cards, Vector3.one, 0.25f));
                break;
            case "preBattle":
                preBattle.append(LeanTween.scale(Cards, Scale, 0.25f));
                preBattle.append(() =>
                {
                    int Health = int.Parse(Parent.GetComponent<CardClass>().Health);
                    if (Health < Defult)
                    {
                        C = Color.red;
                    }
                    else if (Health > Defult)
                    {
                        C = Color.green;
                    }
                    else if (Health == Defult)
                    {
                        C = Color.white;
                    }
                    Cards.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Health;
                    foreach (Transform outline in Parent.transform.Find("Health Background"))
                    {
                        outline.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Health;
                    }
                    LeanTween.value(Parent, Parent.GetComponent<CardClass>().SetHealthColour, Cards.GetComponent<TextMeshProUGUI>().color, C, 0.01f);
                });
                preBattle.append(LeanTween.scale(Cards, Vector3.one, 0.25f));
                break;
            case "postBattle":
                postBattle.append(LeanTween.scale(Cards, Scale, 0.25f));
                postBattle.append(() =>
                {
                    int Health = int.Parse(Parent.GetComponent<CardClass>().Health);
                    if (Health < Defult)
                    {
                        C = Color.red;
                    }
                    else if (Health > Defult)
                    {
                        C = Color.green;
                    }
                    else if (Health == Defult)
                    {
                        C = Color.white;
                    }
                    Cards.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Health;
                    foreach (Transform outline in Parent.transform.Find("Health Background"))
                    {
                        outline.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Health;
                    }
                    LeanTween.value(Parent, Parent.GetComponent<CardClass>().SetHealthColour, Cards.GetComponent<TextMeshProUGUI>().color, C, 0.01f);
                });
                postBattle.append(LeanTween.scale(Cards, Vector3.one, 0.25f));
                break;
            default:
                LTSeq temp;
                temp = LeanTween.sequence();
                temp.append(LeanTween.scale(Cards, Scale, 0.25f));
                temp.append(() =>
                {
                    int Health = Damage;
                    if (Health < Defult)
                    {
                        C = Color.red;
                    }
                    else if (Health > Defult)
                    {
                        C = Color.green;
                    }
                    else if (Health == Defult)
                    {
                        C = Color.white;
                    }
                    Cards.GetComponent<TextMeshProUGUI>().text = Damage.ToString();
                    foreach (Transform outline in Parent.transform.Find("Health Background"))
                    {
                        outline.GetComponent<TextMeshProUGUI>().text = Damage.ToString();
                    }
                    LeanTween.value(Parent, Parent.GetComponent<CardClass>().SetHealthColour, Cards.GetComponent<TextMeshProUGUI>().color, C, 0.01f);
                });
                temp.append(LeanTween.scale(Cards, Vector3.one, 0.25f));
                break;
        }

    }
    public void SpeedDisplay(GameObject Cards, string seqence)
    {
        GameObject Parent = Cards.transform.parent.gameObject;
        int Defult = int.Parse(AllCards.Find(x => x.ID == Parent.GetComponent<CardClass>().ID).Speed);
        
        Vector3 Scale = new Vector3(2f, 2f, 2f);
        Color C = new Color();

        
        switch (seqence)
        {
            case "Battle":
                Battle.append(LeanTween.scale(Cards, Scale, 0.25f));
                Battle.append(() =>
                {
                    int Speed = int.Parse(Parent.GetComponent<CardClass>().Speed);
                    if (Speed < Defult)
                    {
                        C = Color.red;
                    }
                    else if (Speed > Defult)
                    {
                        C = Color.green;
                    }
                    else if (Speed == Defult)
                    {
                        C = Color.white;
                    }
                    Cards.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Speed;
                    foreach (Transform outline in Parent.transform.Find("Speed Background"))
                    {
                        outline.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Speed;
                    }
                    LeanTween.value(Parent, Parent.GetComponent<CardClass>().SetSpeedColour, Cards.GetComponent<TextMeshProUGUI>().color, C, 0.01f);
                });
                Battle.append(LeanTween.scale(Cards, Vector3.one, 0.25f));
                break;
            case "preBattle":
                
                preBattle.append(LeanTween.scale(Cards, Scale, 0.25f));
                preBattle.append(() =>
                {
                    int Speed = int.Parse(Parent.GetComponent<CardClass>().Speed);
                    if (Speed < Defult)
                    {
                        C = Color.red;
                    }
                    else if (Speed > Defult)
                    {
                        C = Color.green;
                    }
                    else if (Speed == Defult)
                    {
                        C = Color.white;
                    }
                    Cards.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Speed;
                    foreach (Transform outline in Parent.transform.Find("Speed Background"))
                    {
                        outline.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Speed;
                    }
                    LeanTween.value(Parent, Parent.GetComponent<CardClass>().SetSpeedColour, Cards.GetComponent<TextMeshProUGUI>().color, C, 0.01f);
                });
                preBattle.append(LeanTween.scale(Cards, Vector3.one, 0.25f));
                break;
            case "postBattle":
                postBattle.append(LeanTween.scale(Cards, Scale, 0.25f));
                postBattle.append(() =>
                {
                    int Speed = int.Parse(Parent.GetComponent<CardClass>().Speed);
                    if (Speed < Defult)
                    {
                        C = Color.red;
                    }
                    else if (Speed > Defult)
                    {
                        C = Color.green;
                    }
                    else if (Speed == Defult)
                    {
                        C = Color.white;
                    }
                    Cards.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Speed;
                    foreach (Transform outline in Parent.transform.Find("Speed Background"))
                    {
                        outline.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Speed;
                    }
                    LeanTween.value(Parent, Parent.GetComponent<CardClass>().SetSpeedColour, Cards.GetComponent<TextMeshProUGUI>().color, C, 0.01f);
                });
                postBattle.append(LeanTween.scale(Cards, Vector3.one, 0.25f));
                break;
        }

    }

    public void DamageDisplay(GameObject Cards, string seqence)
    {
        GameObject Parent = Cards.transform.parent.gameObject;
        int Defult = int.Parse(AllCards.Find(x => x.ID == Parent.GetComponent<CardClass>().ID).Damage);

        Vector3 Scale = new Vector3(2f, 2f, 2f);
        Color C = new Color();


        switch (seqence)
        {
            case "Battle":
                Battle.append(LeanTween.scale(Cards, Scale, 0.25f));
                Battle.append(() =>
                {
                    int Damage = int.Parse(Parent.GetComponent<CardClass>().Damage);
                    if (Damage < Defult)
                    {
                        C = Color.red;
                    }
                    else if (Damage > Defult)
                    {
                        C = Color.green;
                    }
                    else if (Damage == Defult)
                    {
                        C = Color.white;
                    }
                    Cards.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Damage;
                    foreach (Transform outline in Parent.transform.Find("Damage Background"))
                    {
                        outline.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Damage;
                    }
                    LeanTween.value(Parent, Parent.GetComponent<CardClass>().SetDamageColour, Cards.GetComponent<TextMeshProUGUI>().color, C, 0.01f);
                });
                Battle.append(LeanTween.scale(Cards, Vector3.one, 0.25f));
                break;
            case "preBattle":

                preBattle.append(LeanTween.scale(Cards, Scale, 0.25f));
                preBattle.append(() =>
                {
                    int Damage = int.Parse(Parent.GetComponent<CardClass>().Damage);
                    if (Damage < Defult)
                    {
                        C = Color.red;
                    }
                    else if (Damage > Defult)
                    {
                        C = Color.green;
                    }
                    else if (Damage == Defult)
                    {
                        C = Color.white;
                    }
                    Cards.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Damage;
                    foreach (Transform outline in Parent.transform.Find("Damage Background"))
                    {
                        outline.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Damage;
                    }
                    LeanTween.value(Parent, Parent.GetComponent<CardClass>().SetDamageColour, Cards.GetComponent<TextMeshProUGUI>().color, C, 0.01f);
                });
                preBattle.append(LeanTween.scale(Cards, Vector3.one, 0.25f));
                break;
            case "postBattle":
                postBattle.append(LeanTween.scale(Cards, Scale, 0.25f));
                postBattle.append(() =>
                {
                    int Damage = int.Parse(Parent.GetComponent<CardClass>().Damage);
                    if (Damage < Defult)
                    {
                        C = Color.red;
                    }
                    else if (Damage > Defult)
                    {
                        C = Color.green;
                    }
                    else if (Damage == Defult)
                    {
                        C = Color.white;
                    }
                    Cards.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Damage;
                    foreach (Transform outline in Parent.transform.Find("Damage Background"))
                    {
                        outline.GetComponent<TextMeshProUGUI>().text = Parent.GetComponent<CardClass>().Damage;
                    }
                    LeanTween.value(Parent, Parent.GetComponent<CardClass>().SetDamageColour, Cards.GetComponent<TextMeshProUGUI>().color, C, 0.01f);
                });
                postBattle.append(LeanTween.scale(Cards, Vector3.one, 0.25f));
                break;
        }

    }
}
