using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] Color[] scoreTiers;
    [SerializeField] Sprite[] ranks;

    [SerializeField] TextMeshProUGUI racerName, racerScore, racerRank;
    [SerializeField] Image rancerRankBadge;

    public void SetupScore(int rank, string name, int score)
    {
        racerName.text = name;
        racerScore.text = score.ToString();
        racerRank.text = rank.ToString();

        if (rank == 1)
        {
            racerScore.color = scoreTiers[0];
            rancerRankBadge.sprite = ranks[0];
        }
        else if (rank == 2)
        {
            racerScore.color = scoreTiers[1];
            rancerRankBadge.sprite = ranks[1];
        }
        else if (rank == 3)
        {
            racerScore.color = scoreTiers[2];
            rancerRankBadge.sprite = ranks[2];
        }
        else
        {
            racerScore.color = scoreTiers[3];
            rancerRankBadge.sprite = ranks[3];
        }
    }
}
