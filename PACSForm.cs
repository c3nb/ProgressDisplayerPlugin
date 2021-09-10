using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProgressDisplayerPlugin
{
    public partial class PACSForm : Form
    {
        public PACSForm()
        {
            InitializeComponent();
        }

        private void PACSForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = Main.settings.ProgressPlayingForm;
            textBox2.Text = Main.settings.ProgressNotPlayingForm;
            textBox3.Text = Main.settings.AccuracyPlayingForm;
            textBox4.Text = Main.settings.AccuracyNotPlayingForm;
            textBox5.Text = Main.settings.PerfectsComboPlayingForm;
            textBox6.Text = Main.settings.PerfectsComboNotPlayingForm;
            textBox7.Text = Main.settings.ScorePlayingForm;
            textBox8.Text = Main.settings.ScoreNotPlayingForm;
        }

        private void PACSForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Main.settings.ProgressPlayingForm = textBox1.Text;
            Main.settings.ProgressNotPlayingForm = textBox2.Text;
            Main.settings.AccuracyPlayingForm = textBox3.Text;
            Main.settings.AccuracyNotPlayingForm = textBox4.Text;
            Main.settings.PerfectsComboPlayingForm = textBox5.Text;
            Main.settings.PerfectsComboNotPlayingForm = textBox6.Text;
            Main.settings.ScorePlayingForm = textBox7.Text;
            Main.settings.ScoreNotPlayingForm = textBox8.Text;
            Main.settings.OnChange();
        }
    }
}
