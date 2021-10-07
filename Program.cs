﻿using System;
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

                Console.WriteLine("Title of Graph?");
                string title = Console.ReadLine();
                Console.WriteLine("Title of x-axis?");
                string xtitle = Console.ReadLine();
                Console.WriteLine("Title of y-axis?");
                string ytitle = Console.ReadLine();

                graph.PlotGraph(title,xtitle,ytitle);
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
                Graph graph = new Graph();
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
        private float x;
        private float y;

        public Point(string point)
        {
            string[] splitpoint = point.Split(',');
            this.x = float.Parse(splitpoint[0]);
            this.y = float.Parse(splitpoint[1]);
        }
        public float GetX()
        {
            return x;
        }

        public float GetY()
        {
            return y;
        }
    }
    public class Graph
    {
        private List<Point> points = new List<Point>();
        private float xmax = 0;
        private float ymax = 0;
        protected string equation;
        public Graph()
        {

        }
        

        public void AddPoint(Point point)
        {
            points.Add(point);
            if(point.GetX() > xmax)
            {
                xmax = Math.Abs(point.GetX());
            }
            if(point.GetY() > ymax)
            {
                ymax = Math.Abs(point.GetY());
            }
        }
        public void PlotGraph(string title, string xtitle, string ytitle)
        {
            float xmultiplier = 200 / xmax;
            float ymultiplier = 200 / ymax;
            Pen blackPen = new Pen(Color.Black, 3);
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
                    
                    for(int i=0;i<points.Count;i++)
                    {
                        float x = points[i].GetX()*xmultiplier;
                        float y = points[i].GetY()*ymultiplier;
                        //g.DrawString("X", new Font("Calibri", 9), new SolidBrush(Color.Black), 300+x, 300-y);
                        g.DrawEllipse(blackPen, (300 + x)-2, (300 - y)-2, 4, 4);
                    }
                    

                }
                b.Save(@"C:\Users\milda\graph.png", ImageFormat.Png);
            }
        }
    }
    public class Quadratic : Graph
    {
        
        public void Equation()
        {
            equation = "";
        }
    }

    public class Linear : Graph
    {
        public void Equation()
        {

        }
    }
}


