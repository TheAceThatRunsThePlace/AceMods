﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DG;
using UnityEngine;
using System.IO;

namespace Ace
{
    
    
    public partial class AttireExtensionForm : Form
    {
        public static string name;
        public static int id;
        public static int[] test = new int[0];
        public static int[] numVal = new int[8];
        public static SaveData saveData = SaveData.inst;

        public static int cos;
        public static int txtToImport;

        public static AttireExtensionForm instance = null;
        public AttireExtensionForm()
        {
            instance = this;
            InitializeComponent();
        }

        private void AttireExtensionForm_Load(object sender, EventArgs e)
        {
            LoadWrestlers();
        }

        private void LoadWrestlers()
        {
            listBox1.Items.Clear();
            foreach (EditWrestlerData edit in SaveData.inst.editWrestlerData)
            {
                listBox1.Items.Add(DataBase.GetWrestlerFullName(edit.wrestlerParam));
            }
            int[] test = new int[listBox1.Items.Count];
        }

        private void SaveAllAttires()
        {
            name = listBox1.SelectedItem.ToString();
            id = listBox1.SelectedIndex;

            saveData = SaveData.inst;
            EditWrestlerData editDat = saveData.editWrestlerData[id];
            CostumeData[] cosDat = editDat.appearanceData.costumeData;


            try
            {
                for (int a = 0; a < 4; a++)
                {
                    if (!cosDat[a].valid)
                    {
                        return;
                    }

                    using (StreamWriter streamWriter = new StreamWriter("./AceModsData/AttireExtension/" + name + "_" + (a + 1) + ".txt"))
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                streamWriter.WriteLine(cosDat[a].layerTex[i, j]);
                                streamWriter.WriteLine(cosDat[a].color[i, j].r);
                                streamWriter.WriteLine(cosDat[a].color[i, j].g);
                                streamWriter.WriteLine(cosDat[a].color[i, j].b);
                                streamWriter.WriteLine(cosDat[a].color[i, j].a);
                                streamWriter.WriteLine(cosDat[a].highlightIntensity[i, j]);
                            }
                            streamWriter.WriteLine(cosDat[a].partsScale[i]);
                        }
                        streamWriter.Dispose();
                        streamWriter.Close();
                    }
                }

            }
            catch
            {
                MessageBox.Show("Couldn't save attire to './AceModsData/AttireExtension/" + name + ".txt'");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                SaveAllAttires();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            saveData = SaveData.inst;
            if (!saveData.editWrestlerData[listBox1.SelectedIndex].appearanceData.costumeData[0].valid)
            {
                button4.Enabled = false;
            }
            else
            {
                button4.Enabled = true;
            }
            if (!saveData.editWrestlerData[listBox1.SelectedIndex].appearanceData.costumeData[1].valid)
            {
                button5.Enabled = false;
            }
            else
            {
                button5.Enabled = true;
            }
            if (!saveData.editWrestlerData[listBox1.SelectedIndex].appearanceData.costumeData[2].valid)
            {
                button3.Enabled = false;
            }
            else
            {
                button3.Enabled = true;
            }
            if (!saveData.editWrestlerData[listBox1.SelectedIndex].appearanceData.costumeData[3].valid)
            {
                button6.Enabled = false;
            }
            else
            {
                button6.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadWrestlers();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void SaveIndividualAttires(int num)
        {
            name = listBox1.SelectedItem.ToString();
            id = listBox1.SelectedIndex;

            saveData = SaveData.inst;
            EditWrestlerData editDat = saveData.editWrestlerData[id];
            CostumeData[] cosDat = editDat.appearanceData.costumeData;

            if (cosDat[0].valid)
            {
                SaveFileDialog savefile = new SaveFileDialog();
                savefile.InitialDirectory = "./AceModsData/AttireExtension/";
                savefile.Filter = "TXT files (*.txt)|*.txt";
                savefile.FileName = listBox1.SelectedItem.ToString() + "_";
                if (savefile.ShowDialog() == DialogResult.OK)
                {

                    try
                    {
                        using (StreamWriter streamWriter = new StreamWriter(savefile.FileName))
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                for (int j = 0; j < 16; j++)
                                {
                                    streamWriter.WriteLine(cosDat[num].layerTex[i, j]);
                                    streamWriter.WriteLine(cosDat[num].color[i, j].r);
                                    streamWriter.WriteLine(cosDat[num].color[i, j].g);
                                    streamWriter.WriteLine(cosDat[num].color[i, j].b);
                                    streamWriter.WriteLine(cosDat[num].color[i, j].a);
                                    streamWriter.WriteLine(cosDat[num].highlightIntensity[i, j]);
                                }
                                streamWriter.WriteLine(cosDat[num].partsScale[i]);
                            }
                            streamWriter.Dispose();
                            streamWriter.Close();
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Couldn't save attire to './AceModsData/AttireExtension/" + name + ".txt'");
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                SaveIndividualAttires(0);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                SaveIndividualAttires(1);
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                SaveIndividualAttires(2);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                SaveIndividualAttires(3);
            }
        }

        private void ImportAttire(int cos)
        {
            GlobalWork gw = GlobalWork.inst;
            PlayerMan pm = PlayerMan.inst;
            saveData = SaveData.inst;
            int id = listBox1.SelectedIndex;
            string plObjname = DataBase.GetWrestlerFullName(saveData.editWrestlerData[listBox1.SelectedIndex].wrestlerParam);
            CostumeData plObjCos = saveData.editWrestlerData[listBox1.SelectedIndex].appearanceData.costumeData[cos];

            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "TXT files (*.txt)|*.txt";
            fileDialog.InitialDirectory = "./AceModsData/AttireExtension/";


            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                DialogResult dialogResult = MessageBox.Show("Overwrite in-game attire " + (cos+1) + " with " + fileDialog.FileName + "? Are you certain? There is no undoing this.", "Please confirm your action.", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    StreamReader cdReader = new StreamReader(fileDialog.FileName);
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
                        plObjCos.valid = true;
                        for (int i = 0; i < 9; i++)
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                plObjCos.layerTex[i, j] = loadedCostumeData.layerTex[i, j];
                                plObjCos.color[i, j].r = loadedCostumeData.color[i, j].r;
                                plObjCos.color[i, j].g = loadedCostumeData.color[i, j].g;
                                plObjCos.color[i, j].b = loadedCostumeData.color[i, j].b;
                                plObjCos.color[i, j].a = loadedCostumeData.color[i, j].a;
                                plObjCos.highlightIntensity[i, j] = loadedCostumeData.highlightIntensity[i, j];
                            }
                            plObjCos.partsScale[i] = loadedCostumeData.partsScale[i];
                        }

                        //plObjCos = loadedCostumeData;
                        L.D("ATTIRE EXTENSION: ATTIRE IMPORTED TO ATTIRE SLOT " + cos);
                    }
                    catch
                    {
                        L.D("ATTIRE EXTENSION: ATTIRE NOT IMPORTED");
                    }


                    cdReader.Dispose();
                    cdReader.Close();
                }
                else
                {
                    return;
                }
                
            }


        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                ImportAttire(0);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                ImportAttire(1);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                ImportAttire(2);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
                ImportAttire(3);
        }
    }
}
