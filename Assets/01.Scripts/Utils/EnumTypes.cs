using UnityEngine;

namespace EnumTypes
{
    public enum LookDirection { Straight, Up, Down }
    public enum BodyPosture { Stand, Running, InTheAir, Crouch }
    public enum Layers
    {
        Default,
        TransparentFX,
        IgnoreRaycast,
        Reserved1,
        Water,
        UI,
        Reserved2,
        Reserved3,
        Player,
        Enemy,
        World,
        Building
    }

    public enum LauncherType
    {
        Player,
        Enemy
    };

    public enum ThrowableType
    {
        Grenade,
        BossBomb,
        BossHeavyBomb,
        EnemyGrenade,
    };

    public enum GlobalEvents
    {
        PlayerDead,
        PlayerSpawned,
        PlayerInactive,
        PlayerDamaged,

        BossSpawn,
        BossDead,
        SoldierDead,

        PointsEarned,
        KnifeUsed,
        GunUsed,
        GrenadeUsed,

        MissionStart,
        MissionEnd,
        MissionSuccess,

        GameOver,
        Restart,
        ItemPickedUp,
        Home,

        ShowRecordingUI
    }

    public class EnumTypes : MonoBehaviour { }
}