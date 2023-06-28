using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Metroidvania.BaseUnit;
using Metroidvania.Combat.Weapon;
using Metroidvania.Common.Rope;
using Metroidvania.Interfaces;
using Metroidvania.Managers;
using UnityEngine.SceneManagement;
using Zenject;
using Unit = Metroidvania.BaseUnit.Unit;

namespace Metroidvania.Player
{
    public class Player : MonoBehaviour, ISaveAndLoad
    {
        #region StateMachineVars

        private PlayerStateMachine StateMachine { get; set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerJumpState JumpState { get; private set; }
        public PlayerInAirState InAirState { get; private set; }
        public PlayerLandState LandState { get; private set; }
        public PlayerWallSlideState WallSlideState { get; private set; }
        public PlayerWallGrabState WallGrabState { get; private set; }
        public PlayerWallClimbState WallClimbState { get; private set; }
        public PlayerWallJumpState WallJumpState { get; private set; }
        public PlayerRopeGrabState RopeGrabState { get; private set; }
        public PlayerRopeClimbState RopeClimbState { get; private set; }
        public PlayerRopeSwingState RopeSwingState { get; private set; }
        public PlayerCrouchIdleState CrouchIdleState { get; private set; }
        public PlayerCrouchMoveState CrouchMoveState { get; private set; }
        public PlayerRollState RollState { get; private set; }
        public PlayerLedgeClimbState LedgeClimbState { get; private set; }
        public PlayerAttackState PrimaryAttackState { get; private set; }
        public PlayerAttackState SecondaryAttackState { get; private set; }
        public PlayerAimState AimState { get; private set; }

        #endregion

        #region Components

        public Unit Unit { get; private set; }
        public Animator Animator { get; private set; }
        public PlayerInputHandler InputHandler { get; private set; }
        private BoxCollider2D PlayerBodyCollider { get; set; }
        public Rigidbody2D Rb { get; private set; }
        [SerializeField] Vector2 _lastCheckpoint;
        [SerializeField] private PlayerData playerData;
        [Inject] public AudioManager audioManager;

        public bool IsTouchingRope
        {
            get => isTouchingRope;
            private set => isTouchingRope = value;
        }

        public Transform CurrentRope { get; private set; }
        public bool AlreadyOnRope { get; set; }
        [SerializeField] private bool isTouchingRope;
        private float _startCollisionTime;
        private List<RopeLinks> _ropeLinksCollisions;
        private List<IInteractable> _interactables;
        private PlayerInventory Inventory { get; set; }

        public WeaponType CurrentWeaponEquip
        {
            get => _currentWeaponEquip;
            private set => _currentWeaponEquip = value;
        }

        private WeaponType _currentWeaponEquip;
        private Vector2 _workVector;
        public event Action Aiming;
        public event Action EndAiming;

        #endregion

        #region Unity Func

        private void Awake()
        {
            Unit = GetComponentInChildren<Unit>();
            Inventory = GetComponent<PlayerInventory>();
            Animator = GetComponent<Animator>();
            InputHandler = GetComponent<PlayerInputHandler>();
            Rb = GetComponent<Rigidbody2D>();
            PlayerBodyCollider = GetComponentInChildren<BoxCollider2D>();
            StateMachine = new PlayerStateMachine();
            IdleState = new PlayerIdleState(this, StateMachine, playerData, "idle");
            MoveState = new PlayerMoveState(this, StateMachine, playerData, "move");
            JumpState = new PlayerJumpState(this, StateMachine, playerData, "inAir");
            InAirState = new PlayerInAirState(this, StateMachine, playerData, "inAir");
            LandState = new PlayerLandState(this, StateMachine, playerData, "land");
            WallSlideState = new PlayerWallSlideState(this, StateMachine, playerData, "wallSlide");
            WallGrabState = new PlayerWallGrabState(this, StateMachine, playerData, "wallGrab");
            WallClimbState = new PlayerWallClimbState(this, StateMachine, playerData, "wallClimb");
            WallJumpState = new PlayerWallJumpState(this, StateMachine, playerData, "inAir");
            RopeClimbState = new PlayerRopeClimbState(this, StateMachine, playerData, "ropeClimb");
            RopeGrabState = new PlayerRopeGrabState(this, StateMachine, playerData, "ropeGrab");
            RopeSwingState = new PlayerRopeSwingState(this, StateMachine, playerData, "ropeGrab");
            CrouchIdleState = new PlayerCrouchIdleState(this, StateMachine, playerData, "crouchIdle");
            CrouchMoveState = new PlayerCrouchMoveState(this, StateMachine, playerData, "crouchMove");
            RollState = new PlayerRollState(this, StateMachine, playerData, "roll");
            LedgeClimbState = new PlayerLedgeClimbState(this, StateMachine, playerData, "ledgeClimbState");
            PrimaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
            SecondaryAttackState = new PlayerAttackState(this, StateMachine, playerData, "attack");
            AimState = new PlayerAimState(this, StateMachine, playerData, "aim");
        }

        private void Start()
        {
            Inventory.OnAppliedBuff += ApplyBuff;
            _ropeLinksCollisions = new List<RopeLinks>();
            _interactables = new List<IInteractable>();
            PrimaryAttackState.SetWeapon(Inventory.weapons[0]);
            AimState.SetWeapon(Inventory.weapons[0]);
            StateMachine.Initialize(IdleState);
            _currentWeaponEquip = WeaponType.Sword;
        }

        private void ApplyBuff(BuffType buffType, int modifier)
        {
            var unitStats = Unit.GetUnitComponent<UnitStats>();
            switch (buffType)
            {
                case BuffType.Strength:
                    unitStats.strength.AddModifier(modifier);
                    break;
                case BuffType.Agility:
                    unitStats.agility.AddModifier(modifier);
                    break;
                case BuffType.Vitality:
                    unitStats.vitality.AddModifier(modifier);
                    unitStats.GetMaxHealthValue();
                    unitStats.RestoreHealth();
                    break;
                case BuffType.Armor:
                    unitStats.armor.AddModifier(modifier);
                    break;
                default:
                    break;
            }
        }

        private void RemoveBuff(BuffType buffType, int modifier)
        {
            var unitStats = Unit.GetUnitComponent<UnitStats>();
            switch (buffType)
            {
                case BuffType.Strength:
                    unitStats.strength.RemoveModifier(modifier);
                    break;
                case BuffType.Agility:
                    unitStats.agility.RemoveModifier(modifier);
                    break;
                case BuffType.Vitality:
                    unitStats.vitality.RemoveModifier(modifier);
                    unitStats.GetMaxHealthValue();
                    unitStats.RestoreHealth();
                    break;
                case BuffType.Armor:
                    unitStats.armor.RemoveModifier(modifier);
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            Unit.LogicUpdate();
            StateMachine.CurrentState.LogicUpdate();
            if (InputHandler.ChangeWeaponInput) ChangeWeapon();
        }

        private void FixedUpdate()
        {
            StateMachine.CurrentState.PhysicsUpdate();
        }

        #endregion

        private void ChangeWeapon()
        {
            var bowLayer = Animator.GetLayerIndex("Bow Layer");
            if (Animator.GetLayerWeight(bowLayer) == 1)
            {
                Animator.SetLayerWeight(bowLayer, 0f);
                PrimaryAttackState.SetWeapon(Inventory.weapons[0]);
                AimState.SetWeapon(Inventory.weapons[0]);
                _currentWeaponEquip = WeaponType.Sword;
            }
            else
            {
                Animator.SetLayerWeight(bowLayer, 1f);
                PrimaryAttackState.SetWeapon(Inventory.weapons[1]);
                AimState.SetWeapon(Inventory.weapons[1]);
                _currentWeaponEquip = WeaponType.Bow;
            }

            InputHandler.UseChangeWeaponInput();
        }

        #region Other Func

        private void AnimationTrigger() => StateMachine.CurrentState.AnimationTrigger();
        private void AnimationEndTrigger() => StateMachine.CurrentState.AnimationEndTrigger();

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.GetComponent<RopeLinks>() != null)
            {
                _ropeLinksCollisions.Add(collision.GetComponent<RopeLinks>());
                CurrentRope = collision.transform;
                isTouchingRope = true;
            }
            else if (collision.GetComponent<IPickupable>() != null)
            {
                collision.GetComponent<IPickupable>().Pickup();
            }
            else if (collision.GetComponent<IInteractable>() != null)
            {
                _interactables.Add(collision.GetComponent<IInteractable>());
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.GetComponent<RopeLinks>() != null)
            {
                _ropeLinksCollisions.Remove(collision.GetComponent<RopeLinks>());
                if (_ropeLinksCollisions.Count == 0)
                {
                    isTouchingRope = false;
                }
            }
            else if (collision.GetComponent<IInteractable>() != null)
            {
                _interactables.Remove(collision.GetComponent<IInteractable>());
            }
        }

