using UnityEngine;

/// <summary>
/// オブジェクトを上下に滑らかに浮遊させるクラス
/// </summary>
public class FloatingObject : MonoBehaviour
{
    // 浮遊の速度
    [SerializeField] private float speed = 2.0f;

    // 上下の移動幅
    [SerializeField] private float amplitude = 0.5f;


    // オブジェクトの初期位置を保持する変数
    private Vector3 startPosition;


    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // 現在の経過時間を取得してMathf.Sin で上下の動きを計算する
        float yOffset = Mathf.Sin(Time.time * speed) * amplitude;

        // 計算したオフセットを初期位置に加えて座標を更新する
        transform.position = startPosition + new Vector3(0, yOffset, 0);
    }
}