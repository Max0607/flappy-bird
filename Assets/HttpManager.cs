using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttpManager : MonoBehaviour
{

    [SerializeField]
    private string URL;

    public Text[] NameText;
    public Text[] ScoreText;

    public Animator PipeAnim;

    public GameObject ScorePanel;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void ExitPipe()
    {
        ScorePanel.SetActive(false);
    }

    public void ClickGetScores()
    {
        StartCoroutine(GetScores());
    }

    IEnumerator GetScores()
    {
        string url = URL + "/scores";
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if(www.responseCode == 200){
            //Debug.Log(www.downloadHandler.text);
            Scores resData = JsonUtility.FromJson<Scores>(www.downloadHandler.text);
            ScorePanel.SetActive(true);

            foreach (ScoreData score in resData.scores)
            {
                Debug.Log(score.user_id + " | "+score.score);
                //NameText[score.user_id - 1].text = score.user_id.ToString();
            }
            for (int i = 0; i < resData.scores.Length; i++)
            {
                ScoreText[i].text = resData.scores[i].score.ToString();
                NameText[i].text = resData.scores[i].user_id.ToString();
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }
   
}


[System.Serializable]
public class ScoreData
{
    public string user_id;
    public int score;

}

[System.Serializable]
public class Scores
{
    public ScoreData[] scores;
}
