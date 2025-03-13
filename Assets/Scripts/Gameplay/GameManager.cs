using CharacterMechanics;
using DungeonGeneration;
using Louis.Patterns.ServiceLocator;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;


namespace Managers {
    public class GameManager : MonoBehaviour {
        IDungeonGeneratorService dungeonGenerator;
        IPlayerMovementService playerMovement;
        IDungeonService dungeon;

        private void OnEnable() {
            ServiceLocator.AddServiceStatusChangeListener<IDungeonGeneratorService>(OnServiceStateChange);
            ServiceLocator.AddServiceStatusChangeListener<IPlayerMovementService>(OnServiceStateChange);
            ServiceLocator.AddServiceStatusChangeListener<IDungeonService>(OnServiceStateChange);
        }

        private void OnDisable() {
            ServiceLocator.RemoveServiceStatusChangeListener<IDungeonGeneratorService>(OnServiceStateChange);
            ServiceLocator.RemoveServiceStatusChangeListener<IPlayerMovementService>(OnServiceStateChange);
            ServiceLocator.RemoveServiceStatusChangeListener<IDungeonService>(OnServiceStateChange);
        }

        IEnumerator Start() {
            yield return new WaitUntil(() => dungeonGenerator != null && playerMovement != null);
            playerMovement.SetLock(true);
            dungeonGenerator.Generate();
            yield return new WaitUntil(() => dungeon != null);
            RoomInfo spawnRoom = dungeon.GetRoomByType(RoomType.Spawn).FirstOrDefault();
            if (spawnRoom == null) {
                Logging.Log(this, "Spawn Room is null", LogLevel.Error);
                yield break;
            }
            playerMovement.SetPosition(spawnRoom.bounds.center);
            playerMovement.SetLock(false);
        }

        #region Service Monitoring
        private void OnServiceStateChange(Type type, ServiceAvailabilityStatus status, IService t) {
            switch (t) {
                case IDungeonGeneratorService dg:
                    dungeonGenerator = dg;
                    break;
                case IPlayerMovementService pm:
                    playerMovement = pm;
                    break;
                case IDungeonService dg:
                    dungeon = dg;
                    break;
            }
        }
        #endregion
    }
}