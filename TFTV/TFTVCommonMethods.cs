﻿using Base.Defs;
using Base.UI;
using PhoenixPoint.Common.UI;
using PhoenixPoint.Geoscape.Entities;
using PhoenixPoint.Geoscape.Entities.Research;
using PhoenixPoint.Geoscape.Events;
using PhoenixPoint.Geoscape.Events.Eventus;
using PhoenixPoint.Geoscape.Levels;
using System;
using System.Linq;

namespace TFTV
{
    internal class TFTVCommonMethods
    {
        private static readonly DefRepository Repo = TFTVMain.Repo;



        public static void SetStaminaToZero(GeoCharacter __instance)
        {
            try
            {
                if (__instance.Fatigue.Stamina != null && __instance.Fatigue.Stamina > 0)
                {
                    __instance.Fatigue.Stamina.SetToMin();
                }
            }

            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }

        }
        public static void GenerateGeoEventChoice(GeoscapeEventDef geoEvent, string choice, string outcome)
        {
            try
            {
                geoEvent.GeoscapeEventData.Choices.Add(new GeoEventChoice()

                {
                    Text = new LocalizedTextBind(choice),
                    Outcome = new GeoEventChoiceOutcome()
                    {
                        OutcomeText = new EventTextVariation()
                        {
                            General = new LocalizedTextBind(outcome)
                        }
                    }
                });
            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
        }

        public static OutcomeDiplomacyChange GenerateDiplomacyOutcome(GeoFactionDef partyFaction, GeoFactionDef targetFaction, int value)
        {
            try
            {
                return new OutcomeDiplomacyChange()
                {
                    PartyFaction = partyFaction,
                    TargetFaction = targetFaction,
                    Value = value,
                    PartyType = (OutcomeDiplomacyChange.ChangeTarget)1,
                };
            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
            throw new InvalidOperationException();
        }

        public static OutcomeVariableChange GenerateVariableChange(string variableName, int value, bool isSet)
        {
            try
            {
                return new OutcomeVariableChange()
                {
                    VariableName = variableName,
                    Value = { Min = value, Max = value },
                    IsSetOperation = isSet,
                };
            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
            throw new InvalidOperationException();
        }

        public static GeoscapeEventDef CreateNewEvent(string name, string title, string description, string outcome)
        {
            try
            {

                string gUID = Guid.NewGuid().ToString();
                GeoscapeEventDef sourceLoseGeoEvent = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals("PROG_PU12_FAIL_GeoscapeEventDef"));
                GeoscapeEventDef newEvent = Helper.CreateDefFromClone(sourceLoseGeoEvent, gUID, name);
                newEvent.GeoscapeEventData.Choices[0].Outcome.ReEneableEvent = false;
                newEvent.GeoscapeEventData.Choices[0].Outcome.ReactiveEncounters.Clear();
                newEvent.GeoscapeEventData.EventID = name;
                newEvent.GeoscapeEventData.Title.LocalizationKey = title;
                newEvent.GeoscapeEventData.Description[0].General.LocalizationKey = description;
                if (outcome != null)
                {
                    newEvent.GeoscapeEventData.Choices[0].Outcome.OutcomeText.General.LocalizationKey = outcome;
                }
                return newEvent;
            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
            throw new InvalidOperationException();
        }

        public static ResearchDef CreateNewPXResearch(string id, int cost, string gUID, ResearchViewElementDef researchViewElementDef)

        {
            try
            {
                ResearchDef sourceResearchDef = Repo.GetAllDefs<ResearchDef>().FirstOrDefault(ged => ged.name.Equals("PX_AtmosphericAnalysis_ResearchDef"));
                ResearchDef researchDef = Helper.CreateDefFromClone(sourceResearchDef, gUID, id);
                ResearchDef secondarySourceResearchDef = Repo.GetAllDefs<ResearchDef>().FirstOrDefault(ged => ged.name.Equals("PX_AlienGoo_ResearchDef"));


                researchDef.ResearchCost = cost;
                researchDef.ViewElementDef = researchViewElementDef;
                researchDef.Unlocks = secondarySourceResearchDef.Unlocks;
                researchDef.Tags = secondarySourceResearchDef.Tags;
                return researchDef;
            }

            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
            throw new InvalidOperationException();
        }

        public static ResearchViewElementDef CreateNewResearchViewElement(string def, string gUID, string name, string reveal, string unlock, string complete)

        {
            try
            {

                ResearchViewElementDef sourceResearchViewDef = Repo.GetAllDefs<ResearchViewElementDef>().FirstOrDefault(ged => ged.name.Equals("PX_Alien_CorruptionNode_ViewElementDef"));
                ResearchViewElementDef researchViewDef = Helper.CreateDefFromClone(sourceResearchViewDef, gUID, def);
                researchViewDef.DisplayName1.LocalizationKey = name;
                researchViewDef.RevealText.LocalizationKey = reveal;
                researchViewDef.UnlockText.LocalizationKey = unlock;
                researchViewDef.CompleteText.LocalizationKey = complete;
                return researchViewDef; 
            }

            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
            throw new InvalidOperationException();
        }

        public static void ApplyRepPenaltyForRevealDiploMission(string eventIDRevealMission, GeoFactionDef partyFaction, int value)
        {
            try
            {
                GeoFactionDef phoenixPoint = Repo.GetAllDefs<GeoFactionDef>().FirstOrDefault(ged => ged.name.Equals("Phoenix_GeoPhoenixFactionDef"));
                GeoscapeEventDef eventDef = Repo.GetAllDefs<GeoscapeEventDef>().FirstOrDefault(ged => ged.name.Equals(eventIDRevealMission));
                eventDef.GeoscapeEventData.Choices[0].Outcome.Diplomacy[0] = GenerateDiplomacyOutcome(partyFaction, phoenixPoint, value);

            }
            catch (Exception e)
            {
                TFTVLogger.Error(e);
            }
        }
    }
}

