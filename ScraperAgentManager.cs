using HtmlAgilityPack;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WikiScraper.models;

namespace WikiScraper
{
    public partial class ScraperAgentManager : Form
    {

        CancellationTokenSource ts;
        public Dictionary<string, int> dict = new Dictionary<string, int>();

        public List<Link> links = new List<Link>();

        public int articles = 1;

        public string allText = "";

        private void updateMessage()
        {
            richTextBox3.Text = "Currently working...";
        }

        public void Prepare()
        {
            updateMessage();

            articles++;
            Link link = new Link();

            link.URL = textBox1.Text;
            link.Depth = articles;
            link.visited = false;

            links.Add(link);

            ts = new CancellationTokenSource();
            foreach (Link l in links) //pt registrerer den ikke den ever-forøgende mængde af links - den tager blot de som er til stede pt, og scraper disse. Det er derfor, at når man trykker "start" vil man begynde at scrape nye sider
            {
                if (l.visited != true)
                {
                    try
                    {
                        decimal threads = numericUpDown2.Value;

                        if ((int)threads == 0)
                        {
                            throw new Exception();
                        }
                        for (int i = 0; i < (int)threads; i++)
                        {
                            var crawler = new Crawler(this);
                            DateTime when = dateTimePicker1.Value;
                            DateTime now = DateTime.Now;
                            TimeSpan span = when - now;

                            int millis = Convert.ToInt32(span.TotalMilliseconds);
                            if (millis > 0)
                            {
                                Task.Delay(millis).ContinueWith(t => {
                                    MethodInvoker update = delegate
                                    {
                                        dict = this.frequencies(allText);
                                        setGUI();
                                    };
                                    crawler.Start(ts.Token, l, numericUpDown1.Value);
                                    BeginInvoke(update);
                                });


                                /*
                                Task t = Task.Delay(millis);
                                t.ContinueWith(task =>
                                {
                                    crawler.Start(ts.Token, l, numericUpDown1.Value);
                                });*/
                            }
                            else if(millis<0)
                            {
                                Task t = Task.Run(() =>
                                {
                                    crawler.Start(ts.Token, l, numericUpDown1.Value);
                                    Console.WriteLine("hello world");
                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return;
                    }
                }

            }
            
            
           
                
        }

        private Dictionary<string, int> frequencies(string text)
        {
            Dictionary<string, int> count =
                text.Split(' ')
                    .GroupBy(s => s)
                    .ToDictionary(g => g.Key, g => g.Count());
            var items = from item in count orderby item.Value descending select item;
            return items.ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private void setGUI()
        {
            //start off by clearing the GUI and then setting it
            richTextBox1.Clear();

            foreach (var item in dict)
            {
                richTextBox1.AppendText(item.Value + "     " + item.Key + "\n");
            }
            //setting the chart
            /*int[] vals = new int[dict.Count];
            dict.Values.CopyTo(vals, 0);
            int[] shortened = new int[5];
            Array.Copy(vals, shortened, shortened.Length);

            String[] keys = new String[dict.Count];
            dict.Keys.CopyTo(keys, 0);
            String[] shortKeys = new String[5];
            Array.Copy(keys, shortKeys, shortKeys.Length);

            var series = new Series("Highest word frequencies");
            series.Points.DataBindXY(shortKeys, shortened);
            chart1.Series.Add(series);
            dict.Clear();*/
            int num = 0;
            foreach (Link l in links)
            {
                richTextBox2.AppendText(num++ + ": " + l.URL + "\n\n");
            }
            richTextBox3.Text = "Scraping completed";


        }


        public ScraperAgentManager()
        {
            InitializeComponent();
            //Prepare();
            richTextBox3.Clear();
            richTextBox1.Clear();
            richTextBox2.Clear();
            this.webBrowser1.Navigate(this.textBox1.Text);

        }




        #region GUI controls related methods

        private void saveAsCSV(object sender, EventArgs e)
        {
            Console.WriteLine("saving");
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "CSV file|*.csv";
            saveFileDialog1.Title = "Save data locally";
            saveFileDialog1.ShowDialog();


            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(saveFileDialog1.FileName.ToString());
                file.WriteLine(richTextBox1.Text);
                file.Close();
            }
        }

        private void openAboutAuthor(object sender, EventArgs e)
        {
            var formPopup = new Form();
            formPopup.Text = "About the program";

            RichTextBox aboutcontent = new RichTextBox();


            //aboutcontent.Location = new System.Drawing.Point(9, 212);

            aboutcontent.Name = "richTextBox1";
            aboutcontent.ReadOnly = true;
            aboutcontent.Size = new System.Drawing.Size(290, 300);
            //aboutcontent.TabIndex = 16;
            aboutcontent.Text = "Alex Uldahl Pedersen\nComputer Science Student\nLinkedIn: https://www.linkedin.com/in/alexuldahlpedersen/";



            formPopup.Controls.Add(aboutcontent);
            formPopup.Show(this); // if you need non-modal window
        }

        private void openAboutScraper(object sender, EventArgs e)
        {
            var formPopup = new Form();
            formPopup.Text = "About the program";

            RichTextBox aboutcontent = new RichTextBox();


            //aboutcontent.Location = new System.Drawing.Point(9, 212);

            aboutcontent.Name = "richTextBox1";
            aboutcontent.ReadOnly = true;
            aboutcontent.Size = new System.Drawing.Size(290, 300);
            //aboutcontent.TabIndex = 16;
            aboutcontent.Text = "Developed for examination purposes.\nSubject: Autonomous Agents\nLecturer: Bent H. Pedersen\nStudent: Alex Uldahl Pedersen\nInstitution: Business Academy Southwest\n\nC# .NET Framework 4.7.2 Windows Application\nMay 2020";



            formPopup.Controls.Add(aboutcontent);
            formPopup.Show(this); // if you need non-modal window
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.webBrowser1.Navigate(textBox1.Text);
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

        private void textBox2_TextChanged_1(object sender, EventArgs e)
        {

            textBox1.Clear();
        }
        #endregion

        private void button1_Click(object sender, EventArgs e)
        {

            this.Prepare();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click_1(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void openLink(object sender, LinkClickedEventArgs e)
        {
            this.webBrowser1.Navigate(e.LinkText);
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}