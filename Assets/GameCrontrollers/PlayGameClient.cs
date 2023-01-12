using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using Firebase.Auth;



public class PlayGameClient : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().RequestServerAuthCode(false /* Don't force refresh */).Build();
        //PlayGamesPlatform.InitializeInstance(config);
        //PlayGamesPlatform.Activate();
        //Debug.LogFormat("PlayGamesClient: Play games config initialized");
    }

    public void Login()
    {
        /*
        // Initialize Firebase Auth
        Firebase.Auth.FirebaseAuth auth = Firebase.Auth.FirebaseAuth.DefaultInstance;

        // Sign In and Get a server auth code.
        UnityEngine.Social.localUser.Authenticate ((bool success) => {
            if (!success) {
                Debug.LogError ("SignInOnClick: Failed to Sign into Play Games Services."+success.ToString());
                return;
            }
            string authCode = PlayGamesPlatform.Instance.GetServerAuthCode ();
            if (string.IsNullOrEmpty (authCode)) {
                Debug.LogError ("SignInOnClick: Signed into Play Games Services but failed to get the server auth code.");
                return;
            }
            Debug.LogFormat ("SignInOnClick: Auth code is: {0}", authCode);

            // Use Server Auth Code to make a credential
            Firebase.Auth.Credential credential = Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);

            // Sign In to Firebase with the credential
            auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled) {
                Debug.LogError ("SignInOnClick was canceled.");
                return;
            }
            if (task.IsFaulted) {
                Debug.LogError ("SignInOnClick encountered an error: " + task.Exception);
                return;
            }
            Firebase.Auth.FirebaseUser newUser = task.Result;
            Debug.LogFormat ("SignInOnClick: User signed in successfully: {0} ({1})", newUser.DisplayName, newUser.UserId);
            });
        });
        */
    }
}
