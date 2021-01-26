using Microsoft.Extensions.DependencyInjection;
using System;

namespace TestAndLearn
{
    class Program
    {
        static void Main(string[] args)
        {
            // TestAndLearn.ExceptionThrowing.TestRun.Run();

            //TestAndLearn.ExceptionThrowing.EventRisedException ere = new ExceptionThrowing.EventRisedException();
            //ere.Run();

            //-----------------//
            //string[] _args = new string[] { "hello", "buy"};

            //ThreadsIntro.Threadskido threadskido = new ThreadsIntro.Threadskido();
            //threadskido.Run(_args);
            // Console.ReadKey();


            //---------DI   ------//

            DI.ExampleDi example = new DI.ExampleDi();
            ServiceProvider provider = example.Init().BuildServiceProvider();

            DI.CallPrint call = new DI.CallPrint(provider.GetRequiredService<DI.ExampleDi.GetPrintMethod>());


            Console.WriteLine("rand1: ");
            DI.CallRandom callrand = provider.GetService<DI.CallRandom>();
            Console.WriteLine("rand2: ");
            DI.CallRandom rand2 = provider.GetService<DI.CallRandom>();
            Console.ReadLine();

        }
    }
}
