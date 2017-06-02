using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    [System.Serializable]
    public class EnemyStats
    {
        public int enMaxHealth = 3;
        private int _currentHealth;
        public int enCurrentHealth
        {
            get { return _currentHealth; }
            set { _currentHealth = Mathf.Clamp(value, 0, enMaxHealth); }
        }

        public void EnInit()
        {
            enCurrentHealth = enMaxHealth;
        }
    }

    public EnemyStats enStats = new EnemyStats();

    public int enFallBoundry;
    public int enDamage;

    //[SerializeField]
    //private StatusIndicator statusIndicator;
    void Start()
    {
        enStats.EnInit();

        //if (statusIndicator == null)
        //{
        //Debug.LogError("No status indicator referenced on Player.");
        //}

    }
    void Update()
    {
        if (transform.position.y <= enFallBoundry)
        {
            DamageEnemy(99999);
        }
    }

    public void DamageEnemy(int enDamage)
    {
        enStats.enCurrentHealth -= enDamage;
        if (enStats.enCurrentHealth <= 0)
        {
            GameMaster.KillEnemy(this);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.name == "Sword")
        {
            DamageEnemy(enDamage);
        }
    }
}
