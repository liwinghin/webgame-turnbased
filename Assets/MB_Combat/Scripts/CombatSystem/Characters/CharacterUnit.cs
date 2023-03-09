using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using UnityEngine.Events;
using System.Reflection;

namespace MB.Game
{
    [Serializable]
    public class CharacterAbility
    {
        public string unitTeam = string.Empty;
        public string unitName = string.Empty;
        public int tier = 0;
        public int maxHp = 0;
        public int currentHp = 0;
        public int turnSpeed = 0;
        public int energy = 0;
        public int currentEnergy = 0;
        public int luck = 0;
        public int missChance = 0;
        public int criticalChance = 0;
        public int energyRecover = 0;
    }

    [Serializable]
    public class CharacterUnit : MonoBehaviour
    {
        private ActionRunManager actionRunManager
        {
            get { return ActionRunManager.Instance; }
        }
        private CameraManager cameraManager
        {
            get { return CameraManager.Instance; }
        }
        private VFXManager vfxManager
        {
            get { return VFXManager.Instance; }
        }

        [Header("Character Ability")]
        public BaseFormationHandler mineFormation = null;
        public CharRuntime charAnimator = null;
        public CharacterAbility characterAbility;

        [Header("Action")]
        [SerializeField] private int action = 0;
        public List<ActionHandler> actionSet = new List<ActionHandler>();

        [Header("Ship")]
        public ActionHandler shipAttackInfo;
        public bool IsDoingAction { get; private set; }
        public bool IsAttacking { get; private set; }

        [Header("UI")]
        public Transform uiContainer = null;
        public GameObject damageText = null;
        public UICharacterHealthBar uICharacterHealthBars = null;
        public string iconURL = string.Empty;
        public Texture2D charIcon = null;
        private GameObject canvas = null;

        [Header("Dynamic Setting")]
        public Transform missPos = null;
        public float missTime = 0.75f;
        public float movingSpeed = 1f;
        public float jumpSpeed = 0.75f;

        public UnityEvent<AnimationType, Transform> onGunAttack = new UnityEvent<AnimationType, Transform>();
        public UnityEvent<CharOnceMotion, Transform> onSwordAttack = new UnityEvent<CharOnceMotion, Transform>();


        #region Character Ability
        public void ApplyInitValue(string team, CharacterCombat data, CharRuntime animator)
        {
            characterAbility.unitTeam = team;
            characterAbility.unitName = data.name;
            characterAbility.tier = data.tier;
            characterAbility.maxHp = data.hp;
            characterAbility.currentHp = characterAbility.maxHp;
            characterAbility.energy = data.energy;
            characterAbility.currentEnergy = 0;
            characterAbility.turnSpeed = data.turnSpeed;
            characterAbility.missChance = data.missChance;
            characterAbility.criticalChance = data.criticalChance;
            characterAbility.energyRecover = data.energyRecover;
            characterAbility.luck = data.luck;
            charAnimator = animator;
            iconURL = data.image;
            canvas = GameObject.Find("Canvas");
            SetUpGun();
            SetUpSword();
            shipAttackInfo = actionRunManager.SetActionHandler("SHIP_ATTACK", mineFormation.shipInfo.cannonDamage, 0);
            actionSet.Add(actionRunManager.SetActionHandler("NORMAL_ATTACK", data.normalDamage, 0));
            SetUpActionSet(data.specials);
        }

        public void SetUpActionSet(List<SkillCombat> specials)
        {
            for (int i = 0; i < specials.Count; i++)
            {
                actionSet.Add(actionRunManager.SetActionHandler(specials[i].name, specials[i].damage, specials[i].energy));
            }
        }

        private void SetUpSword()
        {
            //Right Hand For Sword
            for (int i = 0; i < charAnimator.nowRightHandItems.Count; i++)
            {
                CharItem charItem = charAnimator.nowRightHandItems[i];
                Sword sword = charItem.transform.GetComponent<Sword>();
                if (sword != null) sword.OnSetUp(this);
            }
        }

