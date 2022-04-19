using System;
using System.IO;//用于文件存取  
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing;//提供画gdi+图形的基本功能 
using System.Drawing.Text;//提供画gdi+图形的高级功能 
using System.Drawing.Drawing2D;//提供画高级二维，矢量图形功能 
using System.Drawing.Imaging;//提供画gdi+图形的高级功能  
using System.Media;


namespace FSky
{


    public class ImageUnit : IDisposable
    {


        static readonly int maxFrame = 100;

        public int nowFrame = 0;
        public int totalFrame = 0;
        public Bitmap[] bmpFrame = new Bitmap[maxFrame];



        public bool bMakeTransparent = true;
        public RotateFlipType rotateFlipType = RotateFlipType.RotateNoneFlipNone;



        bool _bDisposed = false;


        public void Dispose()
        {
            if (_bDisposed) return;
            _bDisposed = true;

            Clear();

            GC.SuppressFinalize(true);
        }



        public void Clear()
        {

            for (int i = 0; i < bmpFrame.Length; i++)
            {
                if (bmpFrame[i] != null)
                {
                    bmpFrame[i].Dispose();
                    bmpFrame[i] = null;
                }
            }
        }




        public bool LoadFromFile(string file)
        {

            nowFrame = 0;
            totalFrame = 0;

            Clear();


            try
            {
                Image img = Image.FromFile(file, true);

                FrameDimension oDimension = new FrameDimension(img.FrameDimensionsList[0]);

                int nFrame = img.GetFrameCount(oDimension);


                for (int c = 0; c < nFrame; c++)
                {

                    img.SelectActiveFrame(oDimension, c);


                    bmpFrame[totalFrame] = new Bitmap(img.Width, img.Height);


                    Graphics g = Graphics.FromImage(bmpFrame[totalFrame]);

                    g.DrawImage(img, 0, 0);

                    g.Dispose();

                    bmpFrame[totalFrame].RotateFlip(rotateFlipType);


                    if (bMakeTransparent)
                    {
                        Color color = bmpFrame[totalFrame].GetPixel(0, 0);

                        if (color.A != 0)
                        {
                            bmpFrame[totalFrame].MakeTransparent(color);

                            bmpFrame[totalFrame].MakeTransparent();

                        }
                    }

                    totalFrame++;
                }

                img.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show("ImageUnit FromFile " + file.ToString(), e.Message );
                return false;
            }


            return true;
        }

         



        public bool FromBitmap(Bitmap[] bitmaps, int count)
        {

            Clear();

            nowFrame = 0;
            totalFrame = 0;

            for (int i = 0; i < count; i++)
            {
                bmpFrame[totalFrame] = (Bitmap)bitmaps[i].Clone();

                if (bMakeTransparent)
                {
                    Color color = bmpFrame[totalFrame].GetPixel(0, 0);

                    if (color.A != 0)
                    {
                        bmpFrame[totalFrame].MakeTransparent(color);

                        bmpFrame[totalFrame].MakeTransparent();

                    }
                }

                totalFrame++;
            }

            return true;
        }

        public bool FromBitmap(Bitmap bitmap)
        {
            return FromBitmap(new Bitmap[] { bitmap }, 1);
        }





        public Bitmap GetCurrentBMP()
        {
            return bmpFrame[nowFrame];
        }




        public void ShowUnscaled(Graphics g, int x, int y)
        {
            //   try
            {

                Bitmap bmp = bmpFrame[nowFrame];

                // if ((x - bmp.Width / 2 < 0) || (y - bmp.Height / 2 < 0)) return;

                g.DrawImageUnscaled(bmp, x - (bmp.Width>>1), y - (bmp.Height>>1) );

                nowFrame = (nowFrame + 1) % totalFrame;
            }
            //     catch (Exception e)
            {
                //         MessageBox.Show("ImageUnit ShowUnscaled " + e.ToString());
            }

        }


        public void Show(Graphics g, int x, int y)
        {
            ShowUnscaled(g, x, y);
        }



        public void Show(Graphics g, int x, int y, int width, int height)
        {
            try
            {
                lock (this)
                {
                    Bitmap bmp = bmpFrame[nowFrame];

                    if ((bmp.Width == 0) || (bmp.Height == 0)) return;

                    double wScale = width / bmp.Width;
                    double hScale = height / bmp.Height;


                    g.DrawImage(bmp, x - (int)(wScale * bmp.Width / 2), y - (int)(hScale * bmp.Height / 2), width, height);

                    nowFrame = (nowFrame + 1) % totalFrame;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ImageUnit Show " + e.ToString());
            }
        }



        public static void ShowText(Graphics g, Font font, Brush brush, string text, int x, int y)
        {
            g.DrawString(text, font, brush, new PointF(x, y));
        }



    }



    public class Render
    {

        static Dictionary<string, ImageUnit> imageDic = new Dictionary<string, ImageUnit>();



        static ImageUnit GetImageUnit(string name)
        {
            if (imageDic.ContainsKey(name)) return imageDic[name];



            ImageUnit unit = new ImageUnit();

            bool bLoad = unit.LoadFromFile(name);

            //bool bLoad = false;

            //if (System.IO.File.Exists(name + ".bmp")) bLoad = unit.LoadFromFile(name + ".bmp");

            //if (!bLoad)
            //    if (System.IO.File.Exists(name + ".jpg")) bLoad = unit.LoadFromFile(name + ".jpg");

            //if (!bLoad)
            //    if (System.IO.File.Exists(name + ".gif")) bLoad = unit.LoadFromFile(name + ".gif");

            if (bLoad)
            {
                imageDic.Add(name, unit);
                return imageDic[name];
            }
            else
                return null;



        }



        public static void Show(Graphics g,string name,int x,int y)
        {
            ImageUnit unit = GetImageUnit(name);

            if(unit!=null) unit.Show(g, x, y);

        }

        public static void Show(Bitmap bm, string name, int x, int y)
        {
            Graphics g = Graphics.FromImage(bm);

            Show(g, name, x, y);

            g.Dispose();
        }

        public static void Show(Graphics g, string name, int x, int y, int width, int height)
        {
            ImageUnit unit = GetImageUnit(name);

            if (unit != null) unit.Show(g, x, y,width,height);
        }

        public static void Show(Bitmap bm, string name, int x, int y, int width, int height)
        {
            Graphics g = Graphics.FromImage(bm);

            Show(g,name, x, y,width,height);

            g.Dispose();
        }
    }










    //-------------------audio-------------------

    public class Audio
    {

        static Dictionary<string, SoundPlayer> dic = new Dictionary<string, SoundPlayer>();

        static SoundPlayer GetSound(string file)
        {
            if (dic.ContainsKey(file)) return dic[file];

            SoundPlayer obj = new SoundPlayer(file);

            dic.Add(file, obj);

            return dic[file];
        }


        public static void Play(string file)
        {
            try
            {
                SoundPlayer sPlayer = GetSound(file);

                if (sPlayer != null) sPlayer.Play();
            }
            catch
            {
            }
        }


    }


}

