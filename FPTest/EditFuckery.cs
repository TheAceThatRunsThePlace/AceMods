using System;
using System.Collections.Generic;
using System.Text;
using DG;
using UnityEngine;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Ace
{
    [GroupDescription(Name = "Attire Extension", Description = "Allows the user to export (and import) attires to .cos and select them when launching a match.", Group = "AttireExtension")]
    [GroupDescription(Name = "Pre Match Taunts", Description = "If two opponents both have a pre match taunt selected, they will both do it at the same time when the ref rings the bell.", Group = "PreMatchTaunts")]

    public class EditFuckery
    {
        public static bool tauntCheck = false;
        public static List<string> tauntList = new List<string>();
        public static List<int> tauntDat = new List<int>();

        public static int attireChosen;
        public static Player plObj;

        //public static CostumeData[] loadedCostumeData2 = new CostumeData[8];
        //public static List<CostumeData> loadedCostumeData2 = new List<CostumeData>();
        public static bool[] needsToChangeToMatchAttire = new bool[8];

        public static CostumeData cosDat1 = new CostumeData();
        public static CostumeData cosDat2 = new CostumeData();
        public static CostumeData cosDat3 = new CostumeData();
        public static CostumeData cosDat4 = new CostumeData();
        public static CostumeData cosDat5 = new CostumeData();
        public static CostumeData cosDat6 = new CostumeData();
        public static CostumeData cosDat7 = new CostumeData();
        public static CostumeData cosDat8 = new CostumeData();

        public static bool enCheck = false;

        public static string entAttire;

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "AttireExtension")]
        public static void LoadAttire()
        {
            GlobalWork gw = GlobalWork.inst;
            PlayerMan pm = PlayerMan.inst;
            SaveData saveData = SaveData.inst;

            for (int pl = 0; pl < 8; pl++)
            {
                needsToChangeToMatchAttire[pl] = false;

                if (gw.MatchSetting.matchWrestlerInfo[pl].entry)
                {
                    plObj = pm.GetPlObj(pl);
                    //string[] list = Directory.GetFiles("./AceModsData/AttireExtension/", DataBase.GetWrestlerFullName(plObj.WresParam) + "*.*");
                    DirectoryInfo di = new DirectoryInfo("./AceModsData/AttireExtension/");
                    FileInfo[] files = di.GetFiles(DataBase.GetWrestlerFullName(plObj.WresParam) + "*.cos");
                    if (files.Length > 0)
                    {
                        //MessageBox.Show(files.Length.ToString());

                        Attire_Select attireSelect = new Attire_Select(files, pl);
                        attireSelect.ShowDialog();

                        if (File.Exists("./AceModsData/AttireExtension/" + DataBase.GetWrestlerFullName(plObj.WresParam) + attireSelect.chosenAttire + ".cos"))
                        {
                            StreamReader cdReader = new StreamReader("./AceModsData/AttireExtension/" + DataBase.GetWrestlerFullName(plObj.WresParam) + attireSelect.chosenAttire + ".cos");
                            CostumeData loadedCostumeData = new CostumeData();
                            while (cdReader.Peek() != -1)
                            {
                                loadedCostumeData.valid = true;
                                for (int i = 0; i < 9; i++)
                                {
                                    for (int j = 0; j < 16; j++)
                                    {
                                        loadedCostumeData.layerTex[i, j] = cdReader.ReadLine();
                                        loadedCostumeData.color[i, j].r = float.Parse(cdReader.ReadLine());
                                        loadedCostumeData.color[i, j].g = float.Parse(cdReader.ReadLine());
                                        loadedCostumeData.color[i, j].b = float.Parse(cdReader.ReadLine());
                                        loadedCostumeData.color[i, j].a = float.Parse(cdReader.ReadLine());
                                        loadedCostumeData.highlightIntensity[i, j] = float.Parse(cdReader.ReadLine());
                                    }
                                    loadedCostumeData.partsScale[i] = float.Parse(cdReader.ReadLine());
                                }
                                entAttire = cdReader.ReadLine();


                            }
                            cdReader.Dispose();
                            cdReader.Close();

                            bool check = false;

                            if (!String.IsNullOrEmpty(entAttire))
                            {
                                if (gw.MatchSetting.BattleRoyalKind != BattleRoyalKindEnum.RoyalRumble)
                                {
                                    if (!gw.MatchSetting.isSkipEntranceScene)
                                    {
                                        if (gw.MatchSetting.arena != VenueEnum.BarbedWire && gw.MatchSetting.arena != VenueEnum.Cage && gw.MatchSetting.arena != VenueEnum.Dodecagon && gw.MatchSetting.arena != VenueEnum.Dojo && gw.MatchSetting.arena != VenueEnum.LandMine_BarbedWire && gw.MatchSetting.arena != VenueEnum.LandMine_FluorescentLamp && gw.MatchSetting.arena != VenueEnum.YurakuenHall)
                                        {
                                            check = true;
                                        }
                                    }
                                }
                            }

                            if (check)
                            {
                                needsToChangeToMatchAttire[pl] = true;
                                StreamReader cdReader2 = new StreamReader("./AceModsData/AttireExtension/" + entAttire + ".cos");
                                CostumeData loadedCostumeDataEnt = new CostumeData();
                                while (cdReader2.Peek() != -1)
                                {
                                    loadedCostumeDataEnt.valid = true;
                                    for (int i = 0; i < 9; i++)
                                    {
                                        for (int j = 0; j < 16; j++)
                                        {
                                            loadedCostumeDataEnt.layerTex[i, j] = cdReader2.ReadLine();
                                            loadedCostumeDataEnt.color[i, j].r = float.Parse(cdReader2.ReadLine());
                                            loadedCostumeDataEnt.color[i, j].g = float.Parse(cdReader2.ReadLine());
                                            loadedCostumeDataEnt.color[i, j].b = float.Parse(cdReader2.ReadLine());
                                            loadedCostumeDataEnt.color[i, j].a = float.Parse(cdReader2.ReadLine());
                                            loadedCostumeDataEnt.highlightIntensity[i, j] = float.Parse(cdReader2.ReadLine());
                                        }
                                        loadedCostumeDataEnt.partsScale[i] = float.Parse(cdReader2.ReadLine());
                                    }
                                }

                                try
                                {
                                    plObj.FormRen.DestroySprite();
                                    plObj.FormRen.InitTexture(loadedCostumeDataEnt);
                                    for (int i = 0; i < 9; i++)
                                    {
                                        plObj.FormRen.partsScale[i] = loadedCostumeDataEnt.partsScale[i];
                                    }
                                    plObj.FormRen.InitSprite(false);
                                    L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO ENTRANCE ATTIRE");
                                }
                                catch
                                {
                                    L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO ENTRANCE ATTIRE");
                                    MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                    plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                    plObj.FormRen.InitSprite(false);
                                    needsToChangeToMatchAttire[pl] = false;
                                }

                                if (pl == 0)
                                {
                                    cosDat1.valid = true;
                                    for (int i = 0; i < 9; i++)
                                    {
                                        for (int j = 0; j < 16; j++)
                                        {
                                            cosDat1.layerTex[i, j] = loadedCostumeData.layerTex[i, j];
                                            cosDat1.color[i, j].r = loadedCostumeData.color[i, j].r;
                                            cosDat1.color[i, j].g = loadedCostumeData.color[i, j].g;
                                            cosDat1.color[i, j].b = loadedCostumeData.color[i, j].b;
                                            cosDat1.color[i, j].a = loadedCostumeData.color[i, j].a;
                                            cosDat1.highlightIntensity[i, j] = loadedCostumeData.highlightIntensity[i, j];
                                        }
                                        cosDat1.partsScale[i] = loadedCostumeData.partsScale[i];
                                    }
                                }
                                else if (pl == 1)
                                {
                                    cosDat2.valid = true;
                                    for (int i = 0; i < 9; i++)
                                    {
                                        for (int j = 0; j < 16; j++)
                                        {
                                            cosDat2.layerTex[i, j] = loadedCostumeData.layerTex[i, j];
                                            cosDat2.color[i, j].r = loadedCostumeData.color[i, j].r;
                                            cosDat2.color[i, j].g = loadedCostumeData.color[i, j].g;
                                            cosDat2.color[i, j].b = loadedCostumeData.color[i, j].b;
                                            cosDat2.color[i, j].a = loadedCostumeData.color[i, j].a;
                                            cosDat2.highlightIntensity[i, j] = loadedCostumeData.highlightIntensity[i, j];
                                        }
                                        cosDat2.partsScale[i] = loadedCostumeData.partsScale[i];
                                    }
                                }
                                else if (pl == 2)
                                {
                                    cosDat3.valid = true;
                                    for (int i = 0; i < 9; i++)
                                    {
                                        for (int j = 0; j < 16; j++)
                                        {
                                            cosDat3.layerTex[i, j] = loadedCostumeData.layerTex[i, j];
                                            cosDat3.color[i, j].r = loadedCostumeData.color[i, j].r;
                                            cosDat3.color[i, j].g = loadedCostumeData.color[i, j].g;
                                            cosDat3.color[i, j].b = loadedCostumeData.color[i, j].b;
                                            cosDat3.color[i, j].a = loadedCostumeData.color[i, j].a;
                                            cosDat3.highlightIntensity[i, j] = loadedCostumeData.highlightIntensity[i, j];
                                        }
                                        cosDat3.partsScale[i] = loadedCostumeData.partsScale[i];
                                    }
                                }
                                else if (pl == 3)
                                {
                                    cosDat4.valid = true;
                                    for (int i = 0; i < 9; i++)
                                    {
                                        for (int j = 0; j < 16; j++)
                                        {
                                            cosDat4.layerTex[i, j] = loadedCostumeData.layerTex[i, j];
                                            cosDat4.color[i, j].r = loadedCostumeData.color[i, j].r;
                                            cosDat4.color[i, j].g = loadedCostumeData.color[i, j].g;
                                            cosDat4.color[i, j].b = loadedCostumeData.color[i, j].b;
                                            cosDat4.color[i, j].a = loadedCostumeData.color[i, j].a;
                                            cosDat4.highlightIntensity[i, j] = loadedCostumeData.highlightIntensity[i, j];
                                        }
                                        cosDat4.partsScale[i] = loadedCostumeData.partsScale[i];
                                    }
                                }
                                else if (pl == 4)
                                {
                                    cosDat5.valid = true;
                                    for (int i = 0; i < 9; i++)
                                    {
                                        for (int j = 0; j < 16; j++)
                                        {
                                            cosDat5.layerTex[i, j] = loadedCostumeData.layerTex[i, j];
                                            cosDat5.color[i, j].r = loadedCostumeData.color[i, j].r;
                                            cosDat5.color[i, j].g = loadedCostumeData.color[i, j].g;
                                            cosDat5.color[i, j].b = loadedCostumeData.color[i, j].b;
                                            cosDat5.color[i, j].a = loadedCostumeData.color[i, j].a;
                                            cosDat5.highlightIntensity[i, j] = loadedCostumeData.highlightIntensity[i, j];
                                        }
                                        cosDat5.partsScale[i] = loadedCostumeData.partsScale[i];
                                    }
                                }
                                else if (pl == 5)
                                {
                                    cosDat6.valid = true;
                                    for (int i = 0; i < 9; i++)
                                    {
                                        for (int j = 0; j < 16; j++)
                                        {
                                            cosDat6.layerTex[i, j] = loadedCostumeData.layerTex[i, j];
                                            cosDat6.color[i, j].r = loadedCostumeData.color[i, j].r;
                                            cosDat6.color[i, j].g = loadedCostumeData.color[i, j].g;
                                            cosDat6.color[i, j].b = loadedCostumeData.color[i, j].b;
                                            cosDat6.color[i, j].a = loadedCostumeData.color[i, j].a;
                                            cosDat6.highlightIntensity[i, j] = loadedCostumeData.highlightIntensity[i, j];
                                        }
                                        cosDat6.partsScale[i] = loadedCostumeData.partsScale[i];
                                    }
                                }
                                else if (pl == 6)
                                {
                                    cosDat7.valid = true;
                                    for (int i = 0; i < 9; i++)
                                    {
                                        for (int j = 0; j < 16; j++)
                                        {
                                            cosDat7.layerTex[i, j] = loadedCostumeData.layerTex[i, j];
                                            cosDat7.color[i, j].r = loadedCostumeData.color[i, j].r;
                                            cosDat7.color[i, j].g = loadedCostumeData.color[i, j].g;
                                            cosDat7.color[i, j].b = loadedCostumeData.color[i, j].b;
                                            cosDat7.color[i, j].a = loadedCostumeData.color[i, j].a;
                                            cosDat7.highlightIntensity[i, j] = loadedCostumeData.highlightIntensity[i, j];
                                        }
                                        cosDat7.partsScale[i] = loadedCostumeData.partsScale[i];
                                    }
                                }
                                else if (pl == 7)
                                {
                                    cosDat8.valid = true;
                                    for (int i = 0; i < 9; i++)
                                    {
                                        for (int j = 0; j < 16; j++)
                                        {
                                            cosDat8.layerTex[i, j] = loadedCostumeData.layerTex[i, j];
                                            cosDat8.color[i, j].r = loadedCostumeData.color[i, j].r;
                                            cosDat8.color[i, j].g = loadedCostumeData.color[i, j].g;
                                            cosDat8.color[i, j].b = loadedCostumeData.color[i, j].b;
                                            cosDat8.color[i, j].a = loadedCostumeData.color[i, j].a;
                                            cosDat8.highlightIntensity[i, j] = loadedCostumeData.highlightIntensity[i, j];
                                        }
                                        cosDat8.partsScale[i] = loadedCostumeData.partsScale[i];
                                    }
                                }
                            }
                            else
                            {
                                try
                                {
                                    plObj.FormRen.DestroySprite();
                                    plObj.FormRen.InitTexture(loadedCostumeData);
                                    for (int i = 0; i < 9; i++)
                                    {
                                        plObj.FormRen.partsScale[i] = loadedCostumeData.partsScale[i];
                                    }
                                    plObj.FormRen.InitSprite(false);
                                    L.D("ATTIRE EXTENSION: ATTIRE CHANGED");
                                }
                                catch
                                {
                                    L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED");
                                    MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                    plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                    plObj.FormRen.InitSprite(false);
                                }
                            }


                        }
                        attireSelect.Dispose();
                    }
                }
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "Update", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "AttireExtension")]
        public static void SkipEntrance()
        {
            if (MatchMain.inst.State == MatchMain.StateEnum.EntranceScene)
            {
                if (EntranceScene.inst.IsSkipAll())
                {
                    MatchSetting mSetting = GlobalWork.inst.MatchSetting;
                    for (int PlIdx = 0; PlIdx < 8; PlIdx++)
                    {
                        if (needsToChangeToMatchAttire[PlIdx])
                        {
                            Player plObjEnt = PlayerMan.inst.PlObj[PlIdx];

                            if (mSetting.matchWrestlerInfo[PlIdx].entry)
                            {
                                if (!mSetting.matchWrestlerInfo[PlIdx].isSecond)
                                {
                                    if (PlIdx == 0)
                                    {
                                        try
                                        {
                                            plObjEnt.FormRen.DestroySprite();
                                            plObjEnt.FormRen.InitTexture(cosDat1);
                                            for (int i = 0; i < 9; i++)
                                            {
                                                plObjEnt.FormRen.partsScale[i] = cosDat1.partsScale[i];
                                            }
                                            plObjEnt.FormRen.InitSprite(false);
                                            L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                        }
                                        catch
                                        {
                                            L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                            MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                            plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                            plObj.FormRen.InitSprite(false);
                                        }
                                    }
                                    else if (PlIdx == 1)
                                    {
                                        try
                                        {
                                            plObjEnt.FormRen.DestroySprite();
                                            plObjEnt.FormRen.InitTexture(cosDat2);
                                            for (int i = 0; i < 9; i++)
                                            {
                                                plObjEnt.FormRen.partsScale[i] = cosDat2.partsScale[i];
                                            }
                                            plObjEnt.FormRen.InitSprite(false);
                                            L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                        }
                                        catch
                                        {
                                            L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                            MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                            plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                            plObj.FormRen.InitSprite(false);
                                        }
                                    }
                                    else if (PlIdx == 2)
                                    {
                                        try
                                        {
                                            plObjEnt.FormRen.DestroySprite();
                                            plObjEnt.FormRen.InitTexture(cosDat3);
                                            for (int i = 0; i < 9; i++)
                                            {
                                                plObjEnt.FormRen.partsScale[i] = cosDat3.partsScale[i];
                                            }
                                            plObjEnt.FormRen.InitSprite(false);
                                            L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                        }
                                        catch
                                        {
                                            L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                            MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                            plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                            plObj.FormRen.InitSprite(false);
                                        }
                                    }
                                    else if (PlIdx == 3)
                                    {
                                        try
                                        {
                                            plObjEnt.FormRen.DestroySprite();
                                            plObjEnt.FormRen.InitTexture(cosDat4);
                                            for (int i = 0; i < 9; i++)
                                            {
                                                plObjEnt.FormRen.partsScale[i] = cosDat4.partsScale[i];
                                            }
                                            plObjEnt.FormRen.InitSprite(false);
                                            L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                        }
                                        catch
                                        {
                                            L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                            MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                            plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                            plObj.FormRen.InitSprite(false);
                                        }
                                    }
                                    else if (PlIdx == 4)
                                    {
                                        try
                                        {
                                            plObjEnt.FormRen.DestroySprite();
                                            plObjEnt.FormRen.InitTexture(cosDat5);
                                            for (int i = 0; i < 9; i++)
                                            {
                                                plObjEnt.FormRen.partsScale[i] = cosDat5.partsScale[i];
                                            }
                                            plObjEnt.FormRen.InitSprite(false);
                                            L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                        }
                                        catch
                                        {
                                            L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                            MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                            plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                            plObj.FormRen.InitSprite(false);
                                        }
                                    }
                                    else if (PlIdx == 5)
                                    {
                                        try
                                        {
                                            plObjEnt.FormRen.DestroySprite();
                                            plObjEnt.FormRen.InitTexture(cosDat6);
                                            for (int i = 0; i < 9; i++)
                                            {
                                                plObjEnt.FormRen.partsScale[i] = cosDat6.partsScale[i];
                                            }
                                            plObjEnt.FormRen.InitSprite(false);
                                            L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                        }
                                        catch
                                        {
                                            L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                            MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                            plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                            plObj.FormRen.InitSprite(false);
                                        }
                                    }
                                    else if (PlIdx == 6)
                                    {
                                        try
                                        {
                                            plObjEnt.FormRen.DestroySprite();
                                            plObjEnt.FormRen.InitTexture(cosDat7);
                                            for (int i = 0; i < 9; i++)
                                            {
                                                plObjEnt.FormRen.partsScale[i] = cosDat7.partsScale[i];
                                            }
                                            plObjEnt.FormRen.InitSprite(false);
                                            L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                        }
                                        catch
                                        {
                                            L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                            MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                            plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                            plObj.FormRen.InitSprite(false);
                                        }
                                    }
                                    else if (PlIdx == 7)
                                    {
                                        try
                                        {
                                            plObjEnt.FormRen.DestroySprite();
                                            plObjEnt.FormRen.InitTexture(cosDat8);
                                            for (int i = 0; i < 9; i++)
                                            {
                                                plObjEnt.FormRen.partsScale[i] = cosDat8.partsScale[i];
                                            }
                                            plObjEnt.FormRen.InitSprite(false);
                                            L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                        }
                                        catch
                                        {
                                            L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                            MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                            plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                            plObj.FormRen.InitSprite(false);
                                        }
                                    }
                                }
                            }
                            needsToChangeToMatchAttire[PlIdx] = false;
                        }
                    }
                }
            }
        }

        [Hook(TargetClass = "EntranceScene", TargetMethod = "End", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "AttireExtension")]
        public static void ChangeToMatchAttire()
        {
            MatchSetting mSetting = GlobalWork.inst.MatchSetting;
            for (int x = 0; x < EntranceScene.inst.plIdxList.Length; x++)
            {
                int PlIdx = EntranceScene.inst.plIdxList[x];
                if (needsToChangeToMatchAttire[PlIdx])
                {
                    Player plObjEnt = PlayerMan.inst.PlObj[PlIdx];

                    if (mSetting.matchWrestlerInfo[PlIdx].entry)
                    {
                        if (!mSetting.matchWrestlerInfo[PlIdx].isSecond)
                        {
                            if (PlIdx == 0)
                            {
                                try
                                {
                                    plObjEnt.FormRen.DestroySprite();
                                    plObjEnt.FormRen.InitTexture(cosDat1);
                                    for (int i = 0; i < 9; i++)
                                    {
                                        plObjEnt.FormRen.partsScale[i] = cosDat1.partsScale[i];
                                    }
                                    plObjEnt.FormRen.InitSprite(false);
                                    L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                }
                                catch
                                {
                                    L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                    MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                    plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                    plObj.FormRen.InitSprite(false);
                                }
                            }
                            else if (PlIdx == 1)
                            {
                                try
                                {
                                    plObjEnt.FormRen.DestroySprite();
                                    plObjEnt.FormRen.InitTexture(cosDat2);
                                    for (int i = 0; i < 9; i++)
                                    {
                                        plObjEnt.FormRen.partsScale[i] = cosDat2.partsScale[i];
                                    }
                                    plObjEnt.FormRen.InitSprite(false);
                                    L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                }
                                catch
                                {
                                    L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                    MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                    plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                    plObj.FormRen.InitSprite(false);
                                }
                            }
                            else if (PlIdx == 2)
                            {
                                try
                                {
                                    plObjEnt.FormRen.DestroySprite();
                                    plObjEnt.FormRen.InitTexture(cosDat3);
                                    for (int i = 0; i < 9; i++)
                                    {
                                        plObjEnt.FormRen.partsScale[i] = cosDat3.partsScale[i];
                                    }
                                    plObjEnt.FormRen.InitSprite(false);
                                    L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                }
                                catch
                                {
                                    L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                    MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                    plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                    plObj.FormRen.InitSprite(false);
                                }
                            }
                            else if (PlIdx == 3)
                            {
                                try
                                {
                                    plObjEnt.FormRen.DestroySprite();
                                    plObjEnt.FormRen.InitTexture(cosDat4);
                                    for (int i = 0; i < 9; i++)
                                    {
                                        plObjEnt.FormRen.partsScale[i] = cosDat4.partsScale[i];
                                    }
                                    plObjEnt.FormRen.InitSprite(false);
                                    L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                }
                                catch
                                {
                                    L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                    MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                    plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                    plObj.FormRen.InitSprite(false);
                                }
                            }
                            else if (PlIdx == 4)
                            {
                                try
                                {
                                    plObjEnt.FormRen.DestroySprite();
                                    plObjEnt.FormRen.InitTexture(cosDat5);
                                    for (int i = 0; i < 9; i++)
                                    {
                                        plObjEnt.FormRen.partsScale[i] = cosDat5.partsScale[i];
                                    }
                                    plObjEnt.FormRen.InitSprite(false);
                                    L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                }
                                catch
                                {
                                    L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                    MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                    plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                    plObj.FormRen.InitSprite(false);
                                }
                            }
                            else if (PlIdx == 5)
                            {
                                try
                                {
                                    plObjEnt.FormRen.DestroySprite();
                                    plObjEnt.FormRen.InitTexture(cosDat6);
                                    for (int i = 0; i < 9; i++)
                                    {
                                        plObjEnt.FormRen.partsScale[i] = cosDat6.partsScale[i];
                                    }
                                    plObjEnt.FormRen.InitSprite(false);
                                    L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                }
                                catch
                                {
                                    L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                    MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                    plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                    plObj.FormRen.InitSprite(false);
                                }
                            }
                            else if (PlIdx == 6)
                            {
                                try
                                {
                                    plObjEnt.FormRen.DestroySprite();
                                    plObjEnt.FormRen.InitTexture(cosDat7);
                                    for (int i = 0; i < 9; i++)
                                    {
                                        plObjEnt.FormRen.partsScale[i] = cosDat7.partsScale[i];
                                    }
                                    plObjEnt.FormRen.InitSprite(false);
                                    L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                }
                                catch
                                {
                                    L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                    MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                    plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                    plObj.FormRen.InitSprite(false);
                                }
                            }
                            else if (PlIdx == 7)
                            {
                                try
                                {
                                    plObjEnt.FormRen.DestroySprite();
                                    plObjEnt.FormRen.InitTexture(cosDat8);
                                    for (int i = 0; i < 9; i++)
                                    {
                                        plObjEnt.FormRen.partsScale[i] = cosDat8.partsScale[i];
                                    }
                                    plObjEnt.FormRen.InitSprite(false);
                                    L.D("ATTIRE EXTENSION: ATTIRE CHANGED TO MATCH");
                                }
                                catch
                                {
                                    L.D("ATTIRE EXTENSION: ATTIRE NOT CHANGED TO MATCH");
                                    MatchWrestlerInfo w = GlobalWork.inst.MatchSetting.matchWrestlerInfo[plObj.PlIdx];
                                    plObj.FormRen.InitTexture(SaveData.GetInst().GetEditWrestlerData(w.wrestlerID).appearanceData.costumeData[w.costume_no]);
                                    plObj.FormRen.InitSprite(false);
                                }
                            }
                        }
                    }
                    needsToChangeToMatchAttire[PlIdx] = false;
                }

            }
        }

        [Hook(TargetClass = "Menu_Title", TargetMethod = "UserInput", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "AttireExtension")]
        public static void AttireExtension()
        {
            if (!Directory.Exists("./AceModsData/AttireExtension/"))
            {
                Directory.CreateDirectory("./AceModsData/AttireExtension/");
            }
        }


        #region Pre Match Taunts
        [Hook(TargetClass = "MatchMain", TargetMethod = "EndMatch", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "PreMatchTaunts")]
        public static void reset()
        {
            tauntCheck = false;
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "PreMatchTaunts")]
        public static void TauntList()
        {

            StreamReader streamReader = new StreamReader("./AceModsData/PreMatchTaunts.txt");

            while (!streamReader.EndOfStream)
            {
                tauntList.Add(streamReader.ReadLine());
                tauntDat.Add(Int32.Parse(streamReader.ReadLine()));
            }

            streamReader.Dispose();
            streamReader.Close();

            //List<string> WrestlerNames = new List<string>();
            //foreach (EditWrestlerData edit in SaveData.inst.editWrestlerData)
            //{
            //    WrestlerNames.Add(DataBase.GetWrestlerFullName(edit.wrestlerParam));
            //}
        }

        [Hook(TargetClass = "Menu_Title", TargetMethod = "UserInput", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "PreMatchTaunts")]
        public static void CreateFolder()
        {
            if (!Directory.Exists("./AceModsData/"))
            {
                Directory.CreateDirectory("./AceModsData/");
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "Update", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "PreMatchTaunts")]
        public static void PreMatchTaunts()
        {
            if (!tauntCheck)
            {
                MatchMain mm = MatchMain.inst;
                if (mm.isTimeCounting)
                {
                    PlayerMan pm = PlayerMan.inst;

                    Player p1 = pm.GetPlObj(0);
                    Player p2 = pm.GetPlObj(4);

                    if (p1 == null || p2 == null)
                    {
                        return;
                    }

                    string p1name = DataBase.GetWrestlerFullName(p1.WresParam);
                    string p2name = DataBase.GetWrestlerFullName(p2.WresParam);

                    if (tauntList.Contains(p1name) && tauntList.Contains(p2name))
                    {
                        int edit1TauntNo = tauntDat[tauntList.IndexOf(p1name)];
                        int edit2TauntNo = tauntDat[tauntList.IndexOf(p2name)];

                        SkillSlotEnum skill1 = new SkillSlotEnum();
                        SkillSlotEnum skill2 = new SkillSlotEnum();

                        // EDIT 1 TAUNT
                        if (edit1TauntNo == 5)
                        {
                            skill1 = SkillSlotEnum.Performance_Enter;
                        }
                        else if (edit1TauntNo == 1)
                        {
                            skill1 = SkillSlotEnum.Performance_1;
                        }
                        else if (edit1TauntNo == 2)
                        {
                            skill1 = SkillSlotEnum.Performance_2;
                        }
                        else if (edit1TauntNo == 3)
                        {
                            skill1 = SkillSlotEnum.Performance_3;
                        }
                        else if (edit1TauntNo == 4)
                        {
                            skill1 = SkillSlotEnum.Performance_4;
                        }

                        // EDIT 2 TAUNT
                        if (edit2TauntNo == 5)
                        {
                            skill2 = SkillSlotEnum.Performance_Enter;
                        }
                        else if (edit2TauntNo == 1)
                        {
                            skill2 = SkillSlotEnum.Performance_1;
                        }
                        else if (edit2TauntNo == 2)
                        {
                            skill2 = SkillSlotEnum.Performance_2;
                        }
                        else if (edit2TauntNo == 3)
                        {
                            skill2 = SkillSlotEnum.Performance_3;
                        }
                        else if (edit2TauntNo == 4)
                        {
                            skill2 = SkillSlotEnum.Performance_4;
                        }

                        p1.animator.StartSlotAnm_Immediately(skill1, 0, true, p1.TargetPlIdx);
                        p2.animator.StartSlotAnm_Immediately(skill2, 0, true, p2.TargetPlIdx);
                        tauntCheck = true;
                    }

                }
            }
        }


        [ControlPanel(Group = "PreMatchTaunts")]
        public static Form PreMatchTauntsConf()
        {
            if (PreMatchTauntsForm.instance == null)
            {
                return new PreMatchTauntsForm();
            }
            else
            {
                return null;
            }
        }
        #endregion


        [ControlPanel(Group = "AttireExtension")]
        public static Form AttireExtensionConf()
        {
            if (AttireExtensionForm.instance == null)
            {
                return new AttireExtensionForm();
            }
            else
            {
                return null;
            }
        }
    }
}
