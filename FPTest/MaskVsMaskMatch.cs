using System;
using System.Collections.Generic;
using System.Text;
using DG;
using UnityEngine;
using System.Windows.Forms;
using MoveLists;

namespace Ace
{
    /* Mask Vs. Mask Code
    [FieldAccess(Class = "Referee", Field = "SentenceLose", Group = "MaskVsMaskMatch")]

    [GroupDescription(Group = "MaskVsMaskMatch", Name = "Mask vs. Mask Match", Description = "Adds a Mask vs. Mask match type.")]

    class MaskVsMaskMatch
    {  
        public static int[,] origMoves = new int[8, 4];
        public static int[] maskDmg = new int[8];
        public static bool maskWin = false;
        public static int redTeamCount = 0;
        public static int blueTeamCount = 0;

        [Hook(TargetClass = "FormAnimator", TargetMethod = "ReqSlotAnm", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance | HookInjectFlags.PassParametersVal, Group = "MaskVsMaskMatch")]
        public static void OverrideMoveLists(FormAnimator animator, SkillSlotEnum skill_slot, bool rev, int def_pl_idx, bool atk_side)
        {
            if (skill_slot == SkillSlotEnum.DFallFoot || skill_slot == SkillSlotEnum.DFallHead || skill_slot == SkillSlotEnum.UFallFoot || skill_slot == SkillSlotEnum.UFallHead)
            {
                MoveLists.MoveLists.noChange[animator.plObj.PlIdx] = true;
            }
        }


        [Hook(TargetClass = "MatchMisc", TargetMethod = "ApplyDamage", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassParametersVal, Group = "MaskVsMaskMatch")]
        public static void MaskTearMove(int atk_pl_idx, int def_pl_idx)
        {
            if (!MatchSelectionForm.instance.maskVsMaskCheckBox.Checked)
            {
                return;
            }

            if (redTeamCount > 1 || blueTeamCount > 1)
            {
                MatchSelectionForm.instance.maskVsMaskCheckBox.Checked = false;
                return;
            }

            Player AtkPlayer = PlayerMan.inst.GetPlObj(atk_pl_idx);
            Player DefPlayer = PlayerMan.inst.GetPlObj(def_pl_idx);
            if (!AtkPlayer || !DefPlayer)
            {
                return;
            }

            if (AtkPlayer.animator.SkillSlotID == SkillSlotEnum.UFallFoot || AtkPlayer.animator.SkillSlotID == SkillSlotEnum.UFallHead || AtkPlayer.animator.SkillSlotID == SkillSlotEnum.DFallFoot || AtkPlayer.animator.SkillSlotID == SkillSlotEnum.DFallHead)
            {
                maskDmg[def_pl_idx] += AtkPlayer.animator.CurrentSkill.atkPow_HP;
            }

            if (MatchMain.inst.isMatchEnd)
            {
                return;
            }

            if (maskDmg[def_pl_idx] >= 100)
            {
                int newCosIdx = GlobalWork.inst.MatchSetting.matchWrestlerInfo[def_pl_idx].costume_no + 1;
                if (newCosIdx > 3)
                {
                    newCosIdx = 0;
                }

                if (!SaveData.inst.GetEditWrestlerData(GlobalWork.inst.MatchSetting.matchWrestlerInfo[def_pl_idx].wrestlerID).appearanceData.costumeData[newCosIdx].valid)
                {
                    newCosIdx = 0;
                }

                DefPlayer.FormRen.DestroySprite();
                DefPlayer.FormRen.InitTexture(SaveData.inst.GetEditWrestlerData(GlobalWork.inst.MatchSetting.matchWrestlerInfo[def_pl_idx].wrestlerID).appearanceData.costumeData[newCosIdx]);
                DefPlayer.FormRen.InitSprite(false);
                GlobalWork.inst.MatchSetting.matchWrestlerInfo[def_pl_idx].costume_no = newCosIdx;

                GlobalWork.inst.MatchSetting.VictoryCondition = VictoryConditionEnum.Count3;
                maskWin = true;
                Referee mRef = RefereeMan.inst.GetRefereeObj();
                mRef.PlDir = PlDirEnum.Left;
                mRef.ReqRefereeAnm(BasicSkillEnum.Refe_Stand_MatchEnd_Front_Left);
                mRef.State = RefeStateEnum.DeclareVictory;
                mRef.matchResult = MatchResultEnum.KO;
                mRef.SentenceLose(def_pl_idx);
            }
        }

        [Hook(TargetClass = "Menu_Result", TargetMethod = "Set_FinishSkill", InjectionLocation = 8, InjectDirection = HookInjectDirection.After, InjectFlags = HookInjectFlags.PassParametersVal | HookInjectFlags.PassLocals, LocalVarIds = new int[] { 1 }, Group = "MaskVsMaskMatch")]
        public static void SetFirstBloodFinishText(ref UILabel finishText, string str)
        {
            if (maskWin)
            {
                string finishString = str.Replace("K.O.", "Mask Removal");
                finishText.text = finishString;
                maskWin = false;
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "MaskVsMaskMatch")]
        public static void ResetMaskDmg()
        {
            if (!MatchSelectionForm.instance.maskVsMaskCheckBox.Checked)
            {
                return;
            }

            if (redTeamCount > 1 || blueTeamCount > 1)
            {
                MatchSelectionForm.instance.maskVsMaskCheckBox.Checked = false;
                return;
            }

            for (int i = 0; i < 8; i++)
            {
                maskDmg[i] = 0;

                MatchWrestlerInfo m = GlobalWork.inst.MatchSetting.matchWrestlerInfo[i];
                if (m.entry && !m.isSecond)
                {
                    for(int j = 0; j < 4; j++)
                    {
                        m.param.skillSlot[56 + j] = (SkillID)origMoves[i, j];
                        origMoves[i, j] = 0;
                    }
                }
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.PassInvokingInstance, Group = "MaskVsMaskMatch")]
        public static void AddMaskTearMove(MatchMain mm)
        {
            if (!MatchSelectionForm.instance.maskVsMaskCheckBox.Checked)
            {
                return;
            }

            blueTeamCount = 0;
            redTeamCount = 0;

            for (int i = 0; i < 8; i++)
            {
                MatchWrestlerInfo m = GlobalWork.inst.MatchSetting.matchWrestlerInfo[i];

                if (m.entry && !m.isSecond)
                {
                    if (i > 3)
                    {
                        blueTeamCount++;
                    }
                    else
                    {
                        redTeamCount++;
                    }
                }

                if (redTeamCount > 1 || blueTeamCount > 1)
                {
                    MatchSelectionForm.instance.maskVsMaskCheckBox.Checked = false;
                    return;
                }

                if (m.entry && !m.isSecond)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        origMoves[i, j] = (int)m.param.skillSlot[56 + j];
                        m.param.skillSlot[56 + j] = (SkillID)933; // 933 = Grounded Chin Lock
                    }
                }

                MatchSetting mSetting = GlobalWork.inst.MatchSetting;

                mSetting.VictoryCondition = VictoryConditionEnum.OnlyEscape;
                mSetting.isFoulCount = false;
                mSetting.isOutOfRingCount = false;
                mSetting.is10CountKO = false;
                mSetting.is3GameMatch = false;
            }
        }

    }
    */
}
