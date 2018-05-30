using System;
using System.Collections.Generic;
using System.Text;
using DG;
using UnityEngine;
using System.Windows.Forms;

namespace Ace
{
    /*First Blood Code
    [FieldAccess(Class = "Referee", Field = "SentenceLose", Group = "FirstBloodMatch")]

    [GroupDescription(Group = "FirstBloodMatch", Name = "First Blood Match", Description = "Adds a First Blood match type.")]

    public class FirstBloodMatch
    {
        public static int[] bloodMeter = new int[8];
        public static bool bloodEnding = false;
        public static int minTime = UnityEngine.Random.Range(8, 12);

        [Hook(TargetClass = "Player", TargetMethod = "Bleeding", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.ModifyReturn, Group = "FirstBloodMatch")]
        public static bool BloodConditions(Player p)
        {
            if (GlobalWork.inst.MatchSetting.BattleRoyalKind != BattleRoyalKindEnum.Off)
            {
                return false;
            }

            // No Deathmatches etc.
            if (GlobalWork.inst.MatchSetting.arena == VenueEnum.BarbedWire || GlobalWork.inst.MatchSetting.arena == VenueEnum.Cage || GlobalWork.inst.MatchSetting.arena == VenueEnum.LandMine_BarbedWire || GlobalWork.inst.MatchSetting.arena == VenueEnum.LandMine_FluorescentLamp)
            {
                return false;
            }

            if (MatchSelectionForm.instance.firstBloodCheckBox.Checked)
            {
                Player plObj = PlayerMan.inst.GetPlObj(p.TargetPlIdx);
                if (plObj == null)
                {
                    return false;
                }

                SkillData currentSkill = plObj.animator.CurrentSkill;
                if (currentSkill != null)
                {
                    bloodMeter[p.PlIdx] += currentSkill.bleedingRate;
                    L.D(bloodMeter[p.PlIdx].ToString());
                }

                if (MatchMain.inst.matchTime.min < minTime)
                {
                    return true;
                }

                if (bloodMeter[p.PlIdx] >= 50)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            return false;
        }


        [Hook(TargetClass = "Player", TargetMethod = "Bleeding", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "FirstBloodMatch")]
        public static void CheckMatchEnd_Blood(Player p)
        {
            if (!MatchSelectionForm.instance.firstBloodCheckBox.Checked)
            {
                return;
            }

            if (GlobalWork.inst.MatchSetting.BattleRoyalKind != BattleRoyalKindEnum.Off)
            {
                return;
            }

            // No Deathmatches etc.
            if (GlobalWork.inst.MatchSetting.arena == VenueEnum.BarbedWire || GlobalWork.inst.MatchSetting.arena == VenueEnum.Cage || GlobalWork.inst.MatchSetting.arena == VenueEnum.LandMine_BarbedWire || GlobalWork.inst.MatchSetting.arena == VenueEnum.LandMine_FluorescentLamp)
            {
                return;
            }

            if (p.isBleeding)
            {
                if (Carlzilla.bloodState[p.PlIdx] < 6)
                {
                    return;
                }

                GlobalWork.inst.MatchSetting.VictoryCondition = VictoryConditionEnum.Count3;
                bloodEnding = true;
                Referee mRef = RefereeMan.inst.GetRefereeObj();
                mRef.PlDir = PlDirEnum.Left;
                mRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
                mRef.State = RefeStateEnum.DeclareVictory;
                mRef.matchResult = MatchResultEnum.KO;
                mRef.SentenceLose(p.PlIdx);
            }
        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 8, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersVal | HookInjectFlags.PassLocals, LocalVarIds = new int[] { 1 }, Group = "FirstBloodMatch")]
        public static void SetFirstBloodFinishText(ref UILabel finishText, string str)
        {
            if (bloodEnding)
            {
                string finishString = str.Replace("K.O.", "First Blood");
                finishText.text = finishString;
                bloodEnding = false;
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "FirstBloodMatch")]
        public static void ResetBloodMeters()
        {
            for (int i = 0; i < 8; i++)
            {
                bloodMeter[i] = 0;
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "FirstBloodMatch")]
        public static void SetMatchRules()
        {
            if (GlobalWork.inst.MatchSetting.BattleRoyalKind != BattleRoyalKindEnum.Off)
            {
                return;
            }

            if (MatchSelectionForm.instance.firstBloodCheckBox.Checked)
            {
                MatchSetting mSetting = GlobalWork.inst.MatchSetting;

                if (mSetting.arena == VenueEnum.BarbedWire || mSetting.arena == VenueEnum.Cage || mSetting.arena == VenueEnum.LandMine_BarbedWire || mSetting.arena == VenueEnum.LandMine_FluorescentLamp)
                {
                    DialogResult result = MessageBox.Show("You cannot combine deathmatches with first blood matches. Please Press 'OK' to continue with a first blood match in Yurakeun Hall or press 'Cancel' to continue with a deathmatch without first blood rules.", "Rules Conflict Detected", MessageBoxButtons.OKCancel);
                    if (result == DialogResult.OK)
                    {
                        mSetting.arena = VenueEnum.YurakuenHall;
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        MatchSelectionForm.instance.firstBloodCheckBox.Checked = false;
                        return;
                    }
                    
                }

                mSetting.VictoryCondition = VictoryConditionEnum.OnlyEscape;
                mSetting.is10CountKO = false;
                mSetting.isOutOfRingCount = false;
                mSetting.isFoulCount = false;
                mSetting.isElimination = false;
                mSetting.isTornadoBattle = true;
                mSetting.CriticalRate = CriticalRateEnum.Off;
            }            
        }

        [ControlPanel(Group = "FirstBloodMatch")]
        public static Form MSForm()
        {
            if (MatchSelectionForm.instance == null)
            {
                return new MatchSelectionForm();
            }
            else
            {
                return null;
            }
        }
    }
    */
}
