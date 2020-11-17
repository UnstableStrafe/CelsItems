﻿using Dungeonator;
using System;
using UnityEngine;
using MonoMod.RuntimeDetour;
using System.Reflection;
using System.Collections.Generic;
using HutongGames.PlayMaker.Actions;
using System.Collections;
using HutongGames.PlayMaker;
using ExpandTheGungeon.ExpandObjects;
using ExpandTheGungeon.ExpandUtilities;
using ExpandTheGungeon.ExpandDungeonFlows;
using Pathfinding;

namespace ExpandTheGungeon.ExpandMain {

    public class ExpandSharedHooks : MonoBehaviour {
        public static Hook cellhook;
        public static Hook enterRoomHook;

        public static bool IsHooksInstalled = false;
        
        public static void InstallPrimaryHooks(bool InstallHooks = true) {
            if (InstallHooks) {
                    
                if (cellhook == null) {
                    if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing FlagCells Hook...."); }
                    cellhook = new Hook(
                        typeof(OccupiedCells).GetMethod("FlagCells", BindingFlags.Public | BindingFlags.Instance),
                        typeof(ExpandSharedHooks).GetMethod("FlagCellsHook", BindingFlags.NonPublic | BindingFlags.Instance),
                        typeof(OccupiedCells)
                    );
                }
                
                IsHooksInstalled = true;
                if (1 == 9) { ETGModConsole.Log("Primary hooks installed...", false); }
                return;
            } else {
                if (cellhook != null) { cellhook.Dispose(); cellhook = null; }
                IsHooksInstalled = false;
                if (1 == 9) { ETGModConsole.Log("Primary hooks removed...", false); }
                return;
            }
        }
        
