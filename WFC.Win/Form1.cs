using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using DeBroglie.Console.Export;

namespace WFC.Win
{


    public partial class Form1 : Form
    {

        //System.Windows.Forms.Button theControl;

        public Form1()
        {
            InitializeComponent();
        }

        int tileSize = 48;
        int mapSize = 15;

        private void Form1_Load(object sender, EventArgs e)
        {

            Model = new SimpleTiledModel("Summer",null, mapSize, mapSize, false,false);


            listView1.View = View.Details;

            listView1.Columns.Add("tiles", 48);
            listView1.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.HeaderSize);

            listView1.SmallImageList = new ImageList();
            listView1.SmallImageList.ImageSize = new Size(tileSize, tileSize);
            int count = 0;
            foreach (var tilename in Model.tilenames)
            {
                string path = $"samples/Summer/{tilename}.png";
                listView1.SmallImageList.Images.Add(Image.FromFile(path));
                listView1.Items.Add(tilename, count++);

            }
        }

        //ItemsProcessor Pro;
        private Thread thread2 = null;
        SimpleTiledModel Model;

        private async void button1_Click(object sender, EventArgs e)
        {
            //thread2 = new Thread(new ThreadStart(SetText));
            //thread2.Start();
            //Thread.Sleep(1000);

            button1.Enabled = false;

            await Task.Run(() =>
            {


                Random random = new Random();
                int seed = random.Next();

                bool finished = Model.Run2(seed, 0,
                    (int count) =>

                    {
                        string path = $"output_{count}.png";

                        Model.Graphics().Save(path);


                        BeginInvoke((Action)(() =>
                        {
                            //button1.Text = path;

                            pictureBox1.Image = Image.FromFile(path);
                            pictureBox1.Update();

                        }));
                    });


                if (finished)
                {
                    Console.WriteLine("DONE");

                    Model.Graphics().Save($"output.png");

                    BeginInvoke((Action)(() =>
                    {
                        //button1.Text = path;
                        pictureBox1.Image = FromImage($"output.png");
                        pictureBox1.Update();

                    }));


                    //if (model is SimpleTiledModel && xelem.Get("textOutput", false))
                    //    System.IO.File.WriteAllText($"{counter} {name} {i}.txt", (model as SimpleTiledModel).TextOutput());

                    //break;
                }
                else Console.WriteLine("CONTRADICTION");


                BeginInvoke((Action)(() =>
                {
                    button1.Enabled = true;

                }));

                

            });


            BeginInvoke((Action)(() =>
            {
                button1.Text = "Done";

            }));


        }


        void aaaa(int count)
        {
            string path = $"output_{count}.png";

            Model.Graphics().Save(path);


            BeginInvoke((Action)(() =>
            {
                //button1.Text = path;

                pictureBox1.Image = FromImage(path);
                pictureBox1.Update();

            }));
        }

        private async void SetText()
        {

            await Task.Run(() =>
            {


                Random random = new Random();
                int seed = random.Next();

                bool finished = Model.Run2(seed, 0, aaaa);

                if (finished)
                {
                    Console.WriteLine("DONE");

                    Model.Graphics().Save($"output.png");

                    BeginInvoke((Action)(() =>
                    {
                        //button1.Text = path;
                        pictureBox1.Image = FromImage($"output.png");
                        pictureBox1.Update();

                    }));


                    //if (model is SimpleTiledModel && xelem.Get("textOutput", false))
                    //    System.IO.File.WriteAllText($"{counter} {name} {i}.txt", (model as SimpleTiledModel).TextOutput());

                    //break;
                }
                else Console.WriteLine("CONTRADICTION");

            });


            BeginInvoke((Action)(() =>
            {
                button1.Text = "Done";

            }));
            




            //pictureBox1.Image = Image.FromFile(pro.Dest);

        }
        private void pictureBox2_Click(object sender, EventArgs e)
        {


            MouseEventArgs me = (MouseEventArgs)e;
            var pos = me.Location;

            pos.X -= pos.X % tileSize;
            pos.Y -= pos.Y % tileSize;

            Rectangle rSrc = new Rectangle(pos, new Size(tileSize, tileSize));
            Rectangle rDest = new Rectangle(System.Drawing.Point.Empty, new Size(pictureBox3.ClientSize.Width, pictureBox3.ClientSize.Height));

            Bitmap bmp = new Bitmap(pictureBox3.ClientSize.Width, pictureBox3.ClientSize.Height);
            using (Graphics G = Graphics.FromImage(bmp))
                G.DrawImage(pictureBox2.Image, rDest, rSrc, GraphicsUnit.Pixel);
            pictureBox3.Image = bmp;

        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
           

        }

        System.Drawing.Point mDown1;
        System.Drawing.Point mDown2;
        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            //MouseEventArgs me = (MouseEventArgs)e;
            //var coordinates = me.Location;
            mDown2 = e.Location;

            textBox1.Text = $"X : {e.Location.X} , Y : {e.Location.Y}";

            pictureBox2.Invalidate();

            //textBox1.Text = me.Location.X.ToString();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {

            mDown2.X -= mDown2.X % tileSize;
            mDown2.Y -= mDown2.Y % tileSize;

            Rectangle r = new Rectangle(mDown2.X, mDown2.Y, tileSize, tileSize);
            e.Graphics.DrawRectangle(Pens.Red, r);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            mDown1 = e.Location;


            pictureBox1.Invalidate();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            mDown1.X -= mDown1.X % tileSize;
            mDown1.Y -= mDown1.Y % tileSize;

            Rectangle r = new Rectangle(mDown1.X, mDown1.Y, tileSize, tileSize);
            e.Graphics.DrawRectangle(Pens.Red, r);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

            if (listView1.SelectedIndices.Count < 1)
                return;

            MouseEventArgs me = (MouseEventArgs)e;
            var pos = me.Location;

            pos.X -= pos.X % tileSize;
            pos.Y -= pos.Y % tileSize;

            int x = pos.X / tileSize;
            int y = pos.Y / tileSize;

            //Pro.Propagator.SeletctPattern(x + y * 20, 1);

            //Model.ChangeTile(x, y, listView1.SelectedIndices[0]);

            //Model.Run2(0, 0, aaaa);


            SimpleTiledModel model;
            model = new SimpleTiledModel("Summer", null, 5, 5, false, false);

            model.Clear(2, 2);
            model.SelectTile(2, 2, listView1.SelectedIndices[0]);

            for (int sy = 0; sy < 5; sy++)
            {
                for (int sx = 0; sx < 5; sx++)
                {
                    if( sx == 0 || sx == 4 || sy == 0 || sy == 4)
                    {
                        int i = sx + x - 2 + (sy + y - 2) * Model.FMX;
                        
                        
                        int tIndex = Model.observed[ i];
                        model.Clear(sx, sy);
                        model.SelectTile(sx, sy, tIndex);
                    }
                    else if()
                    {

                    }
                    else
                    {
                        model.Clear(sx, sy);
                    }
                }
            }

            var r = model.Run2(0, 0, (int index)=>{ 
            
            
                
            
            
            });

            Debug.WriteLine(r.ToString());



            //Pro.Export("test.png");
            model.MakeObserved();

            Model.Graphics(x-2,y-2,model.observed,5,5).Save($"output.png");

            pictureBox1.Image = FromImage($"output.png");
            pictureBox1.Update();

        }


        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        public static Image FromImage(string path)
        {
            MemoryStream ms = new MemoryStream();
            using (var i = File.OpenRead(path))
            {
                i.CopyTo(ms);
            }
            return Image.FromStream(ms);
        }


    }


}
