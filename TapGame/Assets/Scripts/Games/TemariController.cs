using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 手毬の種類を定義する列挙型 数種類の画像を出すため
/// </summary>
public enum TemariType
{
    None, // タイプなし
    Blue, // 青い手毬
    Pink  // ピンクの手毬
}

/// <summary>
/// 手毬の種類と削除をコントロールするクラス
/// </summary>
public class TemariController : MonoBehaviour
{
    // Inspectorから手毬の種類を選択出来るようにする
    [SerializeField] private TemariType temariType;

    /// <summary>
    /// 手毬を消す処理 タップされた時に削除したいため
    /// </summary>
    private void OnMouseDown()
    {
        // スコアを1加算する
        ScoreManager.Instance.AddScore(1);

        // タップされた手毬をシーンから削除する
        Destroy(gameObject);
    }

}
