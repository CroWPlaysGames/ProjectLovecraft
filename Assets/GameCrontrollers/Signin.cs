using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;

public class Signin : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField password;
    private Firebase.Auth.FirebaseUser newUser;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSignInClick()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInrWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
        });
        
    }
    public void OnRegisterInClick()
    {
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text).ContinueWithOnMainThread(task => {
            if (task.IsCanceled) {
                Debug.LogError("SignInrWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            // Firebase user has been created.
            newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            var firestore = FirebaseFirestore.DefaultInstance;
            DocumentReference docRef = firestore.Collection("Users").Document(newUser.UserId.ToString());
            Dictionary<string, object> UserData = new Dictionary<string, object>
            {
                { "UUID", newUser.UserId.ToString()}
            };
            docRef.SetAsync(UserData).ContinueWithOnMainThread(task => 
            {
                Debug.Log("User "+newUser.UserId.ToString()+" Added to firestore");
            });
        });
        
    }
    
}
