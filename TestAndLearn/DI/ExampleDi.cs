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
        public ServiceCollection Init()
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

            collection.AddScoped<RandomScoped>();
            collection.AddTransient<RandomTransy>();
            collection.AddSingleton<RandomSingle>();
            collection.AddTransient<CallRandom>();

            return collection;
        }

        public void Init(IServiceCollection collection)
        {
            collection.AddTransient<HelloPrint>();
            collection.AddTransient<ByePrint>();

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

            collection.AddScoped<RandomScoped>();
            collection.AddTransient<RandomTransy>();
            collection.AddSingleton<RandomSingle>();
            collection.AddTransient<CallRandom>();
        }
       
        public delegate IPrint GetPrintMethod( string key);

       
    }
    #region print
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
    #endregion

    #region random

    public class RandomSingle
    {
        private  Guid guid;

        public RandomSingle()
        {
            guid = Guid.NewGuid();
        }

        public string GetRandom()
        {
            return $"Single: {guid}";
        }
    }

    public class RandomTransy
    {
        private  Guid guid;
        public RandomTransy()
        {
            guid = Guid.NewGuid();
        }
        public string GetGuid()
        {
            return $"Transient: {guid}";
        }
    }

    public class RandomScoped
    {
        private  Guid guid;
        public RandomScoped()
        {
            guid = Guid.NewGuid();
        }

        public string GetGuid()
        {
            return $"Scoped: {guid}";
        }
    }

    public class CallRandom
    {
        public string _Scoped { get; set; }
        public string _Transy { get; set; }
        public string _Single { get; set; }
        public CallRandom(RandomScoped scoped, RandomSingle single, RandomTransy trans)
        {
            _Scoped = scoped.GetGuid();
            _Transy = trans.GetGuid();
            _Single = single.GetRandom();
        }
    }
    #endregion
}
