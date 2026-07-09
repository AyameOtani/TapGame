using UnityEngine;

/// <summary>
/// スコアの保持と管理を行うクラス シーンを跨いでもスコアを保存したいから
/// </summary>
public class ScoreManager : MonoBehaviour
{
    // シーンをまたいでも消えないためのインスタンス
    // GameとResultで使用するから
    private static ScoreManager instance;

    /// <summary>
    /// 外部からアクセスするため ゲッターのようなもの
    /// </summary>
    public static ScoreManager Instance
    {
        get { return instance; }
    }

    // スコア保持用のプロパティ
    // ゲッターのような役割をする
    public int CurrentScore { get; private set; }

    /// <summary>
    /// インスタンスの初期化と、シーン遷移時に消えない設定を行う
    /// </summary>
    private void Awake()
    {
        // 最初のインスタンスを保持して重複を破棄する
        if (instance == null)
        {
            instance = this;
            // DontDestroyOnLoad...このオブジェクトは削除されずに次のシーンへ引き継がれる
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// スコアを加算する 手毬の数によってやるため
    /// </summary>
    public void AddScore(int scoreValue)
    {
        CurrentScore += scoreValue;
    }

    /// <summary>
    /// スコアをリセットするため 毎ゲームリセットして0から始めるため タイトルで処理を呼んでいる
    /// </summary>
    public void ResetScore()
    {
        // ゲームをループしたらスコアがリセットされるように
        CurrentScore = 0;
    }

}