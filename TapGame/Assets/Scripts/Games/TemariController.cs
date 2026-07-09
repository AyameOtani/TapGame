using System.Collections;
using UnityEngine;

/// <summary>
/// 手毬の種類を定義する列挙型
/// </summary>
public enum TemariType
{
    None,
    Blue,
    Pink,
    Yellow
}

/// <summary>
/// 手毬の種類と削除をコントロールするクラス
/// </summary>
public class TemariController : MonoBehaviour
{
    //手毬のタイプの保存
    [SerializeField] private TemariType temariType;

    // 拡大アニメーション速度
    [SerializeField] private float scaleDuration = 0.5f;

    // スコア演出用の文字 (+1とか加算されたものを出すため)
    [SerializeField] private GameObject scorePopUpPrefab;

    // 手毬が消える時のパーティクル
    [SerializeField] private GameObject fxTemariDestroyParticle;

    // タップされたときに流したいSEと音量(0.0f から 1.0f)を入れる
    [SerializeField] private SeManager.SeSetting tapSeSetting;


    // 拡大の目標値
    private Vector2 targetScale;

    // 各色ごとのポイントの定数の設定
    private const int BluePoint = 2;
    private const int PinkPoint = 5;
    private const int YellowPoint = 10;

    // 加算ポイントの文字色の定義
    private static readonly Color32 BlueColor = new Color32(132, 200, 237, 255);
    private static readonly Color32 PinkColor = new Color32(247, 150, 230, 255);
    private static readonly Color32 YellowColor = new Color32(246, 200, 100, 255);



    private void Start()
    {
        targetScale = transform.localScale;

        StartCoroutine(ScaleUpRoutine());
    }


    /// <summary>
    /// タップされたときの処理
    /// </summary>
    public void Tapped()
    {
        // 時間切れでフェード中の時はタップ判定させないため
        if (GameTimer.IsFading) return;

        // TryGetComponent を使うと、Collider2D がある場合だけ処理を実行できます
        if (TryGetComponent<Collider2D>(out Collider2D myCollider))
        {
            myCollider.enabled = false;
        }

        // タイプによって点数を変える処理
        int point = 0;
        Color32 textColor;
        switch (temariType)
        {
            case TemariType.Blue:
                point = BluePoint;
                textColor = BlueColor;
                break;

            case TemariType.Pink:
                textColor = PinkColor;
                point = PinkPoint;
                break;

            case TemariType.Yellow:
                textColor = YellowColor;
                point = YellowPoint;
                break;


            default:
                point = BluePoint;
                textColor = BlueColor;
                break;
        }

        // 指定したスコアを加算して手毬を画面から削除してリソースを解放する
        ScoreManager.Instance.AddScore(point);

        // インスペクターで設定したSEが存在していたらPlaySEを介して流す処理
        if (tapSeSetting != null && tapSeSetting.clip != null)
        {
            SeManager.Instance.PlaySE(tapSeSetting.clip, tapSeSetting.volume);
        }


        if (scorePopUpPrefab != null)
        {
            // Spawnerからキャンパスを聞く
            // UI用の座標に変換するため
            Transform canvasTrans = TemariSpawner.Instance.GetCanvasTransform();
            GameObject popUp = Instantiate(scorePopUpPrefab, canvasTrans);

            // ワールド座標を手毬の位置からUI座標に変換する
            Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            popUp.transform.position = screenPos;

            // テキストを取得
            var temp = popUp.GetComponent<TMPro.TextMeshProUGUI>();
            temp.text = "+" + point;
            temp.color = textColor;

        }

        // パーティクルが設定されていたら生成して初期化する
        if (fxTemariDestroyParticle != null)
        {
            GameObject particle = Instantiate(fxTemariDestroyParticle, transform.position, Quaternion.identity);

            // アタッチしたスクリプトを取得してパーティクルのセットアップを依頼する
            if (particle.TryGetComponent<ParticleInitializer>(out var initializer))
            {
                initializer.Initialize(textColor);
            }
        }

        Destroy(gameObject);

    }


    /// <summary>
    /// 生成時に一定時間かけて徐々に拡大させる
    /// </summary>
    private IEnumerator ScaleUpRoutine()
    {
        transform.localScale = Vector2.zero;

        // 拡大にかける時間
        float duration = scaleDuration;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // 経過時間を加算
            elapsed += Time.deltaTime;

            // 0から1の割合を計算
            float t = Mathf.Clamp01(elapsed / duration);

            // 目標サイズまで割合に応じて補間する
            transform.localScale = Vector2.Lerp(Vector2.zero, targetScale, t);

            yield return null;
        }

        // 目標サイズにする
        transform.localScale = targetScale;
    }

}