﻿using Base.Defs;
using Base.UI;
using PhoenixPoint.Common.Core;
using PhoenixPoint.Common.Entities.GameTags;
using PhoenixPoint.Common.Entities.Items;
using PhoenixPoint.Common.UI;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Levels;
using PhoenixPoint.Tactical.Entities;
using PhoenixPoint.Tactical.Entities.Abilities;
using PhoenixPoint.Tactical.Entities.Equipments;
using PhoenixPoint.Tactical.Entities.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TFTV
{
    internal class TFTVStarts
    {

        private static readonly DefRepository Repo = TFTVMain.Repo;

        public static void CreateNewDefsForTFTVStart()
        {
            try
            {
                CreateNewSophiaAndJacob();
                CreateInitialInfiltrator();
                CreateInitialPriest();
                CreateInitialTechnician();
                CreateStartingTemplatesBuffed();
            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }

        }

        public static void ModifyIntroForSpecialStart(GeoFaction geoFaction, GeoSite site)
        {
            try
            {
                GeoscapeEventDef intro = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("IntroBetterGeo_0"));
                intro.GeoscapeEventData.Description[0].General = new LocalizedTextBind("After all these years, you finally got the call. It meant that Symes and his deputies were dead or unreachable. Phoenix Project had gone dark. " +
                    "You spent many years waiting, dreading for it to happen, leading a discrete existence at " + FindNearestHaven(geoFaction, site) + ", a " + geoFaction.Name.LocalizeEnglish()
                    + " haven.\n\nThe trek to Phoenix Point was long and dangerous, and you wouldn't have made it without " + geoFaction.GeoLevel.PhoenixFaction.Vehicles.First().Soldiers.Last().DisplayName + ", a " + geoFaction.Name.LocalizeEnglish()
                    + " " + geoFaction.GeoLevel.PhoenixFaction.Vehicles.First().Soldiers.Last().ClassTag.className + ".\n\nBut when you reached Phoenix Point, somebody was already home...", true);

            }


            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }

        }


        public static string FindNearestHaven(GeoFaction geoFaction, GeoSite phoenixPoint)
        {
            try
            {


                IOrderedEnumerable<GeoSite> orderedEnumerable = from s in geoFaction.GeoLevel.Map.GetConnectedSitesOfType_Land(phoenixPoint, GeoSiteType.Haven, activeOnly: false)
                                                                orderby GeoMap.Distance(phoenixPoint, s)
                                                                select s;
                foreach (GeoSite geoHaven in orderedEnumerable)
                {
                    if (geoHaven.Owner == geoFaction)
                    {
                        geoHaven.RevealSite(geoFaction.GeoLevel.PhoenixFaction);
                        return geoHaven.Name;
                    }
                }
            }

            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
            throw new InvalidOperationException();
        }

        public static void CreateStartingTemplatesBuffed() 
        {
            TacCharacterDef Jacob2 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Jacob_Tutorial2_TacCharacterDef"));
            TacCharacterDef Sophia2 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Sophia_Tutorial2_TacCharacterDef"));
            TacCharacterDef Omar3 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Omar_Tutorial3_TacCharacterDef"));
            TacCharacterDef Takeshi3 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Takeshi_Tutorial3_TacCharacterDef"));
            TacCharacterDef Irina3 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Irina_Tutorial3_TacCharacterDef"));

            TacCharacterDef newJacobBuffed = Helper.CreateDefFromClone(Jacob2, "B1968124-ABDD-4A2C-9CBC-33DBC0EE3EE5", "PX_JacobBuffed_TFTV_TacCharacterDef");
            TacCharacterDef newSophiaBuffed = Helper.CreateDefFromClone(Sophia2, "B3EA411B-DE35-4B63-874A-553D816C06BC", "PX_SophiaBuffed_TFTV_TacCharacterDef");
            TacCharacterDef newOmarBuffed = Helper.CreateDefFromClone(Omar3, "024AB8C6-A2CD-4B81-A927-C8713A008EF2", "PX_OmarBuffed_TFTV_TacCharacterDef");
            TacCharacterDef newTakeshiBuffed = Helper.CreateDefFromClone(Takeshi3, "4230D9B4-6D88-4545-8680-BBCAE463356B", "PX_TakeshiBuffed_TFTV_TacCharacterDef");
            TacCharacterDef newIrinaBuffed = Helper.CreateDefFromClone(Irina3, "8E57C25C-7289-4F7C-9D0C-5F8E55601B49", "PX_IrinaBuffed_TFTV_TacCharacterDef");
        }
        public static void CreateNewSophiaAndJacob()
        {
            try
            {
                TacCharacterDef Jacob2 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Jacob_Tutorial2_TacCharacterDef"));
                TacCharacterDef Sophia2 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Sophia_Tutorial2_TacCharacterDef"));

                TacCharacterDef newJacob = Helper.CreateDefFromClone(Jacob2, "DDA13436-40BE-4096-9C69-19A3BF6658E6", "PX_Jacob_TFTV_TacCharacterDef");
                TacCharacterDef newSophia = Helper.CreateDefFromClone(Sophia2, "D9EC7144-6EB5-451C-9015-3E67F194AB1B", "PX_Sophia_TFTV_TacCharacterDef");

                newJacob.Data.ViewElementDef = Repo.GetAllDefs<ViewElementDef>().First(ved => ved.name.Equals("E_View [PX_Sniper_ActorViewDef]"));
                GameTagDef Sniper_CTD = Repo.GetAllDefs<GameTagDef>().First(gtd => gtd.name.Equals("Sniper_ClassTagDef"));
                for (int i = 0; i < newJacob.Data.GameTags.Length; i++)
                {
                    if (newJacob.Data.GameTags[i].GetType() == Sniper_CTD.GetType())
                    {
                        newJacob.Data.GameTags[i] = Sniper_CTD;
                    }
                }

                newJacob.Data.Abilites = new TacticalAbilityDef[] // abilities -> Class proficiency
                {
                Repo.GetAllDefs<ClassProficiencyAbilityDef>().First(cpad => cpad.name.Equals("Sniper_ClassProficiency_AbilityDef"))

                };
                newJacob.Data.BodypartItems = new ItemDef[] // Armour
                {
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Equals("PX_Sniper_Helmet_BodyPartDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Equals("PX_Sniper_Torso_BodyPartDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Equals("PX_Sniper_Legs_ItemDef"))
                };

                newJacob.Data.EquipmentItems = new ItemDef[] // Ready slots
               { Repo.GetAllDefs<WeaponDef>().First(wd => wd.name.Equals("PX_SniperRifle_WeaponDef")),
                    Repo.GetAllDefs<WeaponDef>().First(wd => wd.name.Equals("PX_Pistol_WeaponDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Equals("Medkit_EquipmentDef"))
               };
                newJacob.Data.InventoryItems = new ItemDef[] // Backpack
                {
                newJacob.Data.EquipmentItems[0].CompatibleAmmunition[0],
                newJacob.Data.EquipmentItems[1].CompatibleAmmunition[0]
                };

                newJacob.Data.Strength = 0;
                newJacob.Data.Will = 0;
                newJacob.Data.Speed = 0;
                newJacob.Data.CurrentHealth = -1;

                newSophia.Data.Strength = 0;
                newSophia.Data.Will = 0;
                newSophia.Data.Speed = 0;
                newSophia.Data.CurrentHealth = -1;            
            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
        }

        //Adapted from MadSkunky's TutorialTweaks: https://github.com/MadSkunky/PP-Mods-TutorialTweaks
        public static List<TacCharacterDef> SetInitialSquadUnbuffed(GeoLevelController level)
        {
            try
            {
                TFTVConfig config = TFTVMain.Main.Config;

                List<TacCharacterDef> startingTemplates = new List<TacCharacterDef>();

                TacCharacterDef newJacob = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Jacob_TFTV_TacCharacterDef"));
                TacCharacterDef newSophia = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Sophia_TFTV_TacCharacterDef"));

                TacCharacterDef assault = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_AssaultStarting_TacCharacterDef"));
                TacCharacterDef heavy = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_HeavyStarting_TacCharacterDef"));
                TacCharacterDef sniper = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_SniperStarting_TacCharacterDef"));

                TacCharacterDef priest = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_Starting_Priest_TacCharacterDef"));
                TacCharacterDef technician = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_Starting_Technician_TacCharacterDef"));
                TacCharacterDef infiltrator = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_Starting_Infiltrator_TacCharacterDef"));

                startingTemplates.Add(newSophia);
                startingTemplates.Add(newJacob);

                if (config.startingSquad == TFTVConfig.StartingSquadFaction.PHOENIX)
                {
                    startingTemplates.Add(heavy);
                    startingTemplates.Add(assault);
                }
                else if (config.startingSquad == TFTVConfig.StartingSquadFaction.ANU)
                {
                    startingTemplates.Add(priest);
                    level.EventSystem.SetVariable("BG_Start_Faction", 1);
                }
                else if (config.startingSquad == TFTVConfig.StartingSquadFaction.NJ)
                {
                    startingTemplates.Add(technician);
                    level.EventSystem.SetVariable("BG_Start_Faction", 2);
                }
                else if (config.startingSquad == TFTVConfig.StartingSquadFaction.SYNEDRION)
                {
                    startingTemplates.Add(infiltrator);
                    level.EventSystem.SetVariable("BG_Start_Faction", 3);
                }


                if (level.CurrentDifficultyLevel.Order == 2 && config.startingSquad == TFTVConfig.StartingSquadFaction.PHOENIX)
                {
                    startingTemplates.Add(assault);
                }
                else if (level.CurrentDifficultyLevel.Order == 2 && config.startingSquad != TFTVConfig.StartingSquadFaction.PHOENIX)
                {
                    startingTemplates.Add(heavy);
                }
                else if (level.CurrentDifficultyLevel.Order == 1 && config.startingSquad == TFTVConfig.StartingSquadFaction.PHOENIX)
                {
                    startingTemplates.Add(assault);
                    startingTemplates.Add(sniper);
                }
                else if (level.CurrentDifficultyLevel.Order == 1 && config.startingSquad != TFTVConfig.StartingSquadFaction.PHOENIX)
                {
                    startingTemplates.Add(heavy);
                    startingTemplates.Add(assault);
                }

                int strengthBonus = 0;
                int willBonus = 0;

                if (level.CurrentDifficultyLevel.Order == 3)
                {
                    strengthBonus = 4;
                    willBonus = 1;
                }
                else if (level.CurrentDifficultyLevel.Order == 2)
                {
                    strengthBonus = 6;
                    willBonus = 2;
                }
                else if (level.CurrentDifficultyLevel.Order == 1)
                {
                    strengthBonus = 8;
                    willBonus = 3;
                }

                newJacob.Data.Strength += strengthBonus;
                newJacob.Data.Will += willBonus;

                newSophia.Data.Strength += strengthBonus;
                newSophia.Data.Will += willBonus;


                return startingTemplates;


                /*
                TacCharacterDef Omar2 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Omar_Tutorial2_TacCharacterDef"));
                Omar2.Data.Strength = 0;
                Omar2.Data.Will = 0;
                Omar2.Data.Speed = 0;
                Omar2.Data.CurrentHealth = -1;

                TacCharacterDef Takeshi3 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Takeshi_Tutorial3_TacCharacterDef"));
                Takeshi3.Data.Strength = 0;
                Takeshi3.Data.Will = 0;
                Takeshi3.Data.Speed = 0;
                Takeshi3.Data.CurrentHealth = -1;

                TacCharacterDef Irina3 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_Irina_Tutorial3_TacCharacterDef"));
                Irina3.Data.Strength = 0;
                Irina3.Data.Will = 0;
                Irina3.Data.Speed = 0;
                Irina3.Data.CurrentHealth = -1;*/



            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
            throw new InvalidOperationException();
        }

        public static List<TacCharacterDef> SetInitialSquadBuffed(GameDifficultyLevelDef difficultyLevel, GeoLevelController level)
        {
            try
            {
                TFTVConfig config = TFTVMain.Main.Config;
  
                TacCharacterDef Jacob2 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_JacobBuffed_TFTV_TacCharacterDef"));
                TacCharacterDef Sophia2 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_SophiaBuffed_TFTV_TacCharacterDef"));
                TacCharacterDef Omar3 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_OmarBuffed_TFTV_TacCharacterDef"));
                TacCharacterDef Takeshi3 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_TakeshiBuffed_TFTV_TacCharacterDef"));
                TacCharacterDef Irina3 = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("PX_IrinaBuffed_TFTV_TacCharacterDef"));
               
                TacCharacterDef sniper = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_SniperStarting_TacCharacterDef"));
                TacCharacterDef priest = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_Starting_Priest_TacCharacterDef"));
                TacCharacterDef technician = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_Starting_Technician_TacCharacterDef"));
                TacCharacterDef infiltrator = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_Starting_Infiltrator_TacCharacterDef"));

                List<TacCharacterDef> startingTemplates = new List<TacCharacterDef>{Jacob2,Sophia2,Omar3,Takeshi3,Irina3};


                if (config.startingSquad == TFTVConfig.StartingSquadFaction.ANU)
                {
                    startingTemplates.Add(priest);
                    level.EventSystem.SetVariable("BG_Start_Faction", 1);
                }
                else if (config.startingSquad == TFTVConfig.StartingSquadFaction.NJ)
                {
                    startingTemplates.Add(technician);
                    level.EventSystem.SetVariable("BG_Start_Faction", 2);
                }
                else if (config.startingSquad == TFTVConfig.StartingSquadFaction.SYNEDRION)
                {
                    startingTemplates.Add(infiltrator);
                    level.EventSystem.SetVariable("BG_Start_Faction", 3);
                }

                if (difficultyLevel.Order == 1)
                {
                    startingTemplates.Add(sniper);
                }

                return startingTemplates;
            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
            throw new InvalidOperationException();
        }

        public static List<TacCharacterDef> SetInitialSquadRandom(GameDifficultyLevelDef difficultyLevel, GeoLevelController level)
        {
            try
            {
                TFTVConfig config = TFTVMain.Main.Config;

                TacCharacterDef assault = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_AssaultStarting_TacCharacterDef"));
                TacCharacterDef heavy = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_HeavyStarting_TacCharacterDef"));
                TacCharacterDef sniper = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_SniperStarting_TacCharacterDef"));

                TacCharacterDef priest = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_Starting_Priest_TacCharacterDef"));
                TacCharacterDef technician = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_Starting_Technician_TacCharacterDef"));
                TacCharacterDef infiltrator = Repo.GetAllDefs<TacCharacterDef>().FirstOrDefault(a => a.name.Equals("PX_Starting_Infiltrator_TacCharacterDef"));

                List<TacCharacterDef> startingTemplates = new List<TacCharacterDef>
                {
                    heavy,
                    assault,
                    assault,
                    sniper
                };

                if (config.startingSquad == TFTVConfig.StartingSquadFaction.ANU)
                {
                    startingTemplates.Remove(heavy);
                    startingTemplates.Remove(assault);
                    startingTemplates.Add(priest);
                    level.EventSystem.SetVariable("BG_Start_Faction", 1);
                }
                else if (config.startingSquad == TFTVConfig.StartingSquadFaction.NJ)
                {
                    startingTemplates.Remove(heavy);
                    startingTemplates.Remove(assault);
                    startingTemplates.Add(technician);
                    level.EventSystem.SetVariable("BG_Start_Faction", 2);
                }
                else if (config.startingSquad == TFTVConfig.StartingSquadFaction.SYNEDRION)
                {
                    startingTemplates.Remove(heavy);
                    startingTemplates.Remove(assault);
                    startingTemplates.Add(infiltrator);
                    level.EventSystem.SetVariable("BG_Start_Faction", 3);
                }


                if (level.CurrentDifficultyLevel.Order == 2 && config.startingSquad == TFTVConfig.StartingSquadFaction.PHOENIX)
                {
                    startingTemplates.Add(assault);
                }
                else if (level.CurrentDifficultyLevel.Order == 2 && config.startingSquad != TFTVConfig.StartingSquadFaction.PHOENIX)
                {
                    startingTemplates.Add(heavy);
                }
                else if (level.CurrentDifficultyLevel.Order == 1 && config.startingSquad == TFTVConfig.StartingSquadFaction.PHOENIX)
                {
                    startingTemplates.Add(assault);
                    startingTemplates.Add(sniper);
                }
                else if (level.CurrentDifficultyLevel.Order == 1 && config.startingSquad != TFTVConfig.StartingSquadFaction.PHOENIX)
                {
                    startingTemplates.Add(heavy);
                    startingTemplates.Add(assault);
                }

                return startingTemplates;
            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
            throw new InvalidOperationException();
        }

        public static void CreateInitialInfiltrator()
        {
            try
            {
                TacCharacterDef sourceInfiltrator = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("S_SY_Infiltrator_TacCharacterDef"));
                TacCharacterDef startingInfiltrator = Helper.CreateDefFromClone(sourceInfiltrator, "8835621B-CFCA-41EF-B480-241D506BD742", "PX_Starting_Infiltrator_TacCharacterDef");
                startingInfiltrator.Data.Strength = 0;
                startingInfiltrator.Data.Will = 0;

             /*   startingInfiltrator.Data.BodypartItems = new ItemDef[] // Armour
                {
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("SY_Infiltrator_Bonus_Helmet_BodyPartDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("SY_Infiltrator_Bonus_Torso_BodyPartDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("SY_Infiltrator_Bonus_Legs_ItemDef"))
                };           */
            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
        }

        public static void CreateInitialPriest()
        {
            try
            {
                TacCharacterDef sourcePriest = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("S_AN_Priest_TacCharacterDef"));
                TacCharacterDef startingPriest = Helper.CreateDefFromClone(sourcePriest, "B1C9385B-05D1-453D-8665-4102CCBA77BE", "PX_Starting_Priest_TacCharacterDef");
                startingPriest.Data.Strength = 0;
                startingPriest.Data.Will = 0;

                startingPriest.Data.BodypartItems = new ItemDef[] // Armour
                {
              //  Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("AN_Priest_Head02_BodyPartDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("AN_Priest_Torso_BodyPartDef")),
                Repo.GetAllDefs<TacticalItemDef>().First(tad => tad.name.Contains("AN_Priest_Legs_ItemDef"))
                };
                //    TFTVLogger.Always(startingPriest.Data.EquipmentItems.Count().ToString());

                /*  ItemDef[] inventoryList = new ItemDef[]

                  { 
                  startingPriest.Data.EquipmentItems[0].CompatibleAmmunition[0],
                  startingPriest.Data.EquipmentItems[1].CompatibleAmmunition[0]
                  };*/

                startingPriest.Data.EquipmentItems = new ItemDef[] // Ready slots
                { Repo.GetAllDefs<WeaponDef>().First(wd => wd.name.Contains("AN_Redemptor_WeaponDef")),
                  //  Repo.GetAllDefs<WeaponDef>().First(wd => wd.name.Contains("Medkit_EquipmentDef"))
                };
                // startingPriest.Data.InventoryItems = inventoryList;
            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
        }

        public static void CreateInitialTechnician()
        {
            try
            {
                TacCharacterDef sourceTechnician = Repo.GetAllDefs<TacCharacterDef>().First(tcd => tcd.name.Contains("NJ_Technician_TacCharacterDef"));
                TacCharacterDef startingTechnician = Helper.CreateDefFromClone(sourceTechnician, "1D0463F9-6684-4CE1-82CA-386FC2CE18E3", "PX_Starting_Technician_TacCharacterDef");
                startingTechnician.Data.Strength = 0;
                startingTechnician.Data.Will = 0;
                startingTechnician.Data.LevelProgression.Experience = 0;
                
            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
        }

    }
}
