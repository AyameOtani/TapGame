using UnityEngine;
using TMPro;

/// <summary>
/// リザルトシーンで最終スコアを表示するクラス
/// </summary>
public class ResultScoreDisplay : MonoBehaviour
{
    // リザルト画面で表示するテキストの変数
    [SerializeField] private TextMeshProUGUI resultText;

    // インスペクターのテキストを保存する
    private string resultScoreText;

    private void Start()
    {
        resultScoreText = resultText.text;

        // ScoreManagerが存在するか確認し、存在する場合は保持している最終スコアを取得
        if (ScoreManager.Instance != null)
        {
            // スコアを文字列として取得し、結果表示用テキストに反映
            resultText.text = resultScoreText + "  " + ScoreManager.Instance.CurrentScore;
        }
        else
        {
            // ScoreManagerが見つからない場合の安全策
            resultText.text = resultScoreText + "  " + 0;
        }
    }

}