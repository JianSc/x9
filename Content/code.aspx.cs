using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace x9web.Content
{
    public partial class code : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string checkCode = CreateRandomCode(5);
            Session["code"] = checkCode;
            CreateImage(checkCode);
        }

        private string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9";
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1; Random rand = new Random();
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(10);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }

            return randomCode;
        }

        private void CreateImage(string checkCode)
        {
            Bitmap Img = null;
            Graphics g = null;
            MemoryStream ms = null;
            int gheight = checkCode.Length * 12;
            Img = new Bitmap(75, 28);
            g = Graphics.FromImage(Img);
            Random random = new Random();
            g.Clear(Color.Orange);
            for (int i = 0; i < 25; i++)
            {
                int x1 = random.Next(Img.Width);
                int x2 = random.Next(Img.Width);
                int y1 = random.Next(Img.Height);
                int y2 = random.Next(Img.Height);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }

            for (int i = 0; i < 100; i++)
            {
                int x = random.Next(Img.Width);
                int y = random.Next(Img.Height);
                Img.SetPixel(x, y, Color.FromArgb(random.Next()));
            }

            Font f = new Font("Arial Black ", 12, System.Drawing.FontStyle.Bold);
            SolidBrush s = new SolidBrush(Color.MediumBlue);
            g.DrawString(checkCode, f, s, 3, 3);
            ms = new MemoryStream();
            Img.Save(ms, ImageFormat.Jpeg);
            Response.ClearContent();
            Response.ContentType = "image/Jpeg ";
            Response.BinaryWrite(ms.ToArray());
            g.Dispose();
            Img.Dispose();
            Response.End();

            //int iwidth = (int)(checkCode.Length * 11.5);
            //System.Drawing.Bitmap image = new System.Drawing.Bitmap(60, 28);
            //Graphics g = Graphics.FromImage(image);
            //Font f = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold);
            //Brush b = new System.Drawing.SolidBrush(Color.Purple);
            ////g.FillRectangle(new System.Drawing.SolidBrush(Color.Blue),0,0,image.Width, image.Height);
            //g.Clear(Color.MistyRose);
            //g.DrawString(checkCode, f, b, 3, 3);
            //Pen blackPen = new Pen(Color.Black, 0);
            //Random rand = new Random();
            //for (int i = 0; i < 5; i++)
            //{
            //    int y = rand.Next(image.Height);
            //    g.DrawLine(blackPen, 0, y, image.Width, y);
            //}
            //System.IO.MemoryStream ms = new System.IO.MemoryStream();
            //image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            //Response.ClearContent();
            //Response.ContentType = "image/Jpeg";
            //Response.BinaryWrite(ms.ToArray());
            //g.Dispose();
            //image.Dispose();
        }
    }

}