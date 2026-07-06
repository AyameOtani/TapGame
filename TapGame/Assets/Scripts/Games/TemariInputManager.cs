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

    void Update()
    {
        if (cam == null)
            cam = Camera.main;

        // 入力の受付はマウスかタップの押下開始時のみに限定する
        if (!Input.GetMouseButtonDown(0))
            return;

        // クリック位置がワールド上のどこにあるかを計算してその地点のコライダーを確認する
        Vector2 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit == null)
            return;

        // ヒットしたオブジェクトに手毬が含まれている場合タップ処理を実行する
        TemariController temari = hit.GetComponent<TemariController>();
        if (temari != null)
        {
            temari.Tapped();
        }
    }
}