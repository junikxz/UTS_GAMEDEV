using UnityEngine;

// Script ini sangat sederhana, tugasnya hanya memanggil GameManager
public class FlagRewardQuiz : BaseQuizLogic
{
    public override void StartQuiz()
    {
        // Langsung panggil fungsi pemberian hadiah di GameManager
        GameManager.instance.GiveFlagReward();
    }
}