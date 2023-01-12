using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Firestore;
using System;

namespace Cards
{
    [FirestoreData]
    public class CardClass : MonoBehaviour
    {
        //Card fields and get/set methods
        [FirestoreProperty]
        public string ID { get; set; }
        [FirestoreProperty]
        public string Name { get; set; }
        [FirestoreProperty]
        public string Description { get; set; }
        [FirestoreProperty]
        public string Health { get; set; }
        [FirestoreProperty]
        public string Speed { get; set; }
        [FirestoreProperty]
        public string Damage { get; set; }
        [FirestoreProperty]
        public string Mana_Cost { get; set; }
        [FirestoreProperty]
        public string Type { get; set; }
        [FirestoreProperty]
        public string Ability_Type { get; set; }
        [FirestoreProperty]
        public string Ability_Modifier { get; set; }

        public int intID;
        public int intHealth;
        public int intDamage;
        public int intSpeed;
        public int intMana;
        [HideInInspector] public bool hidden = false;
        [HideInInspector] public bool viewing;
        [HideInInspector] public bool is_moving;
        [HideInInspector] public bool placed = false;
        public Animator animator;
        public Abilities abilities;
        private Board board;
        private GameManager game_manager;
        public int player_number;
        public int TempHealth = 0;
        public bool revealed = false;
        private Button touch;
        public GameObject Opposite = null;
        public bool InDeckView = true;
        public CardClass(string id, string nameTemp, string descriptionTemp, string healthTemp, string damageTemp, string mana_costTemp, string speedTemp, string typeTemp, string ability_typeTemp, string ability_modifierTemp)
        {
            ID = id;
            Name = nameTemp;
            Description = descriptionTemp;
            Health = healthTemp;
            Speed = speedTemp;
            Damage = damageTemp;
            Mana_Cost = mana_costTemp;
            Type = typeTemp;
            Ability_Type = ability_typeTemp;
            Ability_Modifier = ability_modifierTemp;
            intID = int.Parse(id);
            intHealth = int.Parse(healthTemp);
            intDamage = int.Parse(damageTemp);
            intSpeed = int.Parse(speedTemp);
            intMana = int.Parse(mana_costTemp);
        }

        // Assign Card Details from Prefab
        public void SetDetails(CardClass card)
        {
            ID = card.ID;
            Name = card.Name;
            Description = card.Description;
            Health = card.Health;
            Speed = card.Speed;
            Damage = card.Damage;
            Mana_Cost = card.Mana_Cost;
            Type = card.Type;
            Ability_Type = card.Ability_Type;
            intID = int.Parse(card.ID);
            intHealth = int.Parse(card.Health);
            intDamage = int.Parse(card.Damage);
            intSpeed = int.Parse(card.Speed);
            intMana = int.Parse(card.Mana_Cost);
            DisplayData();
        }
        void Start()
        {
            touch = gameObject.GetComponent<Button>();
            touch.onClick.AddListener(OnClick);
            try
            {
                board = GameObject.Find("Board").GetComponent<Board>();
                game_manager = GameObject.Find("Game Manager").GetComponent<GameManager>();
            } catch { }
            try
            {
                abilities = gameObject.GetComponent<Abilities>();
            }

            catch (Exception e)
            {
                Debug.LogError("Failed Component Get");
                Debug.LogError(e.StackTrace);
                // Do nothing
            }

            DisplayData();
        }

