using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Pit
{
    /*
     The deepness.txt file contains information of a field.
     Each line represents 1m of the filed and the values are the deepness of a hole there
     
     Example
      0000 11 22 33 4 22 1 2 000000  //in the txt file each number has its own line like 0
      ----                   ------                                                      0
          |--|          |-| |                                                            0
             |--|    |--| |-|
                |--| |
                   |-|
     */
    struct Hole
    {
        public int place;
        public int deepness;

        public Hole(int place, int deepness)
        {
            this.place = place;
            this.deepness = deepness;
        }
    }
    class Pit
    {
        static List<Hole> field = new List<Hole>();

        //Task 1: Read and store the data of the melyseg.txt 
        //        Print the number of the records on the screen
        static void Task1()
        {
            StreamReader sr = new StreamReader("melyseg.txt");
            int place = 0;
            while (!sr.EndOfStream)
            {
                place++;
                int deepness = int.Parse(sr.ReadLine());

                Hole item = new Hole(place, deepness);
                field.Add(item);
            }

            sr.Close();

            Console.WriteLine("Task 1");
            Console.WriteLine($"\tNumber of the records: {field.Count}");
        }

        //Task 2: Ask teh user for a place. Print how deep is the hole at that place
        //        Use that place in task 6 too
        static int givenPlace = 0;
        static void Task2()
        {
            Console.WriteLine("Task 2");
            Console.Write("\tPlace: ");
            givenPlace = int.Parse(Console.ReadLine());
            foreach (Hole item in field)
            {
                if (item.place == givenPlace)
                {
                    Console.WriteLine($"\tThe hole is {item.deepness}m deep.");
                }
            }
        }

        //Task 3: Determine the percentage of the ground level of the field
        static void Task3()
        {
            Console.WriteLine("Task 3");
            int groundLevel = 0;
            foreach (Hole item in field)
            {
                if (item.deepness == 0)
                    groundLevel++;
            }

            Console.WriteLine($"\t{Math.Round(((double)groundLevel / field.Count) * 100, 2)}% of the field is on the ground level.");
        }

        //Task 4: List all the holes on the field in a txt file
        //        Each hole should have its own line in the txt
        static void Task4()
        {
            StreamWriter sw = new StreamWriter("holes.txt");
            int previousDeepness = 0;
            int nextLine = 0;
            foreach (Hole item in field)
            {
                if (item.deepness != 0)
                {
                    nextLine++;
                    if (previousDeepness == 0 && nextLine > 1)
                    {
                        sw.WriteLine();
                    }
                    sw.Write(item.deepness);
                }
                previousDeepness = item.deepness;
            }

            sw.Flush();
            sw.Close();
        }

        //Task 5: Print the number of holes on the field
        static void Task5()
        {
            Console.WriteLine("Task 5");
            int previousDeepness = 0;
            int numberOfHoles = 0;
            foreach (Hole item in field)
            {
                if (item.deepness != 0 && previousDeepness == 0)
                {
                    numberOfHoles++;
                }
                previousDeepness = item.deepness;
            }
            Console.WriteLine($"\tNumber of holes: {numberOfHoles} ");
        }

        //Task 6: if there is no hole at the place given by teh user, then wprint that on the screen
        //        if there is
        //  a.  determine where the hole started and ended
        //  b.  if it is gradually decreasing towards the deepest point and tehn gradually increasing back
        //  c.  determine the deepest point
        //  d.  what is the volume of the hole (10m wide everywhere, one line is one metre)
        //  e.  how much water it can store. But the water surface shall be 1m below the ground so a huge storm wont spill out the water
        static void Task6()
        {
            Console.WriteLine("Task 6");
            bool noHole = true;
            int sDeepness = 0;
            int eDeepness = 0;
            int previousPlace = 0;
            int startingPlace = 0;
            int endingPlace = 0;
            int deepestPoint = 0;
            int deepestPointPlace = 0;
            bool gradual = true;
            int volume = 0;
            int water = 0;
            int previous = 0;

            foreach (Hole item in field)
            {
                if (item.place == givenPlace && item.deepness != 0)
                {
                    noHole = false;
                    foreach (Hole i in field)
                    {
                        if (previousPlace == 0 && i.deepness != 0 && i.place <= givenPlace)
                        {
                            sDeepness = i.deepness;
                            startingPlace = i.place;
                        }
                        if (previousPlace != 0 && i.deepness == 0 && i.place >= givenPlace)
                        {
                            eDeepness = previousPlace;
                            endingPlace = i.place;
                            break;
                        }
                        previousPlace = i.deepness;

                    }
                    foreach (Hole it in field)
                    {
                        if (it.place >= startingPlace && it.place < endingPlace && it.deepness > deepestPoint)
                        {
                            deepestPoint = it.deepness;
                            deepestPointPlace = it.place;
                        }
                    }
                    foreach (Hole ite in field)
                    {
                        if (ite.place >= startingPlace && ite.place <= deepestPointPlace)
                        {
                            if (ite.deepness <= previous)
                                gradual = false;
                            previous = ite.deepness;
                        }
                        if (ite.place > deepestPointPlace && ite.place <= endingPlace)
                        {
                            if (ite.deepness >= previous)
                                gradual = false;
                            previous = ite.deepness;
                        }
                    }
                    foreach (Hole i in field)
                    {
                        if (i.place >= startingPlace && i.place < endingPlace)
                        {
                            volume += i.deepness * 1 * 10;
                            water += (i.deepness - 1) * 1 * 10;
                        }
                    }

                    Console.WriteLine("\ta)");
                    Console.WriteLine($"\tStart of teh hole: {startingPlace}, end of the hole: {endingPlace - 1}.");
                    Console.WriteLine("\tb)");
                    if (gradual)
                    {
                        Console.WriteLine("\tIt is gradual.");
                    }
                    else
                    {
                        Console.WriteLine("\tIt is not gradual.");
                    }
                    Console.WriteLine("\tc)");
                    Console.WriteLine($"\tDeepest point is {deepestPoint} deep.");
                    Console.WriteLine("\td)");
                    Console.WriteLine($"\tThe volume is {volume} m^3.");
                    Console.WriteLine("\te)");
                    Console.WriteLine($"\tIt can store {water} m^3 water safely.");
                }

            }
            if (noHole)
            {
                Console.WriteLine("There is no hole in the ground at that place.");
            }
        }
        static void Main(string[] args)
        {
            Task1();
            Console.WriteLine();
            Task2();
            Console.WriteLine();
            Task3();
            Console.WriteLine();
            Task4();
            Task5();
            Console.WriteLine();
            Task6();
            Console.ReadKey();
        }
    }
}
