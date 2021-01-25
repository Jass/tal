using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestAndLearn.ExceptionThrowing
{
    public class Simple
    {
        public Simple()
        {
            throw new InvalidOperationException("I can't create an instance! Sorry for this!");
        }
    }
    public class ExceptionMaker
    {
        public ExceptionMaker()
        { 
        
        }

        public void MakeInvalidCast()
        {
            throw new InvalidCastException();
        }

        public void MakeOutOfRange()
        {
            throw new IndexOutOfRangeException();
        }


    }
    public static class Factory
    {
        public static T Create<T>() where T : new()
        {
            return new T();
        }
    }

    public static class TestRun
    {
        public static void Run()
        {
            AggregateException ae = new AggregateException();
            try
            {
                var simple = Factory.Create<Simple>();
            }
            catch (InvalidOperationException e)
            {
                
            }
            catch (Exception e)
            {
                Console.WriteLine($" \\n {e.Message}");
            }

        }
    }

    public class EventRisedException
    {
        public event EventHandler MyEvent;
        protected void OnMyEvent() 
        {
            EventHandler handler = MyEvent;
            if (handler != null)
                handler(this, EventArgs.Empty);
            
           
        }
        protected void OnMyEvent1()//eat the exception
        { 
            EventHandler handler = MyEvent; 
            if (handler != null)
            { 
                foreach (var d in handler.GetInvocationList())
                { 
                    try 
                    {
                        ((EventHandler)d)(this, EventArgs.Empty); 
                    } 
                    catch { }
                } 
            } 
        }

        protected void OnMyEvent3() 
        {
            EventHandler handler = MyEvent; 
            if (handler != null)
            { 
                List<Exception> exceptions = null;
                foreach (var d in handler.GetInvocationList()) 
                {
                    try { 
                        ((EventHandler)d)(this, EventArgs.Empty); 
                    } 
                    catch (Exception exc) 
                    { 
                        if (exceptions == null)
                            exceptions = new List<Exception>(); 
                        exceptions.Add(exc); 
                    }
                } 
                if (exceptions != null)
                    throw new AggregateException(exceptions); 
            }
        }

        public static void ParallelInvoke(params Action[] actions)
        { 
            if (actions == null) 
                throw new ArgumentNullException("actions"); 
            if (actions.Any(a => a == null)) 
                throw new ArgumentException("actions");
            if (actions.Length == 0) return;
            
            using (ManualResetEvent mre = new ManualResetEvent(false)) 
            { 
                int remaining = actions.Length;
                var exceptions = new List<Exception>(); 
                foreach (var action in actions) 
                { 
                    ThreadPool.QueueUserWorkItem(state => 
                    { try
                        { 
                            ((Action)state)(); 
                        }
                        catch (Exception exc)
                        { 
                            lock (exceptions) 
                                exceptions.Add(exc); 
                        } finally 
                        
                        { 
                            if (Interlocked.Decrement(ref remaining) == 0)
                                mre.Set(); 
                        } 
                    }, action); 
                } 
                
                mre.WaitOne();
                if (exceptions.Count > 0)
                    throw new AggregateException(exceptions); 
            } 
        }


       
        public void Invocation()
        {
            MyEvent += (s, e) => Console.WriteLine("1");
            
            MyEvent += (s, e)=> Console.WriteLine("2"); 
            
            MyEvent += (s, e) => { throw new DivideByZeroException("uh oh"); };
            
            MyEvent += (s, e) => Console.WriteLine("3");
            MyEvent += (s, e) => { throw new Exception("uh 1 oh"); };

            MyEvent += (s, e) => Console.WriteLine("4");
            MyEvent += (s, e) => { throw new Exception("uh 2 oh"); };
        }

        public void DoExceptionStuff() 
        {
            try
            {
                var inputNum = Int32.Parse(Console.ReadLine());

                Parallel.For(0, 4, i =>
                        {
                            Console.WriteLine($"{i} in loop:");
                            switch (i)
                            {
                                case 1:
                                    
                                    throw new DivideByZeroException(i.ToString());
                                case 2:
                                    throw new InvalidOperationException(i.ToString());
                                case 3:
                                   throw new ArgumentOutOfRangeException(i.ToString());
                                case 0:
                                    throw new InvalidTimeZoneException(i.ToString());

                            }
                    
                        });
            }
            catch (Exception e)
            {
                            // если заворачиваем в один общий ошибки,
                            //то в методе run нет stackTrace!!!
               // throw new InvalidOperationException("parallel.for exception", e);
                            
                            // stack trace saved
                throw e;
            }
        }

        public void Run()
        {
            try
            {
                Invocation();
                OnMyEvent();
                //Action _me = this.OnMyEvent3;


                //ParallelInvoke(_me);
                //DoExceptionStuff();
            }
            catch (AggregateException e)
            {
                foreach (var ie in e.InnerExceptions)
                {
                    Console.WriteLine(e.Message + Environment.NewLine + e.StackTrace);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }


}
