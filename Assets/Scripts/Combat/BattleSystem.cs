using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : StateMachine
{

    public int level;
    [SerializeField] public GameObject _playerPrefab;
    [SerializeField] public GameObject _enemyPrefab;
    [SerializeField] public Transform _playerBattleStation;
    [SerializeField] public Transform _enemyBattleStation;

    public Sprite[] enemySprites;
    public PlayerCombatData _playerData;
    public EnemyCombatData _enemyData;
    public GameObject enemyGameObject;

    public CombatUIManager combatUIManager;
    public static BattleSystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        BattleEventSystem.current.OnPlayerHealthChanged += CheckForVictoryOrDefeat;
        BattleEventSystem.current.OnEnemyHealthChanged += CheckForVictoryOrDefeat;
        level = 1;

        SetState(new AddItem(this));
    }

    private void CheckForVictoryOrDefeat(float x, float y)
    {
        StartCoroutine(CheckForVictoryOrDefeatCourotine());
    }

    public IEnumerator CheckForVictoryOrDefeatCourotine()
    {
        yield return new WaitForSeconds(.5f);
        if (_playerData.CurrentHealth <= 0)
        {
            SetState(new PlayerDefeat(this));
        }
        if (_enemyData != null && _enemyData.CurrentHealth <= 0)
        {
            SetState(new PlayerVictory(this));
        }
    }
}
