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
            int userinput = 0;
            while (userinput != 5)
            {
                userinput = Menu();
                switch (userinput)
                {
                    case 1:
                        CreateGraph();
                        break;
                    case 2:
                        ViewGraphs();
                        break;
                    case 3:
                        CreateTextfile();
                        break;
                    case 4:
                        ViewTextfile();
                        break;
                    default:
                        Console.WriteLine("Enter a valid option (1-5)");
                        break;
                }

                while (userinput != 5 && userinput != 6)
                {
                    Console.WriteLine("Enter 5 to exit or 6 to return to main menu:");
                    try
                    {
                        userinput = int.Parse(Console.ReadLine());
                    }
                    catch
                    {

                    }
                }
            }

        }

        private static void ViewTextfile()
        {
            Console.Clear();

        }

        private static void CreateTextfile()
        {
            Console.Clear();
            Console.WriteLine("How many points would you like to enter?");
            int userinput = int.Parse(Console.ReadLine());
            for (int i = 0; i < userinput; i++)
            {
                Console.WriteLine("Enter the x coordinate of point {0}", i + 1);
                Console.WriteLine("Enter the y coordinate of point {0}", i + 1);
            }
            Console.WriteLine("Enter name of textfile:");
            string name = Console.ReadLine();

        }

        private static void ViewGraphs()
        {
            Console.Clear();
        }

        private static void CreateGraph()
        {
            Console.Clear();
            int userinput = 0;
            while (userinput != 1 && userinput != 2)
            {
                Console.WriteLine("Type of Graph?");
                Console.WriteLine();
                Console.WriteLine("1  Quadratic");
                Console.WriteLine("2  Linear");
                try
                {
                    userinput = int.Parse(Console.ReadLine());
                }
                catch
                {

                }
            }

            Console.Clear();
            Console.WriteLine("Calculating graph....");
            Graph graph = InitGraph(userinput);
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
            Console.Clear();
            Console.WriteLine("Plotting graph....");
            graph.PlotGraph(title, xtitle, ytitle);
            Console.WriteLine("Graph plotted");
        }

        private static int Menu()
        {
            Console.Clear();
            Console.WriteLine("   _____ _____            _____  _    _   _____  _      ____ _______ _______ ______ _____ ");
            Console.WriteLine("  / ____|  __ \\     /\\   |  __ \\| |  | | |  __ \\| |    / __ \\__   __|__   __|  ____|  __ \\ ");
            Console.WriteLine(" | |  __| |__) |   /  \\  | |__) | |__| | | |__) | |   | |  | | | |     | |  | |__  | |__) |");
            Console.WriteLine(" | | |_ |  _  /   / /\\ \\ |  ___/|  __  | |  ___/| |   | |  | | | |     | |  |  __| |  _  / ");
            Console.WriteLine(" | |__| | | \\ \\  / ____ \\| |    | |  | | | |    | |___| |__| | | |     | |  | |____| | \\ \\");
            Console.WriteLine("  \\_____|_|  \\_\\/_/    \\_\\_|    |_|  |_| |_|    |______\\____/  |_|     |_|  |______|_|  \\_\\");
            Console.WriteLine();
            Console.WriteLine("MENU:");
            Console.WriteLine();
            Console.WriteLine("1  Create a graph");
            Console.WriteLine("2  View saved graphs");
            Console.WriteLine("3  Create a textfile");
            Console.WriteLine("4  View saved textfiles");
            Console.WriteLine("5  Exit");
            return int.Parse(Console.ReadLine());
        }

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
                data[i] = new Point(PointsFile.ReadLine()); //reads new line after each row.
                graph.AddPoint(data[i]);
            }

            PointsFile.Close();

        }
    }
    public class Point
    {
        private double x;
        private double y;

        public Point(string point)
        {
            string[] splitpoint = point.Split(',');
            this.x = double.Parse(splitpoint[0]) * Math.Pow(10, -12);
            this.y = double.Parse(splitpoint[1]) * Math.Pow(10, -12);
        }
        public Point(double x, double y)
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
        protected double xmax = 0;
        private double ymax = 0;
        protected string equation;

        abstract public void BestFit();
        abstract public void InputCurveData();
        abstract public void Accuracy();

        public void AddPoint(Point point)
        {
            points.Add(point);
            if (Math.Abs(point.GetX()) > xmax)
            {
                xmax = Math.Abs(point.GetX());
            }
            if (Math.Abs(point.GetY()) > ymax)
            {
                ymax = Math.Abs(point.GetY());
            }
        }
        public void AddbfPoint(Point point)
        {
            bestfit.Add(point);
        }


        public void PlotGraph(string title, string xtitle, string ytitle)
        {

            double xmultiplier = 200 / xmax;
            double ymultiplier = 200 / ymax;

            Pen blackPen = new Pen(Color.Black, (float)0.5);
            Pen greenPen = new Pen(Color.Green, 5);
            PointF point1 = new PointF(100.0F, 300.0F);
            PointF point2 = new PointF(500.0F, 300.0F);
            PointF point3 = new PointF(300.0F, 100.0F);
            PointF point4 = new PointF(300.0F, 500.0F);
            RectangleF rectf = new RectangleF(0, 30, 600, 50);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            using (Bitmap b = new Bitmap(600, 600))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.Gray);
                    g.DrawLine(blackPen, point1, point2);
                    g.DrawLine(blackPen, point3, point4);
                    g.DrawString(title, new Font("Tahoma", 16), Brushes.Black, rectf, sf);

                    foreach (Point i in points)
                    {
                        float x = (float)(i.GetX() * xmultiplier);
                        float y = (float)(i.GetY() * ymultiplier);
                        g.DrawEllipse(greenPen, (300 + x) - 2, (300 - y) - 2, 4, 4);
                    }
                    foreach (Point i in bestfit)
                    {
                        float x = (float)(i.GetX() * xmultiplier);
                        float y = (float)(i.GetY() * ymultiplier);
                        g.DrawEllipse(blackPen, (300 + x) - 2, (300 - y) - 2, 4, 4);
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

            foreach (Point i in points)
            {
                sumx = sumx + i.GetX();
                sumy = sumy + i.GetY();
                sumx2 = sumx2 + Math.Pow(i.GetX(), 2);
                sumx3 = sumx3 + Math.Pow(i.GetX(), 3);
                sumx4 = sumx4 + Math.Pow(i.GetX(), 4);
                sumxy = sumxy + i.GetX() * i.GetY();
                sumx2y = sumx2y + Math.Pow(i.GetX(), 2) * i.GetY();
            }

            double sumxx = (sumx2) - ((Math.Pow(sumx, 2)) / n);
            sumxy = (sumxy) - ((sumx * sumy) / n);
            double sumxx2 = (sumx3) - ((sumx2 * sumx) / n);
            sumx2y = (sumx2y) - ((sumx2 * sumy) / n);
            double sumx2x2 = (sumx4) - ((Math.Pow(sumx2, 2)) / n);

            a = ((sumx2y * sumxx) - (sumxy * sumxx2)) / ((sumxx * sumx2x2) - (Math.Pow(sumxx2, 2)));
            b = ((sumxy * sumx2x2) - (sumx2y * sumxx2)) / ((sumxx * sumx2x2) - (Math.Pow(sumxx2, 2)));
            c = (sumy / n) - (b * (sumx / n)) - (a * (sumx2 / n));
        }

        public override void Accuracy()
        {
            double SSE = 0;
            double SST = 0;
            double ysum = 0;

            foreach (Point i in points)
            {
                ysum = ysum + i.GetY();
            }

            double ymean = ysum / points.Count();
            foreach (Point i in points)
            {
                SSE = SSE + Math.Pow((i.GetY() - a * Math.Pow(i.GetX(), 2) - b * (i.GetX()) - c), 2);
                SST = SST + Math.Pow((i.GetY() - ymean), 2);
            }
            r = 1 - (SSE / SST);
            Console.WriteLine("Accuracy: {0}", r);
        }

        public override void InputCurveData()
        {
            for (int i = -200; i <= 200; i++)
            {
                double x = i * (xmax / 200);
                double j = (a * Math.Pow(x, 2)) + (b * x) + c;
                Point point = new Point(x, j);
                AddbfPoint(point);

            }

        }
    }

    public class Linear : Graph
    {
        double a = 0;
        double b = 0;
        double r = 0;
        double sumxy = 0;
        double sumx = 0;
        double sumy = 0;
        double sumx2 = 0;
        int n = 0;
        public override void BestFit()
        {
            n = points.Count();

            foreach (Point i in points)
            {
                sumx = sumx + i.GetX();
                sumy = sumy + i.GetY();
                sumx2 = sumx2 + Math.Pow(i.GetX(), 2);
                sumxy = sumxy + (i.GetX() * i.GetY());
            }

            a = ((n * sumxy) - (sumx * sumy)) / ((n * sumx2) - Math.Pow(sumx, 2));
            b = (sumy / n) - (a * (sumx / n));

            //Console.WriteLine("BestFit a value: {0}",a);
            //Console.WriteLine("BestFit b value: {0}",b);
        }

        public override void InputCurveData()
        {
            for (int i = -200; i <= 200; i++)
            {
                double x = i * (xmax / 200);
                double j = (a * x) + b;
                Point point = new Point(x, j);
                AddbfPoint(point);
                //Console.WriteLine("InputCurveData x value: {0}",x);
                //Console.WriteLine("InputCurveData j value: {0}",j);
            }
        }
        public override void Accuracy()
        {
            double SXY = 0;
            double SX = 0;
            double SY = 0;
            double sumy2 = 0;
            SXY = (sumxy / n) - (sumx / n) * (sumy / n);
            SX = Math.Sqrt((sumx2 / n) - Math.Pow(sumx / n, 2));
            foreach (Point i in points)
            {
                sumy2 = sumy2 + Math.Pow(i.GetY(), 2);
            }
            SY = Math.Sqrt((sumy2 / n) - Math.Pow(sumy / n, 2));
            r = SXY / (SX * SY);
            Console.WriteLine("Accuracy: {0}", r);
        }
    }
}