        private void SetUpGun()
        {
            //Left Hand For Gun
            for (int i = 0; i < charAnimator.nowLeftHandItems.Count; i++)
            {
                CharItem charItem = charAnimator.nowLeftHandItems[i];
                Gun gun = charItem.transform.GetComponent<Gun>();
                if (gun != null) gun.OnSetUp(this);
            }
        }

        public void UpdateCharacterBars(bool init)
        {
            if (!uICharacterHealthBars.isActiveAndEnabled) { uICharacterHealthBars.SetActive(true); }
            uICharacterHealthBars.UpdateCharacterBar(characterAbility.currentHp, characterAbility.maxHp, characterAbility.currentEnergy, characterAbility.energy, init);
        }
        #endregion

        #region Character Status
        public IEnumerator GetCharacterIcon(Action<Texture2D> callback)
        {
            if (charIcon == null)
            {
                yield return AssetUtils.LoadTexture(iconURL, false, (texture) => {
                    charIcon = texture;
                });
            }
            callback(charIcon);
        }

        public bool isAlive
        {
            get => (characterAbility.currentHp > 0) ? true : false;
        }

        public void EnergyRecovery()
        {
            characterAbility.currentEnergy = (characterAbility.currentEnergy < characterAbility.energy) ? characterAbility.currentEnergy += characterAbility.energyRecover : characterAbility.energy;
            UpdateCharacterBars(false);
        }
        #endregion

        #region Action Logic
        //For UI Button
        public void SetCurrentAction(int Action)
        {
            action = Action;
        }

        public ActionType currentActionType
        {
            get => actionSet[action].actionData.actionType;
        }

        public bool CheckActionSetCount(int index)
        {
            return (index >= actionSet.Count) ? false : true;
        }

        public bool ActionAvailable()
        {
            if (characterAbility.currentEnergy >= actionSet[action].actionEnergy)
            {
                return true;
            }
            else
                return false;
        }

        public bool ActionAvailable(int index)
        {
            if (index >= actionSet.Count) return false;
            bool available = (characterAbility.currentEnergy >= actionSet[index].actionEnergy) ? true : false;
            return available;
        }

        public void DoShipAttack(List<CharacterUnit> targetUnits, bool hit, bool critical, Transform vfxPos)
        {
            if (IsDoingAction)
                return;

            StartCoroutine(DoShipAction(targetUnits, hit, critical, vfxPos));
        }

        public void DoAction(CharacterUnit target, bool hit, bool critical)
        {
            DoAction(action, target, hit, critical);
        }

        public void DoAction(int Action, CharacterUnit target, bool hit, bool critical)
        {
            //set action
            action = Action;
            //if attack
            if (IsDoingAction)
                return;

            print("<" + characterAbility.unitName + ">" + ", Target Type: <" + actionSet[action].actionData.targetType + ">" + ", Attack Type: <" + actionSet[action].actionData.animationType + ">" +
                    ", Damage: <" + actionSet[action].actionValues + ">" + ", Hit: <" + hit + ">" + ", Critical: <" + critical + ">");

            switch (actionSet[action].actionData.targetType)
            {
                case TargetType.SingleAttack:
                    StartCoroutine(DoSingleAttackAction(action, target, hit, critical));
                    break;
                case TargetType.GroupAttack:
                    BaseFormationHandler targetFormation = mineFormation.targetFormationHandler;
                    StartCoroutine(DoGroupAttackAction(action, targetFormation, target, hit, critical));
                    break;
                case TargetType.SingleHeal:
                    break;
                case TargetType.GroupHeal:
                    break;
            }
        }
        #endregion

