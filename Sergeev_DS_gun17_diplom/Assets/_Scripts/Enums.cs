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
        Crate
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
}
