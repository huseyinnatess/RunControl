﻿using System;
using Controller;
using GoogleAdmob;
using GoogleAdmob.Interface;
using Manager;
using ObjectPools;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities.SaveLoad;
using Random = UnityEngine.Random;

namespace Utilities
{
    public class Battlefield : MonoBehaviour
    {
        private Scene _activeScene;
        private int _rewardCoin;
        [SerializeField] private TextMeshProUGUI rewardCoinText;

        #region Awake

        private void Awake()
        {
            _activeScene = SceneManager.GetActiveScene();
        }

        #endregion

        public void WarResult(int enemyCount)
        {
            ShowIntersititalAd();
            if (enemyCount == 0)
                Victory();
            else
                Defeat();
        }

        #region Victory Functions

        private void Victory()
        {
            SetPlayerEndLevel();
            CharacterControl.Instance.SetVictoryAnimation();
            LevelPanelManager.Instance.VictoryPanel(true);
            SetPlayerRewardCoin();
        }

        private void SetPlayerRewardCoin()
        {
            _rewardCoin = (AgentPools.Instance.CharacterCount * 4) + Random.Range(0, 15);
            rewardCoinText.text = _rewardCoin.ToString();
            CoinManager.Instance.EarnCoin(_rewardCoin);
        }

        private void SetPlayerEndLevel()
        {
            if (_activeScene.buildIndex == PlayerData.GetInt("EndLevel"))
            {
                PlayerData.SetInt("EndLevel", PlayerData.GetInt("EndLevel") + 1);
            }
        }

        #endregion

        private void Defeat()
        {
            LevelPanelManager.Instance.DefeatPanel(true);
        }

        private void ShowIntersititalAd()
        {
            InterstitialAD.Instance.ShowAd();
        }
    }
}