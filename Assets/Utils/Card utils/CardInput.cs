using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cards;
using Firebase;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.Assertions;

public class CardInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField _cardidField;
    [SerializeField] private TMP_InputField _nameField;
    [SerializeField] private TMP_InputField _descriptionField;
    [SerializeField] private TMP_InputField _healthField;
    [SerializeField] private TMP_InputField _speedField;
    [SerializeField] private TMP_InputField _damageField;
    [SerializeField] private TMP_InputField _mana_costField;
    [SerializeField] private TMP_InputField _typeField;
    [SerializeField] private TMP_InputField _ability_typeField;
    [SerializeField] private TMP_InputField _abilityID;
    [SerializeField] private TMP_InputField _abilityAMP;
    [SerializeField] private Button _submit;
    [SerializeField] private Button _get;
    private string _path = "Cards/";
    
    void Start()
    {
        //init firebase connection
        var firestore = FirebaseFirestore.DefaultInstance;

        //push data to firebase on submit button press
        _submit.onClick.AddListener(()=>
        {
            var cardData = new CardClass(_cardidField.text, _nameField.text, _descriptionField.text, _healthField.text, _damageField.text, _mana_costField.text, _speedField.text, _typeField.text, _ability_typeField.text ,"1" );//, _abilityID.text, _abilityAMP.text
            Debug.Log("Button pressed");
            firestore.Document(_path+_cardidField.text).SetAsync(cardData); 
            _cardidField.text = "";
            _nameField.text = "";
            _descriptionField.text = "";
            _healthField.text = "";
            _speedField.text = "";
            _damageField.text = "";
            _mana_costField.text = "";
            _typeField.text = "";
            _ability_typeField.text = "";
            //_abilityID.text = "";
            //_abilityAMP.text = "";
        });

        //collect card data when an ID is added to _cardidField
        _get.onClick.AddListener(()=>
        {
            Debug.Log(_path+_cardidField.text);
            firestore.Document(_path+_cardidField.text).GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                Assert.IsNull(task.Exception);
                
                Debug.Log("test1");
                
                var cardData = task.Result.ConvertTo<CardClass>();
                Debug.Log("test2");
                Debug.Log(cardData.ID.ToString()+" "+cardData.Name.ToString());
                _cardidField.text = cardData.ID;
                _nameField.text = cardData.Name;
                _descriptionField.text = cardData.Description;
                _healthField.text = cardData.Health;
                _speedField.text = cardData.Speed;
                _damageField.text = cardData.Damage;
                _mana_costField.text = cardData.Mana_Cost;
                _typeField.text = cardData.Type;
                _ability_typeField.text = cardData.Ability_Type;
                //_abilityID.text = cardData.AbilityID;
                //_abilityAMP.text = cardData.AbilityAmp;
                
            });
        });
    }
}
