using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace TestAndLearn.DI
{
    public class ExampleDi
    {
        public ServiceProvider Init()
        {
            ServiceCollection collection = new ServiceCollection();
                    collection.AddTransient< HelloPrint>();
                    collection.AddTransient< ByePrint>();
            
            collection.AddTransient<GetPrintMethod>(serviceProvider => key =>
            {
                switch (key)
                {
                    case "A":
                        return serviceProvider.GetService<HelloPrint>();
                    case "B":
                        return serviceProvider.GetService<ByePrint>();
                   
                    default:
                        throw new KeyNotFoundException(); // or maybe return null, up to you
                }
            });
           
            return collection.BuildServiceProvider();
        }
       
       
        public delegate IPrint GetPrintMethod( string key);

       
    }

    public interface IPrint
    {
        void Print();

    }

    public class HelloPrint : IPrint
    {
        public void Print()
        {
            Console.WriteLine("Hello ");

        }
    }

    public class ByePrint : IPrint
    {
        public void Print()
        {
            Console.WriteLine("Bye ");

        }
    }

    

    public class CallPrint
    {
        public CallPrint(ExampleDi.GetPrintMethod method)
        {
            IPrint print = method("A");
            print.Print();
        }
    }

}
