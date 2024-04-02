using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TaskScheduler
{
    public partial class Form1 : Form
    {
        int windth = 830;
        public Form1()
        {
            InitializeComponent();
            listView1.CheckBoxes = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            out_file();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            Pen blackPen = new Pen(Color.Black, 2);
            g.DrawLine(blackPen, 0, 25, windth, 25);
            blackPen.Dispose();
            g.Dispose();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                ListViewItem item = new ListViewItem();
                string date = dateTimePicker1.Value.ToString();
                if (checkBox1.Checked == true)
                {
                    item.Checked = true;
                    item.SubItems.Add(textBox1.Text);
                    item.SubItems.Add("Сделано");
                    item.BackColor = Color.Blue;
                }
                else if (DateTime.Now > dateTimePicker1.Value)
                {
                    item.SubItems.Add(textBox1.Text);
                    item.SubItems.Add("Просроченно");
                    item.BackColor = Color.Red;
                }
                else if (DateTime.Now <= dateTimePicker1.Value)
                { 
                    item.SubItems.Add(textBox1.Text);
                    item.SubItems.Add("В процессе");
                    item.BackColor = Color.LightGray;
                }
                item.SubItems.Add(date);
                listView1.Items.Add(item);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach(ListViewItem item2 in listView1.Items)
            {
                if (item2.Selected)
                    item2.Remove();
            }
        }

        public void in_file()
        {
            string file_path = "file.txt";
            if (!File.Exists(file_path))
                File.Create(file_path).Close();
            ListViewItem item = new ListViewItem();
            StreamWriter write = new StreamWriter(file_path);
            foreach (ListViewItem item2 in listView1.Items)
            {
                string str = $"{item2.SubItems[1].Text}_{item2.SubItems[2].Text}_{item2.SubItems[3].Text}";
                write.WriteLine(str);
            }
            write.Close();
        }

        public void out_file()
        {
            string file_path = "file.txt";
            if (File.Exists(file_path))
            {
                StreamReader reader = new StreamReader(file_path);
                string line = reader.ReadLine();
                while (line != null)
                {
                    string[] values = line.Split('_');
                    if (values.Length == 3)
                    {
                        ListViewItem item = new ListViewItem();
                        item.SubItems.Add(values[0]);
                        item.SubItems.Add(values[1]);
                        item.SubItems.Add(values[2]);
                        DateTime date = DateTime.Parse(item.SubItems[3].Text);
                        if (item.SubItems[2].Text == "Сделано")
                        {
                            item.Checked = true;
                            item.BackColor = Color.CornflowerBlue;
                        }
                        else if (DateTime.Now <= date) { 
                            item.SubItems[2].Text = "В процессе";
                            item.BackColor = Color.LightGray;
                        }
                        else if (DateTime.Now > date)
                        {
                            item.SubItems[2].Text = "Просроченно";
                            item.BackColor = Color.Tomato;
                        }

                        listView1.Items.Add(item);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            in_file();
        }
    }
    
}
