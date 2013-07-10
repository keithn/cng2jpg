using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace cng2jpg
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = textBox1.Text;
            if (Directory.Exists(path))
            {
                try
                {                                     
                    Convert(path);                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("UH OH, got an exception: {0}", ex));
                }                
            }
            else
            {
                MessageBox.Show("Not a valid folder...");
            }
        }

        private void Convert(string path)
        {
            
            var directories = Directory.GetDirectories(path, "*.*", SearchOption.AllDirectories).ToList();
            progressBar2.Value = 0;
            progressBar2.Maximum = directories.Count;
            progressBar2.Step = 1; 
            foreach (var list in directories.Select(directory => Directory.GetFiles(directory, "*.cng").ToList()))
            {
                progressBar1.Value = 0;
                progressBar1.Maximum = list.Count;
                Application.DoEvents();
                list.ForEach(f =>
                    {
                        File.WriteAllBytes(Path.ChangeExtension(f, "jpg"),
                                           File.ReadAllBytes(f).Select(b => (byte)(b ^ 0xEF)).ToArray());
                        progressBar1.Increment(1);
                        Application.DoEvents();
                    });    
                progressBar2.Increment(1);
                Application.DoEvents();
            }
            
        }
    }
}
