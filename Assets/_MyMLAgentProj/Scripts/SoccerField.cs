using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoccerField : MonoBehaviour
{
    [SerializeField]
    private Ball[] balls;

    [SerializeField]
    private bool isTraining = false;

    private int redScore;
    private int blueScore;

    [SerializeField]
    private TextMeshPro redScoreText;

    [SerializeField]
    private TextMeshPro blueScoreText;

    [SerializeField]
    private TextMeshPro redRewardText;

    [SerializeField]
    private TextMeshPro blueRewardText;


    [SerializeField]
    private AgentMovement player1;
    [SerializeField]
    private AgentMovement player2;

    private SoccerAgent soccerAgent1;
    private SoccerAgent soccerAgent2;

    [SerializeField]
    private float maxX, maxZ;

    public ReulstCanvas resultCanvas;


    public static SoccerAgent LastTouchPlayer;

    void Start()
    {
        soccerAgent1 = player1.GetComponent<SoccerAgent>();
        soccerAgent2 = player2.GetComponent<SoccerAgent>();
    }

    void Update()
    {
        TouchLine();

        redRewardText.text = soccerAgent1.GetCumulativeReward().ToString();
        blueRewardText.text = soccerAgent2.GetCumulativeReward().ToString();
    }

    void LateUpdate()
    {
        foreach (Ball ball in balls)
        {
            Vector3 pos = ball.transform.localPosition;
            pos.y = 0.2f;
            ball.transform.localPosition = pos;
        }
    }

    private void TouchLine()
    {
        // 공이 나가면

        foreach (Ball ball in balls)
        {
            if (Mathf.Abs(ball.transform.localPosition.x) > maxX + 6f || Mathf.Abs(ball.transform.localPosition.z) > maxZ + 6f)
            {
                ResetPosition();
            }
        }
    }

    public void ResetField()
    {
        // Reset Ball Position
        ResetPosition();

        UpdateUI(redScoreText, redScore = 0);
        UpdateUI(blueScoreText, blueScore = 0);
    }

    public void AddScore(bool isRed)
    {
        if (isRed)
        {
            UpdateUI(blueScoreText, ++blueScore);

            if (LastTouchPlayer == soccerAgent2)
            {
                soccerAgent2.AddRewardToAgent(5f);
                soccerAgent1.AddRewardToAgent(-5f);
                Debug.Log("파랑 골");
            }
            else
            {
                soccerAgent1.AddRewardToAgent(-8.5f);
                Debug.Log("빨강 자책골");
            }
        }
        else
        {
            UpdateUI(redScoreText, ++redScore);

            if (LastTouchPlayer == soccerAgent1)
            {
                soccerAgent1.AddRewardToAgent(5f);
                soccerAgent2.AddRewardToAgent(-5f);
                Debug.Log("빨강 골");
            }
            else
            {
                soccerAgent2.AddRewardToAgent(-7.5f);
                Debug.Log("파랑 자책골");
            }
        }

        if (redScore == 3 || blueScore == 3)
        {
            if (redScore == 3)
            {
                soccerAgent1.AddRewardToAgent(10f);
                soccerAgent2.AddRewardToAgent(-2f);
                resultCanvas.Defeat();
            }
            else
            {
                soccerAgent2.AddRewardToAgent(10f);
                soccerAgent1.AddRewardToAgent(-2f);
                resultCanvas.Victory();
            }

            soccerAgent1.EndEpisode();
            soccerAgent2.EndEpisode();
            ResetField();
            ResetPosition();
        }
    }

    private void UpdateUI(TextMeshPro tmp, int score)
    {
        tmp.SetText(score.ToString());
    }

    private void ResetPosition()
    {
        foreach (Ball ball in balls)
        {
            ball.ResetPosition();
        }
    }
}
