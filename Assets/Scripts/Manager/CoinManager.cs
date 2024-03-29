﻿using MonoSingleton;
using TMPro;
using UnityEngine;
using Utilities.SaveLoad;

namespace Manager
{
    public class CoinManager : MonoSingleton<CoinManager>
    {
        private TextMeshProUGUI _coinText;
        private int _coin;
        
        private void Awake()
        {
            DontDestroyOnLoad(this);
            _coinText = GameObject.FindWithTag("Coin").GetComponent<TextMeshProUGUI>();
            _coin = PlayerData.GetInt("Coin");
            if (_coinText is not null)
            {
                _coinText.text = _coin.ToString();
            }

        }
        
        public bool ProcessPurchase(int price)
        {
            if (!CheckPurchase(price)) return false;
            SpendCoin(price);
            return (true);
        }
        public void EarnCoin(int amount)
        {
            _coin = PlayerData.GetInt("Coin");
            _coin += amount;
            if (_coinText is not null)
                _coinText.text = _coin.ToString();
            SaveCoin();
        }

        private void SpendCoin(int amount)
        {
            _coin = PlayerData.GetInt("Coin");
            _coin -= amount;
            if (_coinText is not null)
                _coinText.text = _coin.ToString();
            SaveCoin();
        }

        private bool CheckPurchase(int price)
        {
            return (_coin >= price);
        }
        
        private void SaveCoin()
        {
            PlayerData.SetInt("Coin", _coin);
        }
    }
}