        public bool CanShoot()
        {
            return Inventory.CanShootArrow();
        }

        public void PlayerShot()
        {
            Inventory.DecreaseAmmo();
        }

        public void SetColliderHeight(float height)
        {
            var center = PlayerBodyCollider.offset;
            _workVector.Set(PlayerBodyCollider.size.x, height);
            center.y += (height - PlayerBodyCollider.size.y) / 2;
            PlayerBodyCollider.size = _workVector;
            PlayerBodyCollider.offset = center;
        }

        public void PlayerMoveRope()
        {
            transform.SetParent(isTouchingRope ? CurrentRope.transform : null);
        }

        public void SetPlayerLayer(LayerMask layer) => gameObject.layer = (int)Mathf.Log(layer.value, 2);

        #endregion

        private void SetNewAmmo(GameObject ammo)
        {
            var rangedWeapon = Inventory.weapons[1] as RangedWeapon;
            if (rangedWeapon != null) rangedWeapon.SetNewAmmo(ammo);
        }

        private void OnSwitchAmmo()
        {
            Inventory.SwitchAmmo();
        }

        private void OnPotionUse(PotionSlotNumber number)
        {
            Inventory.UsePotionInSlot(number);
            Inventory.DecreasePotion(number);
        }

        public void OnAiming()
        {
            Aiming?.Invoke();
        }

