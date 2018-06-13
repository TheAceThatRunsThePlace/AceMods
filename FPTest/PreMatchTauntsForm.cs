using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DG;
using System.Text.RegularExpressions;

namespace Ace
{
    public partial class PreMatchTauntsForm : Form
    {
        public static PreMatchTauntsForm instance = null;
        public PreMatchTauntsForm()
        {
            instance = this;
            InitializeComponent();
        }

        private void PreMatchTauntsForm_Load(object sender, EventArgs e)
        {
            if (!Directory.Exists("./AceModsData/"))
            {
                Directory.CreateDirectory("./AceModsData/");
            }

            if (!File.Exists("./AceModsData/PreMatchTaunts.txt"))
            {
                File.Create("./AceModsData/PreMatchTaunts.txt");
            }

            LoadWrestlers();
        }

        private void LoadWrestlers()
        {
            listBox1.Items.Clear();
            foreach (EditWrestlerData edit in SaveData.inst.editWrestlerData)
            {
                listBox1.Items.Add(DataBase.GetWrestlerFullName(edit.wrestlerParam));
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadWrestlers();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<string> tauntList = new List<string>();

            foreach (string s in File.ReadAllLines("./AceModsData/PreMatchTaunts.txt"))
            {
                tauntList.Add(s);
            }

            if (comboBox1.SelectedItem.ToString() != "None")
            {
                if (tauntList.Contains(listBox1.SelectedItem.ToString()))
                {
                    if (IsNumeric(Regex.Match(comboBox1.SelectedItem.ToString(), @"\d+").Value))
                    {
                        tauntList[tauntList.IndexOf(listBox1.SelectedItem.ToString()) + 1] = Regex.Match(comboBox1.SelectedItem.ToString(), @"\d+").Value;
                    }
                    else
                    {
                        tauntList[tauntList.IndexOf(listBox1.SelectedItem.ToString()) + 1] = "5";
                    }
                }
                else
                {
                    tauntList.Add(listBox1.SelectedItem.ToString());
                    if (IsNumeric(Regex.Match(comboBox1.SelectedItem.ToString(), @"\d+").Value))
                    {
                        tauntList.Add(Regex.Match(comboBox1.SelectedItem.ToString(), @"\d+").Value);
                    }
                    else
                    {
                        tauntList.Add("5");
                    }
                }
            }
            else
            {
                if (tauntList.Contains(listBox1.SelectedItem.ToString()))
                {
                    tauntList.RemoveAt(tauntList.IndexOf(listBox1.SelectedItem.ToString()) + 1);
                    tauntList.RemoveAt(tauntList.IndexOf(listBox1.SelectedItem.ToString()));
                }
            }

            //if (numericUpDown1.Value != 0)
            //{
            //    if (tauntList.Contains(listBox1.SelectedItem.ToString()))
            //    {
            //        tauntList[tauntList.IndexOf(listBox1.SelectedItem.ToString()) + 1] = numericUpDown1.Value.ToString();
            //    }
            //    else
            //    {
            //        tauntList.Add(listBox1.SelectedItem.ToString());
            //        tauntList.Add(numericUpDown1.Value.ToString());
            //    }
            //}
            //else
            //{
            //    if (tauntList.Contains(listBox1.SelectedItem.ToString()))
            //    {
            //        tauntList.RemoveAt(tauntList.IndexOf(listBox1.SelectedItem.ToString()) + 1);
            //        tauntList.RemoveAt(tauntList.IndexOf(listBox1.SelectedItem.ToString()));
            //    }
            //}

            string[] lines = tauntList.ToArray();
            File.WriteAllLines("./AceModsData/PreMatchTaunts.txt", lines);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> tauntList = new List<string>();

            foreach (string s in File.ReadAllLines("./AceModsData/PreMatchTaunts.txt"))
            {
                tauntList.Add(s);
            }

            if (tauntList.Contains(listBox1.SelectedItem.ToString()))
            {
                comboBox1.SelectedIndex = int.Parse(tauntList[tauntList.IndexOf(listBox1.SelectedItem.ToString()) + 1]);
            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }

        }

        public static bool IsNumeric(object Expression)
        {
            double retNum;

            bool isNum = Double.TryParse(Convert.ToString(Expression), System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
            return isNum;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == " " || textBox1.Text == "  ")
            {
                return;
            }
            for (int i = listBox1.Items.Count - 1; i >= 0; i--)
            {
                if (listBox1.Items[i].ToString().ToLower().Contains(textBox1.Text.ToLower()))
                {
                    listBox1.SetSelected(i, true);
                }
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
