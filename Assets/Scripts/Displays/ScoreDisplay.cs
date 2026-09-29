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
    
    // インスペクターのスコアという文字を保存する変数
    private string scoreNome;


    private void Start()
    {
        scoreNome = scoreText.text;
    }

    private void Update()
    {
        // ScoreManagerから現在のスコアを取得して表示を更新
        if (ScoreManager.Instance != null)
        {
            scoreText.text = scoreNome + "  " +ScoreManager.Instance.CurrentScore;
        }
        else
        {
            // ScoreManagerが存在しない場合
            scoreText.text = "スコアが正常に読み込まれませんでした";
        }
    }
}