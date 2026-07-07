using UnityEngine;

/// <summary>
/// 手毬のタップ操作を検知し、対応するコントローラーへ通知するクラス
/// 入力判定とゲームロジックの分離を目的としています
/// </summary>
public class TemariInputManager : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        // 画面座標からワールド座標へ変換するため基準となるカメラをキャッシュする
        cam = Camera.main;
    }


    /// <summary>
    /// 画面のタップ入力を検知し、一番手前にある手毬を特定して処理を実行する
    /// </summary>
    void Update()
    {
        if (cam == null) cam = Camera.main;

        if (!Input.GetMouseButtonDown(0)) return;

        // スクリーン上のタップ座標を、ゲーム内のワールド座標に変換する
        Vector2 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);

        // タップした地点に重なっている全てのオブジェクトを検知する
        Collider2D[] hits = Physics2D.OverlapPointAll(worldPos);

        if (hits == null || hits.Length == 0) return;

        // 複数重なっている場合は描画順が最も大きい = 一番手前の手毬を特定する
        Collider2D topHit = null;
        int topOrder = int.MinValue;

        foreach (var hit in hits)
        {
            SpriteRenderer sr = hit.GetComponent<SpriteRenderer>();

            // SpriteRendererを持たないオブジェクトはタップ対象外とする
            if (sr == null) continue;

            // より手前にあるオブジェクトが見つかったらターゲットを更新する
            if (sr.sortingOrder > topOrder)
            {
                topOrder = sr.sortingOrder;
                topHit = hit;
            }
        }

        // ターゲットが見つかった場合 それが手毬であればタップ処理を呼び出す
        if (topHit != null)
        {
            TemariController temari = topHit.GetComponent<TemariController>();
            if (temari != null)
            {
                temari.Tapped();
            }
        }
    }
}