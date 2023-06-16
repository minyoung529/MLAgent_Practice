using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoccerField : MonoBehaviour
{
    [SerializeField]
    private Transform ball;
    private Rigidbody ballRigid;

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

    #region Property
    public Vector3 BallLocalPosition => ball.localPosition;
    #endregion  

    public static SoccerAgent LastTouchPlayer;

    void Start()
    {
        soccerAgent1 = player1.GetComponent<SoccerAgent>();
        soccerAgent2 = player2.GetComponent<SoccerAgent>();
        ballRigid = ball.GetComponent<Rigidbody>();
    }

    void Update()
    {
        TouchLine();

        redRewardText.text = soccerAgent1.GetCumulativeReward().ToString();
        blueRewardText.text = soccerAgent2.GetCumulativeReward().ToString();
    }

    void LateUpdate()
    {
        Vector3 ballPos = ballRigid.transform.position;
        ballPos.y = 0.2f;

        ballRigid.transform.position = ballPos;
    }

    private void TouchLine()
    {
        // 공이 나가면
        if (Mathf.Abs(BallLocalPosition.x) > maxX + 6f || Mathf.Abs(BallLocalPosition.z) > maxZ + 6f)
        {
            ResetPosition();

            // // 가장 최근에 터치했던 플레이어에게 감점 부여
            // if (LastTouchPlayer)
            // {
            //     LastTouchPlayer.AddReward(-5f);
            // }
        }
    }

    public void ResetField()
    {
        // ball Random Transform
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
            }
            else
            {
                soccerAgent2.AddRewardToAgent(-7.5f);
                Debug.Log("빠랑 자책골");
            }
        }

        if (redScore == 3 || blueScore == 3)
        {
            if (redScore == 3)
            {
                soccerAgent1.AddRewardToAgent(10f);
                soccerAgent2.AddRewardToAgent(-2f);
            }
            else
            {
                soccerAgent2.AddRewardToAgent(10f);
                soccerAgent1.AddRewardToAgent(-2f);
            }

            soccerAgent1.EndEpisode();
            soccerAgent2.EndEpisode();
            ResetField();
        }
        else
        {
            ResetPosition();
        }
    }

    private void UpdateUI(TextMeshPro tmp, int score)
    {
        tmp.SetText(score.ToString());
    }

    private void ResetPosition()
    {
        Debug.Log("RESET POSITION");
        ball.localPosition = Vector3.zero;
        ballRigid.velocity = Vector3.zero;
        ballRigid.angularVelocity = Vector3.zero;

        player1.ResetMovement();
        player2.ResetMovement();
    }
}
