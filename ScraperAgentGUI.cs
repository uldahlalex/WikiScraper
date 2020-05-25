﻿using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WikiScraper
{
    public partial class ScraperAgentGUI : Form
    {

        
        Crawler crawler;
        CancellationTokenSource ts;


        public void Prepare()
        {
            var series = new Series("Finance");

            // Frist parameter is X-Axis and Second is Collection of Y- Axis
            series.Points.DataBindXY(new[] { 2001, 2002, 2003, 2004 }, new[] { 100, 200, 90, 150 });
            chart1.Series.Add(series);
            ts = new CancellationTokenSource();
            try
            {
                int crawlers = 1;
                if (crawlers == 0)
                {
                    throw new Exception();
                }
                for (int i = 0; i < crawlers; i++)
                {
                  
                    var crawler = new Crawler();

                    Task t = Task.Run(() =>
                    { 
                        Dictionary<string, int> dict = crawler.Start(ts.Token, this.textBox1.Text);
                        
                        MethodInvoker update = delegate
                        {

                            foreach (var item in dict)
                            {

                                //ListViewItem listItem = new ListViewItem(item+"");
                                richTextBox1.AppendText(item.Value + "     " + item.Key + "\n");

                            }
                            


                        };
                        richTextBox1.BeginInvoke(update);
                    }
                        );
                }

            }
            catch (Exception ex)
            {
                
                return;
            }
        }


        public ScraperAgentGUI()
        {
            InitializeComponent();

            this.webBrowser1.Navigate(this.textBox1.Text);
            //this.webBrowser1.Navigate("https://en.m.wikipedia.org/wiki/Christopher_Columbus");

        }




        #region GUI controls related methods


        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(textBox1.Text);
            textBox2.Clear();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void saveAsxsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {



        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {
            
            textBox1.Clear();
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.Prepare();
        }
    }
}