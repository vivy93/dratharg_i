using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DatabaseControl; 
using UnityEngine.SceneManagement;

public class LoadMenu : MonoBehaviour {

    public GameObject loginParent;
    public GameObject registerParent;
    public GameObject loggedInParent;
    public GameObject loadingParent;

    public InputField Login_UsernameField;
    public InputField Login_PasswordField;
    public InputField Register_UsernameField;
    public InputField Register_PasswordField;
    public InputField Register_ConfirmPasswordField;
    public InputField LoggedIn_DataInputField;
    public InputField LoggedIn_DataOutputField;

	public Text Login_ErrorText;
    public Text Register_ErrorText;

    public Text LoggedIn_DisplayUsernameText;

    public static string playerUsername { get; protected set; } 
	private static string playerPassword = ""; 
	public static bool IsLoggedIn { get; protected set; }
	
	public static LoadMenu Instance;

    void Awake()
    {
        ResetAllUIElements();
		Instance = this;
    }

    void ResetAllUIElements ()
    {
        Login_UsernameField.text = "";
        Login_PasswordField.text = "";
        Register_UsernameField.text = "";
        Register_PasswordField.text = "";
        Register_ConfirmPasswordField.text = "";
        LoggedIn_DataInputField.text = "";
		LoggedIn_DataOutputField.text = "";
        Login_ErrorText.text = "";
        Register_ErrorText.text = "";
        LoggedIn_DisplayUsernameText.text = "";
    }
    
    IEnumerator LoginUser ()
    {
        IEnumerator e = DCF.Login(playerUsername, playerPassword); 
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; 

        if (response == "Success")
        {
            ResetAllUIElements();
			
			loadingParent.gameObject.SetActive(false);            
        } else
        {
            loadingParent.gameObject.SetActive(false);
            loginParent.gameObject.SetActive(true);
            if (response == "UserError")
            {
                Login_ErrorText.text = "Error: Username not Found";
            } else
            {
                if (response == "PassError")
                {
                    Login_ErrorText.text = "Error: Password Incorrect";
                } else
                {
                    Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
                }
            }
        }
    }
    IEnumerator RegisterUser()
    {
        IEnumerator e = DCF.RegisterUser(playerUsername, playerPassword, "Hello World"); 
        while (e.MoveNext())
        {
            yield return e.Current;
        }
        string response = e.Current as string; 
        if (response == "Success")
        {
            ResetAllUIElements();
            loadingParent.gameObject.SetActive(false);
            LoggedIn_DisplayUsernameText.text = "Logged In As: " + playerUsername;
        } else
        {
            loadingParent.gameObject.SetActive(false);
            registerParent.gameObject.SetActive(true);
            if (response == "UserError")
            {
                Register_ErrorText.text = "Error: Username Already Taken";
            } else
            {
                Login_ErrorText.text = "Error: Unknown Error. Please try again later.";
            }
        }
    }
    
    public void Login_LoginButtonPressed ()
    {
        playerUsername = Login_UsernameField.text;
        playerPassword = Login_PasswordField.text;

        if (playerUsername.Length > 3)
        {
            if (playerPassword.Length > 5)
            {
                loginParent.gameObject.SetActive(false);
                loadingParent.gameObject.SetActive(true);
                StartCoroutine(LoginUser());
				IsLoggedIn = true;
				SceneManager.LoadScene("Lobby");
            }
            else
            {
                Login_ErrorText.text = "Error: Password Incorrect";
            }
        } else
        {
            Login_ErrorText.text = "Error: Username Incorrect";
        }
    }
    public void Login_RegisterButtonPressed ()
    {
        ResetAllUIElements();
        loginParent.gameObject.SetActive(false);
        registerParent.gameObject.SetActive(true);
    }
    public void Register_RegisterButtonPressed ()
    {
        playerUsername = Register_UsernameField.text;
        playerPassword = Register_PasswordField.text;
        string confirmedPassword = Register_ConfirmPasswordField.text;

        if (playerUsername.Length > 3)
        {
            if (playerPassword.Length > 5)
            {
                if (playerPassword == confirmedPassword)
                {
                    registerParent.gameObject.SetActive(false);
                    loadingParent.gameObject.SetActive(true);
                    StartCoroutine(RegisterUser());
					IsLoggedIn = true;
					SceneManager.LoadScene("Lobby");
                }
                else
                {
                    Register_ErrorText.text = "Error: Password's don't Match";
                }
            }
            else
            {
                Register_ErrorText.text = "Error: Password too Short";
            }
        }
        else
        {
            Register_ErrorText.text = "Error: Username too Short";
        }
    }
    public void Register_BackButtonPressed ()
    {
        ResetAllUIElements();
        loginParent.gameObject.SetActive(true);
        registerParent.gameObject.SetActive(false);
    }
    
    public void LoggedIn_LogoutButtonPressed ()
    {
        ResetAllUIElements();
        playerUsername = "";
        playerPassword = "";
		IsLoggedIn = false;
		SceneManager.LoadScene("Login");        
    }
}
