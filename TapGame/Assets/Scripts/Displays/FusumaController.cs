using System.Collections;
using UnityEngine;

/// <summary>
/// 画面の両端に配置したふすま画像を左右にスライドさせ、開閉演出を制御するクラス
/// </summary>
public class FusumaController : MonoBehaviour
{
    [SerializeField] private RectTransform leftFusuma;
    [SerializeField] private RectTransform rightFusuma;
    [SerializeField] private float duration = 0.5f; // アニメーション全体の所要時間

    // 画面外への退避距離の倍率
    private const float HideOffsetMultiplier = 1.5f;

    // アニメーション中かどうかを外部から確認できるようにする
    public bool IsAnimating { get; private set; } = false;

    /// <summary>
    /// ふすまを左右に動かして画面外へ退避させる ゲーム開始時の演出
    /// </summary>
    public void OpenFusuma()
    {
        // 閉じた状態から開いた状態へ遷移させるコルーチンを開始
        StartCoroutine(MoveFusuma(true));
    }

    /// <summary>
    /// ふすまを中央へ動かして画面を覆う ゲーム終了時や遷移前の暗転用
    /// </summary>
    public void CloseFusuma()
    {
        // 開いた状態から閉じた状態へ戻すコルーチンを開始
        StartCoroutine(MoveFusuma(false));
    }

    /// <summary>
    /// 指定された方向にふすまの座標を補完移動させる
    /// </summary>
    /// <param name="isOpen">trueなら開く,falseなら閉じる動きをする</param>
    private IEnumerator MoveFusuma(bool isOpen)
    {
        IsAnimating = true;
        float elapsed = 0.0f;

        // 左のふすま   閉まっている時（X=0） 開いている時（X=-ふすまの幅）
        float leftClosedX = 0;
        float leftOpenX = -leftFusuma.rect.width;

        // 右のふすま   閉まっている時（X=0）  開いている時（X=ふすまの幅）
        float rightClosedX = 0;
        float rightOpenX = rightFusuma.rect.width;

        // isOpenなら開ける    isOpenがfalseなら閉じる
        Vector2 startLeft = isOpen ? new Vector2(leftClosedX, 0) : new Vector2(leftOpenX, 0);
        Vector2 endLeft = isOpen ? new Vector2(leftOpenX, 0) : new Vector2(leftClosedX, 0);

        Vector2 startRight = isOpen ? new Vector2(rightClosedX, 0) : new Vector2(rightOpenX, 0);
        Vector2 endRight = isOpen ? new Vector2(rightOpenX, 0) : new Vector2(rightClosedX, 0);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);

            leftFusuma.anchoredPosition = Vector2.Lerp(startLeft, endLeft, t);
            rightFusuma.anchoredPosition = Vector2.Lerp(startRight, endRight, t);

            yield return null;
        }

        IsAnimating = false;
    }
}