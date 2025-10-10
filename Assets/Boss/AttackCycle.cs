using UnityEngine;
using System.Collections;

public class AttackCycle : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] attackPatterns; // Beam, Split, SineWave など
    [SerializeField] private float attackDuration = 5f; // 各攻撃の持続時間

    private int lastAttackIndex = -1;
    private MonoBehaviour currentAttack;

    private void Start()
    {
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (true)
        {
            // 次の攻撃をランダム選択（前回と同じは避ける）
            int index;
            do
            {
                index = Random.Range(0, attackPatterns.Length);
            } while (index == lastAttackIndex);

            lastAttackIndex = index;

            // 現在の攻撃を停止
            if (currentAttack != null)
                currentAttack.enabled = false;

            // 新しい攻撃を有効化
            currentAttack = attackPatterns[index];
            currentAttack.enabled = true;

            // 次の攻撃まで待機
            yield return new WaitForSeconds(attackDuration);
        }
    }
}
