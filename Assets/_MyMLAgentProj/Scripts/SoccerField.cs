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
    private AgentMovement player1;
    [SerializeField]
    private AgentMovement player2;

    private SoccerAgent soccerAgent1;
    private SoccerAgent soccerAgent2;

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
        // 공이 나가면
        if (Mathf.Abs(BallLocalPosition.x) > 13f || Mathf.Abs(BallLocalPosition.z) > 20f)
        {
            ResetPosition();

            // 가장 최근에 터치했던 플레이어에게 감점 부여
            if (LastTouchPlayer)
            {
                LastTouchPlayer.AddReward(-5f);
            }
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
        }
        else
        {
            UpdateUI(redScoreText, ++redScore);
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
        ball.localPosition = Vector3.zero;
        ballRigid.velocity = Vector3.zero;
        ballRigid.angularVelocity = Vector3.zero;

        player1.ResetMovement();
        player2.ResetMovement();
    }
}
