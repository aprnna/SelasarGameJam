using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Input;
using Player;
using TilemapLayer;
using Turnbase_System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TurnBaseSystem : MonoBehaviour
{
    public static TurnBaseSystem Instance;
    [SerializeField] private UIManagerBattle _uIManagerBattle;
    
    [Header("TilemapLayer")]
    [SerializeField] private BattleAreaTilemap _battleArea;
    [SerializeField] private BattleBoardTilemap _battleBoard;
    [SerializeField] private PreviewTilemap _previewTilemap;

    [Header("Enemy")] 
    [SerializeField] private List<UnitData> _enemies;
    public UIManagerBattle UIManagerBattle => _uIManagerBattle;
    public UnitModel ActiveUnit => _activeUnit;
    private int _maxPlayer;
    private List<UnitData>  _players;
    public FiniteStateMachine<BattleState> BattleState { get; private set; }
    public PlayerTurnState PlayerTurnState { get; private set; }
    public SelectCardState SelectCardState { get; private set; }
    public EnemyTurnState EnemyTurnState { get; private set; }
    public GameEndState GameEndState { get; private set; }
    private List<Vector3> _LocPlayerSpawn;
    private List<Vector3> _LocEnemySpawn;
    private UnitModel _activeUnit;
    private CancellationTokenSource _cts;
    private CancellationToken _cancellationToken;
    private Vector2 _mousePos;
    private Camera _mainCamera;
    private Vector3 _pendingMove;
    private bool _confirmMove;
    private void Awake()
    {
        _players        = new List<UnitData>();
        _enemies        = new List<UnitData>();
        _LocPlayerSpawn = new List<Vector3>();
        _mainCamera = Camera.main;

        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    private void Start()
    {
        SelectCardState = new SelectCardState(this);
        PlayerTurnState = new PlayerTurnState(this);
        EnemyTurnState = new EnemyTurnState(this);
        GameEndState = new GameEndState(this);
        BattleState = new FiniteStateMachine<BattleState>(SelectCardState);
        
        InitializeLocPlayerSpawn();
    }

    private void OnEnable()
    {
        InputManager.Instance.PlayerInput.Performed.OnDown += OnLeftMouseClicked;
    }

    private void OnDisable()
    {
        InputManager.Instance.PlayerInput.Performed.OnDown -= OnLeftMouseClicked;
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
        BattleState.ChangeState(PlayerTurnState);
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
            _battleBoard.Build(_LocPlayerSpawn[idx], _players[idx].UnitPrefab, _players[idx]);
        }
    }

    private void InitializeEnemy()
    {
        _LocEnemySpawn = _battleBoard.GetEnemyLocWorld();
        for (var idx=0; idx<_LocEnemySpawn.Count ; idx++)
        {
            _battleBoard.Build(_LocEnemySpawn[idx],_enemies[idx].UnitPrefab, _enemies[idx]);
        }
    }
    public void ShowPlayerMove(UnitModel unitModel)
    {
        _battleArea.ShowMoveTile(unitModel);
    }

    public void HidePlayerMove()
    {
        _battleArea.HideMoveTile();
    }

    public void ShowPlayerAttack(UnitModel unitModel)
    {
        _battleArea.ShowAttackTile(unitModel);
    }

    public void HidePlayerAttack()
    {
        _battleArea.HideAttackTile();
    }
    public  void ShowPreview(Vector3 position)
        => _previewTilemap.ShowPreview(
            _activeUnit,
            position,
            IsValid(position)
        );
    public void ClearPreview() 
        => _previewTilemap.ClearPreview();
    
    public bool IsValid(Vector3 worldPosition)
        => _battleBoard.IsEmpty(worldPosition) && _battleArea.IsValidMoveCell(worldPosition) ;
    public void StartPreview()
    {
        _cts = new CancellationTokenSource();
        _cancellationToken = _cts.Token;
        PreviewLoopAsync(_cancellationToken).Forget();
    }

    public void StopPreview()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
        ClearPreview();
    }

    public void FreezePreview()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }
    }
    public async UniTaskVoid PreviewLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            _mousePos = InputManager.Instance.PlayerInput.MousePos.Get();
            var worldPosition = _mainCamera.ScreenToWorldPoint(_mousePos);
            ShowPreview(worldPosition);
            await UniTask.Yield(token);
        }
    }
    public void SetActiveUnit(UnitModel unitModel)
    {
        _activeUnit = unitModel;
    }
    public bool IsPointerOverUI(Vector2 screenPosition)
    {
        var eventData = new PointerEventData(EventSystem.current) {
            position = screenPosition
        };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
    private void OnLeftMouseClicked()
    {
   
        _mousePos = InputManager.Instance.PlayerInput.MousePos.Get();
        if (IsPointerOverUI(_mousePos)) return;
        if (BattleState.CurrentState != PlayerTurnState) return;
        var worldPosition = _mainCamera.ScreenToWorldPoint(_mousePos);
        UnitModel item = _battleBoard.GetUnit(worldPosition);
        var unitController = _uIManagerBattle.UnitController;
        if (item != null && !unitController.AlreadyMove && !unitController.OnMoveUnit)
        {
            _uIManagerBattle.ShowUnitAction(item, item.WorldCoords);
        }
        if(_activeUnit != null && !_confirmMove && IsValid(worldPosition))
        {
            _pendingMove = worldPosition;
            FreezePreview();
            _confirmMove = true;
            _uIManagerBattle.ShowConfirmMove();
        }
    }

    public void OnMovePerformed()
    {
        var newUnit = _battleBoard.Build(_pendingMove, _activeUnit.UnitData.UnitPrefab, _activeUnit.UnitData);
        _battleBoard.RemoveUnit(_activeUnit);
        _activeUnit = newUnit;
        HidePlayerMove();
        StopPreview();
        _confirmMove = false;
        _uIManagerBattle.UnitController.SetAlreadyMove(true);
        UIManagerBattle.ShowUnitAction(newUnit, _pendingMove);
    }

    public void OnMoveCanceled()
    {
        HidePlayerMove();
        StopPreview();
        _confirmMove = false;
    }
    public void OnAttackButton() {
        Debug.Log(ActiveUnit);
        if (ActiveUnit == null) return;
        PerformAttack();
        HidePlayerAttack();
        UIManagerBattle.HideUnitAction();
        SetActiveUnit(null);
    }
    private void PerformAttack() {
        var origin = ActiveUnit.Coordinates;
        var data   = ActiveUnit.UnitData;
        var offsets = UnitAttackCalculate.GetOffsets(
            data.AttackPattern, data.Range, data.Direction
        );

        foreach (var off in offsets) {
            var cell = origin + off;
            Vector3 world = _battleBoard.CellToWorld(cell) + new Vector3(0.5f, 0.5f);
            var target = _battleBoard.GetUnit(world);
                // && target.UnitData.UnitSide != data.UnitSide
            if (target != null ) {
                Debug.Log(target.UnitData.Name);
            }
        }
    }

    public void OnAttackCanceled()
    {
        HidePlayerAttack();
    }
 
}
