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
    public class EditFuckery
    {
        public static bool tauntCheck = false;
        public static List<string> tauntList = new List<string>();
        public static List<int> tauntDat = new List<int>();

        public static int attireChosen;
        public static Player plObj;


        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "AttireExtension")]
        public static void LoadAttire()
        {
            GlobalWork gw = GlobalWork.inst;
            PlayerMan pm = PlayerMan.inst;
            SaveData saveData = SaveData.inst;

            for (int pl = 0; pl < 8; pl++)
            {
                if (gw.MatchSetting.matchWrestlerInfo[pl].entry)
                {
                    plObj = pm.GetPlObj(pl);
                    //string[] list = Directory.GetFiles("./AceModsData/AttireExtension/", DataBase.GetWrestlerFullName(plObj.WresParam) + "*.*");
                    DirectoryInfo di = new DirectoryInfo("./AceModsData/AttireExtension/");
                    FileInfo[] files = di.GetFiles(DataBase.GetWrestlerFullName(plObj.WresParam) + "*.*");
                    //MessageBox.Show(files.Length.ToString());
                    if (files.Length > 0)
                    {
                        //MessageBox.Show(files.Length.ToString());

                        Attire_Select attireSelect = new Attire_Select(files, pl);

                        attireSelect.ShowDialog();

                        if (File.Exists("./AceModsData/AttireExtension/" + DataBase.GetWrestlerFullName(plObj.WresParam) + attireSelect.chosenAttire + ".txt"))
                        {
                            StreamReader cdReader = new StreamReader("./AceModsData/AttireExtension/" + DataBase.GetWrestlerFullName(plObj.WresParam) + attireSelect.chosenAttire + ".txt");    
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
                            }
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
                            }


                            cdReader.Dispose();
                            cdReader.Close();


                        }
                        attireSelect.Dispose();
                    }


                    //Attire_Select attireSelect = new Attire_Select();
                    //attireSelect.ShowDialog();


                    // attireSelect.Dispose();
                }
            }
        }
    

        //[Hook(TargetClass = "Menu_Title", TargetMethod = "UserInput", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "AttireExtension")]
        //public static void AttireExtension()
        //{
        //    //List<string> WrestlerNames = new List<string>();
        //    //WrestlerNames.Add(DataBase.GetWrestlerFullName(editDat.wrestlerParam));

        //    //SaveData saveData = SaveData.inst;
        //    //EditWrestlerData editDat = saveData.editWrestlerData[0];
        //    //CostumeData[] cosDat = editDat.appearanceData.costumeData;

        //    //if (!Directory.Exists("./AceModsData/AttireExtension/"))
        //    //{
        //    //    Directory.CreateDirectory("./AceModsData/AttireExtension/");
        //    //}

        //    //if (!File.Exists("./AceModsData/AttireExtension/" + DataBase.GetWrestlerFullName(editDat.wrestlerParam) + ".txt"))
        //    //{
        //    //    File.Create("./AceModsData/AttireExtension/" + DataBase.GetWrestlerFullName(editDat.wrestlerParam) + ".txt");
        //    //}

        //    //StreamWriter streamWriter = new StreamWriter("./AceModsData/AttireExtension/" + DataBase.GetWrestlerFullName(editDat.wrestlerParam) + ".txt");
        //    //for (int i = 0; i < 9; i++)
        //    //{
        //    //    for (int j = 0; j < 16; j++)
        //    //    {
        //    //        streamWriter.WriteLine(cosDat[0].layerTex[i, j]);
        //    //        streamWriter.WriteLine(cosDat[0].color[i, j].r);
        //    //        streamWriter.WriteLine(cosDat[0].color[i, j].g);
        //    //        streamWriter.WriteLine(cosDat[0].color[i, j].b);
        //    //        streamWriter.WriteLine(cosDat[0].color[i, j].a);
        //    //        streamWriter.WriteLine(cosDat[0].highlightIntensity[i, j]);
        //    //    }
        //    //    streamWriter.WriteLine(cosDat[0].partsScale[i]);
        //    //}
        //    //streamWriter.Dispose();
        //    //streamWriter.Close();
        //}

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

        [Hook(TargetClass = "MatchMain", TargetMethod = "Update", InjectionLocation = 0, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "PreMatchTaunts")]
        public static void EditShit()
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
                        if (edit1TauntNo == 0)
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
                        if (edit2TauntNo == 0)
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


        //[ControlPanel(Group = "PreMatchTaunts")]
        //public static Form PreMatchTauntsConf()
        //{
        //    if (PreMatchTauntsForm.instance == null)
        //    {
        //        return new PreMatchTauntsForm();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        

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
