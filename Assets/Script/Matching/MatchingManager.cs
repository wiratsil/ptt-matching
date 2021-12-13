using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class MatchingManager : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenInTab(string url);

    public List<GameObject> imageList;
    public List<GameObject> matchingPos;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI timerCountDownText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI resultText;
    public GameObject result;
    List<int> usedValues = new List<int>();
    List<GameObject> randomPosList = new List<GameObject>();
    [SerializeField]
    List<GameObject> lifePointList;

    GameObject image1 = null;
    GameObject image2 = null;
    [SerializeField]
    int lifePoint = 3;
    [SerializeField]
    int scorePoint = 0;
    [SerializeField]
    bool playable = true;
    [SerializeField]
    float timer = 45;
    [SerializeField]
    float timerCountDown = 3.5f;
    float timerCount ;

    // Start is called before the first frame update
    void Start()
    {
        timerCount = timerCountDown;
        _StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (timerCountDown >= -1)
        {
            timerCountDownText.text = Mathf.CeilToInt(timerCountDown).ToString();
            timerCountDown -= Time.deltaTime;
        }
        else
        {
            timerCountDownText.transform.parent.gameObject.SetActive(false);
        }
        if (timer >= 0 && playable)
        {
            timer -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(timer).ToString();

            if(timer < 5)
            {
                timerText.color = Color.red;
            }
            if (timer <= 0)
            {
                playable = false;
                transform.DOMoveX(0, 1).OnStepComplete(() => GameOver());
            }
        }
        if (Input.GetMouseButtonDown(0) && playable)
        {
            SelectImage();
        }
    }

    public void SelectImage()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && hit.collider.tag == "Player" && Mathf.Abs(hit.collider.transform.rotation.y) != 1)
        {
            if (image1 == null)
            {
                image1 = hit.collider.gameObject;
                hit.collider.transform.DORotate(new Vector3(0, 180, 0), 0.5f);
            }
            else if (image2 == null && hit.collider.name != image1.name)
            {
                playable = false;
                image2 = hit.collider.gameObject;
                hit.collider.transform.DORotate(new Vector3(0, 180, 0), 0.5f).OnStepComplete(() => CheckImage());
            }
        }
    }

    public void CheckImage()
    {
        //Correct
        if (image1.transform.GetChild(0).name == image2.transform.GetChild(0).name)
        {
            scorePoint++;
            playable = true;

            scoreText.text = (scorePoint * 5).ToString();
            resultText.text = (scorePoint * 5).ToString();
        }
        else
        {
            if(lifePoint > 0)
            {
                lifePoint--;
                lifePointList[lifePointList.Count - 1].transform.GetChild(0).gameObject.SetActive(false);
                lifePointList.RemoveAt(lifePointList.Count - 1);
            }
            image1.transform.DORotate(new Vector3(0, 0, 0), 0.5f);
            image2.transform.DORotate(new Vector3(0, 0, 0), 0.5f);

            playable = true;

            if (lifePoint <= 0)
            {
                playable = false;
                transform.DOMoveX(0, 1).OnStepComplete(() => GameOver());
            }
        }

        if (scorePoint == 8)
        {
            playable = false;
            transform.DOMoveX(0, 1).OnStepComplete(() => GameOver());
        }
        image1 = null;
        image2 = null;
    }

    public void _StartGame()
    {
        usedValues.Clear();
        randomPosList.Clear();
        randomPosList.AddRange(matchingPos);
        playable = false;

        for (int i = 0; i < 8; i++)
        {
            int randomIndex = UniqueRandomInt(0, imageList.Count);
            int randomPos1 = Random.RandomRange(0 , randomPosList.Count);
            GameObject clone1 = Instantiate(imageList[randomIndex], randomPosList[randomPos1].transform);
            randomPosList.RemoveAt(randomPos1);


            int randomPos2 = Random.RandomRange(0, randomPosList.Count);
            GameObject clone2 = Instantiate(imageList[randomIndex], randomPosList[randomPos2].transform);
            randomPosList.RemoveAt(randomPos2);
        }

        DoAllFlip(new Vector3(0, 180, 0), 0.5f, false);
        transform.DOMoveX(0, timerCount).OnStepComplete(() => DoAllFlip(new Vector3(0, 0, 0), 0.5f));
    }

    public void DoAllFlip(Vector3 vector3, float time, bool _playable = true)
    {
        for (int j = 0; j < matchingPos.Count; j++)
        {
            matchingPos[j].transform.DORotate(vector3, time);
        }
        playable = _playable;
    }

    public int UniqueRandomInt(int min, int max)
    {
        int val = Random.Range(min, max);
        while (usedValues.Contains(val))
        {
            val = Random.Range(min, max);
        }
        usedValues.Add(val);
        return val;
    }

    public void GameOver ()
    {
        result.SetActive(true);
        API_AddScore.Instance.AddScore(scorePoint * 5);
    }

    public void _ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void _OpenUrl(string url)
    {
        //Application.OpenURL(url);
        OpenInTab(url);
    }
}
