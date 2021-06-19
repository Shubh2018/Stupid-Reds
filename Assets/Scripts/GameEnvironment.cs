using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnvironment : MonoBehaviour
{
    public static GameEnvironment instance;
    public List<Enemy> enemies;

    private void Awake()
    {
        if(instance == null)
            instance = this;
    }

    void Update()
    {
        CheckAllEnemyDead();
    }

    public event Action<int> onShoot;
    public void OnShoot(int id)
    {
        if (onShoot != null)
            onShoot(id);
    }

    public void CheckAllEnemyDead()
    {
        if (enemies.Count < 1)
        {
            UIManager.instance.LevelClear = true;
        }       
    }
}
