using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// ゲームシーンに遷移させるためのクラス
/// </summary>
public class TitleManager : MonoBehaviour
{
    // フェードするため
    [SerializeField] private FadeController fadeController; // インスペクターで設定

    // フェードにかける時間
    [SerializeField] private float maxFadeTime = 1.0f;

    //フェード中かどうかの判定
    private bool IsFading = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    ///  ゲーム開始ボタンが押された時の処理
    /// </summary>
    public void OnTapStartButton()
    {
        if (IsFading) return;

        // スコアをリセット 累計にしないため
        if (ScoreManager.Instance != null)
        {
            // リセットする関数を呼ぶ
            ScoreManager.Instance.ResetScore();
        }

        StartCoroutine(StartGameWithFade());
    }


    /// <summary>
    /// ボタンが押されたらフェードさせるため
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartGameWithFade()
    {
        IsFading = true;

        yield return StartCoroutine(fadeController.FadeIn(maxFadeTime));

        // タイトル画面からゲーム画面へ遷移する
        SceneManager.LoadScene("Game");
    }

    
}
