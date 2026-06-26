using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // TextMeshProを使うため

/// <summary>
/// 現在のスコアをUIに表示するクラス
/// GameシーンとResultシーンで使用する
/// </summary>
public class ScoreDisplay : MonoBehaviour
{
    // インスペクターから設定する表示用テキスト
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Update()
    {
        // ScoreManagerから現在のスコアを取得して表示を更新
        if (ScoreManager.Instance != null)
        {
            scoreText.text = "Score: " + ScoreManager.Instance.CurrentScore;
        }
        else
        {
            // ScoreManagerが存在しない場合
            scoreText.text = "スコアが正常に読み込まれませんでした";
        }
    }
}