using UnityEngine;

[CreateAssetMenu(fileName = "AttackConfig", menuName = "Boss/AttackConfig")]
public class AttackConfig : ScriptableObject
{
    [Header("BeamAttack設定")]
    public float beamFireInterval = 0.2f;
    public float beamBulletSpeed = 8f;
    public float beamFireYRange = 1f;

    [Header("SplitAttack設定")]
    public float splitFireInterval = 2f;

    [Header("ParentBullet設定")]
    public int parentChildCount = 4;
    public float parentChildSpreadAngle = 120f;
    public float parentSpeed = 4f;
    public float parentDecelerationRate = 2f;
    public float parentSplitTime = 3f;
    public float parentDestroyDelay = 0.5f;
    public AudioClip parentSplitSE;

    [Header("ChildBullet設定")]
    public float childSpeed = 6f;
    public float childLifetime = 3f;

    [Header("SineWaveAttack設定")]
    public float sineFireInterval = 0.2f;
    public float sineBulletSpeed = 5f;
    public float sineAmplitude = 1f;
    public float sineFrequency = 2f;
    public float sineFireRangeY = 2f;
    public float sineBulletLifetime = 10f;

}
