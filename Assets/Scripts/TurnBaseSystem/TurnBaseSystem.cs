using System;
using System.Collections.Generic;
using Player;
using TilemapLayer;
using Turnbase_System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TurnBaseSystem : MonoBehaviour
{
    public static TurnBaseSystem Instance;
    [SerializeField] private UIManagerBattle _uIManagerBattle;
    [SerializeField] private GameObject _prefabPlayer;
    
    [Header("TilemapLayer")]
    [SerializeField] private BattleAreaTilemap _battleArea;
    [SerializeField] private BattleBoardTilemap _battleBoard;
    [SerializeField] private PreviewTilemap _previewTilemap;
    public UIManagerBattle UIManagerBattle => _uIManagerBattle;
    
    private int _maxPlayer;
    private List<UnitData>  _players;
    private List<UnitData> _enemies;
    private FiniteStateMachine<BattleState> _battleState;
    private PlayerTurnState _playerTurnState;
    private SelectCardState _selectCardState;
    private EnemyTurnState _enemyTurnState;
    private GameEndState _gameEndState;
    private List<Vector3> _LocPlayerSpawn;
    private UnitData _activeUnitData;
    private void Awake()
    {
        _players        = new List<UnitData>();
        _enemies        = new List<UnitData>();
        _LocPlayerSpawn = new List<Vector3>();

        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    private void Start()
    {
        _selectCardState = new SelectCardState(this);
        _playerTurnState = new PlayerTurnState(this);
        _enemyTurnState = new EnemyTurnState(this);
        _gameEndState = new GameEndState(this);
        _battleState = new FiniteStateMachine<BattleState>(_selectCardState);
        
        InitializeLocPlayerSpawn();
    }

    public void OnSelectPlayerCard(UnitData unitData)
    {
        Debug.Log(_players.Count+1);
        if (_players.Count+1 <= _maxPlayer)
        {
            if (unitData.UnitSide == UnitSide.Player)
            {
                _players.Add(unitData);
            }
        }
        else
        {
            Debug.Log("Max");
        }
    }

    public void OnDoneSelectPlayer()
    {
        InitializeBattle();
        _battleState.ChangeState(_playerTurnState);
    }

    private void InitializeLocPlayerSpawn()
    {
        _LocPlayerSpawn = _battleBoard.GetPlayerLocWorld();
        _maxPlayer = _LocPlayerSpawn.Count+1;
    }
    private void InitializeBattle()
    {
        for (var idx=0; idx<_LocPlayerSpawn.Count ; idx++)
        {
            _battleBoard.Build(_LocPlayerSpawn[idx],_prefabPlayer, _players[idx]);
        }
    }

    public void ShowPlayerMove(UnitModel unitModel)
    {
        _battleArea.ShowMoveTile(unitModel);
    }
    public  void ShowPreview(Vector3 position)
        => _previewTilemap.ShowPreview(
            _activeUnitData,
            position,
            IsValid(position)
        );
    public void ClearPreview() 
        => _previewTilemap.ClearPreview();
    
    public bool IsValid(Vector3 worldPosition)
        => _battleBoard.IsEmpty(worldPosition);

    public void SetActiveUnit(UnitData unitData)
    {
        _activeUnitData = unitData;
    }
}
