using UnityEngine;

/// <summary>
///  パーティクルエフェクトのサイズや色の設定をするためのクラス
/// </summary>
public class ParticleInitializer : MonoBehaviour
{

    [SerializeField] private float minSize = 0.1f;
    [SerializeField] private float maxSize = 0.35f;

    /// <summary>
    /// エフェクトを初期化するための処理
    /// </summary>
    /// <param name="color"></param>
    public void Initialize(Color color)
    {
        var ps = GetComponent<ParticleSystem>();
        var main = ps.main;

        // 色の設定
        main.startColor = new ParticleSystem.MinMaxGradient(color);

        // サイズのランダム設定
        main.startSize = new ParticleSystem.MinMaxCurve(minSize, maxSize);

        // 角度のランダム設定
        main.startRotation = new ParticleSystem.MinMaxCurve(0.0f, 2 * Mathf.PI);
    }
}