        #region Battle Logic
        public void Damaged(int dmg, bool hit, bool critical, string vfxName)
        {
            if (hit)
            {
                SetCharacterOnceAnimation(CharOnceMotion.GetDamage);
                cameraManager.ShakeCamera();
                SetParticleEffect(vfxName, charAnimator.nowBody.centerAnchor); 

                if (critical)
                {
                    dmg = dmg + dmg / 2;
                }
                characterAbility.currentHp -= dmg;

                if (characterAbility.currentHp <= 0)
                {
                    SetCharacterLoopAnimation(CharLoopMotion.Die);
                }
            }
            else
            {
                //Missed
                SetCharacterOnceAnimation(CharOnceMotion.JumpRoot);
                StartCoroutine(missAttackMotion());
            }
            SetFloatingDamageText(hit, dmg, charAnimator.nowBody.centerAnchor);
        }

        private IEnumerator DoShipAction(List<CharacterUnit> targetUnits, bool hit, bool critical, Transform vfxPos)
        {
            IsDoingAction = true;

            AudioManager.PlaySFX(shipAttackInfo.actionData.audioType);
            SetParticleEffect(shipAttackInfo.actionData.attackVFX, vfxPos);

            foreach (CharacterUnit c in targetUnits)
            {
                if (c.isAlive)
                {
                    c.Damaged(shipAttackInfo.actionValues, hit, critical, shipAttackInfo.actionData.gotDamagedVFX);
                    c.UpdateCharacterBars(false);
                }
            }
            yield return new WaitForSeconds(2f);
            IsDoingAction = false;
        }

        #region Group Attack Logic
        private IEnumerator DoGroupAttackAction(int action, BaseFormationHandler targetFormation, CharacterUnit target, bool hit, bool critical)
        {
            SetAttackCamera();
            IsDoingAction = true;
            Quaternion startDir = transform.rotation;

            Coroutine actionCoroutine = (actionSet[action].actionData.animationType == AnimationType.Sword) ?
                StartCoroutine(GroupSwordAttack(targetFormation, target, hit, critical)) : StartCoroutine(GroupGunAttack(targetFormation, target, hit, critical));
            yield return actionCoroutine;

            transform.rotation = startDir;
            yield return new WaitForSeconds(1f);
            IsDoingAction = false;
        }
        private IEnumerator GroupGunAttack(BaseFormationHandler targetUnits, CharacterUnit target, bool hit, bool critical)
        {
            //Attack Animation
            SetCharacterOnceAnimation(actionSet[action].actionData.actionAnimation);
            onGunAttack.Invoke(actionSet[action].actionData.animationType, target.transform);
            //WaitUntilAnimationFinished and Delay
            yield return StartSFX(actionSet[action].actionData.targetType, actionSet[action].actionData.actionAnimation, hit);
            yield return new WaitForSeconds(0.1f);
            IsAttacking = false;
            yield return new WaitUntil(() => cameraManager.isFinished);

            characterAbility.currentEnergy = characterAbility.currentEnergy - actionSet[action].actionEnergy;
            UpdateCharacterBars(false);
            //GroupAttack Logic
            foreach (TeamUnit t in targetUnits.teamUnites)
            {
                if (t.charUnites.Contains(target))
                {
                    TeamUnit targetTeam = targetUnits.teamUnites[targetUnits.teamUnites.IndexOf(t)];
                    foreach (CharacterUnit c in targetTeam.charUnites)
                    {
                        hit = UnityEngine.Random.value < 0.5f;
                        if (c.isAlive)
                        {
                            c.Damaged(actionSet[action].actionValues, hit, critical, actionSet[action].actionData.gotDamagedVFX);
                            c.UpdateCharacterBars(false);
                        }
                    }
                }
            }
        }
        private IEnumerator GroupSwordAttack(BaseFormationHandler targetUnits, CharacterUnit target, bool hit, bool critical)
        {
            Vector3 startPos = transform.position;
            Quaternion startDir = transform.rotation;
            Vector3 dir = (target.transform.position - transform.position).normalized;
            Vector3 newTargetPos = target.transform.position - dir * 4f;
            yield return Move(gameObject, newTargetPos, movingSpeed);

            //Attack Animation
            SetCharacterOnceAnimation(actionSet[action].actionData.actionAnimation);
            onSwordAttack.Invoke(actionSet[action].actionData.actionAnimation, target.transform);

            //WaitUntilAnimationFinished and Delay
            yield return StartSFX(actionSet[action].actionData.targetType, actionSet[action].actionData.actionAnimation, hit);
            yield return new WaitForSeconds(0.1f);
            IsAttacking = false;
            yield return new WaitUntil(() => cameraManager.isFinished);
            // End attack && Move back
            characterAbility.currentEnergy = characterAbility.currentEnergy - actionSet[action].actionEnergy;
            UpdateCharacterBars(false);

            //GroupAttack Logic
            foreach (TeamUnit t in targetUnits.teamUnites)
            {
                if (t.charUnites.Contains(target))
                {
                    TeamUnit targetTeam = targetUnits.teamUnites[targetUnits.teamUnites.IndexOf(t)];
                    foreach (CharacterUnit c in targetTeam.charUnites)
                    {
                        hit = UnityEngine.Random.value < 0.5f;
                        if (c.isAlive)
                        {
                            c.Damaged(actionSet[action].actionValues, hit, critical, actionSet[action].actionData.gotDamagedVFX);
                            c.UpdateCharacterBars(false);
                        }
                    }
                }
            }
            yield return Move(gameObject, startPos, movingSpeed);
        }
        #endregion

