using MySql.Data.MySqlClient;
using System.Timers;
using System.Windows.Forms.Integration; //Not so Given.
using System; //Given 
using System.Windows.Forms; //Given
using System.Diagnostics;

namespace Demo
{
    public partial class Form1 : Form
    {
        public readonly string version = "0.5.9";
        private int buttonX = 1000;
        private int buttonY = 60;

        double total=0;
        Cart c = new Cart();
        bool paid;
        static Form1 thisForm; // probably terrible :(

        public Form1()
        {
         
            InitializeComponent();

            // Make fullscreen
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            //this.TopMost = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;



            //
            Rectangle r = Screen.FromControl(this).Bounds;
            button1.Left = (r.Width - (r.Width)/4);
            button1.Top= (r.Height - (r.Height)/4);
            button2.Left = (r.Width - (r.Width) / 4);
            button2.Top = (r.Height - (r.Height) / 4) -100;

            panel1.Height= (r.Height - 200);
            textBox1.Width=panel1.Width;
            label5.Text = "� " + total;
            Utils.log("Init Form1");
            this.Text += " Version : " + version;
            thisForm = this;

            if (Debugger.IsAttached)
            {
                this.Text += " Debug Mode";
            }

            List<Product> buttons = Utils.getButtons();

            foreach(Product p in buttons)
            {
                Button myNewButton = new()
                {
                    Location = new Point(buttonX, buttonY),
                    Size = new Size(150, 50),
                    Text = p.desc,
                    Tag = p,
                    FlatStyle = FlatStyle.Flat,
                    ForeColor = Color.SlateBlue,
                    BackColor = SystemColors.Control
            };
                myNewButton.FlatAppearance.BorderColor = System.Drawing.Color.SteelBlue;
                myNewButton.FlatAppearance.BorderSize = 2;
                myNewButton.Click += dynButtton_Click;
                this.Controls.Add(myNewButton);


                buttonX += 175;
                if(buttonX >= 1875 )
                {
                    buttonY +=  100;
                    buttonX = 1000;
                }

            }

        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            new AddProd().Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (c.products.Count != 0)
            {
               
                Utils.recSale(c, total);
                Utils.log("Button Pay Now Pressed");

                bool paid = Payment.newPayment(total, c);

                this.textBox1.Enabled = false;

            }
            if (c.products.Count == 0)
            {
                MessageBox.Show("Please add items before Paying", "No Items Added");
                return;
            }


        }
        /*
         * Need to refactor the below as currenlty does not allow voids. 
         */
        private void textBox1_KeyUp(object sender, KeyEventArgs e) 
        {
            
            if (e.KeyCode == Keys.Enter)
            {
                if(textBox1.Text == "")
                {
                    return;
                }
                Product p = Utils.search(textBox1.Text.ToString());
                if (p != null)
                {
                    c.AddProd(p);
                    //string[] row = { p.PLU, p.desc, p.price.ToString(), "" };

                    //dataGridView1.Rows.Add(row);

                    label1.Text += "\n " + p.PLU;
                    label3.Text += "\n " + p.desc;
                    label2.Text += "\n �" + p.price;
                    total=Math.Round(total += p.price,2);
                    label5.Text = "� " + total;

                    textBox1.Focus();
                    textBox1.Text="";

                }
                if (p is null)
                {
                    MessageBox.Show(null,"No product found. \nPlease ensure PLU is correct","No product Found");
                    textBox1.Focus();
                    textBox1.SelectAll();
                }

            }
            else
            {
                
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void clear()
        {
            total = 0;
            label2.Text = "Price";
            label1.Text = "PLU";
            label3.Text = "Desc";
            label5.Text = "�" + total ;
            c.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            clear();
            textBox1.Focus();
        }

        private void dynButtton_Click(object sender, EventArgs e)
        {
            Button s = (Button)sender;
            addToCart((Product)s.Tag);
            //MessageBox.Show(sender.GetType().ToString());
           // addToCart(Utils.search());
        }

        public static void notify()
        {            

            Form1 f = Form1.thisForm;
            f.clear();
            f.textBox1.Enabled = true;
            f = null;
            Utils.log("Clear form");
        }
        public static void notifyBack()
        {

            Form1 f = Form1.thisForm;
            f.textBox1.Enabled = true;
            f = null;
            Utils.log("Back - enabling textbox");
        }

        private void toolStripLabel1_Click_1(object sender, EventArgs e)
        {
            new frmManage().Show();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            new Demo.settings().Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            new Forms.frmStock().Show();
        }

        private void addToCart(Product p)
        {
            //Product p = Utils.search(textBox1.Text.ToString());
            if (p != null)
            {
                c.AddProd(p);
                //string[] row = { p.PLU, p.desc, p.price.ToString(), "" };

                //dataGridView1.Rows.Add(row);

                label1.Text += "\n " + p.PLU;
                label3.Text += "\n " + p.desc;
                label2.Text += "\n �" + p.price;
                total = Math.Round(total += p.price, 2);
                label5.Text = "� " + total;

                textBox1.Focus();
                textBox1.Text = "";
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show($"Are you sure you want to Exit?", "Confirmation", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (res == DialogResult.OK)
            {
                Application.Exit();
            }
            if (res == DialogResult.Cancel)
            {
            }
            
        }
    }

    public class Product {
        public string PLU { get; }
        public string desc { get; }
        public float price { get; }
        public bool allowFra { get; }

        public Product(string PLU, string desc, float price, bool allowFra)
        {
            this.PLU = PLU;
            this.desc = desc;
            this.price = price;
            this.allowFra = allowFra;

        }
        public Product(string PLU, string desc, float price)
        {
            this.PLU = PLU;
            this.desc = desc;
            this.price = price;
            this.allowFra = false; // false by default

        }
        public Product()
        {

        }

    }

}