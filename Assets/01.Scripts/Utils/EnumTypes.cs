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

        GameOver,
        Restart,
        ItemPickedUp,
        Home,

        ShowRecordingUI
    }

    public enum HitBodyPart
    {
        HitHead,
        HitThorax,
        HitArm,
        HitLeg
    }

    public class EnumTypes : MonoBehaviour { }
}