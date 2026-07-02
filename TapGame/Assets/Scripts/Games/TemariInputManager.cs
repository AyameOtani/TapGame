using UnityEngine;

/// <summary>
/// 手毬のタップ判定をするクラス
/// TemariControllerに処理が偏らないよう鬼
/// </summary>
public class TemariInputManager : MonoBehaviour
{
    private Camera cam;



    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (cam == null)
            cam = Camera.main;


        if (!Input.GetMouseButtonDown(0))
            return;

        Vector2 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit == null)
            return;

        TemariController temari = hit.GetComponent<TemariController>();
        if (temari != null)
        {
            temari.Tapped();
        }
    }
}