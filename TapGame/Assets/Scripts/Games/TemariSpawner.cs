using UnityEngine;

/// <summary>
/// 手毬をランダムに生成するクラス
/// </summary>
public class TemariSpawner : MonoBehaviour
{
    // 生成する手毬のprefabを格納するリスト
    [SerializeField] private GameObject[] temariPrefabs;

    // 画面上部のUIとかぶらないように余白を作るための数値
    [SerializeField] private float uiPaddingTop = 2.0f;

    // 手毬の大きさ 画面の中に手毬が表示されるようにするため
    [SerializeField] private float temariRadius = 0.5f;

    // 生成間隔　あとから調整可能なように
    [SerializeField] private float spawnInterval = 1.5f;

    // 最初に生成されるまでの時間 あとから調整可能なように
    [SerializeField] private float startInterval = 1.0f;

    void Start()
    {
        // ゲーム開始時、startInterval秒待ってからspawnInterval秒間隔で生成をするため
        InvokeRepeating("SpawnTemari", startInterval, spawnInterval);
    }


    /// <summary>
    /// 指定した画面内に手毬をランダムに配置して生成する
    /// </summary>
    public void SpawnTemari()
    {
        // カメラの縦幅の半分（OrthographicSize）から、横幅を比率で算出
        float orthoSize = Camera.main.orthographicSize;
        float aspect = (float)Screen.width / Screen.height;
        float halfWidth = orthoSize * aspect;

        // 手毬が画面外にはみ出したり、UIに重なったりしないように
        // 指定された半径（temariRadius）とUI分の隙間（uiPaddingTop）を考慮して範囲を絞り込む

        float minX = -halfWidth + temariRadius;
        float maxX = halfWidth - temariRadius;
        float minY = -orthoSize + temariRadius;
        float maxY = orthoSize - uiPaddingTop - temariRadius;

        // ランダムな位置に手毬を生成
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // 複数の手毬タイプからランダムに選び決定した位置に生成（Instantiate）を実行
        int index = Random.Range(0, temariPrefabs.Length);
        Instantiate(temariPrefabs[index], new Vector3(randomX, randomY, 0), Quaternion.identity);
    }


    /// <summary>
    /// シーンビュー上で手毬の生成可能エリアを可視化するため
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // カメラが存在しない環境でのエラーを回避
        if (Camera.main == null) return;

        // 生成ロジックと計算基準を同期させるためにカメラの視界サイズから座標を算出
        // orthoSizeは縦幅の半分、aspectは縦横比を示す値
        float orthoSize = Camera.main.orthographicSize;
        float halfWidth = orthoSize * Camera.main.aspect;

        // 手毬が画面の端で切れたり、UIと重なったりしないよう
        // インスペクターで設定した半径と余白を考慮して境界線を計算
        float minX = -halfWidth + temariRadius;
        float maxX = halfWidth - temariRadius;
        float minY = -orthoSize + temariRadius;
        float maxY = orthoSize - uiPaddingTop - temariRadius;

        // シーンビュー上で手毬の出現位置を直感的に確認できるよう緑の枠線を描画
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minX, maxY, 0), new Vector3(maxX, maxY, 0)); // 上辺
        Gizmos.DrawLine(new Vector3(maxX, maxY, 0), new Vector3(maxX, minY, 0)); // 右辺
        Gizmos.DrawLine(new Vector3(maxX, minY, 0), new Vector3(minX, minY, 0)); // 下辺
        Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(minX, maxY, 0)); // 左辺
    }

}