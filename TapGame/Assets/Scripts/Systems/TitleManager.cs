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

    // ResultからTitleへフェードアウトするため
    [SerializeField] private FadeController fadeController; // インスペクターで設定

    // フェードにかける時間
    [SerializeField] private float maxFadeTime = 1.0f;

    // BGMをフェードアウトさせるためのボタン
    [SerializeField] private AudioSource bgmAudioSource;

    // ふすまの開くSEを入れるため
    [SerializeField] private SeManager.SeSetting fusumaOpenSe;

    // 決定ボタンのSEを入れるため
    [SerializeField] private SeManager.SeSetting startButtonSe;

    // ボリュームをフェードアウトさせた後にまた戻すため
    private float startVolume;

    // ふすまのSEをふすまアニメーションの半分を過ぎたらフェード開始のための割合
    private const float BgmFadeStartRatio = 0.3f;

    //フェード中かどうかの判定
    private bool IsFading = false;

    // ふすま演出中かどうかの判定
    private bool IsAnimating = false;

    void Start()
    {
        // ここで現在の音量を保存しておく
        startVolume = bgmAudioSource.volume;

        // フェードアウトをON, アニメーションはOFF
        IsFading = true;
        IsAnimating = false;
        StartCoroutine(SetupTitle());
    }


    // フェードアウト処理
    private IEnumerator SetupTitle()
    {
        // フェードアウト処理
        yield return StartCoroutine(fadeController.FadeIn(FadeController.FadeType.FadeOutType, maxFadeTime));
        // フェードが終わったら操作可能にする
        IsFading = false;
    }


    /// <summary>
    /// ゲーム開始ボタンが押された時の処理
    /// </summary>
    public void OnTapStartButton()
    {
        // 演出中なら重複操作を防ぐ
        if (IsFading || IsAnimating) return;

        // ボタンがおされた時にSEを流す処理
        if (startButtonSe != null && startButtonSe.clip != null)
        {
            SeManager.Instance.PlaySE(startButtonSe.clip, startButtonSe.volume);
        }

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

        PlayFusumaSE();

        // ふすまを閉じる関数を呼び出す
        fusumaController.CloseFusuma();

        // BGMフェードアウトを並行して実行
        yield return StartCoroutine(FadeOutBGMSequence());

        // シーン遷移
        SceneManager.LoadScene("Game");
    }


    /// <summary>
    /// ふすまが閉じるSEを再生させるためにPlaySEを呼ぶ処理
    /// </summary>
    private void PlayFusumaSE()
    {
        if (fusumaOpenSe != null && fusumaOpenSe.clip != null)
        {
            SeManager.Instance.PlaySE(fusumaOpenSe.clip, fusumaOpenSe.volume);
        }
    }


    /// <summary>
    /// BGMをフェードアウトさせる処理
    /// 遷移しても自然につながるようにするために必要
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOutBGMSequence()
    {
        float time = 0;

        //アニメーションの半分を過ぎたらフェード開始
        float delayStart = maxFadeTime * BgmFadeStartRatio;
        while (fusumaController.IsAnimating)
        {
            time += Time.deltaTime;
            if (time >= delayStart)
            {
                // 0以上になるように制限しながら徐々に音量を下げる処理
                // ふすまのフェードの演出と時間を合わせてタイミングが合うようにしている
                float fadeDuration = maxFadeTime - delayStart;
                float elapsed = time - delayStart;
                float t = Mathf.Clamp01(elapsed / fadeDuration);
                bgmAudioSource.volume = startVolume * (1 - t);
            }
            yield return null;
        }

        bgmAudioSource.Stop();
    }
}