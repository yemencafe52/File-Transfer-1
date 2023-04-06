namespace FileSender
{
    using System;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            DialogResult r = ofd.ShowDialog();

            if( r == DialogResult.OK)
            {
                textBox3.Text = ofd.FileName;
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox3.Text))
            {
                return;
            }

            button1.Enabled = false;
            button2.Enabled = false;

            FileSender fs = new FileSender(textBox1.Text,int.Parse(textBox2.Text), textBox3.Text);

            var p = new Progress<float>(i =>
            {
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = 100;
                toolStripProgressBar1.Value = ((int)i);

                this.Text = string.Format("Sending %{0}", Math.Round(i, 2));

            }
            );

            bool res = await Task.Run(()=> fs.Send(p));

            //bool res = await Task.Run(() => fs.Send());

            if (!res)
            {
                MessageBox.Show("OPPS, SOMETHING WENT WRONG :(");
                this.Text = "Error! ";
            }

          
            toolStripProgressBar1.Value = 0;

            button1.Enabled = true;
            button2.Enabled = true;
        }
    }
}
