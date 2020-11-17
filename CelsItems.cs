﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ItemAPI;
using GungeonAPI;
using MonoMod.RuntimeDetour;
using System.Reflection;
using UnityEngine;
using Items.Keywords;
using Random = UnityEngine.Random;
using Dungeonator;

namespace Items
{
    public class CelsItems : ETGModule
    {
        public static readonly string Modname = "Cel's Items";
        public static readonly string Version = "3.2";
        public static readonly string Color = "#FCA4E2";
        public static HellDragZoneController hellDrag;
        public override void Init()
        {

        }

        public override void Start()
        {
            
            ItemAPI.FakePrefabHooks.Init();
            GungeonAPI.FakePrefabHooks.Init();
            ItemBuilder.Init();
            Hooks.Init();
            GungeonAPI.Tools.Init();            
            ShrineFactory.Init();
            var forgeDungeon = DungeonDatabase.GetOrLoadByName("Base_Forge");
            hellDrag = forgeDungeon.PatternSettings.flows[0].AllNodes.Where(node => node.overrideExactRoom != null && node.overrideExactRoom.name.Contains("EndTimes")).First().overrideExactRoom.placedObjects.Where(ppod => ppod != null && ppod.nonenemyBehaviour != null).First().nonenemyBehaviour.gameObject.GetComponentsInChildren<HellDragZoneController>()[0];
            forgeDungeon = null;
            MultiActiveReloadManager.SetupHooks();
            TheBullet.Init(); //1                ACTIVE 1
            FunkyBullets.Init(); //2             PASSIVE 1
            NeedleBullets.Init(); //3            PASSIVE 2
            ImpsHorn.Init(); //4                 PASSIVE 3
            SnareBullets.Init(); //5             PASSIVE 4
            CorruptHeart.Init(); //6             ACTIVE 2
            ShockingBattery.Init(); //7          ACTIVE 3
            Dispenser.Add(); //8                 GUN 1
            IronHeart.Init(); //9                ACTIVE 4 
            VoidBottle.Init(); //10              ACTIVE 5
            SprunButBetter.Init(); //11          PASSIVE 5
            PetRock.Init(); //12                 PASSIVE 6
            RGG.Add(); //13                      GUN 2
            Phazor.Add(); //14                   GUN 3
            Tesla.Add(); //15                    GUN 4
            Fallout.Add(); //16                  GUN 5
            SuperCrate.Init(); //17              ACTIVE 6
            SteamSale.Init(); //18               PASSIVE 7
            EmptyBullets.Init(); //19            PASSIVE 8
            Sawblade.Init(); // 20               PASSIVE 9
            Matchbox.Init(); //21                ACTIVE 7
            ACMESupply.Init(); //22              ACTIVE 8
            Mininomicon.Init(); //23             PASSIVE 10
            TerribleAmmoBag.Init(); //24         PASSIVE 11
            Questlog.Init();  //25               ACTIVE 9
            Gunlust.Init(); //26                 PASSIVE 12
            BananaJamHands.Init(); //27          PASSIVE 13
            LiteralTrash.Init();              // MISC 1
            Enemies.SpectreBehav();
            DaggerSpray.Init(); // 28            PASSIVE 14
            AuricVial.Init(); //29               PASSIVE 15
            StakeLauncher.Add(); //30            GUN 6
            DragunClaw.Init(); //31              PASSIVE 16
            DragunWing.Init(); //32              PASSIVE 17
            DragunHeart.Init(); //33             PASSIVE 18         
            DragunSkull.Init(); //34             PASSIVE 19
            GravityWell.Init(); //35             PASSIVE 20
            WaterCup.Init(); //36                ACTIVE 10
            SpinelTonic.Init(); //37             ACTIVE 11
            DDR.Init(); //38                     PASSIVE 21
            CitrineAmmolet.Init(); // 39         PASSIVE 22
            MalachiteAmmolet.Init(); //40        PASSIVE 23
            GlassHeart.Init(); // 41             PASSIVE 24
            AccursedShackles.Init();// 42        PASSIVE 25   
            Hooks.BuffGuns(); //43               GUN 7
            D6.Init(); //44                      ACTIVE 12

            TimeKeepersPistol.Add();                
            ALiteralRock.Add(); // 47            GUN 8 
            FloopBullets.Init(); // 48           PASSIVE 26     
            EliteBullets.Init();// 49            PASSIVE 27
            Enemies.AmmoDotBehav();
            FireworkRifle.Add(); // 50           GUN 9
            Skeleton.Add();// 51                 GUN 10
            Incremental.Add();// 52              GUN 11
          
            VenomRounds.Init();// 53             PASSIVE 28
            BismuthAmmolet.Init();//54           PASSIVE 29
            MercuryAmmolet.Init();//55           PASSIVE 30
            

            BashelliskRifle.Add();//56           GUN 12
            MarkOfWind.Init();//57               PASSIVE 31
            PrimeSaw.Add();//58a                 GUN 13a                      
            PrimeVice.Add();//58b                GUN 13b
            PrimeLaser.Add();//58c               GUN 13c
            PrimeCannon.Add();//58d              GUN 13d
            RussianRevolver.Add();//59           GUN 14
            ImpactRounds.Init();//60             PASSIVE 31
            DroneController.Add();//61           GUN 15
            Drone.Add();
            Drone2.Add();
            //D.E.A.T.H. Set
            LittleDroneBuddy.Init();//62         PASSIVE 32
            DroneBuddy1Gun.Add();//62a
            TriggerPulseDrone.Init();//63        PASSIVE 33
            TriggerDroneGun.Add();//63a
            VenomSpitDrone.Init();//64a          PASSIVE 34
            VenomSpitDGun.Add();//64b
            DEATHGun.Add();//65a                 GUN 16
            DEATHDrone.Add();//65b
            //True Gunpowder Set
            PrimalNitricAcid.Init();//66         PASSIVE 35
            PrimalCharcoal.Init();//67           PASSIVE 36
            PrimalSulfur.Init();//68             PASSIVE 37
            PrimalSaltpeter.Init();//69          PASSIVE 38
            TrueGunpowder.Init();//70            PASSIVE 39
            //
            SpiritOfTheDragun.Add();//71         GUN 17
            BilliardBouncer.Add();//72           GUN 18
            ElectricBass.Add();//73              GUN 19
            IonFist.Add();//74                   GUN 20
            NenFist.Add();//74a
            EvilCharmedBow.Add();
            ReloadedRifle.Add();//75             GUN 21
            AK94.Add();//76                      GUN 22
            LeakingSyringe.Init();//77           PASSIVE 40
            Holoprinter.Init();//78              ACTIVE 14
            Günther.Add();//79                   GUN 23
            //SPINEL TONIC BY TankTheta!
            //Bismuth Ammolet sprite by TankTheta!
            //Spirit spride by TankTheta!
            //Hooks.HardmodeTweaks();
            //Thanks to ExplosivePanda for helping with the code for Soul Forge
            DualGunsManager.AddDual();
            HoveringGunsAdder.AddHovers();
            SynergyFormAdder.AddForms();
            //MunitionsChestController.Init();
            foreach (ETGModule etgmodule in ETGMod.GameMods)
            {
                bool flag = etgmodule is CelsItems;
                if (flag)
                {
                    this.HasCel = true;
                }
            }
            foreach (AdvancedSynergyEntry syn in GameManager.Instance.SynergyManager.synergies)
            {
                if(syn.NameKey == "#IRONSHOT")
                {
                    syn.OptionalGunIDs.Add(ETGMod.Databases.Items["billiard_bouncer"].PickupObjectId);
                }
            }
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.ExtravaganceSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.RulebookSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.SturdyAmmoSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.LeadWeightSyn()
            }).ToArray<AdvancedSynergyEntry>();
        /*    GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.TippedPoiSyn()
            }).ToArray<AdvancedSynergyEntry>();
            */
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.DragunRoarSyn()
            }).ToArray<AdvancedSynergyEntry>();
            /*  GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
              {
                 new CelSynergies.TrueDragunfireSyn()
              }).ToArray<AdvancedSynergyEntry>(); */
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.IronManSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.EldritchLoveSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.TwoForOneSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.CriticalMassSyn()
            }).ToArray<AdvancedSynergyEntry>();
         /*   GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.RerollSyn()
            }).ToArray<AdvancedSynergyEntry>();
            */
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.OverclockedSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.SovietSkillsSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.CommunistIdealsSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.LuckySpinSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.TestGunSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.DEATHGunSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.TrueGunpowderSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.GoldenSpinSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.LoveLeechSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.JajankenSyn()
            }).ToArray<AdvancedSynergyEntry>();
            GameManager.Instance.SynergyManager.synergies = GameManager.Instance.SynergyManager.synergies.Concat(new AdvancedSynergyEntry[]
            {
               new CelSynergies.AllOutOfLoveSyn()
            }).ToArray<AdvancedSynergyEntry>();
            /* foreach(AdvancedSynergyEntry entry in GameManager.Instance.SynergyManager.synergies)
             {
                 bool flag1 = entry.NameKey == "#IRONSTANCE";
                 if (flag1)
                 {
                     entry.OptionalGunIDs.Add(ETGMod.Databases.Items["billiard_bouncer"].PickupObjectId);
                 }
             }
             */
            //ETGModConsole.Commands.AddUnit("m_chest", delegate (string[] args)
            // {
            //     Chest.Spawn(MunitionsChestController.MChest, (GameManager.Instance.PrimaryPlayer.CenterPosition + new Vector2(1f, 0f)).ToIntVector2(VectorConversions.Round));
            // });

            Log($"{Modname} v{Version} started successfully.", Color);
            Log($"Link to Changelog https://pastebin.com/TPvwpdGJ", Color);
        }
        public static void Log(string text, string color = "FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }


        public override void Exit()
        {
            
        }
        
        
        private bool HasCel = false;
        private bool HasRtr = false;

        //written by @UnstableStrafe#3928 with help from KyleTheScientist, Neighborino, Glorfindel, Retrash, Reto, TheTurtleMelon, TankTheta, Spapi, and Eternal Frost
    }
}
