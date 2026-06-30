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
    // パネルを追加出来るようにする
    [SerializeField] private UnityEngine.UI.Image fadePanel;

    // 制限時間を変えられるようにするため　今のところは変えない
    [SerializeField] private float remainingTime = 30.0f;

    // フェードにかける時間
    [SerializeField] private float maxFadeTime = 1.5f;


    // フェード中かを判定するフラグ
    public static bool IsFading { get; private set; } = false;
    public static float RemainingTime { get; private set; }
    public static float TotalTime { get; private set; } = 30.0f;

    // 元のテキストを保存する変数 インスペクターで変更したいため
    private string baseText;

    /// <summary>
    /// タイマーを初期化するための処理
    /// </summary>
    private void Awake()
    {
        IsFading = false;
    }

    // テキストのUIをゲーム画面に表示するため
    [SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        RemainingTime = remainingTime;

        baseText = timerText.text;
    }

    private void Update()
    {
        // もしtimerTextが設定されていない場合は、エラーを防ぐために処理をスキップする
        if (timerText == null) return;

        // 時間を減らしていく
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            RemainingTime = remainingTime;

            // 残り時間を整数にして表示する
            int displayerTime = Mathf.CeilToInt(remainingTime);
            timerText.text = baseText + " " +displayerTime + "秒";
        }
        else
        {
            if (!IsFading)
            {
                StartCoroutine(FadeAndLoad());
            }
        }
    }

    /// <summary>
    /// 時間切れの時に徐々に白くフェードしていく処理
    /// 画面遷移の演出のため
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeAndLoad()
    {
        IsFading = true;

        // 経過した時間
        float elapsed = 0.0f;

        while (elapsed < maxFadeTime)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / maxFadeTime);

            // 透過度を更新
            Color color = fadePanel.color;
            color.a = alpha;
            fadePanel.color = color;

            yield return null;
        }

        SceneManager.LoadScene("Result");
    }

}