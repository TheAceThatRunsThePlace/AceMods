using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Ace
{
    public partial class MatchSelectionForm : Form
    {
        public static MatchSelectionForm instance = null;

        public MatchSelectionForm()
        {
            instance = this;
            InitializeComponent();
        }

        private void MatchSelectionForm_Load(object sender, EventArgs e)
        {

        }

        private void CompetitorList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
