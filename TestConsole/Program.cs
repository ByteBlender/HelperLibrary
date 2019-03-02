﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelperLibrary;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
         //  Post.locResult locResult = Post.locResult.checkLocStPc("WILSON", "WA","6107");
            
            Console.WriteLine(Test.temp);
            Progress<int> progress = new Progress<int>();
            progress.ProgressChanged += Progress_ProgressChanged;

            TextFileRW.readTextFileToTable(@"C:\Users\Gayan\Documents\MSOL\test data\ADHOC JOB\Test sorted file.txt", "\t");

            Console.ReadLine();
        }

        private static void Progress_ProgressChanged(object sender, int e)
        {
            Console.Write($"{e}\r");
        }
    }
}
