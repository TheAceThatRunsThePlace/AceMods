using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using UnityEngine;

namespace Ace
{
    public partial class ArenaFuckeryForm : Form
    {
        public static ArenaFuckeryForm instance = null;
        public ArenaFuckeryForm()
        {
            instance = this;
            InitializeComponent();
        }

        public void listComps()
        {
            listBox1.Items.Clear();
            foreach (UnityEngine.Component c in GameObject.FindObjectsOfType<UnityEngine.Component>())
            {
                listBox1.Items.Add(c.name);
            }
            



        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void ArenaFuckeryForm_Load(object sender, EventArgs e)
        {
            listComps();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listComps();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
