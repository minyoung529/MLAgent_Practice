using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SoccerField : MonoBehaviour
{
    [SerializeField]
    private Transform ball;

    [SerializeField]
    private bool isTraining = false;

    private int redScore;
    private int blueScore;

    [SerializeField]
    private TextMeshPro redScoreText;

    [SerializeField]
    private TextMeshPro blueScoreText;

    #region Property
    public Vector3 BallPosition => ball.position;
    #endregion

    public void ResetField()
    {
        // ball Random Transform
        ball.transform.position = Vector3.zero;

        UpdateUI(redScoreText, redScore = 0);
        UpdateUI(blueScoreText, blueScore = 0);
    }

    public void AddScore(bool isRed)
    {
        if (isRed)
        {
            UpdateUI(redScoreText, ++redScore);
        }
        else
        {
            UpdateUI(blueScoreText, ++blueScore);
        }
    }

    private void UpdateUI(TextMeshPro tmp, int score)
    {
        tmp.SetText(score.ToString());
    }
}
