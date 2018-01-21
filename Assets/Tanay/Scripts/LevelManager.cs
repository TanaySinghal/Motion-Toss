using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text livesText;

    [SerializeField]
    private Target basicTarget;

    [SerializeField]
    private BallScript ball;

    [SerializeField]
    private Transform obstruction; 

    private int _score;
    int score
    {
        get { return _score; }
        set
        {
            _score = value;
            scoreText.text = _score.ToString();
        }
    }

    private int _lives;
    int lives
    {
        get { return _lives;  }
        set
        {
            _lives = value;
            livesText.text = "Lives: " + _lives.ToString();
        }
    }

    // Use this for initialization
    void Start () {
        basicTarget.OnHit += OnTargetHit;
        ball.OnBallDied += OnMiss;
        score = 0;
        lives = 10;
	}

    void OnMiss()
    {
        lives --;

        if (lives <= 0)
        {
            // TODO: Load Start Menu
            SceneManager.LoadScene("Menu");
        }
    }

    void OnTargetHit()
    {
        score++;

        // Move target
        if (score <= 4)
        {
            basicTarget.ChangePositionWithoutMovement(Random.Range(-30f, 30f), Random.Range(1f, 2f));
            obstruction.position = new Vector3(0, 0, -10f);
        }

        else
        {
            if (score >= 8)
            {
                basicTarget.ChangePositionWithMovement(Random.Range(-30f, 30f), Random.Range(1.5f, 3f), Mathf.Max(0.5f, score / 50));
            }

            // Place a cube 2/3rds way through with a bit of randomness
            Vector3 randomOffset = Vector3.up + Camera.main.transform.right * 0.2f * Random.Range(-1f, 1f);
            obstruction.position = basicTarget.transform.position * 2f / 3f + randomOffset;
        }

        ball.ResetBall();
    }
}
