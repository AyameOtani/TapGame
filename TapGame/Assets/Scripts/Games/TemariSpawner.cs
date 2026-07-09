using UnityEngine;

/// <summary>
/// 手毬をランダムに生成するクラス
/// </summary>
public class TemariSpawner : MonoBehaviour
{
    // 生成する手毬のprefabを格納するリスト
    [SerializeField] private GameObject[] temariPrefabs;

    // 手毬の大きさ 画面の中に手毬が表示されるようにするため
    [SerializeField] private float temariRadius = 0.5f;

    // 最初に生成されるまでの時間 あとから調整可能なように
    [SerializeField] private float startInterval = 1.0f;

    // 画面の比率が変わってもboardを基準に余白をつくるため
    [SerializeField] private RectTransform boardRect;

    // ゲーム開始時の生成感覚
    [SerializeField] private float initialSpawnInterval = 1.5f;

    // ゲーム終了時の生成感覚
    [SerializeField] private float minSpawninterval = 0.2f;

    // 新しく生成される手毬を現在の手毬よりも手前に表示させるための描画順序管理用カウンタ
    private static int currentSortingOrder = 0;


    // 他のクラスからCanvasを取得するための窓口
    public static TemariSpawner Instance { get; private set; }
    // UIの親となるCanvasを指定
    [SerializeField] private Transform canvasTransform;

    /// <summary>
    /// キャンパスの大きさを外部から取得するための処理
    /// </summary>
    /// <returns></returns>
    public Transform GetCanvasTransform()
    {
        return canvasTransform;
    }


    // 生成タイミングを制御するための残り時間
    private float timer = 0.0f;

    /// <summary>
    /// ゲーム開始時に初期待ち時間をタイマーに適用する
    /// </summary>
    void Start()
    {
        timer = startInterval;
    }

    void Awake()
    {
        // 自分のインスタンスを保存
        Instance = this;
    }

    /// <summary>
    /// 毎フレーム更新してタイマーが0になるごとに手毬を生成する
    /// </summary>
    void Update()
    {
        if (CountdownManager.Instance != null && CountdownManager.Instance.IsCountingDown)
        {
            return;
        }

        // フェード中はゲーム進行を止めるため生成処理も無効化
        if (GameTimer.IsFading) return;

        // 次の生成までの時間をカウントダウンする
        timer -= Time.deltaTime;

        // タイマーが0以下になったら生成を行い
        // 次の生成間隔を計算してタイマーを再設定する
        if (timer <= 0)
        {
            SpawnTemari();

            // ゲーム終了に近づくにつれて生成間隔を短くして難易度を段階的に上げる
            float progress = 1.0f - (GameTimer.RemainingTime / GameTimer.TotalTime);
            float currentInterval = Mathf.Lerp(initialSpawnInterval, minSpawninterval, progress);

            timer = currentInterval;
        }
    }


    /// <summary>
    /// 指定した画面内に手毬をランダムに配置して生成する
    /// </summary>
    public void SpawnTemari()
    {
        // カメラの縦幅の半分から横幅を比率で算出
        float orthoSize = Camera.main.orthographicSize;
        float aspect = (float)Screen.width / Screen.height;
        float halfWidth = orthoSize * aspect;

         // Boardの下端を計算する関数
        float boardBottom = GetBoardBottomWorldY();

        // 手毬が画面外にはみ出したり、UIに重なったりしないように
        // 指定された半径（temariRadius）とUI分の隙間（uiPaddingTop）を考慮して範囲を絞り込む
        float minX = -halfWidth + temariRadius;
        float maxX = halfWidth - temariRadius;
        float minY = -orthoSize + temariRadius;
        float maxY = boardBottom - temariRadius;

        // ランダムな位置に手毬を生成
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        // 複数の手毬タイプからランダムに選び決定した位置に生成（Instantiate）を実行
        int index = Random.Range(0, temariPrefabs.Length);

        // 回転,位置,タイプをランダムで生成している
        float randomAngle = Random.Range(0.0f, 360.0f);
        Quaternion randomRotation = Quaternion.Euler(0, 0, randomAngle);

        // 手毬を生成
        GameObject obj = Instantiate(temariPrefabs[index],
            new Vector3(randomX, randomY, 0),randomRotation);

        // 後から生成した手毬ほど手前に表示する
        if (obj.TryGetComponent<SpriteRenderer>(out var sr))
        {
            sr.sortingOrder = currentSortingOrder++;
        }
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

        // Boardの下端を計算する関数
        float boardBottom = GetBoardBottomWorldY();

        // 手毬が画面の端で切れたりUIと重なったりしないよう
        // インスペクターで設定した半径と余白を考慮して境界線を計算
        float minX = -halfWidth + temariRadius;
        float maxX = halfWidth - temariRadius;
        float minY = -orthoSize + temariRadius;
        float maxY = boardBottom - temariRadius;

        // シーンビュー上で手毬の出現位置を直感的に確認できるよう緑の枠線を描画
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(minX, maxY, 0), new Vector3(maxX, maxY, 0)); // 上辺
        Gizmos.DrawLine(new Vector3(maxX, maxY, 0), new Vector3(maxX, minY, 0)); // 右辺
        Gizmos.DrawLine(new Vector3(maxX, minY, 0), new Vector3(minX, minY, 0)); // 下辺
        Gizmos.DrawLine(new Vector3(minX, minY, 0), new Vector3(minX, maxY, 0)); // 左辺
    }


    /// <summary>
    /// boardよりも下に手毬を表示させるための関数
    /// </summary>
    /// <returns></returns>
    private float GetBoardBottomWorldY()
    {
        if (boardRect == null) return Camera.main.orthographicSize;

        // Boardの下端のスクリーン 座標を計算
        // Pivotが(0.5, 0.5)ならY座標から高さの半分を引く
        Vector3 bottomPosition = boardRect.position;
        bottomPosition.y -= (boardRect.rect.height * boardRect.lossyScale.y / 2f);

        // スクリーン座標をワールド座標に変換
        // Z軸にカメラとの距離を考慮する必要があるため、カメラのNearClipPlaneなどを利用
        Vector3 worldBottom = Camera.main.ScreenToWorldPoint(new Vector3(0, bottomPosition.y, -Camera.main.transform.position.z));

        return worldBottom.y;
    }
}