using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Player;
using TilemapLayer;
using Unity.Mathematics;
using UnityEngine;

namespace Turnbase_System
{
    public class EnemyTurnState:BattleState
    {
     public EnemyTurnState(TurnBaseSystem tbs) : base(tbs) { }

        public override void OnEnter()
        {
            // Mulai rutinitas giliran lawan
            ExecuteEnemyTurnAsync().Forget();
        }

        public override void OnUpdate()
        {
            // Tidak perlu update rutin per-frame di state ini
        }

        public override void OnExit()
        {
            // Bersihkan highlight atau preview jika masih ada
            TurnBaseSystem.Instance.HidePlayerMove();
            TurnBaseSystem.Instance.HidePlayerAttack();
        }

        private async UniTaskVoid ExecuteEnemyTurnAsync()
        {
            var tbs = TurnBaseSystem.Instance;

            // Ambil semua unit enemy dari board
            var enemyUnits = tbs.GetUnitsBySide(UnitSide.Enemy);
            var playerUnits = tbs.GetUnitsBySide(UnitSide.Player);

            foreach (var enemy in enemyUnits)
            {
                // Set aktif
                tbs.SetActiveUnit(enemy);

                // 1) Tampilkan area gerak
                tbs.ShowPlayerMove(enemy);
                await UniTask.Delay(500);

                // 2) Pilih target terdekat
                UnitModel target = FindClosest(enemy, playerUnits);
                if (target != null)
                {
                    // 3) Hitung path sederhana: ambil satu langkah mendekat
                    Vector3Int step = GetStepTowards(enemy.Coordinates, target.Coordinates);
                    Vector3 worldStep = tbs.CellToWorld(step) + new Vector3(0.5f, 0.5f);

                    // 4) Pindah unit
                    tbs.HidePlayerMove();
                    tbs.MoveEnemy(enemy, worldStep);
                    await UniTask.Delay(500);

                    // 5) Jika dalam jangkauan serang, lakukan serang
                    if (tbs.IsInAttackRange(enemy, target))
                    {
                        tbs.ShowPlayerAttack(enemy);
                        tbs.PerformAttack();
                        await UniTask.Delay(500);
                        tbs.HidePlayerAttack();
                    }
                }

                // Tunggu sejenak sebelum giliran unit berikutnya
                await UniTask.Delay(300);
            }

            // Selesai semua enemy, lanjutkan ke giliran player
            tbs.BattleState.ChangeState(tbs.PlayerTurnState);
        }

        private UnitModel FindClosest(UnitModel self, List<UnitModel> others)
        {
            UnitModel best = null;
            int minDist = int.MaxValue;
            foreach (var u in others)
            {
                int d = math.abs(u.Coordinates.x - self.Coordinates.x)
                      + math.abs(u.Coordinates.y - self.Coordinates.y);
                if (d < minDist)
                {
                    minDist = d;
                    best = u;
                }
            }
            return best;
        }

        private Vector3Int GetStepTowards(Vector3Int from, Vector3Int to)
        {
            var dir = to - from;
            if (math.abs(dir.x) > math.abs(dir.y))
                return from + new Vector3Int(math.sign(dir.x), 0, 0);
            else
                return from + new Vector3Int(0, math.sign(dir.y), 0);
        }
    }
}