using UnityEngine;
using TMPro;

/// <summary>
/// ゲーム画面で表示する加算されたスコア
/// </summary>
public class ScorePopUp : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI textMeshPro;
    private Color originalColor;

    // 消えるまでの時間
    [SerializeField] private float fadeDuration = 0.5f;
    // 上に移動する距離
    [SerializeField] private float moveDistance = 50f;

    private float elapsedTime;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        textMeshPro = GetComponent<TextMeshProUGUI>();
        originalColor = textMeshPro.color;
    }

    private void Start()
    {
        // 生成されてから fadeDuration秒後 に自動的に削除
        Destroy(gameObject, fadeDuration);
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float t = Mathf.Clamp01(elapsedTime / fadeDuration);

        // 上にふわっと移動
        // (y軸に移動距離分だけ、時間とともに Lerp で移動させる)
        rectTransform.anchoredPosition += Vector2.up * (moveDistance * Time.deltaTime);

        // 透明にして消す
        // (color.aを減らす)
        Color fadeColor = originalColor;
        fadeColor.a = Mathf.Lerp(1f, 0f, t);
        textMeshPro.color = fadeColor;
    }
}