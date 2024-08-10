using System.Collections;
using System.Collections.Generic;
using System.IO;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public bool userNameStored = false;
    public string newUserName;
    public string bestScoreUserName;
    public int bestScore = 0;
    

    [Header("Data to Save")] 
    [SerializeField] private TMP_InputField _usernameInput;
    [SerializeField] private TMP_Text  _userNameWarningText;
    [SerializeField] private TMP_Text  _leaderboardText;

    
    public static GameManager Instance;
    private MainManager _mainManager;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAllSavedData();

    }

    

    // Start is called before the first frame update
    void Start()
    {
        _mainManager = FindObjectOfType<MainManager>();
        //Subscribe to the event to hadnle username
        if (_usernameInput != null)
        {
            _usernameInput.onEndEdit.AddListener(HandleUserNameInput);
        }

        _leaderboardText.text = $"Best Score : {bestScoreUserName} : {bestScore}";
    }

    
    private void HandleUserNameInput(string arg0)
    {
        //Debug.Log("EnterButton Pressed");

        /*if (_usernameInput.text != "")
        {
            userName = _usernameInput.text;
            Debug.Log("UsernameEntered");
            userNameStored = true;

        }
        else
        {
            Debug.Log("EnterUserName");
        }*/
        
        if (!string.IsNullOrEmpty(_usernameInput.text))
        {
            newUserName = _usernameInput.text;
            Debug.Log("UsernameEntered");
            userNameStored = true;
            SaveUserName();
        }
        else
        {
            Debug.Log("EnterUserName");
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadMainScene()
    {
        SceneManager.LoadScene(1);
        
        //Check if UserName data is available
        /*if (!userNameStored)
        {
            _userNameWarningText.gameObject.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(1);
        }*/
        
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif

    }

    //Declare savedata class
    [System.Serializable]
    class SessionData
    {
        //declare savedata date type
        public string newUserName;
        
    }
    public void SaveUserName()
    {
        SessionData userName = new SessionData();
        userName.newUserName = newUserName;

        string json = JsonUtility.ToJson(userName);
        
        
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }


    public void LoadUserName()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SessionData userName = JsonUtility.FromJson<SessionData>(json);

            newUserName = userName.newUserName;
        }
    }

    //Declare serializable save data class
    [System.Serializable]
    class BestScoreData
    {
        //Declare the data type to be saved
        public string bestScoreUserName;
        public int bestScore;
    }
    


    public void SaveBestScoreDetails()
    {
        if (!_mainManager)
        {
            AssignMainmanger();
        }
        //check if bestscore is greater than the current recorded score.
        BestScoreData bestScoreData = new BestScoreData();
        bestScoreData.bestScoreUserName = _mainManager.UserNameTxt.text;
        bestScoreData.bestScore = _mainManager.points;


        string json = JsonUtility.ToJson(bestScoreData);

        File.WriteAllText(Application.persistentDataPath + "/bestScoreData.json", json);        


    }

    public void LoadBestScoreDetails()
    {
        string path = Application.persistentDataPath + "/bestScoreData.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BestScoreData bestScoreData = JsonUtility.FromJson<BestScoreData>(json);

            bestScoreUserName = bestScoreData.bestScoreUserName;
            bestScore = bestScoreData.bestScore;
        }
    }
    private void LoadAllSavedData()
    {
        LoadUserName();
        LoadBestScoreDetails();
    }

    private void AssignMainmanger()
    {
        _mainManager = FindObjectOfType<MainManager>();
    }
}
