using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Cards;

public class AddButtonClick : MonoBehaviour
{
    [HideInInspector] public int cardIndex;
    private GameObject CardSelect;
    [HideInInspector] public AddButtonViewController addButtonViewController;
    [SerializeField] private GameObject AddButtonPerf;
    public void OnAddButtonClick()
    {
        
        CardSelect = Instantiate(AddButtonPerf, this.transform.parent.transform.parent) as GameObject;
    }
}
