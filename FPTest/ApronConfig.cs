using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Ace
{
    public partial class ApronConfig : Form
    {
        public static ApronConfig instance = null;
        public ApronConfig()
        {
            instance = this;
            InitializeComponent();
        }

        private void ApronConfig_Load(object sender, EventArgs e)
        {
            UItimingMethod.SelectedIndex = 0;
            LoadRings();

            comboBox1.SelectedIndex = 0;
            LoadRingsM();

            comboBox2.SelectedIndex = 0;
            LoadRingsP();
        }

        private void LoadRings()
        {
            UIringList.Items.Clear();
            foreach (RingData ring in SaveData.inst.editRingData)
            {
                UIringList.Items.Add(ring.name);
            }
        }

        private void LoadRingsM()
        {
            listBox1.Items.Clear();
            foreach (RingData ring in SaveData.inst.editRingData)
            {
                listBox1.Items.Add(ring.name);
            }
        }

        private void LoadRingsP()
        {
            listBox2.Items.Clear();
            foreach (RingData ring in SaveData.inst.editRingData)
            {
                listBox2.Items.Add(ring.name);
            }
        }

        private void refreshRings_Click(object sender, EventArgs e)
        {
            LoadRings();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (UIringList.SelectedIndex == -1)
            {
                return;
            }

            if (!Directory.Exists("./AceModsData/AnmAprons/" + UIringList.SelectedItem.ToString() + "/"))
            {
                Directory.CreateDirectory("./AceModsData/AnmAprons/" + UIringList.SelectedItem.ToString() + "/");
            }

            StreamWriter datWriter = new StreamWriter("./AceModsData/AnmAprons/" + UIringList.SelectedItem.ToString() + "/" + UIringList.SelectedItem.ToString() + ".dat");
            datWriter.WriteLine(UItimingMethod.SelectedItem.ToString());
            datWriter.WriteLine(numericUpDown1.Value);
            datWriter.WriteLine(numericUpDown2.Value);
            
            switch (UImirrorRight.Checked)
            {
                case true:
                    datWriter.WriteLine(1);
                    break;
                case false:
                    datWriter.WriteLine(0);
                    break;
            }

            datWriter.Dispose();
            datWriter.Close();
        }

        private void UIringList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (File.Exists("./AceModsData/AnmAprons/" + UIringList.SelectedItem.ToString() + "/" + UIringList.SelectedItem.ToString() + ".dat"))
            {
                StreamReader datReader = new StreamReader("./AceModsData/AnmAprons/" + UIringList.SelectedItem.ToString() + "/" + UIringList.SelectedItem.ToString() + ".dat");

                try
                {
                    string format = datReader.ReadLine();
                    if (format == "Frames")
                    {
                        UItimingMethod.SelectedIndex = 0;
                    }
                    else
                    {
                        UItimingMethod.SelectedIndex = 1;
                    }
                    numericUpDown1.Value = int.Parse(datReader.ReadLine());
                    numericUpDown2.Value = int.Parse(datReader.ReadLine());
                    int check = int.Parse(datReader.ReadLine());
                    switch (check)
                    {
                        case 0:
                            UImirrorRight.Checked = false;
                            break;
                        case 1:
                            UImirrorRight.Checked = true;
                            break;
                    }
                }
                catch
                {
                    UItimingMethod.SelectedIndex = 0;
                    numericUpDown1.Value = 1;
                    numericUpDown2.Value = 1;
                    UImirrorRight.Checked = false;


                }

                /* 
                if (datReader.ReadLine() == "Seconds")
                {
                    UItimingMethod.SelectedIndex = 1;
                }
                else
                {
                    UItimingMethod.SelectedIndex = 0;
                }
                

                 UItimingMethod.SelectedItem = datReader.ReadLine();

                int timingMethod = -1;

                if (datReader.Peek() != -1)
                {
                    int.TryParse(datReader.ReadLine(), out timingMethod);
                    if (timingMethod != -1)
                    {
                        UItimingMethod.SelectedIndex = timingMethod;
                    }
                    else
                    {
                        UItimingMethod.SelectedIndex = 0;
                    }
                }
                else
                {
                    UItimingMethod.SelectedIndex = 0;
                }


                int numUpDown1 = -1;

                if (datReader.Peek() != -1)
                {
                    int.TryParse(datReader.ReadLine(), out numUpDown1);
                    if (numUpDown1 != -1)
                    {
                        numericUpDown1.Value = numUpDown1;
                    }
                    else
                    {
                        numericUpDown1.Value = 1;
                    }
                }
                else
                {
                    numericUpDown1.Value = 1;
                }

                int numUpDown2 = -1;

                if (datReader.Peek() != -1)
                {
                    int.TryParse(datReader.ReadLine(), out numUpDown2);
                    if (numUpDown2 != -1)
                    {
                        numericUpDown2.Value = numUpDown2;
                    }
                    else
                    {
                        numericUpDown2.Value = 1;
                    }
                }
                else
                {
                    numericUpDown2.Value = 1;
                }


                int mirrorBox = -1;

                if (datReader.Peek() != -1)
                {
                    int.TryParse(datReader.ReadLine(), out mirrorBox);
                    if (mirrorBox != -1)
                    {
                        switch (mirrorBox)
                        {
                            case 0:
                                UImirrorRight.Checked = false;
                                break;
                            case 1:
                                UImirrorRight.Checked = true;
                                break;
                        }
                    }
                    else
                    {
                        UImirrorRight.Checked = false;
                    }
                }
                else
                {
                    UImirrorRight.Checked = false;
                }


                // UItimingMethod.SelectedIndex = int.Parse(datReader.ReadLine());
                // numericUpDown1.Value = int.Parse(datReader.ReadLine());
                // numericUpDown2.Value = int.Parse(datReader.ReadLine());

                /* int check = int.Parse(datReader.ReadLine());

                switch (check)
                {
                    case 0:
                        UImirrorRight.Checked = false;
                        break;
                    case 1:
                        UImirrorRight.Checked = true;
                        break;
                        
                } */

                datReader.Dispose();
                datReader.Close();
            }
            else
            {
                UItimingMethod.SelectedIndex = 0;
                numericUpDown1.Value = 1;
                numericUpDown2.Value = 1;
                
                UImirrorRight.Checked = false;
            }
            saveButton_Click(null, null);
        }
        private void UItimingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (UItimingMethod.SelectedIndex)
            {
                case 0:
                    numericUpDown1.Value = 1;
                    numericUpDown2.Value = 1;
                    numericUpDown1.Maximum = 29;
                    numericUpDown2.Maximum = 29;
                    break;
                case 1:
                    numericUpDown1.Maximum = 59;
                    numericUpDown2.Maximum = 59;
                    break;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void UItimingMethod_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void UImirrorRight_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadRingsM();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
            {
                return;
            }

            if (!Directory.Exists("./AceModsData/AnmMats/" + listBox1.SelectedItem.ToString() + "/"))
            {
                Directory.CreateDirectory("./AceModsData/AnmMats/" + listBox1.SelectedItem.ToString() + "/");
            }

            StreamWriter datWriter = new StreamWriter("./AceModsData/AnmMats/" + listBox1.SelectedItem.ToString() + "/" + listBox1.SelectedItem.ToString() + ".dat");
            datWriter.WriteLine(comboBox1.SelectedItem.ToString());
            datWriter.WriteLine(numericUpDown1.Value);

            datWriter.Dispose();
            datWriter.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (File.Exists("./AceModsData/AnmMats/" + listBox1.SelectedItem.ToString() + "/" + listBox1.SelectedItem.ToString() + ".dat"))
            {
                StreamReader datReader = new StreamReader("./AceModsData/AnmMats/" + listBox1.SelectedItem.ToString() + "/" + listBox1.SelectedItem.ToString() + ".dat");

                try
                {
                    string format = datReader.ReadLine();
                    if (format == "Frames")
                    {
                        comboBox1.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBox1.SelectedIndex = 1;
                    }
                    numericUpDown3.Value = int.Parse(datReader.ReadLine());
                }
                catch
                {
                    comboBox1.SelectedIndex = 0;
                    numericUpDown3.Value = 1;
                }

                datReader.Dispose();
                datReader.Close();
            }
            else
            {
                comboBox1.SelectedIndex = 0;
                numericUpDown3.Value = 1;
            }
            button2_Click(null, null);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    numericUpDown3.Value = 1;
                    numericUpDown3.Maximum = 29;
                    break;
                case 1:
                    numericUpDown3.Maximum = 59;
                    break;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            LoadRingsP();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
            {
                return;
            }

            if (!Directory.Exists("./AceModsData/AnmPosts/" + listBox2.SelectedItem.ToString() + "/"))
            {
                Directory.CreateDirectory("./AceModsData/AnmPosts/" + listBox2.SelectedItem.ToString() + "/");
            }

            StreamWriter datWriter = new StreamWriter("./AceModsData/AnmPosts/" + listBox2.SelectedItem.ToString() + "/" + listBox2.SelectedItem.ToString() + ".dat");
            datWriter.WriteLine(comboBox2.SelectedItem.ToString());
            datWriter.WriteLine(numericUpDown4.Value);

            datWriter.Dispose();
            datWriter.Close();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (File.Exists("./AceModsData/AnmPosts/" + listBox2.SelectedItem.ToString() + "/" + listBox2.SelectedItem.ToString() + ".dat"))
            {
                StreamReader datReader = new StreamReader("./AceModsData/AnmPosts/" + listBox2.SelectedItem.ToString() + "/" + listBox2.SelectedItem.ToString() + ".dat");

                try
                {
                    string format = datReader.ReadLine();
                    if (format == "Frames")
                    {
                        comboBox2.SelectedIndex = 0;
                    }
                    else
                    {
                        comboBox2.SelectedIndex = 1;
                    }
                    numericUpDown4.Value = int.Parse(datReader.ReadLine());
                }
                catch
                {
                    comboBox2.SelectedIndex = 0;
                    numericUpDown4.Value = 1;
                }

                datReader.Dispose();
                datReader.Close();
            }
            else
            {
                comboBox2.SelectedIndex = 0;
                numericUpDown4.Value = 1;
            }
            button3_Click(null, null);
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    numericUpDown4.Value = 1;
                    numericUpDown4.Maximum = 29;
                    break;
                case 1:
                    numericUpDown4.Maximum = 59;
                    break;
            }
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}