        void Update()
        {
            if (hidden)
            {
                transform.Find("Cover").GetComponent<Image>().enabled = true;
            }
            else
            {
                transform.Find("Cover").GetComponent<Image>().enabled = false;
            }

            if (!InDeckView) 
            {
                //gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
            }

            if (player_number.Equals(1))
            {
                if (int.Parse(Mana_Cost) > game_manager.player_1_mana && !placed)
                {
                    transform.Find("Grayed Out").GetComponent<Image>().enabled = true;
                }

                else
                {
                    transform.Find("Grayed Out").GetComponent<Image>().enabled = false;
                }
            }
            else if (player_number.Equals(2))
            {
                if (int.Parse(Mana_Cost) > game_manager.player_2_mana && !placed)
                {
                    transform.Find("Grayed Out").GetComponent<Image>().enabled = true;
                }

                else
                {
                    transform.Find("Grayed Out").GetComponent<Image>().enabled = false;
                }
            }
            try
            {
                switch (ID)
                {
                    case "020":
                        gameObject.GetComponent<Effects>().SummonCheck(new Dictionary<string, int>() { { "019", 1 }, { "018", 0 }, { "016", 1 } });
                        break;

                    case "021":
                        gameObject.GetComponent<Effects>().SummonCheck(new Dictionary<string, int>() { { "019", 1 }, { "018", 2 }, { "016", 1 } });
                        break;
                }
            } catch { }


        }
        public void OnClick() 
        {
            if (!InDeckView) 
            {
                game_manager.ViewCards();
            }
            
        }
        public void SetSpeedColour(Color c) 
        {
            gameObject.transform.GetChild(11).GetComponent<TextMeshProUGUI>().color = c;
        }
        public void SetDamageColour(Color c)
        {
            gameObject.transform.GetChild(9).GetComponent<TextMeshProUGUI>().color = c;
        }
        public void SetHealthColour(Color c)
        {
            gameObject.transform.GetChild(7).GetComponent<TextMeshProUGUI>().color = c;
        }
        public void DisplayData()
        {
            //Display information onto card
            transform.Find("Artwork").GetComponent<Image>().sprite = Resources.Load<Sprite>("Card Artwork/" + ID);
            transform.Find("Name").GetComponent<TextMeshProUGUI>().text = Name;
            transform.Find("Mana Cost").GetComponent<TextMeshProUGUI>().text = Mana_Cost;
            transform.Find("Health").GetComponent<TextMeshProUGUI>().text = Health;
            transform.Find("Damage").GetComponent<TextMeshProUGUI>().text = Damage;
            transform.Find("Speed").GetComponent<TextMeshProUGUI>().text = Speed;
            transform.Find("Ability Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("Ability Icons/" + Ability_Type);

            switch (Ability_Type)
            {
                case ("None"):
                    transform.Find("Ability Phase").GetComponent<TextMeshProUGUI>().text = "Battle";
                    break;

                case ("Pre Battle"):
                    transform.Find("Ability Phase").GetComponent<TextMeshProUGUI>().text = "Pre-Battle";
                    break;

                case ("Active"):
                    transform.Find("Ability Phase").GetComponent<TextMeshProUGUI>().text = "Passive";
                    break;

                case ("Passive"):
                    transform.Find("Ability Phase").GetComponent<TextMeshProUGUI>().text = "Passive";
                    break;

                case ("Post Battle"):
                    transform.Find("Ability Phase").GetComponent<TextMeshProUGUI>().text = "Post-Battle";
                    break;

                case ("Place"):
                    transform.Find("Ability Phase").GetComponent<TextMeshProUGUI>().text = "Post-Battle";
                    break;
            }

            if (!transform.Find("Artwork").GetComponent<Image>().sprite)
            {
                transform.Find("Artwork").GetComponent<Image>().enabled = false;
            }

            else
            {
                transform.Find("Name").gameObject.SetActive(false);
                transform.Find("Artwork").GetComponent<Image>().enabled = true;
            }

            if (!transform.Find("Ability Icon").GetComponent<Image>().sprite)
            {
                transform.Find("Ability Icon").GetComponent<Image>().enabled = false;
            }

            try
            {
                foreach (Transform outline in transform.Find("Health Background"))
                {
                    outline.GetComponent<TextMeshProUGUI>().text = Health;
                }

                foreach (Transform outline in transform.Find("Damage Background"))
                {
                    outline.GetComponent<TextMeshProUGUI>().text = Damage;
                }

                foreach (Transform outline in transform.Find("Speed Background"))
                {
                    outline.GetComponent<TextMeshProUGUI>().text = Speed;
                }

                foreach (Transform outline in transform.Find("Mana Cost Background"))
                {
                    outline.GetComponent<TextMeshProUGUI>().text = Mana_Cost;
                }
            }

            catch
            {

            }            
        }

        // Figure out what cards to affect and what ability to action
        public void ExecuteAblity()
        {
            List<GameObject> affected_cards = new List<GameObject>();
            List<GameObject> locations = new List<GameObject>();
            
            string[] undead_mobs = new string[]{"008", "009", "011"};
            board.UpdateBoard();
            switch (ID)
            {
                case "000":
                    affected_cards.Add(gameObject);
                    abilities.GainSpeed(affected_cards, 1, "postBattle");
                    break;

                case "001":
                    if (player_number == 1) 
                    {
                        foreach (GameObject tile in board.CardPositions2)
                        {
                            if (tile.transform.childCount == 0)
                            {
                                if (tile.GetComponent<FieldSpellMarker>().FieldSpellSlot == true)
                                { }
                                else
                                {
                                    locations.Add(tile);
                                }
                            }
                        }
                    }

                    if (player_number == 2) 
                    {
                        foreach (GameObject tile in board.CardPositions)
                        {
                            if (tile.transform.childCount == 0)
                            {
                                if (tile.GetComponent<FieldSpellMarker>().FieldSpellSlot == true) 
                                { }
                                else 
                                {
                                    locations.Add(tile);
                                }
                                
                            }
                        }
                        
                    }
                    var random_number_1 = new System.Random();
                    int index_1 = random_number_1.Next(locations.Count);
                    if (locations.Count == 0) 
                    {
                        break;
                    }
                    GameObject random_location_1 = locations[index_1];
                    abilities.Spawn("000", random_location_1.transform);
                    Health = (int.Parse(Health) - 1).ToString();
                    locations.Clear();
                    break;

                case "002":
                    //No ability tied to card
                    break;

                case "003":
                    // No ability tied to card
                    break;

                case "004":
                    abilities.Splitshot(gameObject, "Battle");
                    break;

                case "005":
                    abilities.StealCard(gameObject);
                    break;

                case "006":
                    string[] goblins = new string[] { "000", "006", "002", "003", "004", "005", "007" };
                    if (player_number == 1) 
                    {
                        foreach (GameObject tile in board.CardPositions2)
                        {
                            foreach (string goblin in goblins)
                            {
                                if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(goblin))
                                {
                                    affected_cards.Add(tile.transform.GetChild(0).gameObject);
                                }
                            }
                        }
                    }
                    if (player_number == 2) 
                    {
                        foreach (GameObject tile in board.CardPositions)
                        {
                            foreach (string goblin in goblins)
                            {
                                if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(goblin))
                                {
                                    affected_cards.Add(tile.transform.GetChild(0).gameObject);
                                }
                            }
                        }
                    }
                    abilities.Heal(affected_cards, 1, "postBattle");
                    
                    break;

                case "007":
                    goblins = new string[] { "000", "006", "002", "003", "004", "005"};
                    if (player_number == 1) 
                    {
                        foreach (GameObject tile in board.CardPositions2)
                        {
                            foreach (string goblin in goblins)
                            {
                                if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(goblin))
                                {
                                    affected_cards.Add(tile.transform.GetChild(0).gameObject);
                                }
                            }
                        }
                    }
                    if(player_number == 2)
                    {
                        foreach (GameObject tile in board.CardPositions)
                        {
                            foreach (string goblin in goblins)
                            {
                                if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(goblin))
                                {
                                    affected_cards.Add(tile.transform.GetChild(0).gameObject);
                                }
                            }
                        }
                    }
                    abilities.BuffSpeed(affected_cards, 2);
                    break;

