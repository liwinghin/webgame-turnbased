using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System;

namespace MB.Game
{
    public enum AudioType
    {
        swordAtk,
        jumpAtk,
        gunAtk,
        ShipAtk,
        none
    }

    public class AudioManager : MonoBehaviour
    {
        public static List<AudioClip> swordAttacks = new List<AudioClip>();
        public static List<AudioClip> jumpAttacks = new List<AudioClip>();
        public static List<AudioClip> gunAttacks = new List<AudioClip>();
        public static List<AudioClip> explosion = new List<AudioClip>();
        public static List<AudioClip> mouse = new List<AudioClip>();
        public static List<AudioClip> bgm = new List<AudioClip>();

        private static AudioSource bgmSrc;
        private static AudioSource sfxSrc;

        [SerializeField] private GameObject bgmObj;
        [SerializeField] private GameObject sfxObj;

        private void Awake()
        {
            OnLoadingAudioClip("SwordAtk", swordAttacks);
            OnLoadingAudioClip("JumpAtk", jumpAttacks);
            OnLoadingAudioClip("GunAtk", gunAttacks);
            OnLoadingAudioClip("Bgm", bgm);
            OnLoadingAudioClip("Win", bgm);
            OnLoadingAudioClip("Lose", bgm);
            OnLoadingAudioClip("Mouse", mouse);
            OnLoadingAudioClip("Explosion", explosion);

            bgmSrc = bgmObj.GetComponent<AudioSource>();
            sfxSrc = sfxObj.GetComponent<AudioSource>();
        }

        private void OnLoadingAudioClip(string labelName, List<AudioClip> list)
        {
            Addressables.LoadAssetsAsync<AudioClip>(labelName, list.Add);
        }

        private static int RandomClip(int count)
        {
            int clip = 0;
            clip = UnityEngine.Random.Range(0, count - 1);
            return clip;
        }

        public static void SetVolume(float bgmVolume, float sfxVolume)
        {
            bgmSrc.volume = bgmVolume;
            sfxSrc.volume = sfxVolume;
        }

        public static void PlayBgm(int index)
        {
            AudioClip c = bgm[index];
            bgmSrc.clip = c;
            bgmSrc.playOnAwake = false;
            bgmSrc.loop = true;
            bgmSrc.Play();
        }

        public static void PlayGameFinishedMusic(bool win)
        {
            AudioClip c = win? bgm[1] : bgm[2];
            sfxSrc.PlayOneShot(c);
        }

        public static void PlayMouseClick()
        {
            AudioClip c = mouse[0];
            sfxSrc.PlayOneShot(c);
        }

        public static void PlaySFX(AudioType type)
        {
            AudioClip c = null;
            switch (type)
            {
                case AudioType.swordAtk:
                    c = swordAttacks[RandomClip(swordAttacks.Count)];
                    break;
                case AudioType.jumpAtk:
                    c = jumpAttacks[RandomClip(jumpAttacks.Count)];
                    break;
                case AudioType.gunAtk:
                    c = gunAttacks[RandomClip(gunAttacks.Count)];
                    break;
                case AudioType.ShipAtk:
                    c = explosion[0];
                    break;
            }
            sfxSrc.PlayOneShot(c);
        }
    }
}
