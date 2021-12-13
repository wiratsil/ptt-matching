using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class API_Login : Singleton<API_Login>
{
    public LoginData loginData;

    public void StartLogin()
    {
        StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        WWWForm form = new WWWForm();
        form.AddField("identifier", "pttcg2021");
        form.AddField("password", "63RQHKck");

        using (UnityWebRequest www = UnityWebRequest.Post("https://universal-leaderboards.hocco.work/auth/local", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                loginData = LoginData.CreateFromJSON(www.downloadHandler.text);
                Debug.Log(loginData.jwt);
            }
        }
    }
}
