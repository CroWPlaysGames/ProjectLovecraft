using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Cards;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.Assertions;
public class AddButtonViewController : MonoBehaviour
{
    private GameObject Decks;
    [SerializeField] private GameObject DeckPerf;
    [SerializeField] private TMP_Text ID;
    [SerializeField] private Transform DeckParent;

    public void Start() 
    {
        LoadDecks();
    }

    public void LoadDecks()
    {
        var auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        var firestore = FirebaseFirestore.DefaultInstance;
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;
        firestore.Collection("Users").Document(user.UserId).GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            //var Result = task.Result;
            //foreach (var Deck in Result.Documents)
            {
                //var DeckData = Deck.ConvertTo<CardCollection>();
            }
            ID.text = "Result.UUID";
            
        });
    }

    public void NewDeck()
    {
       
    }

}