        #region Single Attack Logic
        private IEnumerator DoSingleAttackAction(int action, CharacterUnit target, bool hit, bool critical)
        {
            SetAttackCamera();
            IsDoingAction = true;
            Quaternion startDir = transform.rotation;

            Coroutine actionCoroutine = (actionSet[action].actionData.animationType == AnimationType.Sword) ?
                StartCoroutine(SingleSwordAttack(target, hit, critical)) : StartCoroutine(SingleGunAttack(target, hit, critical));
            yield return actionCoroutine;

            transform.rotation = startDir;
            yield return new WaitForSeconds(1f);
            IsDoingAction = false;
        }

        private IEnumerator SingleGunAttack(CharacterUnit target, bool hit, bool critical)
        {
            print(actionSet[action].actionData.actionAnimation);
            print(actionSet[action].actionData.targetType);

            //Attack Animation
            SetCharacterOnceAnimation(actionSet[action].actionData.actionAnimation);
            onGunAttack.Invoke(actionSet[action].actionData.animationType, target.transform);

            //WaitUntilAnimationFinished and Delay
            yield return StartSFX(actionSet[action].actionData.targetType, actionSet[action].actionData.actionAnimation, hit);

            yield return new WaitForSeconds(0.1f);
            IsAttacking = false;
            yield return new WaitUntil(() => cameraManager.isFinished);
            characterAbility.currentEnergy = characterAbility.currentEnergy - actionSet[action].actionEnergy;

            target.Damaged(actionSet[action].actionValues, hit, critical, actionSet[action].actionData.gotDamagedVFX);
            UpdateCharacterBars(false);
            target.UpdateCharacterBars(false);
            yield return new WaitForSeconds(1f);
        }

        private IEnumerator SingleSwordAttack(CharacterUnit target, bool hit, bool critical)
        {
            Vector3 startPos = transform.position;
            Vector3 dir = (target.transform.position - transform.position).normalized;
            Vector3 newTargetPos = target.transform.position - dir * 1.25f;
            yield return Move(gameObject, newTargetPos, movingSpeed);
            //Attack Animation
            SetCharacterOnceAnimation(actionSet[action].actionData.actionAnimation);
            onSwordAttack.Invoke(actionSet[action].actionData.actionAnimation, target.transform);

            //WaitUntilAnimationFinished and Delay
            yield return StartSFX(actionSet[action].actionData.targetType, actionSet[action].actionData.actionAnimation, hit);
            target.Damaged(actionSet[action].actionValues, hit, critical, actionSet[action].actionData.gotDamagedVFX);
            yield return new WaitForSeconds(1f);
            IsAttacking = false;
            yield return new WaitUntil(() => cameraManager.isFinished);

            characterAbility.currentEnergy = characterAbility.currentEnergy - actionSet[action].actionEnergy;
            UpdateCharacterBars(false);
            target.UpdateCharacterBars(false);
            //Move back
            yield return Move(gameObject, startPos, movingSpeed);
        }
        #endregion

