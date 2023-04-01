using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class HighScoreTable : MonoBehaviour
{
    [SerializeField]
    string highScoreFile = "scores.txt";
    struct HighScoreEntry 
    {
        public int score;
        public string name;
    }

    List<HighScoreEntry> allScores = new List<HighScoreEntry>();
    public Dictionary<string, int> highscore = new Dictionary<string, int>();


    // Start is called before the first frame update
    void Start()
    {
        LoadHighScoreTable();
        SortHighScoreEntries();
        CreateHighScoreText();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void LoadHighScoreTable()
    {
        allScores.Clear();
        using (TextReader file = File.OpenText(highScoreFile))
        {
            string text = null;
            HighScoreEntry entry;
            int previousvalue = 0;
            entry.score = 0;
            highscore.Add("", -1);
            while ((text = file.ReadLine()) != null)
            {
                Debug.Log(text);
                string[] splits = text.Split(' ');
                entry.name = splits[0];
                entry.score = int.Parse(splits[1]);
                allScores.Add(entry);
                
                if (highscore.ElementAt(0).Value < entry.score){
                    highscore.Clear();
                    highscore.Add(entry.name, entry.score);
                }
            }      
        }
    }

    public void WriteHighScore(string name, int score)
    {
        string file = highScoreFile;
        {
            if (name != null && name != "" && name != " " && !string.IsNullOrWhiteSpace(name) &&
                !string.IsNullOrEmpty(name)) {
                StreamWriter writer = new StreamWriter(file, true);
                string jointscore = name + ' ' + score.ToString();
                writer.WriteLine(jointscore);
                writer.Close();
            }
            
            Application.Quit();
        }
    }

    [SerializeField]
    Font scoreFont;

    void CreateHighScoreText()
    {
        for (int i = 0; i<allScores.Count ; ++i)
        {
            GameObject o = new GameObject();
            o.transform.parent = transform ;

            Text t = o.AddComponent<Text>();
            t.text = allScores[i]. name + "\t\t" + allScores[i].score;
            t.font = scoreFont ;
            t.fontSize = 50;
            t.color = Color.red;

            o.transform.localPosition = new Vector3(0, -(i)* 6, 0);

            o.transform.localRotation = Quaternion.identity ;
            o.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

            o.GetComponent<RectTransform>(). sizeDelta = new Vector2(400 , 100);
        }
    }
    public void SortHighScoreEntries()
    {
        allScores.Sort((x, y) => y.score.CompareTo(x.score));
    }
}
