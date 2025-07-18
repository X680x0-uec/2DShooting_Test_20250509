using UnityEngine;
using System.Collections; // コルーチンを使用するために必要な名前空間

public class EnemyController : MonoBehaviour
{
    [SerializeField] GameObject hammer; // ハンマーのGameObjectを参照するための変数
    [SerializeField] private float throwIntervalSec = 1; // ハンマーを投げる間隔（秒）
    [SerializeField] private float throwForce = 10; // ハンマーを投げる力
    [SerializeField] private float throwAngleDeg = 120; // ハンマーを投げる角度（度）
    [SerializeField] private float hammerSize = 0.5f; // ハンマーのサイズ
    [SerializeField] private float destroyDelaySec = 0.03f; // ハンマーが着弾してから破壊されるまでの遅延時間（秒）

    void Start()
    {
        // ハンマーを投げるコルーチンを開始
        if (hammer == null) // hammerが設定されていない場合
        {
            Debug.LogError("ハンマーのプレハブが設定されていません。Inspectorで設定してください。");
            return; // コルーチンを開始しない
        }
        if (throwIntervalSec <= 0) // throwIntervalSecが0以下の場合
        {
            Debug.LogError("ハンマーを投げる間隔は0より大きい値に設定してください。");
            return; // コルーチンを開始しない
        }
        StartCoroutine(ThrowHammerCoroutine());
    }

    IEnumerator ThrowHammerCoroutine()
    {
        while (true) // 無限ループ
        {
            
            // ハンマーを投げる力を計算
            float throwAngleRad = throwAngleDeg * Mathf.Deg2Rad; // 角度をラジアンに変換
            Vector3 throwDirection = new Vector3(Mathf.Cos(throwAngleRad), Mathf.Sin(throwAngleRad),0); // 投げる方向を計算

            // ハンマーを生成 Instantiateは指定したプレハブをシーンに生成するためのメソッドで、第一引数に生成するプレハブ、第二引数に生成位置、第三引数に生成時の回転(クオータニオン)を指定します。今回は第二引数をtransform.position、第三引数をQuaternion.identityに設定して、敵の位置に回転なし(0,0,0)で生成します。生成したオブジェクトへの参照がhammerInstanceに格納されます。
            GameObject hammerInstance = Instantiate(hammer, transform.position, Quaternion.identity);
            // 生成したハンマーのRigidbody2Dコンポーネントを取得
            Rigidbody2D rb = hammerInstance.GetComponent<Rigidbody2D>();
            if (rb != null) // hammerにRigidbody2Dコンポーネントが存在する場合
            {
                // ハンマーに力を加える
                rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
            }
            else // hammerにRigidbody2Dコンポーネントが存在しない場合
            {
                Debug.LogError("ハンマーに Rigidbody2D コンポーネントが見つかりません。");
            }

            // ハンマーのサイズを設定
            hammerInstance.transform.localScale = new Vector3(hammerSize, hammerSize, 1);

            // ハンマーについているHammerControllerコンポーネントを取得
            HammerController hc = hammerInstance.GetComponent<HammerController>();
            if (hc != null) // hammerにHammerControllerコンポーネントが存在する場合
            {
                hc.delaySec = destroyDelaySec; // ハンマーの破壊までの遅延時間を設定
            }
            else // hammerにHammerControllerコンポーネントが存在しない場合
            {
                Debug.LogError("ハンマーに HammerController コンポーネントが見つかりません。");
            }

            // 指定した間隔だけ待機
            yield return new WaitForSeconds(throwIntervalSec);
        }
    }
}
