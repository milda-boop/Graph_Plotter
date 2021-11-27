using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;
using System.Drawing;
using System.Drawing.Imaging;

namespace Graph_Plotter
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("GRAPH PLOTTER"); 
            Console.WriteLine("Menu:");
            Console.WriteLine("1  Input data");
            Console.WriteLine("2  Exit");
            if(int.Parse(Console.ReadLine())==1)
            {
                Console.WriteLine("Type of Graph?");
                Console.WriteLine("1  Quadratic");
                Console.WriteLine("2  Linear");
                int answer = int.Parse(Console.ReadLine());

                Graph graph = InitGraph(answer);
                InputData(graph);
                graph.BestFit();
                graph.Accuracy();
                graph.InputCurveData();
                Console.WriteLine("Title of Graph?");
                string title = Console.ReadLine();
                Console.WriteLine("Title of x-axis?");
                string xtitle = Console.ReadLine();
                Console.WriteLine("Title of y-axis?");
                string ytitle = Console.ReadLine();

                graph.PlotGraph(title,xtitle,ytitle);
                Console.ReadLine();
            }
        }  // Main

        private static Graph InitGraph(int type)
        {
            if (type == 1)
            {
                Quadratic graph = new Quadratic();
                return graph;
                
            }
            else
            {
                Linear graph = new Linear();
                return graph;
                
            }
        }

        const string graphpoints = "graphpoints.txt";
        private static void InputData(Graph graph)
        {
            
            StreamReader PointsFile = new StreamReader(graphpoints); //reads the text file.
            int LineCount = File.ReadLines(graphpoints).Count();
            
            Point[] data = new Point[LineCount];
            for (int i = 0; i < LineCount; i++)
            {
                data[i]  = new Point(PointsFile.ReadLine()); //reads new line after each row.
                graph.AddPoint(data[i]);
            }
            
            PointsFile.Close();
            

            Console.ReadLine();
        }
    }
    public class Point
    {
        private double x;
        private double y;

        public Point(string point)
        {
            string[] splitpoint = point.Split(',');
            this.x = double.Parse(splitpoint[0]);
            this.y = double.Parse(splitpoint[1]);
        }
        public Point(int x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public double GetX()
        {
            return x;
        }

        public double GetY()
        {
            return y;
        }
    }
    abstract public class Graph
    {
        protected List<Point> points = new List<Point>();
        protected List<Point> bestfit = new List<Point>();
        private double xmax = 0;
        private double ymax = 0;
        private double y_max = 0;
        protected string equation;

        abstract public void BestFit();
        abstract public void InputCurveData();
        abstract public void Accuracy();
        
        public Graph()
        {

        }
        

        public void AddPoint(Point point)
        {
            points.Add(point);
            if(Math.Abs(point.GetX()) > xmax)
            {
                xmax = Math.Abs(point.GetX());
            }
            if(Math.Abs(point.GetY()) > ymax)
            {
                ymax = Math.Abs(point.GetY());
            }
        }
        public void AddbfPoint(Point point)
        {
            bestfit.Add(point);
           
            if (Math.Abs(point.GetY()) > y_max)
            {
                y_max = Math.Abs(point.GetY());
            }
        }

        
        public void PlotGraph(string title, string xtitle, string ytitle)
        {
            
            double xmultiplier = 200 / xmax;
            double ymultiplier = 200 / ymax;
            double y_multiplier = 200 / y_max;
            Pen blackPen = new Pen(Color.Black, (float)0.5);
            Pen greenPen = new Pen(Color.Green, 5);
            PointF point1 = new PointF(100.0F, 300.0F);
            PointF point2 = new PointF(500.0F, 300.0F);
            PointF point3 = new PointF(300.0F, 100.0F);
            PointF point4 = new PointF(300.0F, 500.0F);
            RectangleF rectf = new RectangleF(0, 30, 600, 50);
            StringFormat sf= new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            using (Bitmap b = new Bitmap(600, 600))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.Gray);
                    g.DrawLine(blackPen, point1, point2);
                    g.DrawLine(blackPen, point3, point4);
                    g.DrawString(title, new Font("Tahoma", 16), Brushes.Black, rectf,sf);
                    
                    
                    foreach(Point i in points)
                    {
                        float x = (float)(i.GetX()*xmultiplier);
                        float y = (float)(i.GetY() * ymultiplier);
                        g.DrawEllipse(greenPen, (300 + x) - 2, (300 - y) - 2, 4, 4);
                        //Console.WriteLine(x);
                        //Console.WriteLine(y);
                    }
                    foreach(Point i in bestfit)
                    {
                        float x = (float)(i.GetX());
                        float y = (float)(i.GetY()*y_multiplier);
                        
                        g.DrawEllipse(blackPen, (300 + x) - 2, (300 - y) - 2, 4, 4);
                        //Console.WriteLine(x);
                        //Console.WriteLine(y);

                    }

                }
                b.Save(@"C:\Users\milda\graph.png", ImageFormat.Png);
            }
        }
    }
    public class Quadratic : Graph
    {
       double a = 0;
       double b = 0;
       double c = 0;
       double r = 0;
        
        public override void BestFit()
        {
            int n = points.Count();
            double sumx = 0;
            double sumy = 0;
            double sumx2 = 0;
            double sumx3 = 0;
            double sumx4 = 0;
            double sumxy = 0;
            double sumx2y = 0;
            foreach(Point i in points)
            {
               sumx = sumx + i.GetX();
               sumy = sumy + i.GetY();
               sumx2 = sumx2 + Math.Pow(i.GetX(), 2);
               sumx3 = sumx3 + Math.Pow(i.GetX(), 3);
               sumx4 = sumx4 + Math.Pow(i.GetX(), 4);
               sumxy = sumxy + i.GetX() * i.GetY();
               sumx2y = sumx2y + Math.Pow(i.GetX(), 2) * i.GetY();
            }
            //Console.WriteLine(sumx2y);
            double sumxx = (sumx2) - ((Math.Pow(sumx, 2)) / n);
            sumxy = (sumxy) - ((sumx*sumy) / n);
            double sumxx2 = (sumx3) - ((sumx2*sumx) / n);
            sumx2y = (sumx2y) - ((sumx2 * sumy) / n);
            double sumx2x2 = (sumx4) - ((Math.Pow(sumx2,2)) / n);

            a = ((sumx2y*sumxx)-(sumxy*sumxx2))/((sumxx*sumx2x2)-(Math.Pow(sumxx2,2)));
            b = ((sumxy * sumx2x2) - (sumx2y * sumxx2)) / ((sumxx * sumx2x2) - (Math.Pow(sumxx2, 2))); 
            c = (sumy/n)-(b*(sumx/n))-(a*(sumx2/n));
            Console.WriteLine(a);
            Console.WriteLine(b);
            Console.WriteLine(c);
        }

        public override void Accuracy()
        {
            double SSE = 0;
            double SST = 0;
            double ysum = 0;
            foreach(Point i in points)
            {
                ysum = ysum + i.GetY();
            }
            double ymean = ysum / points.Count();
            foreach(Point i in points)
            {
                SSE = SSE + Math.Pow((i.GetY() - a * Math.Pow(i.GetX(), 2) - b * (i.GetX()) - c), 2);
                SST = SST + Math.Pow((i.GetY() - ymean), 2);
            }
            r = 1 - (SSE / SST);
            Console.WriteLine("Accuracy: {0}", r);
        }

        public override void InputCurveData()
        {
            for(int i = -200;i<=200;i++)
            {
                double j = (a*Math.Pow(i,2)) + (b*i) + c ;
                Point point = new Point(i,j);
                AddbfPoint(point);
                
            }
            
        }
    }

    public class Linear : Graph
    {
        
        public override void BestFit()
        {

        }

        public override void InputCurveData()
        {

        }
        public override void Accuracy()
        {
            
        }
    }
}