                case "008":
                    affected_cards.Add(gameObject);
                    abilities.Heal(affected_cards, 1, "postBatle");
                    break;

                case "009":
                    break;

                case "010":
                    affected_cards.Add(this.gameObject);
                    abilities.DeathTouch(affected_cards);
                    break;

                case "011":
                    for (int i = 0; i < 6; i++)
                    {
                        if (player_number == 1 && !(int.Parse(Health) <= 0)) 
                        {
                            if(!(board.CardPositions2[i].transform.childCount == 0)) 
                            {
                                if (board.CardPositions2[i].transform.GetChild(0) == gameObject.transform && !(board.CardPositions[i].transform.childCount == 0))
                                {
                                    affected_cards.Add(board.CardPositions[i].transform.GetChild(0).gameObject);
                                }
                            }
                            
                        }
                        else if (player_number == 2 && !(int.Parse(Health) <= 0)) 
                        {
                            if (!(board.CardPositions[i].transform.childCount == 0)) 
                            {
                                if (board.CardPositions[i].transform.GetChild(0) == gameObject.transform && !(board.CardPositions2[i].transform.childCount == 0))
                                {
                                    affected_cards.Add(board.CardPositions2[i].transform.GetChild(0).gameObject);
                                }
                            }
                            
                        }
                        
                    }
                    abilities.Poison(affected_cards, 3);
                    break;

                case "012":
                    if (player_number == 1)
                    {
                        foreach (GameObject tile in board.CardPositions2)
                        {
                            if (tile.transform.childCount == 0)
                            {
                                if (tile.GetComponent<FieldSpellMarker>().FieldSpellSlot == true)
                                { }
                                else
                                {
                                    locations.Add(tile);
                                }
                            }
                        }
                    }

