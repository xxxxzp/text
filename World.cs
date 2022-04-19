using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;//提供画gdi+图形的基本功能 
using System.Drawing.Text;//提供画gdi+图形的高级功能 
using System.Drawing.Drawing2D;//提供画高级二维，矢量图形功能 
using System.Drawing.Imaging;//提供画gdi+图形的高级功能  
using System.Media;





namespace myGame
{


    public class Animal
    {
        public int x;       //坐标
        public int y;
        public int directionX;      //坦克方向
        public int directionY;
        public int velocity;        //速度
        public string image;

        public bool bExist = true;

        //public bool bGameOver = false;
         
        public int blood = 100;

        public int type = 0;  // 1,2,3,4,5,6,7....

        public void InitializeByType(int _type)
        {
            type = _type;

            if (type == 0) image = "gif\\tk_s.gif";         //己方
            if (type == 1) image = "gif\\missile1.gif";     //己方子弹

            if (type == 13) image = "gif\\missile2.gif";     //敌方子弹

            //==========敌方=========
            if (type == 2) image = "gif\\tk02_x.gif";
            if (type == 3) image = "gif\\tk03_x.gif";
            if (type == 4) image = "gif\\tk04_z.gif";
            if (type == 5) image = "gif\\tk05_x.gif";
            if (type == 6) image = "gif\\tk06_x.gif";
            if (type == 7) image = "gif\\tk07.gif";

            if (type == 9) image = "gif\\x2.gif";       //铁墙
            if (type == 10) image = "gif\\x3.gif";      //砖墙
            if (type == 11) image = "gif\\x1.gif";      //草
            if (type == 12) image = "gif\\wan.gif";     //home
        }


        Random rand = new Random();

        public void  NextRound(World world=null )     //成员变量和世界交互
        {
            // update x,y  
            x += directionX * velocity;
            y += directionY * velocity;

            if ((x <= 5) || (x > 635) || (y < 5) || (y > 400))
            {
                //x --; y=0;
                directionX = -directionX;
                directionY = -directionY;
                if (type == 1 || type == 13)
                {
                    bExist = false;
                    return;
                }
            }
        }

        //---------------------------------------------------
        public void Show(Bitmap bmp)        //图片自己显示
        {//显示结果
            FSky.Render.Show(bmp, image, x, y);
            //可在此处控制图片
        }
       
        public void Meet(Animal oAnimal)
        {
            //============己方子弹互销===================
            if ((type == 2||type==3||type==4||type==5||type==6||type==7||type==8||type==10||type==12||type==13) && (oAnimal.type == 1))  //碰到1己方子弹时消失(死亡)
            {
                bExist = false;
            }
            if ((type == 1 ) && (oAnimal.type == 2||oAnimal.type==3||oAnimal.type==4||oAnimal.type==5||oAnimal.type==6||oAnimal.type==7||oAnimal.type==8||oAnimal.type==9||oAnimal.type==10||oAnimal.type==12||oAnimal.type==13))     //碰到1的图片时消失(死亡)
            {
                bExist = false;
            }
            //========敌方子弹13碰到9铁墙时，子弹消失===================
            if ((type == 13) && (oAnimal.type == 9))
            {
                bExist = false;
            }
            //============碰到坦克4或7时自己死亡=======================
            if ((type == 0) && (oAnimal.type == 4||oAnimal.type==7))
            {
                FSky.Audio.Play("audio\\Explode.wav");  //插入声音
                bExist = false;
            }
            //==============================
            if ((type == 11) && (oAnimal.type == 1))  //碰到11草的图片时不消失(不死亡)
            {
                bExist = true;
            }
            //==============互销=============================
            if ((type == 0||type==12||type==10) && (oAnimal.type == 13)) //碰到13敌方子弹时死亡
            {
                FSky.Audio.Play("audio\\Explode.wav");  //插入声音
                bExist = false;
            }
            if ((type == 13) && (oAnimal.type == 0||oAnimal.type==12||oAnimal.type==10))//碰到13时死亡
            {
                bExist = false;
            }
            //================================
            /*
            if (oAnimal.type == 8)
            {
                blood -= 20;
                if (blood <= 0) bExist = false;
            }*/
            if (oAnimal.type == 9 || oAnimal.type == 10||oAnimal.type==12)
            {
                directionX = -directionX;
                directionY = -directionY;
            }
        }
    }
 
