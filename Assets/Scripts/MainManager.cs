using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
   
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public int points;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text UserNameTxt;
    public Text LeaderBoardTxt;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private static GameManager _gameManager;
    private string _bestScoreUserName;
    
    


    /*private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
    }*/

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.Instance;
        UserNameTxt.text = _gameManager.newUserName;
        LeaderBoardTxt.text = $"Best Score : {_gameManager.bestScoreUserName} : {_gameManager.bestScore}";
        
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            
            const float step = 0.6f;
            int perLine = Mathf.FloorToInt(4.0f / step);
        
            int[] pointCountArray = new [] {1,1,2,2,5,5};
            for (int i = 0; i < LineCount; ++i)
            {
                for (int x = 0; x < perLine; ++x)
                {
                    Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                    var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                    brick.PointValue = pointCountArray[i];
                    brick.onDestroyed.AddListener(AddPoint);
                }
            }
        }
        
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                SceneManager.LoadScene(1);
                _gameManager.LoadBestScoreDetails();
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        points = m_Points;
    }

    public void GameOver()
    {
       
        m_GameOver = true;
        GameOverText.SetActive(true);

        if (_gameManager.bestScore <= points)
        {
            _gameManager.SaveBestScoreDetails();
            

        }
    }

    
    
}