        #endregion

        #region Camera
        public void SetAttackCamera()
        {
            IsAttacking = true;
            cameraManager.ChasingTarget(mineFormation, this);
        }
        #endregion

        #region Effect
        private IEnumerator StartSFX(TargetType attackType, CharOnceMotion motion, bool hit)
        {
            yield return new WaitForSeconds(0.5f);

            switch (motion)
            {
                case CharOnceMotion.GunAttack:
                    if (actionSet[action].actionData.animationType == AnimationType.Gun) { AudioManager.PlaySFX(actionSet[action].actionData.audioType); }
                    break;
                case CharOnceMotion.JumpAttack:
                case CharOnceMotion.MeleeAttack1:
                    if (hit && actionSet[action].actionData.animationType == AnimationType.Sword) { AudioManager.PlaySFX(actionSet[action].actionData.audioType); }
                    break;
            }
        }

        public void SetFloatingDamageText(bool hit, int dmg, Transform targetPos)
        {
            Camera cam = cameraManager.mainCam;
            Vector2 initPos = cam.WorldToScreenPoint(targetPos.position);

            FloatingDamageText dmgText = Instantiate(damageText).GetComponent<FloatingDamageText>();
            dmgText.transform.SetParent(canvas.transform, false);
            dmgText.transform.position = initPos;
            if (hit) { dmgText.SetDamageText(dmg); }
            else { dmgText.SetMissText(); };
        }

        public void SetParticleEffect(string vfxTag, Transform targetPos)
        {
            vfxManager.SpawnFromPool(vfxTag, targetPos.position, Quaternion.identity);
        }
        #endregion

        #region Animation
        public void SetCharacterLoopAnimation(CharLoopMotion action)
        {
            charAnimator.SetLoopAction(action);
        }

        public void SetCharacterOnceAnimation(CharOnceMotion action)
        {
            charAnimator.SetOnceAction(action);
        }

        private IEnumerator missAttackMotion()
        {
            Vector3 startPos = transform.localPosition;
            yield return StartCoroutine(Jump(gameObject, missPos.transform.localPosition, missTime, false));
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(Jump(gameObject, startPos, jumpSpeed, true));
            SetCharacterLoopAnimation(CharLoopMotion.Idle);
        }

        private IEnumerator Jump(GameObject obj, Vector3 targetPos, float seconds, bool isBack)
        {
            float elapsedTime = 0;
            Vector3 start = obj.transform.localPosition;
            if (isBack)
            {
                SetCharacterLoopAnimation(CharLoopMotion.Running);
            }
            while (elapsedTime < seconds)
            {
                obj.transform.localPosition = Vector3.Lerp(start, targetPos, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
        }

        private IEnumerator Move(GameObject obj, Vector3 targetPos, float seconds)
        {
            float elapsedTime = 0;
            Vector3 start = obj.transform.position;
            SetCharacterLoopAnimation(CharLoopMotion.Running);
            var targetRotation = Quaternion.LookRotation(targetPos - obj.transform.position);

            while (elapsedTime < seconds)
            {
                obj.transform.position = Vector3.Lerp(start, targetPos, (elapsedTime / seconds));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, (elapsedTime / seconds));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            obj.transform.position = targetPos;
            SetCharacterLoopAnimation(CharLoopMotion.Idle);
        }
        #endregion

    }
}