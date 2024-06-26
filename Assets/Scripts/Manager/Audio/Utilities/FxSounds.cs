using System.Collections;
using MonoSingleton;
using UnityEngine;

namespace Manager.Audio.Utilities
{
    public class FxSounds : MonoSingleton<FxSounds>
    {
        public AudioClip ButtonClickFx; // Buton tıklama efekti.
        
        [Header("Agents Fx")]
        public AudioSource SpawnAgentFx; // Agent'lar spawn efekti.
        public AudioClip SpawnAgentClip; // Agent'lar spawn klibi.
        
        public AudioSource DeadAgentFx; // Agent'lar ölüm efekti.
        public AudioClip DeadAgentClip; // Agent'lar ölüm klibi.
        
        [Header("Game Panels Fx")]
        public AudioSource VictoryFx; // Victory efekti.
        public AudioSource DefeatFx; // Defeat efekti.
        
        [Header("Character Fx")]
        public AudioSource MoveSoundFx; // Karakterin sağa sola hareket sesi.
        public AudioSource CharacterRunFx; // Karaterin koşma sesi.
        
        [Header("Obstacles")]
        public AudioSource HammerFx;
        public AudioSource FanFx;
        
        #region Start
        private void Start()
        {
            SetFxSoundsVolume();
        }
        
        #endregion
        
        /// <summary>
        /// Fx Sound'ların volume değerlerini ayarlar.
        /// </summary>
        public void SetFxSoundsVolume()
        {
            float fxSliderValue = AudioManager.GetFxSliderValue();
            SpawnAgentFx.volume = fxSliderValue;
            DeadAgentFx.volume = fxSliderValue;
            VictoryFx.volume = fxSliderValue;
            DefeatFx.volume = fxSliderValue;
            MoveSoundFx.volume = fxSliderValue;
            CharacterRunFx.volume = fxSliderValue;
            HammerFx.volume = fxSliderValue - .2f;
            FanFx.volume = fxSliderValue;
        }

        /// <summary>
        /// Bir ses'i tekrarlı şekilde oynatır.
        /// </summary>
        /// <param name="audioSource">Oynatılacak ses kaynağı.</param>
        /// <param name="audioClip">Oynatılacak ses klibi.</param>
        /// <param name="repeat">Sesin oynatılacağı tekrar sayısı.</param>
        /// <returns>Herhangi bir return değeri yok.</returns>
        public IEnumerator RepeatedSound(AudioSource audioSource,AudioClip audioClip = default, int repeat = 1)
        {
            repeat = repeat < 0 ? repeat * -1 : repeat;
            while (repeat != 0)
            {
                audioSource.Play();
                repeat--;
                yield return new WaitForSeconds(.1f);
            }
        }
        
        /// <summary>
        /// Anakarakter ile ses kaynağı arasındaki mesafeye göre ses volume'ünü dinamik olarak ayarlar..
        /// </summary>
        /// <param name="audioManager">Ses düzeyi değiştirilecek ses kaynağı.</param>
        /// <param name="distance">Ses kaynağı ile anakarakter arasındaki mesafe.</param>
        /// <param name="minDistance">Sesin duyulacağı minimum mesafe.</param>
        /// <param name="distanceRange">Sesin duyulacağı maxDistance - minDistance değeri.</param>
        public void SetVolumeToDistance(AudioSource audioManager, float distance, float minDistance, float distanceRange)
        {
            audioManager.volume = 1.0f - (distance - minDistance) / distanceRange;
        }
    }
}