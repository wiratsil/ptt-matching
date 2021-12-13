using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class API_AddScore : Singleton<API_AddScore>
{
    public enum Games { None = 0, Quiz = 1, SayNo = 2, Jigsaw = 3, Matching = 4 }
    public Games games;
    [Space]
    public bool testing;

    public void AddScore(int score = 0)
    {
        StartCoroutine(Upload(score));
    }

    IEnumerator Upload(int score = 0 )
    {
        WWWForm form = new WWWForm();

        form.AddField("value", score.ToString());
        form.AddField("gameId", (int)games);
        form.AddField("player", "PTTCGDay2021-" + GetUserId());

        using (UnityWebRequest www = UnityWebRequest.Post("https://universal-leaderboards.hocco.work/scores/add", form))
        {
            www.SetRequestHeader("Authorization", "Bearer " + API_Login.Instance.loginData.jwt);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    string GetUserId()
    {
        string[] urls ;
        string userId = "";

        if (testing)
        {
            urls = "www.game.com?user_id=1234".Split(new string[] { "?user_id=" }, System.StringSplitOptions.None);
        }
        else
        {
            urls = Application.absoluteURL.Split(new string[] { "?user_id=" }, System.StringSplitOptions.None);
        }

        if (urls.Length > 1)
        {
            userId = urls[1].Trim();
        }
        return userId;
    }
}
