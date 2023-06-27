namespace Metroidvania
{
    public enum CombatInputs: byte
    {
        Primary,
        Secondary
    }
    public enum WeaponType: byte
    {
        Sword,
        Bow
    }
    public enum ArrowType: byte
    {
        Normal,
        Rope
    }

    public enum LootType : byte
    {
        Boar,
        Bat,
        Spider,
        Slime,
        Skeleton,
        SkeletonArcher,
        Boss,
        Crate,
        Chest,
        HealBuffChest,
        DamageBuffChest,
        SpecialTutorialChest,
        MaterialChest,
        HealPotionChest,
        ArrowChest
    }

    public enum ItemType : byte
    {
        Material,
        Buff,
        Ammo,
        Potion
    }

    public enum BuffType : byte
    {
        Strength,
        Agility,
        Vitality,
        Armor
    }

    public enum PotionType : byte
    {
        Health,
        Buff
    }

    public enum StatType : byte
    {
        Strength,
        Agility,
        Vitality,
        Armor,
        Evasion,
        CritChance,
        CritPower,
        Health,
        CurrentHealth
    }

    public enum PotionSlotNumber : byte
    {
        First,
        Second,
        Third,
        Fourth
    }

    public enum SFXSlots : byte
    {
        SwordAttackMetal,
        SwordAttackFlesh,
        SwordAttackWind,
        CampfireBurningVariant1,
        CampfireBurningVariant2,
        Click,
        DeathScreen,
        EvilVoice,
        FootStep,
        ItemPickup,
        MonsterBreathing,
        MonsterGrowl1,
        MonsterGrowl2,
        OpenChest,
        Key,
        SkeletonBones,
        SwordThrow,
        SwordThrow2,
        ThunderStrike,
        WindSound,
        WomanSigh,
        FootstepEnemy
    }

    public enum BGMSlots : byte
    {
        EpicMusic,
        CastleMusic,
        EpicMusic2,
        BossBattle,
        SlowMusic1,
        SlowMusic2
    }
    
    public enum TeleportVariant : byte
    {
        Above,
        Left,
        Right,
        Behind
    }
}
