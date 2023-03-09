using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MB.Game
{
    public class CharSpawner : MonoBehaviour, IWebCaller, IGameCaller
    {
        [SerializeField] private CharRuntime m_charTemplate = null;
        [Space]
        [SerializeField] private BaseFormationHandler m_pirateFormation = null;
        [SerializeField] private BaseFormationHandler m_enemyFormation = null;
        [Space]
        public UnityEvent onSetupDone = new UnityEvent();

        private int m_loadSumMax = 0;
        private int m_loadSumNow = 0;

        private List<string> m_meleCodes = new List<string>();
        private List<string> m_gunCodes = new List<string>();

        private CombatContainer combat => this.GetCombatContainer();

        private void Start()
        {
            SetupCodes();
            ClearLoadSum();
            SetupShip();
            SetupPirates();
            SetupEnemies();
        }

        private void ClearLoadSum()
        {
            m_loadSumMax = 0;
            m_loadSumNow = 0;
        }

        private void SetupCodes()
        {
            m_meleCodes = ItemConfig.GetSwordValues();
            m_gunCodes = ItemConfig.GetGunValues();
        }

        private void SetupShip()
        {
            m_pirateFormation.shipInfo = combat.ship;
            m_enemyFormation.shipInfo = combat.ship;
        }

        private void SetupPirates()
        {
            m_pirateFormation.ClearCharacters();
            m_pirateFormation.targetFormationHandler = m_enemyFormation;

            var pirates = this.combat.teammates;

            m_pirateFormation.AddCharacterTeam(pirates.Count);
            m_loadSumMax += pirates.Count;

            for (var i = 0; i < pirates.Count; i++)
            {
                var charIndex = i;
                var charData = pirates[i];

                StartCoroutine(CreateCharacterThread(charData, (character) =>
                {
                    SetCharacter(0, charIndex, m_pirateFormation, character, charData);
                }));
            }
        }

        private void SetupEnemies()
        {
            m_enemyFormation.ClearCharacters();
            m_enemyFormation.targetFormationHandler = m_pirateFormation;

            var waves = this.combat.waves;
            for (var i = 0; i < waves.Count; i++)
            {
                var waveIndex = i;
                var wave = waves[waveIndex];

                m_enemyFormation.AddCharacterTeam(wave.enemies.Count);
                m_loadSumMax += wave.enemies.Count;

                for (var j = 0; j < wave.enemies.Count; j++)
                {
                    var charIndex = j;
                    var charData = wave.enemies[charIndex];

                    StartCoroutine(CreateCharacterThread(charData, (character) =>
                    {
                        SetCharacter(waveIndex, charIndex, m_enemyFormation, character, charData);
                    }));
                }
            }
        }

        private void SetCharacter(int teamIndex, int charIndex, BaseFormationHandler formation, CharRuntime character, CharacterCombat charData)
        {
            formation.SetCharacter(teamIndex, charIndex, character.transform, charData);
            m_loadSumNow += 1;

            if (m_loadSumNow >= m_loadSumMax)
            {
                onSetupDone.Invoke();
            }
        }

        private IEnumerator CreateCharacterThread(CharacterCombat data, Action<CharRuntime> callBack)
        {
            if (data.characterType == CharacterType.Pirate)
            {
                yield return CreatePirateThread(data, callBack);
            }
            else
            {
                callBack.Invoke(null);
            }
        }

        private IEnumerator CreatePirateThread(CharacterCombat data, Action<CharRuntime> callBack)
        {
            var builder = new PirateBuilder();
            builder.character = this.m_charTemplate.Clone();
            builder.characterApply = true;
            builder.leftHandApply = true;
            builder.rightHandApply = true;
            builder.combine.Decode(data.partsCode);

            var meleCode = m_meleCodes[UnityEngine.Random.Range(0, m_meleCodes.Count)];
            builder.combine.rightHands.Add(meleCode);

            if (data.tier >= PirateCate.Normal_4.GetTierLevel() + 1)
            {
                var gunCode = m_gunCodes[UnityEngine.Random.Range(0, m_gunCodes.Count)];
                builder.combine.leftHands.Add(gunCode);
            }

            yield return builder.Apply();
            callBack.Invoke(builder.character);
        }
    }
}