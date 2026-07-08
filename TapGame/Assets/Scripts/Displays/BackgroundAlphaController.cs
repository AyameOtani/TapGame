using UnityEngine;
using UnityEngine.UI;

public class BackgroundAlphaController : MonoBehaviour
{
    void Start()
    {
        // 自分のImageコンポーネントを取得
        Image img = GetComponent<Image>();

        // ColorのAだけを変更し、他の色（RGB）は維持します
        Color newColor = img.color;
        newColor.a = 130f / 255f;
        img.color = newColor;
    }
}