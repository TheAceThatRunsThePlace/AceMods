using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DG;

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

        }
    }
}
