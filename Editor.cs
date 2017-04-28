using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Editor_de_Texto
{
    public partial class Editor : Form
    {
        UdpClient udpCliente = new UdpClient();
        public Editor()
        {
            InitializeComponent();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox_Editor.Clear();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            udpCliente.Close();
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text Files (.txt)|*.txt";
            ofd.Title = "Open a file...";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(ofd.FileName);
                richTextBox_Editor.Text = sr.ReadToEnd();
                sr.Close();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sendFile(richTextBox_Editor.Text);
            sendFileFormat(richTextBox_Editor.SelectionFont.ToString() + "\n" +
                           richTextBox_Editor.SelectionColor.ToString());
            //SaveFileDialog svf = new SaveFileDialog();
            //svf.Filter = "Text Files (.txt)|*.txt";
            //svf.Title = "Save file...";
            //if (svf.ShowDialog() == DialogResult.OK)
            //{
            //    System.IO.StreamWriter sw = new System.IO.StreamWriter(svf.FileName);
            //    sw.Write(richTextBox_Editor.Text);
            //    sw.Close();

            //    System.IO.StreamWriter sw_aux = new System.IO.StreamWriter(svf.FileName + "_format.txt");
            //    sw_aux.Write(richTextBox_Editor.SelectionFont.ToString() + "\n" +
            //                 richTextBox_Editor.SelectionColor.ToString());
            //    sw_aux.Close();
            //}
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sendFile(richTextBox_Editor.Text);
            sendFileFormat(richTextBox_Editor.SelectionFont.ToString() + "\n" +
                           richTextBox_Editor.SelectionColor.ToString());
            //SaveFileDialog svf = new SaveFileDialog();
            //svf.Filter = "Text Files (.txt)|*.txt";
            //svf.Title = "Save file...";
            //if (svf.ShowDialog() == DialogResult.OK)
            //{
            //    System.IO.StreamWriter sw = new System.IO.StreamWriter(svf.FileName);
            //    sw.Write(richTextBox_Editor.Text);
            //    sw.Close();

            //    System.IO.StreamWriter sw_aux = new System.IO.StreamWriter(svf.FileName + "_format.txt");
            //    sw_aux.Write(richTextBox_Editor.SelectionFont.ToString() + "\n" +
            //                 richTextBox_Editor.SelectionColor.ToString());
            //    sw_aux.Close();
            //}
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox_Editor.Undo();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox_Editor.Redo();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox_Editor.Cut();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox_Editor.Copy();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox_Editor.Paste();
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox_Editor.SelectAll();
        }

        private void customizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();
            fd.Font = richTextBox_Editor.SelectionFont;
            if (fd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                richTextBox_Editor.SelectionFont = fd.Font;
            }
        }

        private void fontColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = richTextBox_Editor.SelectionColor;
            if (cd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                richTextBox_Editor.SelectionColor = cd.Color;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Trabalho II - Editor de Texto - 5 pontos: Socket UDP \n" + 
                            "- Salvar dois arquivos(extensão txt) no servidor, um deverá conter o texto digitado, o outro a formatação escolhida pelo usuário. \n" +
                            "- Opções de Formatação básica: Tipo de fonte, Tamanho, Cor, Negrito, Itálico e Normal.");
        }

        private void sendFile (string text)
        {
            Int32 port = 11000;
            IPAddress ip = IPAddress.Parse("192.168.75.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ip,port);
            byte[] content = Encoding.ASCII.GetBytes(text);
            try
            {
                int count = udpCliente.Send(content, content.Length, ipEndPoint);
                if (count > 0)
                {
                    MessageBox.Show("Texto salvo.", "Informação", MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Erro ao salvar!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void sendFileFormat(string text)
        {
            Int32 port2 = 11001;
            IPAddress ip = IPAddress.Parse("192.168.75.1");
            IPEndPoint ipEndPoint = new IPEndPoint(ip, port2);
            byte[] content2 = Encoding.ASCII.GetBytes(text);
            try
            {
                int count = udpCliente.Send(content2, content2.Length, ipEndPoint);
                if (count > 0)
                {
                    MessageBox.Show("Arquivo de formato salvo.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch
            {
                MessageBox.Show("Erro ao salvar!", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