    //--------------------------------------------------------
    public class World     //World类
    {
        public Bitmap bmp = new Bitmap(650, 415);

        public bool bGameOver = false;
      
        Animal animal = new Animal();
        public List<Animal> animalList = new List<Animal>(); //列表(可增加、删除)

        public World()      //构造函数World
        {
            int[] arrx = { 250, 580, 77, 100, 165, 245, 292, 339, 386, 300, 347, 256, 256, 303, 350, 397, 397, 480, 530, 70, 450, 30, 100, 570 };
            int[] arry = { 70, 70, 180, 391, 300, 250, 250, 250, 250, 203, 203, 391, 343, 343, 343, 343, 391, 250, 391, 70, 70, 180, 300, 250 };
            
            Random rand = new Random(); //系统自带的产生随机数
            //---------------己方------------------------------
            for (int i = 0; i < 1; i++)     //0、控制生成坦克的数量
            {
                animal.InitializeByType(0);
                animal.x = 200;      //控制坦克生成的位置
                animal.y = 390;

                animal.velocity = rand.Next(2) + 2;     //随机产生的速度

                animalList.Add(animal);    //将animal加入到animalList链表中
            }
            for (int i = 0; i < 1; i++)     //1、生成home
            {
                Animal animal = new Animal();
                animal.InitializeByType(12);
                animal.x = 327;
                animal.y = 389;

                animalList.Add(animal);    //将animal加入到animalList链表中
            }
            //------------敌方(第2个位置)------------------------------
            for (int i = 0; i < 1; i++)     //2、控制生成坦克的数量
            {
                Animal animal = new Animal();
                animal.InitializeByType(2);
                animal.x = 120;      //控制坦克生成的位置
                animal.y = 110;

                animal.directionX = 0;    //方向
                animal.directionY = 2;
                animal.velocity = 2;     //速度
                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            for (int i = 0; i < 1; i++)     //3、控制生成坦克的数量
            {
                Animal animal = new Animal();
                animal.InitializeByType(3);
                animal.x = 180;      //控制坦克生成的位置
                animal.y = 110;

                animal.directionX = 0;   //方向
                animal.directionY = 1;
                animal.velocity = 1;     //速度
                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            for (int i = 0; i < 1; i++)     //4、控制生成坦克的数量
            {
                Animal animal = new Animal();
                animal.InitializeByType(4);
                animal.x = 530;      //控制坦克生成的位置
                animal.y = 120;

                animal.directionX = -3;   //方向
                animal.directionY = 0;
                animal.velocity = 3;     //速度
                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            for (int i = 0; i < 1; i++)     //5、控制生成坦克的数量
            {
                Animal animal = new Animal();
                animal.InitializeByType(5);
                animal.x = 520;      //控制坦克生成的位置
                animal.y = 160;

                animal.directionX = 0;   //方向
                animal.directionY = 1;
                animal.velocity = 1;        //速度
                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            for (int i = 0; i < 1; i++)     //6、控制生成坦克的数量
            {
                Animal animal = new Animal();
                animal.InitializeByType(6);
                animal.x = 360;      //控制坦克生成的位置
                animal.y = 100;

                animal.directionX = 0;   //方向
                animal.directionY = 2;
                animal.velocity = 3;        //速度
                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            for (int i = 0; i < 1; i++)     //7、控制生成坦克的数量
            {
                Animal animal = new Animal();
                animal.InitializeByType(2);
                animal.x = 300;      //控制坦克生成的位置
                animal.y = 50;

                animal.directionX = 0;   //方向
                animal.directionY = 2;
                animal.velocity = 3;        //速度
                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            for (int i = 0; i < 1; i++)     //8、
            {
                Animal animal = new Animal();
                animal.InitializeByType(2);
                animal.x = 180;      //控制坦克生成的位置
                animal.y = 200;

                animal.directionX = 0;    //方向
                animal.directionY = 2;
                animal.velocity = 2;     //速度
                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            for (int i = 0; i < 1; i++)     //9、
            {
                Animal animal = new Animal();
                animal.InitializeByType(2);
                animal.x = 350;      //控制坦克生成的位置
                animal.y = 50;

                animal.directionX = 0;   //方向
                animal.directionY = 2;
                animal.velocity = 3;        //速度
                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            for (int i = 0; i < 1; i++)     //10、
            {
                Animal animal = new Animal();
                animal.InitializeByType(2);
                animal.x = 630;      //控制坦克生成的位置
                animal.y = 100;

                animal.directionX = 0;   //方向
                animal.directionY = 2;
                animal.velocity = 3;        //速度
                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            for (int i = 0; i < 1; i++)     //11、
            {
                Animal animal = new Animal();
                animal.InitializeByType(7);
                animal.x = 60;      //控制坦克生成的位置
                animal.y = 140;

                animal.directionX = 2;   //方向
                animal.directionY = 0;
                animal.velocity = 2;        //速度
                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            #region   墙壁    //隐藏代码
            //-----------------墙壁-------------------------------
            for (int i = 0; i < 19; i++)     //控制生成墙壁的数量
            {
                Animal animal = new Animal();
                animal.InitializeByType(10);
                animal.x = arrx[i];
                animal.y = arry[i];

                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            for (int i = 19; i < 24; i++)     //控制生成墙壁的数量
            {
                Animal animal = new Animal();
                animal.InitializeByType(9);
                animal.x = arrx[i];
                animal.y = arry[i];

                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            //====================================
            for (int i = 0; i < 1; i++)     //控制生成草的数量
            {
                Animal animal = new Animal();
                animal.InitializeByType(11);
                animal.x = 70;
                animal.y = 250;

                animalList.Add(animal);    //将animal加入到animalList队列中
            }
            //-------------------墙壁-------------
            #endregion      可以隐藏代码
        }

        void Clear()        //清除痕迹
        {
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Black);
        }
       
        public Bitmap Show()    //显示
        {
            for (int i = 0; i < animalList.Count; i++)
            {
                animalList[i].Show(bmp);    //在bmp位图上显示
            }
             return bmp;
        }

        public void NextRound()
        {   //整个世界到达下一个时刻
            //bool gameover=true;
            for (int i = 0; i < animalList.Count; i++)
            {
                animalList[i].NextRound(this);
                Clear();
            }
             Animal animal = new Animal();
            //------------此处判断碰撞------------------------
            for (int i = 0; i < animalList.Count; i++)
            {
                for (int j = i+1; j < animalList.Count; j++)
                {
                    if (Math.Abs(animalList[i].x - animalList[j].x) + Math.Abs(animalList[i].y - animalList[j].y) < 35)
                    {
                        animalList[i].Meet(animalList[j]);
                        animalList[j].Meet(animalList[i]);

                        //FSky.Render.Show(bmp, "gif\\explode1.gif", animalList[i].x, animalList[j].y);  //显示图片
                    }
                }
            }

            for (int i = animalList.Count - 1; i > 0; i--)
            {//移除
                if (!animalList[i].bExist)
                {
                    animalList.RemoveAt(i);
                    FSky.Audio.Play("audio\\Explode.wav");  //插入声音
                    //FSky.Render.Show(bmp, "gif\\explode1.gif", animalList[i].x, animalList[i].y);  //显示图片
                }
                if (!animalList[1].bExist)      //移除home时死亡
                {
                    bGameOver = true;
                    FSky.Render.Show(bmp, "gif\\explode1.gif", 327, 389);   //插入爆炸图片
                }
                if (!animalList[0].bExist)      //己死亡
                {
                    bGameOver = true;
                    FSky.Render.Show(bmp, "gif\\explode1.gif", animalList[0].x, animalList[0].y);
                }
            }
                
            //============敌方子弹===================
            Random rand = new Random(); //系统自带的随机数类
            int x=rand.Next(1, 100);
            if (x == 2)
            {
                animal.x = animalList[2].x;                     //位置
                animal.y = animalList[2].y;
                animal.directionX = animalList[2].directionX;   //方向
                animal.directionY = animalList[2].directionY;

                animal.InitializeByType(13);
                animal.velocity = 4;   //速度
                animalList.Add(animal);
            }
            if (x == 21)
            {
                animal.x = animalList[3].x;                     //位置
                animal.y = animalList[3].y;
                animal.directionX = animalList[3].directionX;   //方向
                animal.directionY = animalList[3].directionY;
                
                animal.InitializeByType(13);
                animal.velocity = 4;   //速度
                animalList.Add(animal);
            }
            if (x == 13)
            {
                animal.x = animalList[4].x;                     //位置
                animal.y = animalList[4].y;
                animal.directionX = animalList[4].directionX;   //方向
                animal.directionY = animalList[4].directionY;
                
                animal.InitializeByType(13);
                animal.velocity = 4;   //速度
                animalList.Add(animal);
            }
            if (x == 35)
            {
                animal.x = animalList[5].x;                     //位置
                animal.y = animalList[5].y;
                animal.directionX = animalList[5].directionX;   //方向
                animal.directionY = animalList[5].directionY;

                animal.InitializeByType(13);
                animal.velocity = 5;   //速度
                animalList.Add(animal);
            }
            //=================================================

            //===========敌方转向======================
            int y = rand.Next(1, 100);
            if (y == 3)
            {
                animalList[2].directionX = 2;   //方向
                animalList[2].directionY = 0;
                animalList[2].image = "gif\\tk02_y.gif";
            }
            if (y == 5)
            {
                animalList[2].directionX = -2;   //方向
                animalList[2].directionY = 0;
                animalList[2].image = "gif\\tk02_z.gif";
            }
            if (y == 7)
            {
                animalList[2].directionX = 0;   //方向
                animalList[2].directionY = 2;
                animalList[2].image = "gif\\tk02_x.gif";
            }
            if (y == 9)
            {
                animalList[2].directionX = 0;   //方向
                animalList[2].directionY = -1;
                animalList[2].image = "gif\\tk02_s.gif";
            }
            //--------------------------------
            if (y == 33)
            {
                animalList[3].directionX = 1;   //方向
                animalList[3].directionY = 0;
                animalList[3].image = "gif\\tk03_y.gif";
            }
            if (y == 55)
            {
                animalList[3].directionX = -1;   //方向
                animalList[3].directionY = 0;
                animalList[3].image = "gif\\tk03_z.gif";
            }
            if (y == 77)
            {
                animalList[3].directionX = 0;   //方向
                animalList[3].directionY = 1;
                animalList[3].image = "gif\\tk03_x.gif";
            }
            if (y == 99)
            {
                animalList[3].directionX = 0;   //方向
                animalList[3].directionY = -1;
                animalList[3].image = "gif\\tk03_s.gif";
            }
            //----------------------------------
            if (y == 2)
            {
                animalList[4].directionX = 3;   //方向
                animalList[4].directionY = 0;
                animalList[4].image = "gif\\tk04_y.gif";
            }
            if (y == 4)
            {
                animalList[4].directionX = -3;   //方向
                animalList[4].directionY = 0;
                animalList[4].image = "gif\\tk04_z.gif";
            }
            if (y == 6)
            {
                animalList[4].directionX = 0;   //方向
                animalList[4].directionY = 3;
                animalList[4].image = "gif\\tk04_x.gif";
            }
            if (y == 8)
            {
                animalList[4].directionX = 0;   //方向
                animalList[4].directionY = -3;
                animalList[4].image = "gif\\tk04_s.gif";
            }
            //---------------------------------
            if (y == 43)
            {
                animalList[5].directionX = 1;   //方向
                animalList[5].directionY = 0;
                animalList[5].image = "gif\\tk05_y.gif";
            }
            if (y == 57)
            {
                animalList[5].directionX = -1;   //方向
                animalList[5].directionY = 0;
                animalList[5].image = "gif\\tk05_z.gif";
            }
            if (y == 67)
            {
                animalList[5].directionX = 0;   //方向
                animalList[5].directionY = 1;
                animalList[5].image = "gif\\tk05_x.gif";
            }
            if (y == 46)
            {
                animalList[5].directionX = 0;   //方向
                animalList[5].directionY = -1;
                animalList[5].image = "gif\\tk05_s.gif";

            }
            //-------------------------
            if (y == 17)
            {
                animalList[6].directionX = 3;   //方向
                animalList[6].directionY = 0;
                animalList[6].image = "gif\\tk06_y.gif";
            }
            if (y == 29)
            {
                animalList[6].directionX = -3;   //方向
                animalList[6].directionY = 0;
                animalList[6].image = "gif\\tk06_z.gif";
            }
            if (y == 65)
            {
                animalList[6].directionX = 0;   //方向
                animalList[6].directionY = 3;
                animalList[6].image = "gif\\tk06_x.gif";
            }
            if (y == 81)
            {
                animalList[6].directionX = 0;   //方向
                animalList[6].directionY = -3;
                animalList[6].image = "gif\\tk06_s.gif";
            }
            //------------------------
            if (y == 38)
            {
                animalList[7].directionX = 2;   //方向
                animalList[7].directionY = 0;
                animalList[7].image = "gif\\tk02_y.gif";
            }
            if (y == 43)
            {
                animalList[7].directionX = -2;   //方向
                animalList[7].directionY = 0;
                animalList[7].image = "gif\\tk02_z.gif";
            }
            if (y == 52)
            {
                animalList[7].directionX = 0;   //方向
                animalList[7].directionY = 2;
                animalList[7].image = "gif\\tk02_x.gif";
            }
            if (y == 65)
            {
                animalList[7].directionX = 0;   //方向
                animalList[7].directionY = -1;
                animalList[7].image = "gif\\tk02_s.gif";
            }
           //==============================================
              
        }


        //------------------------------
        public void KeyPress(char key)
        {
            FSky.Audio.Play("audio\\score.wav");       //插入音乐(每按一次键盘播放一次音乐)
            if (key == 'w')
            {
                animalList[0].image = "gif\\tk_s.gif";
                animalList[0].directionX = 0;
                animalList[0].directionY = -3;
            }
            if (key == 's')
            {
                animalList[0].image = "gif\\tk_x.gif";
                animalList[0].directionX = 0;
                animalList[0].directionY = 3;
            }
            if (key == 'a') //向左
            {
                animalList[0].image = "gif\\tk_z.gif";
                animalList[0].directionX = -3;
                animalList[0].directionY = 0;
            }
            if (key == 'd')
            {
                animalList[0].image = "gif\\tk_y.gif";
                animalList[0].directionX = 3;
                animalList[0].directionY = 0;
            }

            if (key == ' ')                                     //子弹
            {
                Animal animal = new Animal();
                animal.x = animalList[0].x;                     //位置
                animal.y = animalList[0].y;
                animal.directionX = animalList[0].directionX;   //方向
                animal.directionY = animalList[0].directionY;

                //animal.image = "gif\\missile1.gif";
                animal.InitializeByType(1);
                animal.velocity = animalList[0].velocity * 2+1;   //速度
                animalList.Add(animal);

            }
        }

    }
}
