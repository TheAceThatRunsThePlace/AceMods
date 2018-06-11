using System;
using System.Collections.Generic;
using System.Text;
using DG;
using UnityEngine;
using System.IO;
using System.Windows.Forms;

namespace Ace
{
    [FieldAccess(Class = "EntranceScene", Field = "StateEnum", Group = "AnmAprons")]
    [FieldAccess(Class = "EntranceScene", Field = "state", Group = "AnmAprons")]
    [FieldAccess(Class = "EntranceScene", Field = "totalTimer", Group = "AnmAprons")]
    [FieldAccess(Class = "MatchMain", Field = "State", Group = "AnmAprons")]
    [FieldAccess(Class = "MatchMain", Field = "StateEnum", Group = "AnmAprons")]

    [GroupDescription(Name = "Animated Ring Parts", Description = "Animate several different ring parts using a sequence of images.", Group = "AnmAprons")]
    public class AnimatedAprons
    {
        public static Texture[] apronTexRight = new Texture[1];
        public static Texture[] apronTexLeft = new Texture[1];        
        public static int texSwapTimeR = 1;
        public static int texSwapTimeL = 1;
        public static string timeMethod = "Frames";
        public static int mirroredAprons = 0;
        public static bool loadApron = false;
        public static bool loadApronEdit0 = false;
        public static bool loadApronEdit1 = false;
        public static int texIdxRight = 0;
        public static int texIdxLeft = 0;        
        public static Renderer apronRend = null;

        public static Texture[] apronTexEdit0 = new Texture[1];
        public static Texture[] apronTexEdit1 = new Texture[1];
        public static int texIdxEdit0 = 0;
        public static int texIdxEdit1 = 0;

        public static Texture[] matTex = new Texture[1];
        public static int matTexSwapTime = 1;
        public static string matTimeMethod = "Frames";
        public static bool loadMat = false;
        public static int texIdxMat = 0;

        public static Texture[] postTex = new Texture[1];
        public static int postTexSwapTime = 1;
        public static string postTimeMethod = "Frames";
        public static bool loadPost = false;
        public static int texIdxPost = 0;
        public static Renderer postRend0 = null;
        public static Renderer postRend1 = null;
        public static Renderer postRend2 = null;
        public static Renderer postRend3 = null;

        /*
        public static Texture[] rampTex = new Texture[1];
        public static int rampTexSwapTime = 1;
        public static string rampTimeMethod = "Frames";
        public static bool loadRamp = false;
        public static int texIdxRamp = 0;
        public static Renderer rampRend = null;
        */

        [Hook(TargetClass = "Menu_Title", TargetMethod = "UserInput", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "AnmAprons")]
        public static void CreateAnmFolders()
        {
            if (!Directory.Exists("./AceModsData/AnmAprons/"))
            {
                Directory.CreateDirectory("./AceModsData/AnmAprons/");
            }
            if (!Directory.Exists("./AceModsData/AnmMats/"))
            {
                Directory.CreateDirectory("./AceModsData/AnmMats/");
            }
            if (!Directory.Exists("./AceModsData/AnmPosts/"))
            {
                Directory.CreateDirectory("./AceModsData/AnmPosts/");
            }
        }

