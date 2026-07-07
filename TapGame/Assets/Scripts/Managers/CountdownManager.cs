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

    [Header("ふすまが開くまでの待ち時間")]
    // ふすまが開くまでに待つカウント すぐ開かないように制御
    [SerializeField] private float waitBeforeOpen = 2.0f;

    private TextMeshProUGUI countdownText;

    // 外部から状態を取得するため
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

    private IEnumerator StartGameRoutine()
    {
        yield return new WaitForSeconds(waitBeforeOpen);


        // ふすまを開く処理を開始
        if (fusumaController != null)
        {
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
        yield return new WaitForSeconds(CountdownInterval);

        countdownText.gameObject.SetActive(false);
        IsCountingDown = false;
    }
}