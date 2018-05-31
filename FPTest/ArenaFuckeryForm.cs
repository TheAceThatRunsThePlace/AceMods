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
            listBox2.Items.Clear();
            // Ring r = Ring.inst;
            // var test = r.transform;
            //foreach (UnityEngine.Component c in test.GetComponentsInChildren<UnityEngine.Component>())
            foreach (UnityEngine.Component c in GameObject.FindObjectsOfType<UnityEngine.Component>())
            {
                if (c.transform.childCount > 1)
                {
                    listBox1.Items.Add(c.name);
                }
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateListBox2();
        }

        public void populateListBox2()
        {
            listBox2.Items.Clear();
            GameObject test = GameObject.Find(listBox1.SelectedItem.ToString());
            // var t = test.transform;
            if (test.transform.childCount > 0)
            {
                foreach (UnityEngine.Component c in test.GetComponentsInChildren<UnityEngine.Component>())
                {
                    listBox2.Items.Add(c.name);
                }
            }            
        }

        private void ArenaFuckeryForm_Load(object sender, EventArgs e)
        {
            // listComps();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listComps();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
