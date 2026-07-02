using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

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

    [SerializeField] private GameObject fxTemariDestroy;


    // 拡大の目標値
    private Vector2 targetScale;

    private void Start()
    {
        targetScale = transform.localScale;

        StartCoroutine(ScaleUpRoutine());
    }

    void Update()
    {
        // 入力があった場合、タップ位置と自身の当たり判定が重なっているかの判定
        if (Input.GetMouseButtonDown(0))
        {
            // 画面座標からワールド座標に変換
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapPoint(worldPos);

            // 自分自身に当たっていたら
            if (hit != null && hit.gameObject == this.gameObject)
            {
                Tapped();
            }
        }
    }

    /// <summary>
    /// タップされたときの処理
    /// </summary>
    private void Tapped()
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
                point = 2;
                textColor = new Color32(80, 120, 200, 255);
                break;

            case TemariType.Pink:
                textColor = new Color32(200, 80, 180, 255);
                point = 5;
                break;

            case TemariType.Yellow:
                textColor = new Color32(210, 190, 80, 255);
                point = 10;
                break;


            default:
                point = 2;
                textColor = new Color32(0, 180, 255, 255);
                break;
        }

        // 指定したスコアを加算して手毬を画面から削除してリソースを解放する
        ScoreManager.Instance.AddScore(point);

        if (scorePopUpPrefab != null)
        {
            // Canvasを検索して親にする
            GameObject canvas = GameObject.Find("Canvas");
            GameObject popUp = Instantiate(scorePopUpPrefab, canvas.transform);

            // ワールド座標を手毬の位置からUI座標に変換する
            Vector2 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            popUp.transform.position = screenPos;

            // テキストを取得
            var temp = popUp.GetComponent<TMPro.TextMeshProUGUI>();
            temp.text = "+" + point;
            temp.color = textColor;

        }

        // パーティクルが設定されていたら生成する
        if (fxTemariDestroy != null)
        {
            GameObject particle = Instantiate(fxTemariDestroy, transform.position, Quaternion.identity);

            // 消された手毬と色を統一させる
            // 一体感を出すため
            var main = particle.GetComponent<ParticleSystem>().main;
            main.startColor = new ParticleSystem.MinMaxGradient(textColor);
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