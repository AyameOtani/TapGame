using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// ゲーム開始前のカウントダウン処理を管理するクラス
/// </summary>
public class CountdownManager : MonoBehaviour
{
    // カウントを待つ秒数の定数定義
    private const float CountdownInterval = 1.0f;

    // 初期カウントの最大数
    [SerializeField] private int startCount = 3;

    // ふすまを制御するスクリプトへの参照
    [SerializeField] private FusumaController fusumaController;

    // ふすまが開くまでに待つカウント すぐ開かないように制御
    [Header("ふすまが開くまでの待ち時間")]
    [SerializeField] private float waitBeforeOpen = 2.0f;

    // ふすまの開くSEを入れるため
    [SerializeField] private SEManager.SeSetting fusumaOpenSe;

    // 始めの琴のSEを入れるため
    [SerializeField] private SEManager.SeSetting startKotoFadeOut;


    private TextMeshProUGUI countdownText;

    // カウントダウン中かを外部からも取得出来るフラグにしら
    public bool IsCountingDown { get; private set; } = true;
    public static CountdownManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        // 自分自身にアタッチされているコンポーネントを自動取得
        countdownText = GetComponent<TextMeshProUGUI>();
        // テキストを始めは隠す
        countdownText.enabled = false;
    }


    private void Start()
    {
        StartCoroutine(StartGameRoutine());
    }

    /// <summary>
    /// ふすま開閉とカウントダウンを制御するコルーチン処理
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartGameRoutine()
    {
        yield return new WaitForSeconds(waitBeforeOpen);

        // ふすまを開く処理を開始
        if (fusumaController != null)
        {
            PlayFusumaSE();

            fusumaController.OpenFusuma();

            // ふすまがアニメーション中なら終わるまで待機する
            while (fusumaController.IsAnimating)
            {
                yield return null;
            }
        }
        // ふすまが開ききったらカウントダウンを開始
        yield return StartCoroutine(CountdownRoutine());
    }

    /// <summary>
    /// ふすまが開くSEを再生させるためにPlaySEを呼ぶ処理
    /// </summary>
    private void PlayFusumaSE()
    {
        if (fusumaOpenSe != null && fusumaOpenSe.clip != null)
        {
            SEManager.Instance.PlaySE(fusumaOpenSe.clip, fusumaOpenSe.volume);
        }
    }

    /// <summary>
    /// 始めの合図の時に琴のSEを呼ぶための処理
    /// </summary>
    private void PlayKotoSE()
    {
        if (startKotoFadeOut != null && startKotoFadeOut.clip != null)
        {
            SEManager.Instance.PlaySE(startKotoFadeOut.clip, startKotoFadeOut.volume);
        }
    }



    /// <summary>
    /// カウントダウンの数値を表示し、終了後にゲーム開始フラグを立てる
    /// </summary>
    private IEnumerator CountdownRoutine()
    {
        // 表示をONにする
        countdownText.enabled = true;
        int count = startCount;

        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(CountdownInterval);
            count--;
        }

        countdownText.text = "始め！";
        PlayKotoSE();
        yield return new WaitForSeconds(CountdownInterval);

        countdownText.gameObject.SetActive(false);
        IsCountingDown = false;
    }
}