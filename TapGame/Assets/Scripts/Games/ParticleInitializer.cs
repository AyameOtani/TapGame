using UnityEngine;

/// <summary>
///  パーティクルエフェクトのサイズや色の設定をするためのクラス
/// </summary>
public class ParticleInitializer : MonoBehaviour
{
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
    }
}
