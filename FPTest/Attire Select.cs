using System;
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
    public partial class Attire_Select : Form
    {
        public string chosenAttire;
        public static Attire_Select instance = null;

        public FileInfo[] extraCostumes;
        public int pl2;

        public Attire_Select(FileInfo[] list, int pl)
        {
            extraCostumes = list;
            pl2 = pl;
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Attire_Select_Load(object sender, EventArgs e)
        {
            //comboBox1.DataSource = Path.GetFileNameWithoutExtension("./AceModsData/AttireExtension/" + DataBase.GetWrestlerFullName(EditFuckery.plObj.WresParam) + "*.txt");
            foreach (FileInfo s in extraCostumes)
            {
                comboBox1.Items.Add(s.Name.Replace(s.Extension, ""));
            }
            label2.Text = DataBase.GetWrestlerFullName(EditFuckery.plObj.WresParam);
            label3.Text = "Slot: " + pl2.ToString();
            this.TopMost = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            chosenAttire = (comboBox1.SelectedItem.ToString().Replace(DataBase.GetWrestlerFullName(EditFuckery.plObj.WresParam), ""));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
