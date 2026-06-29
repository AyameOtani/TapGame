using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 手毬の種類を定義する列挙型
/// </summary>
public enum TemariType
{
    None,
    Blue,
    Pink
}

/// <summary>
/// 手毬の種類と削除をコントロールするクラス
/// </summary>
public class TemariController : MonoBehaviour
{
    [SerializeField] private TemariType temariType;

    void Update()
    {
        // クリック（PC） or タップ（スマホ）
        if (Input.GetMouseButtonDown(0))
        {
            // 画面座標 → ワールド座標に変換
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // その位置にあるCollider2Dを取得
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
        ScoreManager.Instance.AddScore(1);
        Destroy(gameObject);
    }
}