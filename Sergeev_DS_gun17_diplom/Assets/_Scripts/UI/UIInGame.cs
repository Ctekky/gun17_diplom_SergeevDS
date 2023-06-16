using System;
using Metroidvania.BaseUnit;
using Metroidvania.Common.Items;
using Metroidvania.Player;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Metroidvania.UI
{
    public class UIInGame : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private UIItemSlot arrowSlot;
        [SerializeField] private UIItemSlot potion1;
        [SerializeField] private UIItemSlot potion2;
        [SerializeField] private UIItemSlot potion3;
        [SerializeField] private UIItemSlot potion4;
        [Inject] private Player.Player _player;
        private UnitStats _playerStats;
        private float _sliderWidth;
        private float _sliderCurrentWidth;

        private void Awake()
        {
            _playerStats = _player.Unit.GetUnitComponent<UnitStats>();
        }

        private void OnEnable()
        {
            _playerStats.onHealthChange += OnHealthChange;
            _player.GetComponent<PlayerInventory>().SetNewAmmoIcon += ChangeAmmoIcon;
            _player.GetComponent<PlayerInventory>().SetEmptyAmmo += CleanUpAmmoSlot;
            _player.GetComponent<PlayerInventory>().SetNewPotionIcon += ChangePotionIcon;
            _player.GetComponent<PlayerInventory>().SetEmptyPotion += CleanPotionIcon;
        }
        private void OnDisable()
        {
            _playerStats.onHealthChange -= OnHealthChange;
            _player.GetComponent<PlayerInventory>().SetNewAmmoIcon -= ChangeAmmoIcon;
            _player.GetComponent<PlayerInventory>().SetEmptyAmmo -= CleanUpAmmoSlot;
            _player.GetComponent<PlayerInventory>().SetNewPotionIcon -= ChangePotionIcon;
            _player.GetComponent<PlayerInventory>().SetEmptyPotion -= CleanPotionIcon;
        }
        private void ChangeAmmoIcon(InventoryItem item)
        {
            ChangeSlotIcon(item, arrowSlot);
        }
        private void CleanUpAmmoSlot()
        {
            CleanSlotIcon(arrowSlot);
        }
        private void ChangePotionIcon(InventoryItem item, PotionSlotNumber number)
        {
            switch (number)
            {
                case PotionSlotNumber.First:
                    ChangeSlotIcon(item, potion1);
                    break;
                case PotionSlotNumber.Second:
                    ChangeSlotIcon(item, potion2);
                    break;
                case PotionSlotNumber.Third:
                    ChangeSlotIcon(item, potion3);
                    break;
                case PotionSlotNumber.Fourth:
                    ChangeSlotIcon(item, potion4);
                    break;
                default:
                    break;
            }
        }
        private void CleanPotionIcon(PotionSlotNumber number)
        {
            switch (number)
            {
                case PotionSlotNumber.First:
                    CleanSlotIcon(potion1);
                    break;
                case PotionSlotNumber.Second:
                    CleanSlotIcon(potion2);
                    break;
                case PotionSlotNumber.Third:
                    CleanSlotIcon(potion3);
                    break;
                case PotionSlotNumber.Fourth:
                    CleanSlotIcon(potion4);
                    break;
                default:
                    break;
            }
        }
        private void ChangeSlotIcon(InventoryItem item, UIItemSlot slot)
        {
            slot.UpdateSlot(item);
        }

        private void CleanSlotIcon(UIItemSlot slot)
        {
            slot.CleanUpSlot();
        }
        private void OnHealthChange(int health)
        {
            slider.maxValue = _playerStats.GetMaxHealthValue();
            slider.value = health;
        }
    }
}