        [Hook(TargetClass = "MatchMain", TargetMethod = "CreatePlayers", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "AnmAprons")]
        public static void LoadTextures()
        {
            // SET UP ACE MOD DATA FOLDER
            if (!Directory.Exists("./AceModsData/"))
            {
                Directory.CreateDirectory("./AceModsData/");
            }

            MatchSetting mSetting = GlobalWork.inst.MatchSetting;

            if (mSetting.ringID < 10000)
            {
                return;
            }

            string ringName = SaveData.inst.editRingData[mSetting.ringID - 10000].name;

            if (!Directory.Exists("./AceModsData/AnmAprons/" + ringName + "/"))
            {
                Directory.CreateDirectory("./AceModsData/AnmAprons/" + ringName + "/");
            }

            apronTexRight = new Texture[1];
            apronTexLeft = new Texture[1];
            apronTexEdit0 = new Texture[1];
            apronTexEdit1 = new Texture[1];
            texSwapTimeR = 1;
            texSwapTimeL = 1;
            mirroredAprons = 0;
            loadApron = false;
            loadApronEdit0 = false;
            loadApronEdit1 = false;
            texIdxRight = 0;
            texIdxLeft = 0;
            texIdxEdit0 = 0;
            texIdxEdit1 = 0;

            

            if (File.Exists("./AceModsData/AnmAprons/" + ringName + "/" + ringName + ".dat"))
            {
                StreamReader rdReader = new StreamReader("./AceModsData/AnmAprons/" + ringName + "/" + ringName + ".dat");
                timeMethod = rdReader.ReadLine();
                texSwapTimeR = int.Parse(rdReader.ReadLine());
                texSwapTimeL = int.Parse(rdReader.ReadLine());
                mirroredAprons = int.Parse(rdReader.ReadLine());

                rdReader.Dispose();
                rdReader.Close();
            }
            else
            {
                texSwapTimeR = 1;
                texSwapTimeL = 1;
                timeMethod = "Frames";
                mirroredAprons = 0;
            }

            DirectoryInfo di = new DirectoryInfo("./AceModsData/AnmAprons/" + ringName + "/");
            FileInfo[] filesRight = di.GetFiles(ringName + "_R*.png");
            FileInfo[] filesLeft = di.GetFiles(ringName + "_L*.png");

            List<Texture> texListR = new List<Texture>();
            List<Texture> texListL = new List<Texture>();


            foreach (FileInfo f in filesRight)
            {
                byte[] fileData = new byte[1];
                fileData = File.ReadAllBytes(f.FullName);
                Texture2D tempTex = new Texture2D(2, 2);
                tempTex.LoadImage(fileData);
                Texture rTex = new Texture();
                rTex = (Texture)tempTex;
                texListR.Add(rTex);

            }

            if (mirroredAprons == 0)
            {
                foreach (FileInfo f in filesLeft)
                {
                    byte[] fileData = new byte[1];
                    fileData = File.ReadAllBytes(f.FullName);
                    Texture2D tempTex = new Texture2D(2, 2);
                    tempTex.LoadImage(fileData);
                    Texture lTex = new Texture();
                    lTex = (Texture)tempTex;
                    texListL.Add(lTex);
                }
            }

            /*
            DirectoryInfo AceDI = new DirectoryInfo("./AceModsData/AnmAprons/Wrestlers/Bailey Ace/");
            FileInfo[] filesAce = AceDI.GetFiles("Bailey Ace" + "_R*.png");

            List<Texture> texListAce = new List<Texture>();

            foreach (FileInfo f in filesAce)
            {
                byte[] fileData = new byte[1];
                fileData = File.ReadAllBytes(f.FullName);
                Texture2D tempTex = new Texture2D(2, 2);
                tempTex.LoadImage(fileData);
                Texture AceTex = new Texture();
                AceTex = (Texture)tempTex;
                texListAce.Add(AceTex);
            }

            apronTexEdit = texListAce.ToArray();
            */

            /*
            DirectoryInfo EditDI = new DirectoryInfo("./AceModsData/AnmAprons/Wrestlers/");

            
                FileInfo[] filesEdit0 = EditDI.GetFiles(DataBase.GetWrestlerFullName(PlayerMan.inst.GetPlObj(0).WresParam) + "/" + DataBase.GetWrestlerFullName(PlayerMan.inst.GetPlObj(0).WresParam) + "*.png");

                List<Texture> texListEdit0 = new List<Texture>();

                foreach (FileInfo f in filesEdit0)
                {
                    byte[] fileData = new byte[1];
                    fileData = File.ReadAllBytes(f.FullName);
                    Texture2D tempTex = new Texture2D(2, 2);
                    tempTex.LoadImage(fileData);
                    Texture EditTex = new Texture();
                    EditTex = (Texture)tempTex;
                    texListEdit0.Add(EditTex);
                }
                apronTexEdit0 = texListEdit0.ToArray();
                L.D("SLOT 1 EDIT APRON TRONS LOADED: " + apronTexEdit0.Length);
            



                FileInfo[] filesEdit1 = EditDI.GetFiles(DataBase.GetWrestlerFullName(PlayerMan.inst.GetPlObj(1).WresParam) + "/" + DataBase.GetWrestlerFullName(PlayerMan.inst.GetPlObj(1).WresParam) + "*.png");

                List<Texture> texListEdit1 = new List<Texture>();

                foreach (FileInfo f in filesEdit1)
                {
                    byte[] fileData = new byte[1];
                    fileData = File.ReadAllBytes(f.FullName);
                    Texture2D tempTex = new Texture2D(2, 2);
                    tempTex.LoadImage(fileData);
                    Texture EditTex = new Texture();
                    EditTex = (Texture)tempTex;
                    texListEdit1.Add(EditTex);
                }
                apronTexEdit1 = texListEdit1.ToArray();
                L.D("SLOT 2 EDIT APRON TRONS LOADED: " + apronTexEdit1.Length);
            */            

            apronTexRight = texListR.ToArray();
            L.D("RIGHT APRON TEX LOADED: " + apronTexRight.Length);
            if (mirroredAprons == 0)
            {
                apronTexLeft = texListL.ToArray();
            }
            L.D("LEFT APRON TEX LOADED: " + apronTexLeft.Length);
            if (timeMethod == "Seconds")
            {
                L.D("TIMING METHOD: SECONDS");
            }
            else
            {
                L.D("TIMING METHOD: FRAMES");
            }



            if (apronTexRight.Length > 0 && apronTexLeft.Length > 0)
            {
                loadApron = true;
            }

            if (apronTexEdit0.Length > 0)
            {
                loadApronEdit0 = true;
            }

            if (apronTexEdit1.Length > 0)
            {
                loadApronEdit1 = true;
            }



            // LOAD MATS

            if (!Directory.Exists("./AceModsData/AnmMats/" + ringName + "/"))
            {
                Directory.CreateDirectory("./AceModsData/AnmMats/" + ringName + "/");
            }

            matTex = new Texture[1];
            matTexSwapTime = 1;
            loadMat = false;
            texIdxMat = 0;

            if (File.Exists("./AceModsData/AnmMats/" + ringName + "/" + ringName + ".dat"))
            {
                StreamReader rdReader = new StreamReader("./AceModsData/AnmMats/" + ringName + "/" + ringName + ".dat");
                matTimeMethod = rdReader.ReadLine();
                matTexSwapTime = int.Parse(rdReader.ReadLine());

                rdReader.Dispose();
                rdReader.Close();
            }
            else
            {
                matTimeMethod = "Frames";
                matTexSwapTime = 1;
            }

            DirectoryInfo Mdi = new DirectoryInfo("./AceModsData/AnmMats/" + ringName + "/");
            FileInfo[] filesMat = Mdi.GetFiles(ringName + "_Mat*.png");

            List<Texture> texListM = new List<Texture>();

            foreach (FileInfo f in filesMat)
            {
                byte[] fileData = new byte[1];
                fileData = File.ReadAllBytes(f.FullName);
                Texture2D tempTex = new Texture2D(2, 2);
                tempTex.LoadImage(fileData);
                Texture mTex = new Texture();
                mTex = (Texture)tempTex;
                texListM.Add(mTex);
            }


            matTex = texListM.ToArray();
            L.D("MAT TEX LOADED: " + matTex.Length);
            if (matTimeMethod == "Seconds")
            {
                L.D("TIMING METHOD: SECONDS");
            }
            else
            {
                L.D("TIMING METHOD: FRAMES");
            }

            if (matTex.Length > 0)
            {
                loadMat = true;
            }



            // LOAD POSTS

            if (!Directory.Exists("./AceModsData/AnmPosts/" + ringName + "/"))
            {
                Directory.CreateDirectory("./AceModsData/AnmPosts/" + ringName + "/");
            }

            postTex = new Texture[1];
            postTexSwapTime = 1;
            loadPost = false;
            texIdxPost = 0;

            if (File.Exists("./AceModsData/AnmPosts/" + ringName + "/" + ringName + ".dat"))
            {
                StreamReader rdReader = new StreamReader("./AceModsData/AnmPosts/" + ringName + "/" + ringName + ".dat");
                postTimeMethod = rdReader.ReadLine();
                postTexSwapTime = int.Parse(rdReader.ReadLine());

                rdReader.Dispose();
                rdReader.Close();
            }
            else
            {
                postTimeMethod = "Frames";
                postTexSwapTime = 1;
            }

            DirectoryInfo Pdi = new DirectoryInfo("./AceModsData/AnmPosts/" + ringName + "/");
            FileInfo[] filesPost = Pdi.GetFiles(ringName + "_Post*.png");

            List<Texture> texListP = new List<Texture>();

            foreach (FileInfo f in filesPost)
            {
                byte[] fileData = new byte[1];
                fileData = File.ReadAllBytes(f.FullName);
                Texture2D tempTex = new Texture2D(2, 2);
                tempTex.LoadImage(fileData);
                Texture pTex = new Texture();
                pTex = (Texture)tempTex;
                texListP.Add(pTex);
            }


            postTex = texListP.ToArray();
            L.D("POST TEX LOADED: " + postTex.Length);
            if (postTimeMethod == "Seconds")
            {
                L.D("TIMING METHOD: SECONDS");
            }
            else
            {
                L.D("TIMING METHOD: FRAMES");
            }

            if (postTex.Length > 0)
            {
                loadPost = true;
            }



            /* RAMPS
            rampTex = new Texture[1];
            rampTexSwapTime = 1;
            rampTimeMethod = "Frames";
            loadRamp = false;
            texIdxRamp = 0;

            if (File.Exists("./AceModsData/AnmRamps/" + ringName + "/" + ringName + ".dat"))
            {
                StreamReader rdReader = new StreamReader("./AceModsData/AnmRamps/" + ringName + "/" + ringName + ".dat");
                rampTimeMethod = rdReader.ReadLine();
                rampTexSwapTime = int.Parse(rdReader.ReadLine());

                rdReader.Dispose();
                rdReader.Close();
            }
            else
            {
                rampTimeMethod = "Frames";
                rampTexSwapTime = 1;
            }

            DirectoryInfo Rdi = new DirectoryInfo("./AceModsData/AnmRamps/" + ringName + "/");
            FileInfo[] filesRamp = Rdi.GetFiles(ringName + "_AR01_Stage*.png");

            List<Texture> texListRamp = new List<Texture>();

            foreach (FileInfo f in filesRamp)
            {
                byte[] fileData = new byte[1];
                fileData = File.ReadAllBytes(f.FullName);
                Texture2D tempTex = new Texture2D(2, 2);
                tempTex.LoadImage(fileData);
                Texture rampTex = new Texture();
                rampTex = (Texture)tempTex;
                texListRamp.Add(rampTex);
            }


            rampTex = texListRamp.ToArray();
            L.D("RAMP TEX LOADED: " + rampTex.Length);
            if (rampTimeMethod == "Seconds")
            {
                L.D("TIMING METHOD: SECONDS");
            }
            else
            {
                L.D("TIMING METHOD: FRAMES");
            }

            if (rampTex.Length > 0)
            {
                loadRamp = true;
            }
            */
        }


