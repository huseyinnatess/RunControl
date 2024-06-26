﻿using System.Linq;
using MonoSingleton;
using UnityEngine;

namespace Controller.AgentsController
{
    public class EnemyController : MonoSingleton<EnemyController>
    {
        public static bool IsCanAttack; // Enemy'nin saldırıya başlayacağını belirleyen değişken
        public static int EnemyAgentCount; // Level'daki enemy sayısı
        public GameObject[] EnemyAgents; // Enemy'lerin tutulduğu array

        private Transform _target; // Enemy'ler için hedef
        private Animator[] _enemyAnimators; // Enemey'lerin animator componentini tutan array
        private bool _attackAnimationWork; // Enemy'lerin saldırı animasyonuna geçmesini belirleyen değişken
        
        private float _enemySpeed; // Enemy'lerin hızı

        #region Awake Get, Set Functions

        private void Awake()
        {
            GetEnemyComponent();
            SetReferences();
        }

        private void GetEnemyComponent()
        {
            _enemyAnimators = EnemyAgents
                .Where(agent => agent.activeInHierarchy)
                .Select(agent => agent.GetComponent<Animator>())
                .ToArray();
        }

        private void SetReferences()
        {
            IsCanAttack = false;
            _attackAnimationWork = false;
            _target = CharacterControl.Instance.transform;
            EnemyAgentCount = EnemyAgents.Length;
            _enemySpeed = 1.5f;
        }

        #endregion
        
        /// <summary>
        /// Hiyerarşide aktif olan ilk enemy'nin transformunu return eder.
        /// </summary>
        /// <returns> Aktif olan enemy'nin transform componentini aksi halde null return eder</returns>
        public Transform GetActiveEnemy() => EnemyAgents.FirstOrDefault(agent => agent.activeInHierarchy)?.transform;

        /// <summary>
        ///  Enemy'lerin saldırıya başlamasını sağlar.
        /// </summary>
        /// <param name="attack"> Saldırıya başlayıp başlanmamasını belirler</param>
        public static void Attack(bool attack) => IsCanAttack = attack;

        #region LateUpdate

        private void LateUpdate()
        {
            if (_target is not null && IsCanAttack)
                EnemyAttack();
            if (_target is not null && !_target.gameObject.activeInHierarchy)
                _target = AgentController.GetActiveAgent();
        }

        #endregion
        
        
        // LateUpdate
        // Enemylerin agentlar'a saldırı yapmasını başlatır.
        private void EnemyAttack()
        {
            for (int i = 0; i < EnemyAgents.Length; i++)
            {
                if (!EnemyAgents[i].activeInHierarchy) continue;
                if (!_attackAnimationWork)
                    SetAttackAnimations(i);
                LookTarget(i);
                EnemyAgents[i].transform.position += GetDirectionToTarget(i) * (_enemySpeed * Time.deltaTime);
            }
            _attackAnimationWork = true;
        }
        
        // Enemy'nin attack animasyonunu başlatır
        private void SetAttackAnimations(int i) => _enemyAnimators[i].SetBool("Attack", true);

        // Target ve enemy arasındaki farkı return eder.
        private Vector3 GetDirectionToTarget(int i) => (_target.position - EnemyAgents[i].transform.position).normalized;
        
        // Enemy'lerin yönlerini target'a ayarlar.
        private void LookTarget(int i) => EnemyAgents[i].transform.LookAt(_target, Vector3.up);
    }
}