using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using Microsoft.Win32;
using System.IO;

namespace MailClient2
{
    public partial class SendMessageForm : Form
    {

        public static string login;
        public static string pass;
        //от кого
        // public static string from;
        //Кому письмо
        public static string to;
        //Тема письма
        public static string subject;
        //Текст письма
        public static string body;

        public static string ServerSmtp;



        public static MailMessage mail = new MailMessage();

        public SendMessageForm()
        {
            InitializeComponent();
        }

        private void SendMessageForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }


        public static void SendMail(string smtpServer, string from, string password,
string mailto, string caption, string message)
        {

            try
            {

                mail.From = new MailAddress(from);
                mail.To.Add(new MailAddress(mailto));
                mail.Subject = caption;
                mail.Body = message;


                SmtpClient client = new SmtpClient();
                client.Host = smtpServer;
                client.Port = 587;
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(from.Split('@')[0], password);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Send(mail);
                mail.Dispose();
                
                MessageBox.Show("Сообщение отправлено");

            }
            catch (Exception e)
            {
                //throw new Exception("Mail.Send: " + e.Message);
                MessageBox.Show("Сбой отправки сообщения : " + e.Message);
            }
            
        }

        private void groupBox3_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (!string.IsNullOrEmpty(files[0]))
                mail.Attachments.Add(new Attachment(files[0]));
            MessageBox.Show(files[0] + " Прикреплен");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StatusLabel.Text = "Отправлено";
            SendMail(ServerSmtp, textBox1.Text, passwordTextBox.Text, textBox3.Text, textBox4.Text, richTextBox1.Text);
           // MessageBox.Show(ServerSmtp + "\n" + textBox1.Text + "\n" + passwordTextBox.Text + "\n" + textBox3.Text + "\n" + textBox4.Text+ "\n" +richTextBox1.Text);
            listBox1.Items.Clear();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            ServerSmtp = comboBox1.SelectedItem.ToString();

        }

        private void listBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void listBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            foreach (string file in files)
            {
                listBox1.Items.Add(file);
                mail.Attachments.Add(new Attachment(file));
                StatusLabel.Text = "Файл прикреплен";
            }
            
        }


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                mail.Attachments.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                StatusLabel.Text = "Файл удален из списка";
            }
            catch (Exception){ }

        }









       

        
    }
}
