using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// タイトル画面の制御とゲームシーンへの遷移を管理するクラス
/// </summary>
public class TitleManager : MonoBehaviour
{
    // ふすまの演出を制御するため
    [SerializeField] private FusumaController fusumaController;

    // ふすま演出中かどうかの判定
    private bool IsAnimating = false;

    void Start()
    {
        // 画面が開いた状態からスタートする想定ならここを調整
        IsAnimating = false;
    }

    /// <summary>
    /// ゲーム開始ボタンが押された時の処理
    /// </summary>
    public void OnTapStartButton()
    {
        // 演出中なら重複操作を防ぐ
        if (IsAnimating) return;

        // スコアをリセットし、ゲームの初期状態を確保する
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ResetScore();
        }

        // ふすまを閉じてからシーン遷移を開始する
        StartCoroutine(StartGameWithFusuma());
    }

    /// <summary>
    /// ふすまを閉じる演出を行い、完了後にシーンを遷移させる
    /// </summary>
    private IEnumerator StartGameWithFusuma()
    {
        IsAnimating = true;

        // ふすまを閉じる関数を呼び出す
        fusumaController.CloseFusuma();

        // ふすまが閉じていない場合は待機
        while (fusumaController.IsAnimating)
        {
            yield return null;
        }

        // タイトル画面からゲーム画面へ遷移する
        SceneManager.LoadScene("Game");
    }
}