using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour    
{
    [SerializeField]
    Fellow player; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       Text scoreText= GameObject.Find("CurrentScoreTitle").GetComponent<Text>();
        scoreText.text = "Score: \n  " + player.score.ToString();
    }
}