                    if (player_number == 2)
                    {
                        foreach (GameObject tile in board.CardPositions)
                        {
                            if (tile.transform.childCount == 0)
                            {
                                if (tile.GetComponent<FieldSpellMarker>().FieldSpellSlot == true)
                                { }
                                else
                                {
                                    locations.Add(tile);
                                }

                            }
                        }

                    }
                    var random_number_2 = new System.Random();
                    var random_number_3 = new System.Random();
                    int index_2 = random_number_2.Next(locations.Count);
                    int index_3 = random_number_3.Next(3);
                    if (locations.Count == 0){ break;}
                    GameObject random_location_2 = locations[index_2];
                    abilities.Spawn(undead_mobs[index_3], random_location_2.transform);
                    Health = (int.Parse(Health) - 1).ToString();
                    locations.Clear();
                    break;

                case "013":
                    if (player_number == 1)
                    {
                        foreach (GameObject tile in board.CardPositions2)
                        {
                            foreach (string mod in undead_mobs)
                            {
                                if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(mod))
                                {
                                    affected_cards.Add(tile.transform.GetChild(0).gameObject);
                                }
                            }
                        }
                    }
                    if (player_number == 2)
                    {
                        foreach (GameObject tile in board.CardPositions)
                        {
                            foreach (string mod in undead_mobs)
                            {
                                if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals(mod))
                                {
                                    affected_cards.Add(tile.transform.GetChild(0).gameObject);
                                }
                            }
                        }
                    }
                    abilities.BuffSpeed(affected_cards, 1);
                    Health = (int.Parse(Health) - 1).ToString();
                    break;

                case "014":
                    for (int i = 0; i < 6; i++)
                    {
                        if (player_number == 1)
                        {
                            if (!(board.CardPositions2[i].transform.childCount == 0))
                            {
                                if (board.CardPositions2[i].transform.GetChild(0) == gameObject.transform && !(board.CardPositions[i].transform.childCount == 0))
                                {
                                    affected_cards.Add(board.CardPositions[i].transform.GetChild(0).gameObject);
                                }
                            }

                        }
                        else if (player_number == 2)
                        {
                            if (!(board.CardPositions[i].transform.childCount == 0))
                            {
                                if (board.CardPositions[i].transform.GetChild(0) == gameObject.transform && !(board.CardPositions2[i].transform.childCount == 0))
                                {
                                    affected_cards.Add(board.CardPositions2[i].transform.GetChild(0).gameObject);
                                }
                            }

                        }

                    }
                    abilities.DeathTouch(affected_cards);
                    break;

                case "015":
                    break;

                case "016":
                    // No ability tied to card
                    break;

                case "017":
                    affected_cards.Add(gameObject);
                    if (player_number == 1)
                    {
                        foreach (GameObject tile in board.CardPositions2)
                        {
                            if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals("016"))
                            {
                                abilities.BuffSpeed(affected_cards, 1);
                                abilities.BuffHealth(affected_cards, 1);
                                abilities.BuffDamage(affected_cards, 1);
                            }
                        }
                    }
                    if (player_number == 2)
                    {
                        foreach (GameObject tile in board.CardPositions)
                        {
                            if (!(tile.transform.childCount == 0) && tile.transform.GetChild(0).GetComponent<CardClass>().ID.Equals("016"))
                            {
                                abilities.BuffSpeed(affected_cards, 1);
                                abilities.BuffHealth(affected_cards, 1);
                                abilities.BuffDamage(affected_cards, 1);
                            }
                        }
                    }
                    
                    break;

                case "018":
                    // No ability tied to card
                    break;

                case "019":
                    if (player_number == 1) 
                    {
                        abilities.SpawnToHand("018", game_manager.player_1_hand.transform);
                    }
                    else if (player_number == 2)
                    {
                        abilities.SpawnToHand("018", game_manager.player_2_hand.transform);
                    }
                    break;

                case "020":
                    
                    break;

                case "021":
                    break;

                case "022":
                    if (player_number ==1) 
                    {
                        if (!gameObject.GetComponent<Effects>().EffectsList.ContainsKey("Actived"))
                        {
                            gameObject.GetComponent<Effects>().EffectsList.Add("Actived", "0");
                            abilities.AdjustCardDeck("016", game_manager.player_1_deck);
                        }
                        
                    }
                    else if (player_number == 2) 
                    {
                        if (!gameObject.GetComponent<Effects>().EffectsList.ContainsKey("Actived"))
                        {
                            gameObject.GetComponent<Effects>().EffectsList.Add("Actived", "0");
                            abilities.AdjustCardDeck("016", game_manager.player_2_deck);
                        }
                    }
                    
                    break;
            }
        }
    }
}