        [Hook(TargetClass = "MatchMain", TargetMethod = "Awake", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "AnmAprons")]
        public static void SetMatRender()
        {
            Ring r = Ring.inst;
            apronRend = null;

            /*
            rampRend = null;

            if (loadRamp)
            {
                foreach (Component c in r.transform.GetComponentsInChildren<Component>())
                {
                    if (c.GetComponent<MeshRenderer>() != null)
                    {
                        Renderer rend = c.GetComponent<MeshRenderer>();
                        if (c.name == "AR01_Stage")
                        {
                            rampRend = rend;
                            break;
                        }
                    }
                }
                if (!MatchMain.inst.isTimeCounting && !MatchMain.inst.Pause)
                {
                    if (loadRamp)
                    {
                        rampRend.materials[0].SetTexture("_MainTex", rampTex[0]);
                    }
                   
                }
            }
            */

            if (loadApron || loadMat)
            {
                foreach (Component c in r.ringObj.transform.GetComponentsInChildren<Component>())
                {
                    if (c.GetComponent<MeshRenderer>() != null)
                    {
                        Renderer rend = c.GetComponent<MeshRenderer>();
                        if (c.name == "Mat")
                        {
                            apronRend = rend;
                            break;
                        }
                    }
                }

                if (!MatchMain.inst.isTimeCounting && !MatchMain.inst.Pause)
                {
                    if (loadApron)
                    {
                        apronRend.materials[5].SetTexture("_DecalTex", apronTexRight[0]); // 5 = Apron Right
                        if (mirroredAprons == 1)
                        {
                            apronRend.materials[4].SetTexture("_DecalTex", apronTexRight[0]); // 4 = Apron Left
                        }
                        else
                        {
                            apronRend.materials[4].SetTexture("_DecalTex", apronTexLeft[0]); // 4 = Apron Left
                        }
                        
                    }
                    if (loadMat)
                    {
                        apronRend.materials[0].SetTexture("_DecalTex", matTex[0]);
                        apronRend.materials[1].SetTexture("_DecalTex", matTex[0]);
                        apronRend.materials[2].SetTexture("_DecalTex", matTex[0]);
                        apronRend.materials[3].SetTexture("_DecalTex", matTex[0]);
                    }
                }
            }

            
            


            /*
            apronRend = null;

            if (loadMat)
            {
                foreach (Component c in r.ringObj.transform.GetComponentsInChildren<Component>())
                {
                    if (c.GetComponent<MeshRenderer>() != null)
                    {
                        Renderer rend = c.GetComponent<MeshRenderer>();
                        if (c.name == "Mat")
                        {
                            apronRend = rend;
                            break;
                        }
                    }
                }

                if (!MatchMain.inst.isTimeCounting && !MatchMain.inst.Pause)
                {
                    if (loadMat)
                    {
                        apronRend.materials[3].SetTexture("_DecalTex", matTex[0]); // 0 = Mat
                    }
                }
            }
            */

            



            postRend0 = null;
            postRend1 = null;
            postRend2 = null;
            postRend3 = null;

            if (loadPost)
            {
                bool check0 = false;
                bool check1 = false;
                bool check2 = false;
                bool check3 = false;
                foreach (Component c in r.ringObj.transform.GetComponentsInChildren<Component>())
                {
                    if (c.GetComponent<MeshRenderer>() != null)
                    {
                        Renderer rend = c.GetComponent<MeshRenderer>();
                        if (c.name == "Post_East")
                        {                            
                            if (!check0)
                            {
                                postRend0 = rend;
                                L.D("postRend0 = " + postRend0.ToString());
                                // break;
                                check0 = true;
                            }
                        }
                        if (c.name == "Post_West")
                        {
                            if (!check1)
                            {
                                postRend1 = rend;
                                L.D("postRend1 = " + postRend1.ToString());
                                // break;
                                check1 = true;
                            }
                        }
                        if (c.name == "Post_South")
                        {
                            if (!check2)
                            {
                                postRend2 = rend;
                                L.D("postRend2 = " + postRend2.ToString());
                                // break;
                                check2 = true;
                            }
                        }
                        if (c.name == "Post_North")
                        {
                            if (!check3)
                            {
                                postRend3 = rend;
                                L.D("postRend3 = " + postRend3.ToString());
                                // break;
                                check3 = true;
                            }
                        }
                    }
                }
                /*
                foreach (Component c in r.ringObj.transform.GetComponentsInChildren<Component>())
                {
                    if (c.GetComponent<MeshRenderer>() != null)
                    {
                        Renderer rend = c.GetComponent<MeshRenderer>();
                        if (c.name == "Post_East")
                        {
                            postRend0 = rend;
                            L.D("postRend0 = " + postRend0.ToString());
                            break;
                        }
                    }
                }

                foreach (Component c in r.ringObj.transform.GetComponentsInChildren<Component>())
                {
                    if (c.GetComponent<MeshRenderer>() != null)
                    {
                        Renderer rend = c.GetComponent<MeshRenderer>();
                        if (c.name == "Post_West")
                        {
                            postRend1 = rend;
                            L.D("postRend1 = " + postRend1.ToString());
                            break;
                        }
                    }
                }

                foreach (Component c in r.ringObj.transform.GetComponentsInChildren<Component>())
                {
                    if (c.GetComponent<MeshRenderer>() != null)
                    {
                        Renderer rend = c.GetComponent<MeshRenderer>();
                        if (c.name == "Post_South")
                        {
                            postRend2 = rend;
                            L.D("postRend2 = " + postRend2.ToString());
                            break;
                        }
                    }
                }

                foreach (Component c in r.ringObj.transform.GetComponentsInChildren<Component>())
                {
                    if (c.GetComponent<MeshRenderer>() != null)
                    {
                        Renderer rend = c.GetComponent<MeshRenderer>();
                        if (c.name == "Post_North")
                        {
                            postRend3 = rend;
                            L.D("postRend3 = " + postRend3.ToString());
                            break;
                        }
                    }
                }
                */
            }

            

            /*
            foreach (Component c in r.ringObj.transform.GetComponentsInChildren<Component>())
            {
                if (c.GetComponent<MeshRenderer>() != null)
                {
                    Renderer rend = c.GetComponent<MeshRenderer>();
                    if (c.name == "Post_East")
                    {
                        postRend0 = rend;
                        L.D("postRend0 = " + postRend0.ToString());
                        break;
                    }

                    if (c.name == "Post_West")
                    {
                        postRend1 = rend;
                        L.D("postRend1 = " + postRend1.ToString());
                        break;
                    }
                    if (c.name == "Post_South")
                    {
                        postRend2 = rend;
                        L.D("postRend2 = " + postRend2.ToString());
                        break;
                    }
                    if (c.name == "Post_North")
                    {
                        postRend3 = rend;
                        L.D("postRend3 = " + postRend3.ToString());
                        break;
                    }
                }
            }
            */


            if (!MatchMain.inst.isTimeCounting && !MatchMain.inst.Pause)
            {
                if (loadPost)
                {
                    postRend0.materials[0].SetTexture("_MainTex", postTex[0]);
                    postRend1.materials[0].SetTexture("_MainTex", postTex[0]);
                    postRend2.materials[0].SetTexture("_MainTex", postTex[0]);
                    postRend3.materials[0].SetTexture("_MainTex", postTex[0]);
                }
            }
            
        }


