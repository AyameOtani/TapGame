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

    // テキストUIを入れるための定義
    [SerializeField] private TextMeshProUGUI countdownText;

    // ふすまを制御するスクリプトへの参照
    [SerializeField] private FusumaController fusumaController;


    // 外部から状態を取得するため
    public bool IsCountingDown { get; private set; } = true;

    public static CountdownManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartGameRoutine());
    }

    private IEnumerator StartGameRoutine()
    {
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