using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン遷移に必須

/// <summary>
/// 制限時間を管理し、時間切れでリザルト画面へ遷移するクラス
/// </summary>
public class GameTimer : MonoBehaviour
{
    // 制限時間を変えられるようにするため　今のところは変えない
    [SerializeField] private float remainingTime = 30.0f;

    // テキストのUIをゲーム画面に表示するため
    [SerializeField] private TextMeshProUGUI timerText;

    private void Update()
    {
        // もしtimerTextが設定されていない場合は、エラーを防ぐために処理をスキップする
        if (timerText == null) return;

        // 時間を減らしていく
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;

            // 残り時間を整数にして表示する
            int displayerTime = Mathf.CeilToInt(remainingTime);
            timerText.text = "Time: " + displayerTime;
        }
        else
        {
            // 時間切れでシーン切り替え
            SceneManager.LoadScene("Result");
        }
    }
}