        [Hook(TargetClass = "MatchMain", TargetMethod = "Update", InjectionLocation = int.MaxValue, InjectDirection = HookInjectDirection.Before, InjectFlags = HookInjectFlags.None, Group = "AnmAprons")]
        public static void TextureCycle()
        {


            if (MatchMain.inst.Pause)
            {
                return;
            }

            if (apronRend != null)
            {
                if (MatchMain.inst.isTimeCounting)
                {
                    if (timeMethod == "Seconds")
                    {
                        if (loadApron)
                        {
                            if (MatchMain.inst.matchTime.frm != 0)
                            {
                                return;
                            }
                            if (MatchMain.inst.matchTime.sec % texSwapTimeR == 0)
                            {
                                apronRend.materials[5].SetTexture("_DecalTex", apronTexRight[texIdxRight]); // 5 = Apron Right
                                texIdxRight++;
                                // L.D("RIGHT APRON TEX CHANGE // SECONDS METHOD");


                                if (texIdxRight >= apronTexRight.Length)
                                {
                                    texIdxRight = 0;
                                }
                            }

                            if (MatchMain.inst.matchTime.sec % texSwapTimeL == 0)
                            {
                                if (mirroredAprons == 1)
                                {
                                    apronRend.materials[4].SetTexture("_DecalTex", apronTexRight[texIdxLeft]); // 4 = Apron Left
                                    texIdxLeft++;
                                    // L.D("LEFT APRON TEX CHANGE // SECONDS METHOD");

                                    if (texIdxLeft >= apronTexRight.Length)
                                    {
                                        texIdxLeft = 0;
                                    }
                                }
                                else
                                {
                                    apronRend.materials[4].SetTexture("_DecalTex", apronTexLeft[texIdxLeft]); // 4 = Apron Left
                                    texIdxLeft++;
                                    // L.D("LEFT APRON TEX CHANGE // SECONDS METHOD");

                                    if (texIdxLeft >= apronTexLeft.Length)
                                    {
                                        texIdxLeft = 0;
                                    }
                                }
                            }
                        }
                    }
                    else if (timeMethod == "Frames")
                    {
                        if (loadApron)
                        {
                            if (MatchMain.inst.matchTime.frm % texSwapTimeR == 0)
                            {
                                if (loadApron)
                                {
                                    apronRend.materials[5].SetTexture("_DecalTex", apronTexRight[texIdxRight]); // 5 = Apron Right
                                    texIdxRight++;
                                    // L.D("RIGHT APRON TEX CHANGE // FRAMES METHOD");


                                    if (texIdxRight >= apronTexRight.Length)
                                    {
                                        texIdxRight = 0;
                                    }
                                }
                            }

                            if (MatchMain.inst.matchTime.frm % texSwapTimeL == 0)
                            {
                                if (mirroredAprons == 1)
                                {
                                    apronRend.materials[4].SetTexture("_DecalTex", apronTexRight[texIdxLeft]); // 4 = Apron Left
                                    texIdxLeft++;
                                    // L.D("LEFT APRON TEX CHANGE // FRAMES METHOD");

                                    if (texIdxLeft >= apronTexRight.Length)
                                    {
                                        texIdxLeft = 0;
                                    }
                                }
                                else
                                {
                                    apronRend.materials[4].SetTexture("_DecalTex", apronTexLeft[texIdxLeft]); // 4 = Apron Left
                                    texIdxLeft++;
                                    // L.D("LEFT APRON TEX CHANGE // FRAMES METHOD");

                                    if (texIdxLeft >= apronTexLeft.Length)
                                    {
                                        texIdxLeft = 0;
                                    }
                                }
                            }
                        }
                    }
                }

                else if (MatchMain.inst.State != MatchMain.StateEnum.EntranceScene || MatchMain.inst.State == MatchMain.StateEnum.Finished)
                {
                    int frames = 0;
                    frames++;
                    if (frames > 29)
                    {
                        frames = 0;
                    }

                    if (timeMethod == "Seconds")
                    {
                        if (loadApron)
                        {
                            if (frames % (texSwapTimeR * 30) == 0)
                            {
                                apronRend.materials[5].SetTexture("_DecalTex", apronTexRight[texIdxRight]); // 5 = Apron Right
                                texIdxRight++;
                                // L.D("RIGHT APRON TEX CHANGE // SECONDS METHOD");


                                if (texIdxRight >= apronTexRight.Length)
                                {
                                    texIdxRight = 0;
                                }
                            }

                            if (frames % (texSwapTimeL * 30) == 0)
                            {
                                if (mirroredAprons == 1)
                                {
                                    apronRend.materials[4].SetTexture("_DecalTex", apronTexRight[texIdxLeft]); // 4 = Apron Left
                                    texIdxLeft++;
                                    // L.D("LEFT APRON TEX CHANGE // SECONDS METHOD");

                                    if (texIdxLeft >= apronTexRight.Length)
                                    {
                                        texIdxLeft = 0;
                                    }
                                }
                                else
                                {
                                    apronRend.materials[4].SetTexture("_DecalTex", apronTexLeft[texIdxLeft]); // 4 = Apron Left
                                    texIdxLeft++;
                                    // L.D("LEFT APRON TEX CHANGE // SECONDS METHOD");

                                    if (texIdxLeft >= apronTexLeft.Length)
                                    {
                                        texIdxLeft = 0;
                                    }
                                }
                            }
                        }
                    }
                    else if (timeMethod == "Frames")
                    {
                        if (loadApron)
                        {
                            if (frames % texSwapTimeR == 0)
                            {
                                if (loadApron)
                                {
                                    apronRend.materials[5].SetTexture("_DecalTex", apronTexRight[texIdxRight]); // 5 = Apron Right
                                    texIdxRight++;
                                    // L.D("RIGHT APRON TEX CHANGE // FRAMES METHOD");


                                    if (texIdxRight >= apronTexRight.Length)
                                    {
                                        texIdxRight = 0;
                                    }
                                }
                            }

                            if (frames % texSwapTimeL == 0)
                            {
                                if (mirroredAprons == 1)
                                {
                                    apronRend.materials[4].SetTexture("_DecalTex", apronTexRight[texIdxLeft]); // 4 = Apron Left
                                    texIdxLeft++;
                                    // L.D("LEFT APRON TEX CHANGE // FRAMES METHOD");

                                    if (texIdxLeft >= apronTexRight.Length)
                                    {
                                        texIdxLeft = 0;
                                    }
                                }
                                else
                                {
                                    apronRend.materials[4].SetTexture("_DecalTex", apronTexLeft[texIdxLeft]); // 4 = Apron Left
                                    texIdxLeft++;
                                    // L.D("LEFT APRON TEX CHANGE // FRAMES METHOD");

                                    if (texIdxLeft >= apronTexLeft.Length)
                                    {
                                        texIdxLeft = 0;
                                    }
                                }
                            }
                        }
                    }
                    /*
                    if (MatchMain.inst.matchTime.frm != 0)
                    {
                        return;
                    }
                    */
                }
                else
                {
                    /*  
                    if (DataBase.GetWrestlerFullName(PlayerMan.inst.GetPlObj(EntranceScene.inst.plIdxList[0]).WresParam) == "Bailey Ace")
                    {
                        if (loadApron)
                        {
                            apronRend.materials[5].SetTexture("_DecalTex", apronTexEdit[texIdxEdit]); // 5 = Apron Right
                            apronRend.materials[4].SetTexture("_DecalTex", apronTexEdit[texIdxEdit]); // 4 = Apron Left
                            texIdxEdit++;

                            if (texIdxEdit >= apronTexEdit.Length)
                            {
                                texIdxEdit = 0;
                            }
                        }
                    }
                    else */

                    /*
                    if (loadApronEdit0)
                    {
                        apronRend.materials[5].SetTexture("_DecalTex", apronTexEdit0[texIdxEdit0]); // 5 = Apron Right
                        apronRend.materials[4].SetTexture("_DecalTex", apronTexEdit0[texIdxEdit0]); // 4 = Apron Left

                        texIdxEdit0++;

                        if (texIdxEdit0 >= apronTexEdit0.Length)
                        {
                            texIdxEdit0 = 0;
                        }
                    }

                    if (loadApronEdit1)
                    {
                        apronRend.materials[5].SetTexture("_DecalTex", apronTexEdit1[texIdxEdit1]); // 5 = Apron Right
                        apronRend.materials[4].SetTexture("_DecalTex", apronTexEdit1[texIdxEdit1]); // 4 = Apron Left

                        texIdxEdit1++;

                        if (texIdxEdit1 >= apronTexEdit1.Length)
                        {
                            texIdxEdit1 = 0;
                        }
                    }
                    */

                    if (timeMethod == "Seconds")
                    {
                        if (loadApron)
                        {
                            if (EntranceScene.inst.totalTimer % (texSwapTimeR * 30) == 0)
                            {
                                apronRend.materials[5].SetTexture("_DecalTex", apronTexRight[texIdxRight]); // 5 = Apron Right
                                texIdxRight++;
                                // L.D("RIGHT APRON TEX CHANGE // SECONDS METHOD");


                                if (texIdxRight >= apronTexRight.Length)
                                {
                                    texIdxRight = 0;
                                }
                            }

                            if (EntranceScene.inst.totalTimer % (texSwapTimeL * 30) == 0)
                            {
                                if (mirroredAprons == 1)
                                {
                                    apronRend.materials[4].SetTexture("_DecalTex", apronTexRight[texIdxLeft]); // 4 = Apron Left
                                    texIdxLeft++;
                                    // L.D("LEFT APRON TEX CHANGE // SECONDS METHOD");

                                    if (texIdxLeft >= apronTexRight.Length)
                                    {
                                        texIdxLeft = 0;
                                    }
                                }
                                else
                                {
                                    apronRend.materials[4].SetTexture("_DecalTex", apronTexLeft[texIdxLeft]); // 4 = Apron Left
                                    texIdxLeft++;
                                    // L.D("LEFT APRON TEX CHANGE // SECONDS METHOD");

                                    if (texIdxLeft >= apronTexLeft.Length)
                                    {
                                        texIdxLeft = 0;
                                    }
                                }
                            }
                        }
                    }
                    else if (timeMethod == "Frames")
                    {
                        if (loadApron)
                        {
                            if (EntranceScene.inst.totalTimer % texSwapTimeR == 0)
                            {
                                if (loadApron)
                                {
                                    apronRend.materials[5].SetTexture("_DecalTex", apronTexRight[texIdxRight]); // 5 = Apron Right
                                    texIdxRight++;
                                    // L.D("RIGHT APRON TEX CHANGE // FRAMES METHOD");


                                    if (texIdxRight >= apronTexRight.Length)
                                    {
                                        texIdxRight = 0;
                                    }
                                }
                            }

                            if (EntranceScene.inst.totalTimer % texSwapTimeL == 0)
                            {
                                if (mirroredAprons == 1)
                                {
                                    apronRend.materials[4].SetTexture("_DecalTex", apronTexRight[texIdxLeft]); // 4 = Apron Left
                                    texIdxLeft++;
                                    // L.D("LEFT APRON TEX CHANGE // FRAMES METHOD");

                                    if (texIdxLeft >= apronTexRight.Length)
                                    {
                                        texIdxLeft = 0;
                                    }
                                }
                                else
                                {
                                    apronRend.materials[4].SetTexture("_DecalTex", apronTexLeft[texIdxLeft]); // 4 = Apron Left
                                    texIdxLeft++;
                                    // L.D("LEFT APRON TEX CHANGE // FRAMES METHOD");

                                    if (texIdxLeft >= apronTexLeft.Length)
                                    {
                                        texIdxLeft = 0;
                                    }
                                }
                            }
                        }
                    }



                }
            }
            



            /*
            if (apronRend == null)
            {
                return;
            }
            */

            
            if (apronRend != null)
            {
                if (MatchMain.inst.isTimeCounting)
                {
                    if (matTimeMethod == "Seconds")
                    {
                        if (loadMat)
                        {
                            if (MatchMain.inst.matchTime.frm != 0)
                            {
                                return;
                            }
                            if (MatchMain.inst.matchTime.sec % matTexSwapTime == 0)
                            {
                                apronRend.materials[0].SetTexture("_DecalTex", matTex[texIdxMat]);
                                apronRend.materials[1].SetTexture("_DecalTex", matTex[texIdxMat]);
                                apronRend.materials[2].SetTexture("_DecalTex", matTex[texIdxMat]);
                                apronRend.materials[3].SetTexture("_DecalTex", matTex[texIdxMat]);
                                texIdxMat++;
                                // L.D("MAT TEX CHANGE // SECONDS METHOD");

                                if (texIdxMat >= matTex.Length)
                                {
                                    texIdxMat = 0;
                                }
                            }
                        }
                    }
                    else if (matTimeMethod == "Frames")
                    {
                        if (loadMat)
                        {
                            if (MatchMain.inst.matchTime.frm % matTexSwapTime == 0)
                            {
                                if (loadMat)
                                {
                                    apronRend.materials[0].SetTexture("_DecalTex", matTex[texIdxMat]);
                                    apronRend.materials[1].SetTexture("_DecalTex", matTex[texIdxMat]);
                                    apronRend.materials[2].SetTexture("_DecalTex", matTex[texIdxMat]);
                                    apronRend.materials[3].SetTexture("_DecalTex", matTex[texIdxMat]);
                                    texIdxMat++;
                                    // L.D("MAT TEX CHANGE // FRAMES METHOD");


                                    if (texIdxMat >= matTex.Length)
                                    {
                                        texIdxMat = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (MatchMain.inst.State != MatchMain.StateEnum.EntranceScene || MatchMain.inst.State == MatchMain.StateEnum.Finished)
                {
                    int frames = 0;
                    frames++;
                    if (frames > 29)
                    {
                        frames = 0;
                    }

                    if (matTimeMethod == "Seconds")
                    {
                        if (loadMat)
                        {
                            if (frames % (matTexSwapTime * 30) == 0)
                            {
                                apronRend.materials[0].SetTexture("_DecalTex", matTex[texIdxMat]);
                                apronRend.materials[1].SetTexture("_DecalTex", matTex[texIdxMat]);
                                apronRend.materials[2].SetTexture("_DecalTex", matTex[texIdxMat]);
                                apronRend.materials[3].SetTexture("_DecalTex", matTex[texIdxMat]);
                                texIdxMat++;
                                // L.D("MAT TEX CHANGE // SECONDS METHOD");

                                if (texIdxMat >= matTex.Length)
                                {
                                    texIdxMat = 0;
                                }
                            }
                        }
                    }
                    else if (matTimeMethod == "Frames")
                    {
                        if (loadMat)
                        {
                            if (frames % matTexSwapTime == 0)
                            {
                                if (loadMat)
                                {
                                    apronRend.materials[0].SetTexture("_DecalTex", matTex[texIdxMat]);
                                    apronRend.materials[1].SetTexture("_DecalTex", matTex[texIdxMat]);
                                    apronRend.materials[2].SetTexture("_DecalTex", matTex[texIdxMat]);
                                    apronRend.materials[3].SetTexture("_DecalTex", matTex[texIdxMat]);
                                    texIdxMat++;
                                    // L.D("MAT TEX CHANGE // FRAMES METHOD");


                                    if (texIdxMat >= matTex.Length)
                                    {
                                        texIdxMat = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (matTimeMethod == "Seconds")
                    {
                        if (loadMat)
                        {
                            if (EntranceScene.inst.totalTimer % (matTexSwapTime * 30) == 0)
                            {
                                apronRend.materials[0].SetTexture("_DecalTex", matTex[texIdxMat]);
                                apronRend.materials[1].SetTexture("_DecalTex", matTex[texIdxMat]);
                                apronRend.materials[2].SetTexture("_DecalTex", matTex[texIdxMat]);
                                apronRend.materials[3].SetTexture("_DecalTex", matTex[texIdxMat]);
                                texIdxMat++;
                                // L.D("MAT TEX CHANGE // SECONDS METHOD");

                                if (texIdxMat >= matTex.Length)
                                {
                                    texIdxMat = 0;
                                }
                            }
                        }
                    }
                    else if (matTimeMethod == "Frames")
                    {
                        if (loadMat)
                        {
                            if (EntranceScene.inst.totalTimer % matTexSwapTime == 0)
                            {
                                if (loadMat)
                                {
                                    apronRend.materials[0].SetTexture("_DecalTex", matTex[texIdxMat]);
                                    apronRend.materials[1].SetTexture("_DecalTex", matTex[texIdxMat]);
                                    apronRend.materials[2].SetTexture("_DecalTex", matTex[texIdxMat]);
                                    apronRend.materials[3].SetTexture("_DecalTex", matTex[texIdxMat]);
                                    texIdxMat++;
                                    // L.D("MAT TEX CHANGE // FRAMES METHOD");


                                    if (texIdxMat >= matTex.Length)
                                    {
                                        texIdxMat = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }


            /*
            if (postRend0 == null)
            {
                return;
            }
            if (postRend1 == null)
            {
                return;
            }
            if (postRend2 == null)
            {
                return;
            }
            if (postRend3 == null)
            {
                return;
            }
            */


            if (postRend0 != null && postRend1 != null && postRend2 != null && postRend3 != null)
            {
                if (MatchMain.inst.isTimeCounting)
                {
                    if (postTimeMethod == "Seconds")
                    {
                        if (loadPost)
                        {
                            if (MatchMain.inst.matchTime.frm != 0)
                            {
                                return;
                            }
                            if (MatchMain.inst.matchTime.sec % postTexSwapTime == 0)
                            {
                                postRend0.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                postRend1.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                postRend2.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                postRend3.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                texIdxPost++;
                                // L.D("POST TEX CHANGE // SECONDS METHOD");

                                if (texIdxPost >= matTex.Length)
                                {
                                    texIdxPost = 0;
                                }
                            }
                        }
                    }
                    else if (postTimeMethod == "Frames")
                    {
                        if (loadPost)
                        {
                            if (MatchMain.inst.matchTime.frm % postTexSwapTime == 0)
                            {
                                if (loadPost)
                                {
                                    postRend0.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                    postRend1.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                    postRend2.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                    postRend3.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                    texIdxPost++;
                                    // L.D("POST TEX CHANGE // FRAMES METHOD");


                                    if (texIdxPost >= postTex.Length)
                                    {
                                        texIdxPost = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (MatchMain.inst.State != MatchMain.StateEnum.EntranceScene || MatchMain.inst.State == MatchMain.StateEnum.Finished)
                {
                    int frames = 0;
                    frames++;
                    if (frames > 29)
                    {
                        frames = 0;
                    }

                    if (postTimeMethod == "Seconds")
                    {
                        if (loadPost)
                        {
                            if (frames % (postTexSwapTime * 30) == 0)
                            {
                                postRend0.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                postRend1.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                postRend2.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                postRend3.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                texIdxPost++;
                                // L.D("POST TEX CHANGE // SECONDS METHOD");

                                if (texIdxPost >= postTex.Length)
                                {
                                    texIdxPost = 0;
                                }
                            }
                        }
                    }
                    else if (postTimeMethod == "Frames")
                    {
                        if (loadPost)
                        {
                            if (frames % postTexSwapTime == 0)
                            {
                                postRend0.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                postRend1.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                postRend2.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                postRend3.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                texIdxPost++;
                                // L.D("POST TEX CHANGE // FRAMES METHOD");


                                if (texIdxPost >= postTex.Length)
                                {
                                    texIdxPost = 0;
                                }

                            }
                        }
                    }
                }
                else
                {
                    if (postTimeMethod == "Seconds")
                    {
                        if (loadPost)
                        {
                            if (EntranceScene.inst.totalTimer % (postTexSwapTime * 30) == 0)
                            {
                                postRend0.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                postRend1.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                postRend2.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                postRend3.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                texIdxPost++;
                                // L.D("POST TEX CHANGE // SECONDS METHOD");

                                if (texIdxPost >= postTex.Length)
                                {
                                    texIdxPost = 0;
                                }
                            }
                        }
                    }
                    else if (postTimeMethod == "Frames")
                    {
                        if (loadPost)
                        {
                            if (EntranceScene.inst.totalTimer % postTexSwapTime == 0)
                            {
                                if (loadPost)
                                {
                                    postRend0.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                    postRend1.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                    postRend2.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                    postRend3.materials[0].SetTexture("_MainTex", postTex[texIdxPost]);
                                    texIdxPost++;
                                    // L.D("POST TEX CHANGE // FRAMES METHOD");


                                    if (texIdxPost >= postTex.Length)
                                    {
                                        texIdxPost = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }





            /* RAMPS

            if (rampRend != null)
            {
                if (MatchMain.inst.isTimeCounting)
                {
                    if (rampTimeMethod == "Seconds")
                    {
                        if (loadRamp)
                        {
                            if (MatchMain.inst.matchTime.frm != 0)
                            {
                                return;
                            }
                            if (MatchMain.inst.matchTime.sec % rampTexSwapTime == 0)
                            {
                                rampRend.materials[0].SetTexture("_MainTex", rampTex[texIdxRamp]);
                                texIdxRamp++;
                                // L.D("RAMP TEX CHANGE // SECONDS METHOD");

                                if (texIdxRamp >= rampTex.Length)
                                {
                                    texIdxRamp = 0;
                                }
                            }
                        }
                    }
                    else if (rampTimeMethod == "Frames")
                    {
                        if (loadRamp)
                        {
                            if (MatchMain.inst.matchTime.frm % rampTexSwapTime == 0)
                            {
                                if (loadRamp)
                                {
                                    rampRend.materials[0].SetTexture("_MainTex", rampTex[texIdxRamp]);

                                    texIdxRamp++;
                                    // L.D("RAMP TEX CHANGE // FRAMES METHOD");


                                    if (texIdxRamp >= rampTex.Length)
                                    {
                                        texIdxRamp = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                else if (MatchMain.inst.State != MatchMain.StateEnum.EntranceScene || MatchMain.inst.State == MatchMain.StateEnum.Finished)
                {
                    int frames = 0;
                    frames++;
                    if (frames > 29)
                    {
                        frames = 0;
                    }

                    if (rampTimeMethod == "Seconds")
                    {
                        if (loadRamp)
                        {
                            if (frames % (rampTexSwapTime * 30) == 0)
                            {
                                rampRend.materials[0].SetTexture("_MainTex", rampTex[texIdxRamp]);
                                texIdxRamp++;
                                // L.D("RAMP TEX CHANGE // SECONDS METHOD");

                                if (texIdxRamp >= rampTex.Length)
                                {
                                    texIdxRamp = 0;
                                }
                            }
                        }
                    }
                    else if (rampTimeMethod == "Frames")
                    {
                        if (loadRamp)
                        {
                            if (frames % rampTexSwapTime == 0)
                            {
                                if (loadRamp)
                                {
                                    rampRend.materials[0].SetTexture("_MainTex", rampTex[texIdxRamp]);
                                    texIdxRamp++;
                                    // L.D("RAMP TEX CHANGE // FRAMES METHOD");


                                    if (texIdxRamp >= rampTex.Length)
                                    {
                                        texIdxRamp = 0;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (rampTimeMethod == "Seconds")
                    {
                        if (loadRamp)
                        {
                            if (EntranceScene.inst.totalTimer % (rampTexSwapTime * 30) == 0)
                            {
                                rampRend.materials[0].SetTexture("_MainTex", rampTex[texIdxRamp]);
                                texIdxRamp++;
                                // L.D("RAMP TEX CHANGE // SECONDS METHOD");

                                if (texIdxRamp >= rampTex.Length)
                                {
                                    texIdxRamp = 0;
                                }
                            }
                        }
                    }
                    else if (rampTimeMethod == "Frames")
                    {
                        if (loadRamp)
                        {
                            if (EntranceScene.inst.totalTimer % rampTexSwapTime == 0)
                            {
                                if (loadRamp)
                                {
                                    rampRend.materials[0].SetTexture("_MainTex", rampTex[texIdxRamp]);
                                    texIdxRamp++;
                                    // L.D("RAMP TEX CHANGE // FRAMES METHOD");


                                    if (texIdxRamp >= rampTex.Length)
                                    {
                                        texIdxRamp = 0;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            */
        }







        [ControlPanel(Group = "AnmAprons")]
        public static Form apronConf()
        {
            if (ApronConfig.instance == null)
            {
                return new ApronConfig();
            }
            else
            {
                return null;
            }
        }
    }
}
