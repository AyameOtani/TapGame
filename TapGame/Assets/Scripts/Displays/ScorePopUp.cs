using UnityEngine;
using TMPro;

/// <summary>
/// スコア加算時に一時的に表示し、演出とともに自動消滅させるためのコンポーネント
/// </summary>
public class ScorePopUp : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI textMeshPro;
    private Color originalColor;

    // フェードにかける時間
    [SerializeField] private float fadeDuration = 0.5f;
    // 上に移動する距離
    [SerializeField] private float moveDistance = 80.0f;
    //演出の現在の経過時間
    private float elapsedTime;

    private void Awake()
    {
        // 演出制御に必要なコンポーネントをキャッシュしてGetComponentの負荷を軽減
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        originalColor = textMeshPro.color;
    }

    private void Start()
    {
        // メモリリークを防ぐため、演出終了後にはオブジェクトを破棄する
        Destroy(gameObject, fadeDuration);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        // 進捗度合いを0から1に正規化することで、各アニメーションの計算を共通化
        float t = Mathf.Clamp01(elapsedTime / fadeDuration);

        // UIが上に上昇する視覚的なフィードバックを与える
        rectTransform.anchoredPosition += Vector2.up * (moveDistance * Time.deltaTime);

        // 時間経過とともに透明度を下げることで、自然に消える演出を表現
        textMeshPro.alpha = Mathf.Lerp(1.0f, 0.0f, t);
    }
}