        public static void InstallRequiredHooks() {


            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing PlaceWallMimics Hook...."); }
            Hook wallmimichook = new Hook(
                typeof(Dungeon).GetMethod("PlaceWallMimics", BindingFlags.Public | BindingFlags.Instance),
                typeof(ExpandPlaceWallMimic).GetMethod("PlaceWallMimics", BindingFlags.NonPublic | BindingFlags.Instance),
                GameManager.Instance.Dungeon
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing GetEnemiesString Hook...."); }
            Hook Stringhook = new Hook(
                typeof(StringTableManager).GetMethod("GetEnemiesString", BindingFlags.Public | BindingFlags.Static),
                typeof(ExpandStringTableManager).GetMethod("GetEnemiesString", BindingFlags.Public | BindingFlags.Static)
            );
            

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing FlowDatabase.GetOrLoadByName Hook...."); }
            Hook flowhook = new Hook(
                typeof(FlowDatabase).GetMethod("GetOrLoadByName", BindingFlags.Public | BindingFlags.Static),
                typeof(ExpandDungeonFlow).GetMethod("LoadCustomFlow", BindingFlags.Public | BindingFlags.Static)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing ApplyObjectStamp Hook...."); }
            Hook objectstamphook = new Hook(
                typeof(TK2DDungeonAssembler).GetMethod("ApplyObjectStamp", BindingFlags.Public | BindingFlags.Static),
                typeof(ExpandSharedHooks).GetMethod("ApplyObjectStampHook", BindingFlags.Public | BindingFlags.Static)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing LoopBuilderComposite.PlaceRoom Hook...."); }
            Hook placeRoomHook = new Hook(
                typeof(LoopBuilderComposite).GetMethod("PlaceRoom", BindingFlags.Public | BindingFlags.Static),
                typeof(ExpandSharedHooks).GetMethod("PlaceRoomHook", BindingFlags.Public | BindingFlags.Static)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing SemioticDungeonGenSettings.GetRandomFlow Hook...."); }
            Hook getRandomFlowHook = new Hook(
                typeof(SemioticDungeonGenSettings).GetMethod("GetRandomFlow", BindingFlags.Public | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("GetRandomFlowHook", BindingFlags.Public | BindingFlags.Instance),
                typeof(SemioticDungeonGenSettings)
            );

            /*if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing AIActor.OnPlayerEntered Hook...."); }
            Hook onPlayerEnteredHook = new Hook(
               typeof(AIActor).GetMethod("OnPlayerEntered", BindingFlags.NonPublic | BindingFlags.Instance),
               typeof(ChaosSharedHooks).GetMethod("OnPlayerEnteredHook", BindingFlags.Public | BindingFlags.Instance),
               typeof(AIActor)
            );*/

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing GetKicked.HandlePitfall Hook...."); }
            Hook handlePitfallHook = new Hook(
                typeof(GetKicked).GetMethod("HandlePitfall", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("HandlePitfallHook", BindingFlags.Public | BindingFlags.Instance),
                typeof(GetKicked)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing DungeonDoorController.CheckForPlayerCollision Hook...."); }
            Hook checkforPlayerCollisionHook = new Hook(
                typeof(DungeonDoorController).GetMethod("CheckForPlayerCollision", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("CheckForPlayerCollisionHook", BindingFlags.Public | BindingFlags.Instance),
                typeof(DungeonDoorController)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing ElevatorArrivalController.Update Hook...."); }
            Hook arrivalElevatorUpdateHook = new Hook(
                typeof(ElevatorArrivalController).GetMethod("Update", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("ArrivalElevatorUpdateHook", BindingFlags.Public | BindingFlags.Instance),
                typeof(ElevatorArrivalController)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing TK2DDungeonAssembler.ConstructTK2DDungeon Hook...."); }
            Hook constructTK2DDungeonHook = new Hook(
                typeof(TK2DDungeonAssembler).GetMethod("ConstructTK2DDungeon", BindingFlags.Public | BindingFlags.Instance),
                typeof(ExpandAssemblerHook).GetMethod("ConstructTK2DDungeonHook", BindingFlags.Public | BindingFlags.Instance),
                typeof(TK2DDungeonAssembler)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing HellDragZoneController.HandleGrabbyGrab Hook...."); }
            Hook handleGrabbyGrabHook = new Hook(
                typeof(HellDragZoneController).GetMethod("HandleGrabbyGrab", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("HandleGrabbyGrabHook", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(HellDragZoneController)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing ItemDB.DungeonStart Hook...."); }
            Hook dungeonStartHook = new Hook(
                typeof(ItemDB).GetMethod("DungeonStart", BindingFlags.Public | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("DungeonStartHook", BindingFlags.Public | BindingFlags.Instance),
                typeof(ItemDB)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing SellCellController.ConfigureOnPlacement Hook...."); }
            Hook sellPitConfigureOnPlacementHook = new Hook(
                typeof(SellCellController).GetMethod("ConfigureOnPlacement", BindingFlags.Public | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("SellPitConfigureOnPlacementHook", BindingFlags.Public | BindingFlags.Instance),
                typeof(SellCellController)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing BaseShopController.PlayerFired Hook...."); }
            Hook playerFiredHook = new Hook(
               typeof(BaseShopController).GetMethod("PlayerFired", BindingFlags.NonPublic | BindingFlags.Instance),
               typeof(ExpandSharedHooks).GetMethod("PlayerFiredHook", BindingFlags.NonPublic | BindingFlags.Instance),
               typeof(BaseShopController)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing OccupiedCells.FlagCells Hook...."); }
            Hook FlagCellsHook  = new Hook(
                typeof(OccupiedCells).GetMethod("FlagCells", BindingFlags.Public | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("FlagCellsHook", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(OccupiedCells)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing SecretRoomBuilder.GenerateRoomDoorMesh Hook...."); }
            Hook generateRoomDoorMeshHook = new Hook(
                typeof(SecretRoomBuilder).GetMethod("GenerateRoomDoorMesh", BindingFlags.NonPublic | BindingFlags.Static),
                typeof(ExpandSharedHooks).GetMethod("GenerateRoomDoorMeshHook", BindingFlags.NonPublic | BindingFlags.Static)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing AIAnimator.AnimationCompleted Hook...."); }
            Hook animationCompletedHook = new Hook(
                typeof(AIAnimator).GetMethod("AnimationCompleted", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("AnimationCompletedHook", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(AIAnimator)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing GameManager.FlushMusicAudio Hook...."); }
            Hook flushMusicAudioHook = new Hook(
                typeof(GameManager).GetMethod("FlushMusicAudio", BindingFlags.Public | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("FlushMusicAudioHook", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(GameManager)
            );
            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing DungeonFloorMusicController.SwitchToState Hook...."); }
            Hook switchToStateHook = new Hook(
                typeof(DungeonFloorMusicController).GetMethod("SwitchToState", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("SwitchToStateHook", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(DungeonFloorMusicController)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing PaydayDrillItem.HandleCombatWaves Hook...."); }
            Hook paydayDrillCombatWaveHook = new Hook(
                typeof(PaydayDrillItem).GetMethod("HandleCombatWaves", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("HandleCombatWavesHook", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(PaydayDrillItem)
            );


            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing GameManager.FlushAudio Hook...."); }
            Hook flushAudioHook = new Hook(
                typeof(GameManager).GetMethod("FlushAudio", BindingFlags.Public | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("FlushAudioHook", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(GameManager)
            );


            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing PunchoutController.TeardownPunchout Hook...."); }
            Hook teardownPunchoutHook = new Hook(
                typeof(PunchoutController).GetMethod("TeardownPunchout", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("TeardownPunchout_Hook", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(PunchoutController)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing StringTableManager.GetSynergyString Hook...."); }
            Hook GetSynergyStringHook = new Hook(
                typeof(StringTableManager).GetMethod("GetSynergyString", BindingFlags.Static | BindingFlags.Public), 
                typeof(ExpandStringTableManager).GetMethod("GetSynergyString", BindingFlags.Static | BindingFlags.Public)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing TK2DDungeonAssembler.BuildBorderLayerCenterJungle Hook...."); }
            Hook buildorderLayerCenterJungleHook = new Hook(
                typeof(TK2DDungeonAssembler).GetMethod("BuildBorderLayerCenterJungle", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("BuildBorderLayerCenterJungleHook", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(TK2DDungeonAssembler)
            );

            if (1 == 9) { Debug.Log("[ExpandTheGungeon] Installing TK2DDungeonAssembler.BuildOcclusionLayerCenterJungle Hook...."); }
            Hook buildOcclusionLayerCenterJungle = new Hook(
                typeof(TK2DDungeonAssembler).GetMethod("BuildOcclusionLayerCenterJungle", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(ExpandSharedHooks).GetMethod("BuildBorderLayerCenterJungleHook", BindingFlags.NonPublic | BindingFlags.Instance),
                typeof(TK2DDungeonAssembler)
            );

            return;
        }

        private void FlagCellsHook(Action<OccupiedCells> orig, OccupiedCells self) {
            try { orig(self); } catch (Exception ex) {
                if (1 == 9) {
                    ETGModConsole.Log("[DEBUG] Warning: Exception caught in OccupiedCells.FlagCells!");
                    Debug.Log("Warning: Exception caught in OccupiedCells.FlagCells!");
                    Debug.LogException(ex);
                }
                return;
            }
        }

        private void EnteredNewRoomHook(Action<RoomHandler, PlayerController> orig, RoomHandler self, PlayerController player) {
            orig(self, player);

            if (string.IsNullOrEmpty(self.GetRoomName())) {
                if (1 == 9) {
                    ETGModConsole.Log("[DEBUG] Player entered a room with null name field (hallway room?)", false);
                }
                return;
            }
            
            if (1 == 9) {
                ETGModConsole.Log("[DEBUG] Player entered room with name '" + player.CurrentRoom.GetRoomName() + "' .", false);
            }
        }

        public static GameObject ApplyObjectStampHook(int ix, int iy, ObjectStampData osd, Dungeon d, tk2dTileMap map, bool flipX = false, bool isLightStamp = false) {
            try {
                DungeonTileStampData.StampSpace occupySpace = osd.occupySpace;
                for (int i = 0; i < osd.width; i++) {
                    for (int j = 0; j < osd.height; j++) {
                        CellData cellData = d.data.cellData[ix + i][iy + j];
                        CellVisualData cellVisualData = cellData.cellVisualData;
                        if (cellVisualData.forcedMatchingStyle != DungeonTileStampData.IntermediaryMatchingStyle.ANY && cellVisualData.forcedMatchingStyle != osd.intermediaryMatchingStyle) {
                            return null;
                        }
                        if (osd.placementRule != DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS || !isLightStamp) {
                            bool flag = cellVisualData.containsWallSpaceStamp;
                            if (cellVisualData.facewallGridPreventsWallSpaceStamp && isLightStamp) { flag = false; }
                            if (occupySpace == DungeonTileStampData.StampSpace.BOTH_SPACES) {
                                if (cellVisualData.containsObjectSpaceStamp || flag || (!isLightStamp && cellVisualData.containsLight)) {
                                    return null;
                                }
                                if (cellData.type == CellType.PIT) { return null; }
                            } else if (occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE) {
                                if (cellVisualData.containsObjectSpaceStamp) { return null; }
                                if (cellData.type == CellType.PIT) { return null; }
                            } else if (occupySpace == DungeonTileStampData.StampSpace.WALL_SPACE && (flag || (!isLightStamp && cellVisualData.containsLight))) {
                                return null;
                            }
                        }
                    }
                }
                int num = (occupySpace != DungeonTileStampData.StampSpace.OBJECT_SPACE) ? GlobalDungeonData.wallStampLayerIndex : GlobalDungeonData.objectStampLayerIndex;
                float z = map.data.Layers[num].z;
                Vector3 vector = osd.objectReference.transform.position;
                ObjectStampOptions component = osd.objectReference.GetComponent<ObjectStampOptions>();
                if (component != null) { vector = component.GetPositionOffset(); }
                GameObject gameObject = Instantiate(osd.objectReference);            
                gameObject.transform.position = new Vector3(ix, iy, z) + vector;
                if (!isLightStamp && osd.placementRule == DungeonTileStampData.StampPlacementRule.ALONG_LEFT_WALLS) {
                    gameObject.transform.position = new Vector3(ix + 1, iy, z) + vector.WithX(-vector.x);
                }
                tk2dSprite component2 = gameObject.GetComponent<tk2dSprite>();
                RoomHandler absoluteRoomFromPosition = GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(new IntVector2(ix, iy));
                MinorBreakable componentInChildren = gameObject.GetComponentInChildren<MinorBreakable>();
                if (componentInChildren) {
                    if (osd.placementRule == DungeonTileStampData.StampPlacementRule.ON_ANY_FLOOR) {
                        componentInChildren.IgnoredForPotShotsModifier = true;
                    }
                    componentInChildren.IsDecorativeOnly = true;
                }
                IPlaceConfigurable @interface = gameObject.GetInterface<IPlaceConfigurable>();
                if (@interface != null) { @interface.ConfigureOnPlacement(absoluteRoomFromPosition); }
                SurfaceDecorator component3 = gameObject.GetComponent<SurfaceDecorator>();
                if (component3 != null) {
                    component3.Decorate(absoluteRoomFromPosition);
                }
                if (flipX) {
                    if (component2 != null) {
                        component2.FlipX = true;
                        float x = Mathf.Ceil(component2.GetBounds().size.x);
                        gameObject.transform.position = gameObject.transform.position + new Vector3(x, 0f, 0f);
                    } else {
                        gameObject.transform.localScale = Vector3.Scale(gameObject.transform.localScale, new Vector3(-1f, 1f, 1f));
                    }
                }
                gameObject.transform.parent = ((absoluteRoomFromPosition == null) ? null : absoluteRoomFromPosition.hierarchyParent);
                DepthLookupManager.ProcessRenderer(gameObject.GetComponentInChildren<Renderer>());
                if (component2 != null) {
                    component2.UpdateZDepth();
                }
                for (int k = 0; k < osd.width; k++) {
                    for (int l = 0; l < osd.height; l++) {
                        CellVisualData cellVisualData2 = d.data.cellData[ix + k][iy + l].cellVisualData;
                        if (occupySpace == DungeonTileStampData.StampSpace.OBJECT_SPACE) {
                            cellVisualData2.containsObjectSpaceStamp = true;
                        }
                        if (occupySpace == DungeonTileStampData.StampSpace.WALL_SPACE) {
                            cellVisualData2.containsWallSpaceStamp = true;
                        }
                        if (occupySpace == DungeonTileStampData.StampSpace.BOTH_SPACES) {
                            cellVisualData2.containsObjectSpaceStamp = true;
                            cellVisualData2.containsWallSpaceStamp = true;
                        }
                    }
                }
                return gameObject;
            } catch (Exception ex) {
                if (1 == 9) {
                    ETGModConsole.Log("Warning: Exception caught during ApplyObjectStamp method during Dungeon generation!");
                    Debug.Log("Warning: Exception caught during ApplyObjectStamp method during Dungeon generation!");
                    Debug.LogException(ex);
                }
                return null;
            }
        }        
        
        public static RoomHandler PlaceRoomHook(BuilderFlowNode current, SemioticLayoutManager layout, IntVector2 newRoomPosition) {
            try { 
                IntVector2 d = new IntVector2(current.assignedPrototypeRoom.Width, current.assignedPrototypeRoom.Height);
			    CellArea cellArea = new CellArea(newRoomPosition, d, 0);
			    cellArea.prototypeRoom = current.assignedPrototypeRoom;
			    cellArea.instanceUsedExits = new List<PrototypeRoomExit>();
			    if (current.usesOverrideCategory) { cellArea.PrototypeRoomCategory = current.overrideCategory; }
			    RoomHandler roomHandler = new RoomHandler(cellArea);
			    roomHandler.distanceFromEntrance = 0;
			    roomHandler.CalculateOpulence();
			    roomHandler.CanReceiveCaps = current.node.receivesCaps;
			    current.instanceRoom = roomHandler;
			    if (roomHandler.area.prototypeRoom != null && current.Category == PrototypeDungeonRoom.RoomCategory.SECRET && current.parentBuilderNode != null && current.parentBuilderNode.instanceRoom != null) {
			    	roomHandler.AssignRoomVisualType(current.parentBuilderNode.instanceRoom.RoomVisualSubtype, false);
			    }
			    layout.StampCellAreaToLayout(roomHandler, false);
			    return roomHandler;
            } catch (Exception ex) {
                ETGModConsole.Log("[DEBUG] ERROR: Exception during LoopBuilderComposite.PlaceRoom!");
                ETGModConsole.Log("[DEBUG] Name of assigned room: " + current.assignedPrototypeRoom.name);
                if (current.instanceRoom != null) {
                    ETGModConsole.Log("[DEBUG] Name of instanced room: " + current.instanceRoom.GetRoomName());
                }
                Debug.Log("ERROR: Exception during LoopBuilderComposite.PlaceRoom!");
                Debug.Log("Name of assigned room: " + current.assignedPrototypeRoom.name);
                Debug.LogException(ex);
                return null;
            }
		}

        public DungeonFlow GetRandomFlowHook(Action<SemioticDungeonGenSettings>orig, SemioticDungeonGenSettings self) { try { 
                float num = 0f;
                List<DungeonFlow> list = new List<DungeonFlow>();
                float num2 = 0f;
                List<DungeonFlow> list2 = new List<DungeonFlow>();
                for (int i = 0; i < self.flows.Count; i++) {
                    if (GameStatsManager.Instance.QueryFlowDifferentiator(self.flows[i].name) > 0) {
                        num += 1f;
                        list.Add(self.flows[i]);
                    } else {
                        num2 += 1f;
                        list2.Add(self.flows[i]);
                    }
                }
                if (list2.Count <= 0 && list.Count > 0) { list2 = list; num2 = num; }
                if (list2.Count <= 0) { return null; }
                float num3 = BraveRandom.GenerationRandomValue() * num2;
                float num4 = 0f;
                for (int j = 0; j < list2.Count; j++) {
                    num4 += 1f;
                    if (num4 >= num3) { return list2[j]; }
                }
                return self.flows[BraveRandom.GenerationRandomRange(0, self.flows.Count)];
            } catch (Exception ex) {
                if (1 == 9) { ETGModConsole.Log("[DEBUG] WARNING: Attempted to return a null DungeonFlow or primary flow list is empty in SemioticDungeonGenSettings.GetRandomFlow!"); }
                Debug.Log("WARNING: Attempted to return a null DungeonFlow or primary flow list is empty in SemioticDungeonGenSettings.GetRandomFlow!");
                Debug.LogException(ex);
                // Falling back to mod's compiled list of Flows                
                if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER) {
                    return ExpandDungeonFlow.Foyer_Flow;
                } else {
                    Dungeon dungeon = GameManager.Instance.Dungeon;
                    List<DungeonFlow> m_fallbacklist = new List<DungeonFlow>();

                    if (dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CASTLEGEON) {
                        for (int i = 0; i < ExpandDungeonFlow.KnownFlows.Count; i ++) {
                            if (ExpandDungeonFlow.KnownFlows[i].name.ToLower().StartsWith("f1_castle_flow")) {
                                m_fallbacklist.Add(ExpandDungeonFlow.KnownFlows[i]);
                            }
                        }
                    } else if (dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.SEWERGEON) {
                        for (int i = 0; i < ExpandDungeonFlow.KnownFlows.Count; i ++) {
                            if (ExpandDungeonFlow.KnownFlows[i].name.ToLower().StartsWith("f1a_sewers_flow")) {
                                m_fallbacklist.Add(ExpandDungeonFlow.KnownFlows[i]);
                            }
                        }
                    } else if (dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.GUNGEON) {
                        for (int i = 0; i < ExpandDungeonFlow.KnownFlows.Count; i ++) {
                            if (ExpandDungeonFlow.KnownFlows[i].name.ToLower().StartsWith("f2_gungeon_flow")) {
                                m_fallbacklist.Add(ExpandDungeonFlow.KnownFlows[i]);
                            }
                        }
                    } else if (dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATHEDRALGEON) {
                        for (int i = 0; i < ExpandDungeonFlow.KnownFlows.Count; i ++) {
                            if (ExpandDungeonFlow.KnownFlows[i].name.ToLower().StartsWith("f2a_cathedral_flow")) {
                                m_fallbacklist.Add(ExpandDungeonFlow.KnownFlows[i]);
                            }
                        }
                    } else if (dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.MINEGEON) {
                        for (int i = 0; i < ExpandDungeonFlow.KnownFlows.Count; i ++) {
                            if (ExpandDungeonFlow.KnownFlows[i].name.ToLower().StartsWith("f3_mines_flow")) {
                                m_fallbacklist.Add(ExpandDungeonFlow.KnownFlows[i]);
                            }
                        }
                    } else if (dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.RATGEON) {
                        for (int i = 0; i < ExpandDungeonFlow.KnownFlows.Count; i ++) {
                            if (ExpandDungeonFlow.KnownFlows[i].name.ToLower().StartsWith("resourcefulratlair_flow")) {
                                m_fallbacklist.Add(ExpandDungeonFlow.KnownFlows[i]);
                            }
                        }
                    } else if (dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATACOMBGEON) {
                        for (int i = 0; i < ExpandDungeonFlow.KnownFlows.Count; i ++) {
                            if (ExpandDungeonFlow.KnownFlows[i].name.ToLower().StartsWith("f3_mines_flow")) {
                                m_fallbacklist.Add(ExpandDungeonFlow.KnownFlows[i]);
                            }
                        }
                    } else if (dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.OFFICEGEON) {
                        for (int i = 0; i < ExpandDungeonFlow.KnownFlows.Count; i ++) {
                            if (ExpandDungeonFlow.KnownFlows[i].name.ToLower().StartsWith("fs4_nakatomi_flow")) {
                                m_fallbacklist.Add(ExpandDungeonFlow.KnownFlows[i]);
                            }
                        }
                    } else if (dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON) {
                        for (int i = 0; i < ExpandDungeonFlow.KnownFlows.Count; i ++) {
                            if (ExpandDungeonFlow.KnownFlows[i].name.ToLower().StartsWith("f5_forge_flow")) {
                                m_fallbacklist.Add(ExpandDungeonFlow.KnownFlows[i]);
                            }
                        }
                    } else if (dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON) {
                        for (int i = 0; i < ExpandDungeonFlow.KnownFlows.Count; i ++) {
                            if (ExpandDungeonFlow.KnownFlows[i].name.ToLower().StartsWith("f6_bullethell_flow")) {
                                m_fallbacklist.Add(ExpandDungeonFlow.KnownFlows[i]);
                            }
                        }
                    }
                    if (m_fallbacklist.Count > 0) {
                        if (m_fallbacklist.Count == 1) {
                            return m_fallbacklist[0];
                        } else {
                            return m_fallbacklist[BraveRandom.GenerationRandomRange(0, m_fallbacklist.Count)];
                        }
                    }
                    if (1 == 9) {
                        ETGModConsole.Log("[DEBUG] WARNING: Could not determine a proper fallback flow! Using a default flow instead!");
                    }
                    Debug.Log("WARNING: Could not determine a proper fallback flow! Using a default flow instead!");
                    return complex_flow_test.Complex_Flow_Test();
                }                
            }            
        }
        
        // Fix exception if Rat Corpse is kicked into a pit in a room that doesn't have TargetPitFallRoom setup.
        public IEnumerator HandlePitfallHook(Action<GetKicked, SpeculativeRigidbody>orig, GetKicked self, SpeculativeRigidbody srb) {
            FieldInfo field = typeof(GetKicked).GetField("m_isFalling", BindingFlags.Instance | BindingFlags.NonPublic);
            field.SetValue(self, true);
            RoomHandler firstRoom = srb.UnitCenter.GetAbsoluteRoom();
            TalkDoerLite talkdoer = srb.GetComponent<TalkDoerLite>();
            firstRoom.DeregisterInteractable(talkdoer);
            srb.Velocity = srb.Velocity.normalized * 0.125f;
            AIAnimator anim = srb.GetComponent<AIAnimator>();
            anim.PlayUntilFinished("pitfall", false, null, -1f, false);
            while (anim.IsPlaying("pitfall")) { yield return null; }
            anim.PlayUntilCancelled("kick1", false, null, -1f, false);
            srb.Velocity = Vector2.zero;        
            // if TargetPitfallRoom is null/not setup, we will choose a random room (or maintance room if rat fell in the elevator shaft in entrance room)    
            if (firstRoom.TargetPitfallRoom != null) {
                RoomHandler targetRoom = firstRoom.TargetPitfallRoom;
                Transform[] childTransforms = targetRoom.hierarchyParent.GetComponentsInChildren<Transform>(true);
                Transform arrivalTransform = null;
                for (int i = 0; i < childTransforms.Length; i++) {
                    if (childTransforms[i].name == "Arrival") { arrivalTransform = childTransforms[i]; break; }
                }
                if (arrivalTransform != null) {
                    srb.transform.position = arrivalTransform.position + new Vector3(1f, 1f, 0f);
                    srb.Reinitialize();
                    RoomHandler.unassignedInteractableObjects.Add(talkdoer);
                    yield break;
                } else {
                    if (1 == 9) {
                        ETGModConsole.Log("[DEBUG] Destination room does not have an Arrival object! Using a random location for the landing spot.");
                    }
                    Debug.Log("[HutongGames.PlayMaker.Actions.GetKicked] Destination room does not have an Arrival object! Using a random location for the landing spot.");
                    // if target room doesn't have arrival object, choose a random landing spot instead.
                    IntVector2? randomPosition = ExpandUtility.Instance.GetRandomAvailableCellSmart(targetRoom, new IntVector2(2, 2));
                    if (randomPosition != null && randomPosition.HasValue) {
                        srb.transform.position = randomPosition.Value.ToVector3(srb.transform.position.z);
                        srb.Reinitialize();
                        RoomHandler.unassignedInteractableObjects.Add(talkdoer);
                        field.SetValue(self, false);
                        yield break;
                    } else {
                        // Could not find a suitable location to place corpse. Let's just destroy it and call it a day. :P
                        Destroy(talkdoer.gameObject);
                        yield break;
                    }
                }                
            } else {
                if (1 == 9) {
                    ETGModConsole.Log("[DEBUG] Rat corpse fell into a pit that belonged to a room that didn't have TargetPitFallRoom setup! Using fall back method.");
                }
                Debug.Log("[HutongGames.PlayMaker.Actions.GetKicked] Rat corpse fell into a pit that belonged to a room that didn't have TargetPitFallRoom setup! Using fall back method instead.");
                if (GameManager.Instance.Dungeon.data.rooms != null && GameManager.Instance.Dungeon.data.rooms.Count > 0) {
                    RoomHandler startRoom = srb.UnitCenter.GetAbsoluteRoom();
                    RoomHandler randomTargetRoom = BraveUtility.RandomElement(GameManager.Instance.Dungeon.data.rooms);
                    RoomHandler maintanenceRoom = null;
                    if (startRoom == null) { Destroy(talkdoer.gameObject); yield break; }

                    for (int i = 0; i < GameManager.Instance.Dungeon.data.rooms.Count; i++) {
                        if (GameManager.Instance.Dungeon.data.rooms[i] != null &&
                            GameManager.Instance.Dungeon.data.rooms[i].GetRoomName() != null &&
                            GameManager.Instance.Dungeon.data.rooms[i].GetRoomName() != string.Empty)
                        {
                            if (GameManager.Instance.Dungeon.data.rooms[i].GetRoomName().ToLower().StartsWith("elevatormaintenance")) {
                                maintanenceRoom = GameManager.Instance.Dungeon.data.rooms[i];
                                break;
                            }
                        }                        
                    }
                    if (startRoom.GetRoomName().StartsWith(GameManager.Instance.Dungeon.data.Entrance.GetRoomName()) && maintanenceRoom != null) {
                        srb.transform.position = (new IntVector2(6, 6) + maintanenceRoom.area.basePosition).ToVector3(srb.transform.position.z);
                        srb.Reinitialize();
                        RoomHandler.unassignedInteractableObjects.Add(talkdoer);
                        field.SetValue(self, false);
                        yield break;
                    } else {
                        if (randomTargetRoom == null) { Destroy(talkdoer.gameObject); yield break; }
                        IntVector2? RandomPosition = ExpandUtility.Instance.GetRandomAvailableCellSmart(randomTargetRoom, new IntVector2(2, 2));
                        if (RandomPosition.HasValue) {
                            srb.transform.position = RandomPosition.Value.ToVector3(srb.transform.position.z);
                        } else {
                            Destroy(talkdoer.gameObject);
                            yield break;
                        }                        
                        srb.Reinitialize();
                        RoomHandler.unassignedInteractableObjects.Add(talkdoer);
                        field.SetValue(self, false);
                        yield break;
                    }
                } else {
                    Destroy(talkdoer.gameObject);
                    yield break;
                }
            }
        }

        // Allow AIActors to open doors. AIActors with IgnoreForRoomClear set will not be able to open doors in COOP. (to prevent companions from opening doors in COOP mode)
        public void CheckForPlayerCollisionHook(Action<DungeonDoorController, SpeculativeRigidbody, Vector2>orig, DungeonDoorController self, SpeculativeRigidbody otherRigidbody, Vector2 normal) {
            orig(self, otherRigidbody, normal);
            bool isSealed = ReflectionHelpers.ReflectGetField<bool>(typeof(DungeonDoorController), "isSealed", self);
            bool m_open = ReflectionHelpers.ReflectGetField<bool>(typeof(DungeonDoorController), "m_open", self);
            if (isSealed || self.isLocked) { return; }
            AIActor component = otherRigidbody.GetComponent<AIActor>();
            if (component != null && !m_open) {
                bool flipped = false;
                if (normal.y < 0f && self.northSouth) { flipped = true; }
                if (normal.x < 0f && !self.northSouth) { flipped = true; }                
                if (GameManager.Instance.CurrentGameType == GameManager.GameType.SINGLE_PLAYER) {
                    self.Open(flipped);
                } else if (!component.IgnoreForRoomClear) {
                    self.Open(flipped);
                }
            }
        }
        
        // Prevent Arrival Elevator from departing while room still has active enemies. (currently only relevent to custom Giant Elevator Room)
        // Used to prevent player from going down elevator shaft while there are still enemies to clear.
        public void ArrivalElevatorUpdateHook(Action<ElevatorArrivalController>orig, ElevatorArrivalController self) {
            
            if (!self.gameObject.transform.position.GetAbsoluteRoom().HasActiveEnemies(RoomHandler.ActiveEnemyType.RoomClear)) { orig(self); }

        }
        
        // Make the HellDragZone thing actually take player to direct to bullet hell instead of using normal DelayedLoadNextLevel().
        // Since if the EndTimes room is loaded from a different level other then Forge, this could cause issues. :P
       /* private IEnumerator HandleGrabbyGrabHook(Action<HellDragZoneController, PlayerController>orig, HellDragZoneController self, PlayerController grabbedPlayer) {
            FsmBool m_cryoBool = ReflectionHelpers.ReflectGetField<FsmBool>(typeof(HellDragZoneController), "m_cryoBool", self);
            grabbedPlayer.specRigidbody.Velocity = Vector2.zero;
            grabbedPlayer.specRigidbody.CapVelocity = true;
            grabbedPlayer.specRigidbody.MaxVelocity = Vector2.zero;
            yield return new WaitForSeconds(0.2f);
            grabbedPlayer.IsVisible = false;
            yield return new WaitForSeconds(2.3f);
            grabbedPlayer.specRigidbody.CapVelocity = false;
            Pixelator.Instance.FadeToBlack(0.5f, false, 0f);
            if (m_cryoBool != null && m_cryoBool.Value) {
                AkSoundEngine.PostEvent("Stop_MUS_All", self.gameObject);
                GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.HELLGEON);
                float delay = 0.6f;
                GameManager.Instance.DelayedLoadCharacterSelect(delay, true, true);
            } else {
                GameManager.DoMidgameSave(GlobalDungeonData.ValidTilesets.HELLGEON);
                GameManager.Instance.DelayedLoadCustomLevel(0.5f, "tt_bullethell");
            }
            yield break;
        }
        */
        public void DungeonStartHook(Action<ItemDB>orig, ItemDB self) {
            List<WeightedGameObject> collection;
            if (self.ModLootPerFloor.TryGetValue("ANY", out collection)) {
                GameManager.Instance.Dungeon.baseChestContents.defaultItemDrops.elements.AddRange(collection);
            }
            string dungeonFloorName = GameManager.Instance.Dungeon.DungeonFloorName;
            string key = string.Empty;
            try {
                key = dungeonFloorName.Substring(1, dungeonFloorName.IndexOf('_') - 1);
            } catch (Exception ex) {
                if (1 == 9) { ETGModConsole.Log("WARNING: dungoenFloorname.SubString() returned a negative or 0 length value!"); }
                Debug.Log("WARNING: dungoenFloorname.SubString() returned a negative or 0 length value!");
                Debug.LogException(ex);
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.PHOBOSGEON /*ExpandTheGungeon.isGlitchFloor*/) { key = "TEST"; } else { key = "???"; }
            }
            if (self.ModLootPerFloor.TryGetValue(key, out collection)) {
                GameManager.Instance.Dungeon.baseChestContents.defaultItemDrops.elements.AddRange(collection);
            }
        }        
        
        // Fix Pit size + make sure it only creates pit on the special room from Catacombs. Creating pit under all instances of sell pit makes selling items difficult post FTA update.
        public void SellPitConfigureOnPlacementHook(Action<SellCellController, RoomHandler>orig, SellCellController self, RoomHandler room) {
            if (room != null && room.GetRoomName().StartsWith("SubShop_SellCreep_CatacombsSpecial")) {
                for (int X = 0; X < self.GetWidth(); X++) {
                    for (int Y = 0; Y < self.GetHeight(); Y++) {
                        IntVector2 intVector = self.transform.position.IntXY(VectorConversions.Round) + new IntVector2(X, Y);
                        if (GameManager.Instance.Dungeon.data.CheckInBoundsAndValid(intVector)) {
                            CellData cellData = GameManager.Instance.Dungeon.data[intVector];
                            cellData.type = CellType.PIT;
                            cellData.fallingPrevented = true;
                        }
                    }
                }
            }
        }
        
        // Buff Disarming Personality Item. Players holding the item can now shoot in Bello's shop without causing him to get angry/vanish.
        // (does not prevent getting caught stealing though. :P )
        private void PlayerFiredHook(Action<BaseShopController>orig, BaseShopController self){

            foreach (PlayerController player in GameManager.Instance.AllPlayers) {
                if (player.IsFiring) {
                    if (!player.HasPassiveItem(187)) { orig(self); } 
                }
            }

        }

        private static GameObject GenerateRoomDoorMeshHook(RuntimeExitDefinition exit, RoomHandler room, DungeonData dungeonData) {
            try { 
                DungeonData.Direction directionFromRoom = exit.GetDirectionFromRoom(room);
                IntVector2 intVector;
                if (exit.upstreamRoom == room) {
                    intVector = exit.GetDownstreamBasePosition();
                } else {
                    intVector = exit.GetUpstreamBasePosition();
                }
                DungeonData.Direction exitDirection = directionFromRoom;
                IntVector2 exitBasePosition = intVector;         
                if (!string.IsNullOrEmpty(room.GetRoomName())) {
                    if (room.GetRoomName().StartsWith(ExpandRoomPrefabs.Expand_GlitchedSecret.name)) {
                        IntVector2 TilePositionOffset = (exitBasePosition + new IntVector2(1, 0));
                        IntVector2 TilePlacerSize = new IntVector2(8, 4);
                        int OffsetAmount = 1;
                        int OffsetAmount2 = 3;
                        if (exitDirection == DungeonData.Direction.NORTH) {
                            TilePositionOffset += new IntVector2(0, OffsetAmount);
                        } else if (exitDirection == DungeonData.Direction.SOUTH) {
                            TilePositionOffset += new IntVector2(0, OffsetAmount);
                        } else if (exitDirection == DungeonData.Direction.EAST) {
                            TilePositionOffset += new IntVector2(0, OffsetAmount2);
                            TilePlacerSize = new IntVector2(4, 8);
                        } else {
                            TilePositionOffset += new IntVector2(0, OffsetAmount2);
                            TilePositionOffset -= new IntVector2(OffsetAmount, 0);
                            TilePlacerSize = new IntVector2(4, 8);
                        }
                        GameObject m_SecretWallMesh = ExpandUtility.GenerateWallMesh(exitDirection, exitBasePosition, "secret room door object", dungeonData, false, true);
                        ExpandUtility.GenerateCorruptedTilesAtPosition(GameManager.Instance.Dungeon, exit.upstreamRoom, TilePositionOffset, TilePlacerSize, m_SecretWallMesh, 0.55f);
                        return m_SecretWallMesh;
                    } else {
                        return SecretRoomBuilder.GenerateWallMesh(exitDirection, exitBasePosition, "secret room door object", dungeonData, false);
                    }
                } else {
                    return SecretRoomBuilder.GenerateWallMesh(exitDirection, exitBasePosition, "secret room door object", dungeonData, false);
                }
            } catch (Exception ex) {
                if (1 == 9) {
                    ETGModConsole.Log("[ExpandTheGungeon] Warning: Exception caught in SecretRoomBuilder.GenerateRoomDoorMesh!");
                    Debug.Log(ex);
                }
                return new GameObject("NULL Secret Room Door") { layer = 0 };
            }
        }
        
        private void AnimationCompletedHook(Action<AIAnimator, tk2dSpriteAnimator, tk2dSpriteAnimationClip> orig, AIAnimator self, tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip) {
            try {
                orig(self, animator, clip);
            } catch (Exception ex) {
                if (1 == 9) {
                    ETGModConsole.Log("[ExpandTheGungeon] Warning: Exception caught in AIAnimator.AnimationCompleted on object '" + self.gameObject.name + "'!");
                    Debug.Log("[ExpandTheGungeon] Warning: Exception caught in AIAnimator.AnimationCompleted on object '" + self.gameObject.name + "'!");
                    Debug.LogException(ex);
                }
                return;
            }
        }

        // These hooks ensure our custom music/audio gets stopped properly.
        private void FlushMusicAudioHook(Action<GameManager>orig, GameManager self) {
            orig(self);
            AkSoundEngine.PostEvent("Stop_EX_MUS_All", self.gameObject);
        }

        private void SwitchToStateHook(Action<DungeonFloorMusicController, DungeonFloorMusicController.DungeonMusicState> orig, DungeonFloorMusicController self, DungeonFloorMusicController.DungeonMusicState targetState) {
            AkSoundEngine.PostEvent("Stop_EX_MUS_All", self.gameObject);
            orig(self, targetState);
        }

        private void FlushAudioHook(Action<GameManager> orig, GameManager self) {
            orig(self);
            AkSoundEngine.PostEvent("Stop_EX_SFX_All", self.gameObject);
        }

        // Add Null checks and use fall back GUIDs if it fails. Incase Drill tries to spawn custom enemies. (the GenerateEnemyData list returns null for some reason if it tries to pull a GUID tied to a custom enemy).
        private IEnumerator HandleCombatWavesHook(Action<PaydayDrillItem, Dungeon, RoomHandler, Chest>orig, PaydayDrillItem self, Dungeon d, RoomHandler newRoom, Chest sourceChest) {
            DrillWaveDefinition[] wavesToUse = self.D_Quality_Waves;
            switch (GameManager.Instance.RewardManager.GetQualityFromChest(sourceChest)) {
                case PickupObject.ItemQuality.C:
                    wavesToUse = self.C_Quality_Waves;
                    break;
                case PickupObject.ItemQuality.B:
                    wavesToUse = self.B_Quality_Waves;
                    break;
                case PickupObject.ItemQuality.A:
                    wavesToUse = self.A_Quality_Waves;
                    break;
                case PickupObject.ItemQuality.S:
                    wavesToUse = self.S_Quality_Waves;
                    break;
            }
            foreach (DrillWaveDefinition currentWave in wavesToUse) {
                int numEnemiesToSpawn = UnityEngine.Random.Range(currentWave.MinEnemies, currentWave.MaxEnemies + 1);
                for (int i = 0; i < numEnemiesToSpawn; i++) {
                    string EnemyGUID = d.GetWeightedProceduralEnemy().enemyGuid;
                    if (string.IsNullOrEmpty(EnemyGUID)) {
                        List<string> FallbackGUIDs = new List<string>() {
                            ExpandCustomEnemyDatabase.BootlegBullatGUID,
                            ExpandCustomEnemyDatabase.BootlegBulletManGUID,
                            ExpandCustomEnemyDatabase.BootlegBulletManBandanaGUID,
                            ExpandCustomEnemyDatabase.BootlegShotgunManBlueGUID,
                            ExpandCustomEnemyDatabase.BootlegShotgunManRedGUID
                        };
                        FallbackGUIDs = FallbackGUIDs.Shuffle();
                        EnemyGUID = BraveUtility.RandomElement(FallbackGUIDs);
                    }
                    AddSpecificEnemyToRoomProcedurallyFixed(newRoom, EnemyGUID, true);
                }
                yield return new WaitForSeconds(3f);
                while (newRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.RoomClear) > 0) {
                    yield return new WaitForSeconds(1f);
                }
                if (newRoom.GetActiveEnemiesCount(RoomHandler.ActiveEnemyType.All) > 0) {
                    List<AIActor> activeEnemies = newRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                    for (int j = 0; j < activeEnemies.Count; j++) {
                        if (activeEnemies[j].IsNormalEnemy) { activeEnemies[j].EraseFromExistence(false); }
                    }
                }
            }
            yield break;
        }

        // This function doesn't null check orLoadByGuid. If non fake prefab custom enemies are spawned (like the special rats on Hollow), then this would cause exception.
        // Added fall back GUIDs and use one of those for AIActor instead if this happens.
        public void AddSpecificEnemyToRoomProcedurallyFixed(RoomHandler room, string enemyGuid, bool reinforcementSpawn = false, Vector2? goalPosition = null) {
            AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(enemyGuid);
            if (!orLoadByGuid) {
                List<string> FallbackGUIDs = new List<string>() {
                            ExpandCustomEnemyDatabase.BootlegBullatGUID,
                            ExpandCustomEnemyDatabase.BootlegBulletManGUID,
                            ExpandCustomEnemyDatabase.BootlegBulletManBandanaGUID,
                            ExpandCustomEnemyDatabase.BootlegShotgunManBlueGUID,
                            ExpandCustomEnemyDatabase.BootlegShotgunManRedGUID
                };
                FallbackGUIDs = FallbackGUIDs.Shuffle();
                orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(BraveUtility.RandomElement(FallbackGUIDs));
            }
            IntVector2 clearance = orLoadByGuid.specRigidbody.UnitDimensions.ToIntVector2(VectorConversions.Ceil);
            CellValidator cellValidator = delegate (IntVector2 c) {
                for (int i = 0; i < clearance.x; i++) {
                    int x = c.x + i;
                    for (int j = 0; j < clearance.y; j++) {
                        int y = c.y + j;
                        if (GameManager.Instance.Dungeon.data.isTopWall(x, y)) { return false; }
                    }
                }
                return true;
            };
            IntVector2? intVector;
            if (goalPosition != null) {
                intVector = room.GetNearestAvailableCell(goalPosition.Value, new IntVector2?(clearance), new CellTypes?(CellTypes.FLOOR), false, cellValidator);
            } else {
                intVector = room.GetRandomAvailableCell(new IntVector2?(clearance), new CellTypes?(CellTypes.FLOOR), false, cellValidator);
            }
            if (intVector != null) {
                AIActor aiactor = AIActor.Spawn(orLoadByGuid, intVector.Value, room, true, AIActor.AwakenAnimationType.Spawn, false);
                if (aiactor && reinforcementSpawn) {
                    if (aiactor.specRigidbody) { aiactor.specRigidbody.CollideWithOthers = false; }
                    aiactor.HandleReinforcementFallIntoRoom(0f);
                }
            } else {
                Debug.LogError("failed placement");
            }
        }

        private void TeardownPunchout_Hook(Action<PunchoutController> orig, PunchoutController self) {
            if (!GameStatsManager.Instance.IsRainbowRun) { orig(self); return; }
            
            if (ReflectionHelpers.ReflectGetField<bool>(typeof(PunchoutController), "m_isInitialized", self)) {
                Minimap.Instance.TemporarilyPreventMinimap = false;
                GameUIRoot.Instance.ShowCoreUI("punchout");
                GameUIRoot.Instance.ShowCoreUI(string.Empty);
                GameUIRoot.Instance.ToggleLowerPanels(true, false, string.Empty);
                CameraController mainCameraController = GameManager.Instance.MainCameraController;
                mainCameraController.SetManualControl(false, false);
                mainCameraController.LockToRoom = false;
                mainCameraController.SetZoomScaleImmediate(1f);
                foreach (PlayerController playerController in GameManager.Instance.AllPlayers) {
                    playerController.ClearInputOverride("punchout");
                    playerController.healthHaver.IsVulnerable = true;
                    playerController.SuppressEffectUpdates = false;
                    playerController.IsOnFire = false;
                    playerController.CurrentFireMeterValue = 0f;
                    playerController.CurrentPoisonMeterValue = 0f;
                }
                GameManager.Instance.DungeonMusicController.EndBossMusic();
                MetalGearRatRoomController metalGearRatRoomController = FindObjectOfType<MetalGearRatRoomController>();
                if (metalGearRatRoomController) {
                    GameObject gameObject = PickupObjectDatabase.GetById(GlobalItemIds.RatKey).gameObject;
                    Vector3 position = metalGearRatRoomController.transform.position;
                    if (self.Opponent.NumKeysDropped >= 1) {
                        LootEngine.SpawnItem(gameObject, position + new Vector3(14.25f, 17f), Vector2.zero, 0f, true, false, false);
                    }
                    if (self.Opponent.NumKeysDropped >= 2) {
                        LootEngine.SpawnItem(gameObject, position + new Vector3(13.25f, 14.5f), Vector2.zero, 0f, true, false, false);
                    }
                    if (self.Opponent.NumKeysDropped >= 3) {
                        LootEngine.SpawnItem(gameObject, position + new Vector3(14.25f, 12f), Vector2.zero, 0f, true, false, false);
                    }
                    if (self.Opponent.NumKeysDropped >= 4) {
                        LootEngine.SpawnItem(gameObject, position + new Vector3(30.25f, 17f), Vector2.zero, 0f, true, false, false);
                    }
                    if (self.Opponent.NumKeysDropped >= 5) {
                        LootEngine.SpawnItem(gameObject, position + new Vector3(31.25f, 14.5f), Vector2.zero, 0f, true, false, false);
                    }
                    if (self.Opponent.NumKeysDropped >= 6) {
                        LootEngine.SpawnItem(gameObject, position + new Vector3(30.25f, 12f), Vector2.zero, 0f, true, false, false);
                    }
                    Vector2 a = position + new Vector3(22.25f, 14.5f);

                    List<int> m_AllowedItemsInRainbowMode = new List<int>() {
                        GlobalItemIds.SmallHeart,
                        GlobalItemIds.FullHeart,
                        GlobalItemIds.AmmoPickup,
                        GlobalItemIds.SpreadAmmoPickup,
                        GlobalItemIds.Spice,
                        GlobalItemIds.Junk,
                        GlobalItemIds.GoldJunk,
                        GlobalItemIds.Key,
                        GlobalItemIds.GlassGuonStone,
                        GlobalItemIds.Junk,
                        GlobalItemIds.GoldJunk,
                        GlobalItemIds.SackKnightBoon,
                        GlobalItemIds.Blank,
                        GlobalItemIds.RatKey,
                        GlobalItemIds.Map,
                        120, // armor
                    };

                    foreach (int id in self.Opponent.DroppedRewardIds) {
                        float degrees = ((!BraveUtility.RandomBool()) ? 180 : 0) + UnityEngine.Random.Range(-30f, 30f);
                        // RoomHandler m_room = self.gameObject.transform.position.GetAbsoluteRoom();
                        RoomHandler m_room = GameManager.Instance.PrimaryPlayer.transform.position.GetAbsoluteRoom();                        
                        if (m_AllowedItemsInRainbowMode.Contains(id)) {
                            GameObject gameObject2 = PickupObjectDatabase.GetById(id).gameObject;
                            LootEngine.SpawnItem(gameObject2, (a + new Vector2(11f, 0f).Rotate(degrees)), Vector2.zero, 0f, true, false, false);
                        } else {
                            if (m_room != null && GameManager.Instance.RewardManager.BowlerNoteOtherSource) {
                                string[] CustomText = new string[] {
                                    "Doesn't look like this rat stole this from a {wb}Rainbow Chest{w}.\n\nNo RAAAAAIIIINBOW, no item!\n\n{wb}-Bowler{w}",
                                    "Rats are GROOOOOOSS!\n\n{wb}-Bowler{w}"
                                };
                                ExpandUtility.SpawnCustomBowlerNote(GameManager.Instance.RewardManager.BowlerNoteOtherSource, (a + new Vector2(11f, 0f).Rotate(degrees)), m_room, BraveUtility.RandomElement(CustomText), false);
                            }
                        }
                    }
                }
                GameStatsManager.Instance.SetFlag(GungeonFlags.ITEMSPECIFIC_BOXING_GLOVE, true);
                BraveTime.ClearMultiplier(self.Player.gameObject);
                Destroy(self.gameObject);
            } else {
                self.Reset();                
            }            
        }


        private void BuildOcclusionLayerCenterJungleHook(TK2DDungeonAssembler self, CellData current, Dungeon d, tk2dTileMap map, int ix, int iy) {
            if (current == null | !d | !map) { return; }

            if (!IsValidJungleOcclusionCell(self, current, d, ix, iy)) { return; }
            bool flag = true;
            bool flag2 = true;
            if (!self.BCheck(d, ix, iy)) { flag = false; flag2 = false; }
            if (current.UniqueHash < 0.05f) { flag = false; flag2 = false; }
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (!IsValidJungleOcclusionCell(self, d.data[ix + i, iy + j], d, ix + i, iy + j)) {
                        flag2 = false;
                        if (i < 2 || j < 2) { flag = false; }
                    }
                    if (!flag2 && !flag) { break; }
                }
                if (!flag2 && !flag) { break; }
            }
            if (flag2 && current.UniqueHash < 0.75f) {
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 352);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y, 353);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y, 354);
                d.data[ix + 1, iy].cellVisualData.occlusionHasBeenProcessed = true;
                d.data[ix + 2, iy].cellVisualData.occlusionHasBeenProcessed = true;
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, 330);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1, 331);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y + 1, 332);
                d.data[ix, iy + 1].cellVisualData.occlusionHasBeenProcessed = true;
                d.data[ix + 1, iy + 1].cellVisualData.occlusionHasBeenProcessed = true;
                d.data[ix + 2, iy + 1].cellVisualData.occlusionHasBeenProcessed = true;
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 2, 308);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 2, 309);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y + 2, 310);
                d.data[ix, iy + 2].cellVisualData.occlusionHasBeenProcessed = true;
                d.data[ix + 1, iy + 2].cellVisualData.occlusionHasBeenProcessed = true;
                d.data[ix + 2, iy + 2].cellVisualData.occlusionHasBeenProcessed = true;
            } else if (flag) {
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 418);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y, 419);
                d.data[ix + 1, iy].cellVisualData.occlusionHasBeenProcessed = true;
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, 396);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1, 397);
                d.data[ix, iy + 1].cellVisualData.occlusionHasBeenProcessed = true;
                d.data[ix + 1, iy + 1].cellVisualData.occlusionHasBeenProcessed = true;
            } else {
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 374);
            }
            d.data[ix, iy].cellVisualData.occlusionHasBeenProcessed = true;
        }

        private void BuildBorderLayerCenterJungleHook(TK2DDungeonAssembler self, CellData current, Dungeon d, tk2dTileMap map, int ix, int iy) {
            if (current == null | !d | !map) { return; }

            if (!IsValidJungleBorderCell(current, d, ix, iy)) { return; }
            bool flag = true;
            bool flag2 = true;
            if (!self.BCheck(d, ix, iy)) { flag = false; flag2 = false; }
            if (current.UniqueHash < 0.05f) { flag = false; flag2 = false; }
            for (int i = 0; i < 3; i++) {
                for (int j = 0; j < 3; j++) {
                    if (!IsValidJungleBorderCell(d.data[ix + i, iy + j], d, ix + i, iy + j)) {
                        flag2 = false;
                        if (i < 2 || j < 2) { flag = false; }
                    }
                    if (!flag2 && !flag) { break; }
                }
                if (!flag2 && !flag) { break; }
            }
            if (flag2 && current.UniqueHash < 0.75f) {
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 352);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 352);
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y, 353);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y, 353);
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y, 354);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y, 354);
                d.data[ix + 1, iy].cellVisualData.ceilingHasBeenProcessed = true;
                d.data[ix + 2, iy].cellVisualData.ceilingHasBeenProcessed = true;
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, 330);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, 330);
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1, 331);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1, 331);
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y + 1, 332);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y + 1, 332);
                d.data[ix, iy + 1].cellVisualData.ceilingHasBeenProcessed = true;
                d.data[ix + 1, iy + 1].cellVisualData.ceilingHasBeenProcessed = true;
                d.data[ix + 2, iy + 1].cellVisualData.ceilingHasBeenProcessed = true;
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 2, 308);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 2, 308);
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 2, 309);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 2, 309);
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y + 2, 310);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 2, current.positionInTilemap.y + 2, 310);
                d.data[ix, iy + 2].cellVisualData.ceilingHasBeenProcessed = true;
                d.data[ix + 1, iy + 2].cellVisualData.ceilingHasBeenProcessed = true;
                d.data[ix + 2, iy + 2].cellVisualData.ceilingHasBeenProcessed = true;
            } else if (flag) {
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 418);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 418);
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y, 419);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y, 419);
                d.data[ix + 1, iy].cellVisualData.ceilingHasBeenProcessed = true;
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, 396);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y + 1, 396);
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1, 397);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x + 1, current.positionInTilemap.y + 1, 397);
                d.data[ix, iy + 1].cellVisualData.ceilingHasBeenProcessed = true;
                d.data[ix + 1, iy + 1].cellVisualData.ceilingHasBeenProcessed = true;
            } else {
                map.Layers[GlobalDungeonData.borderLayerIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 374);
                map.Layers[GlobalDungeonData.occlusionPartitionIndex].SetTile(current.positionInTilemap.x, current.positionInTilemap.y, 374);
            }
            d.data[ix, iy].cellVisualData.ceilingHasBeenProcessed = true;
        }
        
        private bool IsValidJungleBorderCell(CellData current, Dungeon d, int ix, int iy) {
            bool isValid = false;
            try {
                isValid = !current.cellVisualData.ceilingHasBeenProcessed && !IsCardinalBorder(current, d, ix, iy) && current.type == CellType.WALL && (iy < 2 || !d.data.isFaceWallLower(ix, iy)) && !d.data.isTopDiagonalWall(ix, iy);
            } catch (Exception ex) {
                if (1 == 9) {
                    Debug.Log("[ExpandTheGungeon] Excpetion caught in TK2DDungeonAssembler.IsValidJungleBorderCell!");
                    Debug.LogException(ex);
                }
                return false;
            }
            return isValid;
        }

        private bool IsValidJungleOcclusionCell(TK2DDungeonAssembler assembler, CellData current, Dungeon d, int ix, int iy) {
            bool isValid = false;
            try {
                isValid = assembler.BCheck(d, ix, iy, 1) && (!current.cellVisualData.ceilingHasBeenProcessed && !current.cellVisualData.occlusionHasBeenProcessed) && (current.type != CellType.WALL || IsCardinalBorder(current, d, ix, iy) || (iy > 2 && (d.data.isFaceWallLower(ix, iy) || d.data.isFaceWallHigher(ix, iy))));
            } catch (Exception ex) {
                if (1 == 9) {
                    Debug.Log("[ExpandTheGungeon] Excpetion caught in TK2DDungeonAssembler.IsValidJungleOcclusionCell!");
                    Debug.LogException(ex);
                }
                return false;
            }
            return isValid;
        }

        private bool IsCardinalBorder(CellData current, Dungeon d, int ix, int iy) {
            bool flag = d.data.isTopWall(ix, iy);
            flag = (flag && !d.data[ix, iy + 1].cellVisualData.shouldIgnoreBorders);
            bool flag2 = (!d.data.isWallRight(ix, iy) && !d.data.isRightTopWall(ix, iy)) || d.data.isFaceWallHigher(ix + 1, iy) || d.data.isFaceWallLower(ix + 1, iy);
            flag2 = (flag2 && !d.data[ix + 1, iy].cellVisualData.shouldIgnoreBorders);
            bool flag3 = iy > 3 && d.data.isFaceWallHigher(ix, iy - 1);
            flag3 = (flag3 && !d.data[ix, iy - 1].cellVisualData.shouldIgnoreBorders);
            bool flag4 = (!d.data.isWallLeft(ix, iy) && !d.data.isLeftTopWall(ix, iy)) || d.data.isFaceWallHigher(ix - 1, iy) || d.data.isFaceWallLower(ix - 1, iy);
            flag4 = (flag4 && !d.data[ix - 1, iy].cellVisualData.shouldIgnoreBorders);
            return flag || flag2 || flag3 || flag4;
        }

    }
}
