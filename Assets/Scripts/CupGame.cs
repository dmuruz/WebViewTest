using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class CupGame : MonoBehaviour
{
    [SerializeField] private GameObject[] cups;
    [SerializeField] private GameObject startButton;
    [SerializeField] private TMP_Text resultText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private float gameDuration;
    private float gameTimer = 3f;
    private int hiddenCupIndex;
    private int score;
    private bool gameStarted;
    private List<Vector3> cupPositions = new List<Vector3>();
    private List<Vector3> randomPositions = new List<Vector3>();

    public void StartGame()
    {
        hiddenCupIndex = Random.Range(0, cups.Length);
        startButton.SetActive(false);
        resultText.text = "";
        StartCoroutine(HideBall());
    }

    public static List<T> Randomize<T>(List<T> list)
    {
        List<T> randomizedList = new List<T>();
        System.Random rnd = new System.Random();
        while (list.Count > 0)
        {
            int index = rnd.Next(0, list.Count);
            randomizedList.Add(list[index]);
            list.RemoveAt(index);
        }
        return randomizedList;
    }
    private void Start()
    {
        resultText.text = "";
        for (int i = 0; i < cups.Length; i++)
        {
            int copy = i;
            cups[i].gameObject.GetComponent<Button>().onClick.AddListener(delegate { cupClicked(copy); });
            cupPositions.Add(cups[i].transform.position);
        }
        gameStarted = false;
    }
    private IEnumerator HideBall()
    {
        cups[hiddenCupIndex].GetComponent<Image>().color = Color.green;
        yield return new WaitForSeconds(0.3f);
        cups[hiddenCupIndex].GetComponent<Image>().color = Color.white;
        List<Vector3> l = new List<Vector3>(cupPositions);
        randomPositions = Randomize<Vector3>(l);
        gameStarted = true;
        gameTimer = Time.time + gameDuration;
    }

    private void Update()
    {
        if (gameStarted && Time.time <= gameTimer)
        {
            for (int i = 0; i < cups.Length; i++)
            {
                cups[i].transform.position += (randomPositions[i] - cups[i].transform.position) / gameDuration;
            }
        }
    }

    public void cupClicked(int cupIndex)
    {
        if (gameStarted && Time.time < gameTimer)
        {
            if (cupIndex == hiddenCupIndex)
            {
                resultText.text = "Congratulations! You found the hidden object!";
                score++;
                scoreText.text = score.ToString();
            }
            else
            {
                resultText.text = "Oops! Try again!";
            }
            startButton.SetActive(true);
            gameStarted = false;
        }
    }
}