        public void OnEndAiming()
        {
            EndAiming?.Invoke();
        }

        public void SetLastSpawnPoint(Vector2 position)
        {
            _lastCheckpoint = position;
        }

        private void OnHealthUsedFromSlot(int healAmount)
        {
            Unit.GetUnitComponent<UnitStats>().IncreaseHealth(healAmount);
        }

        private void SpawnPlayer()
        {
            if (_lastCheckpoint is { x: 0, y: 0 }) return;
            transform.position = _lastCheckpoint;
        }

        public void SpawnPlayer(bool isHealed)
        {
            if (isHealed)
                Unit.GetUnitComponent<UnitStats>().RestoreHealth();
            SpawnPlayer();
        }

        private void SpawnPlayer(Vector2 position)
        {
            transform.position = position;
        }

        private void OnInteract()
        {
            foreach (var interactable in _interactables)
            {
                interactable.Interact();
            }
        }

        public void AddDoubleJump()
        {
            playerData.jumpCount = 2;
        }

        private void OnEnable()
        {
            Inventory.OnAppliedBuff += ApplyBuff;
            Inventory.SetNewAmmo += SetNewAmmo;
            InputHandler.SwitchedAmmo += OnSwitchAmmo;
            InputHandler.UsedPotion += OnPotionUse;
            Inventory.HealthPotionUsed += OnHealthUsedFromSlot;
            InputHandler.PressedInteract += OnInteract;
        }

        private void OnDisable()
        {
            Inventory.OnAppliedBuff -= ApplyBuff;
            Inventory.SetNewAmmo -= SetNewAmmo;
            InputHandler.SwitchedAmmo -= OnSwitchAmmo;
            InputHandler.UsedPotion -= OnPotionUse;
            Inventory.HealthPotionUsed -= OnHealthUsedFromSlot;
            InputHandler.PressedInteract -= OnInteract;
        }

        public void LoadData(GameData.GameData gameData)
        {
            var playerStats = Unit.GetUnitComponent<UnitStats>();
            playerStats.SetCurrentHealth(gameData.playerHealth);
            playerData.jumpCount = gameData.jumpCount;
            var currentScene = SceneManager.GetActiveScene().name;
            foreach (var pair in gameData.xPlayerPosition.Where(pair => pair.Key == currentScene))
            {
                _lastCheckpoint.x = pair.Value;
            }

            foreach (var pair in gameData.yPlayerPosition.Where(pair => pair.Key == currentScene))
            {
                _lastCheckpoint.y = pair.Value;
            }

            SpawnPlayer();
        }

        public void SaveData(ref GameData.GameData gameData)
        {
            var playerStats = Unit.GetUnitComponent<UnitStats>();
            gameData.playerHealth = playerStats.GetCurrentHealth();
            gameData.jumpCount = playerData.jumpCount;
            var currentScene = SceneManager.GetActiveScene().name;
            if (gameData.xPlayerPosition.TryGetValue(currentScene, out var value1))
            {
                gameData.xPlayerPosition[currentScene] = _lastCheckpoint.x;
            }
            else
            {
                gameData.xPlayerPosition.Add($"{currentScene}", _lastCheckpoint.x);
            }

            if (gameData.yPlayerPosition.TryGetValue(currentScene, out var value2))
            {
                gameData.yPlayerPosition[currentScene] = _lastCheckpoint.y;
            }
            else
            {
                gameData.yPlayerPosition.Add($"{currentScene}", _lastCheckpoint.y);
            }
        }
    }
}