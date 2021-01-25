using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAndLearn.ThreadsIntro
{
    public class Threadskido
    {
       
           public  void Run(string[] args)
            {
                Thread worker = new Thread(
                        delegate () 
                        { 
                            Console.WriteLine("input data");
                            Console.ReadLine(); 
                        });

            if (args.Length > 0)
            {
                worker.IsBackground = true;
            }

                worker.Start();
            }
        
    }
}
