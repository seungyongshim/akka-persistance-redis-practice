using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Configuration;
using Akka.Persistence.Redis;
using Akka.Util.Internal;

namespace Akka_Persistence_Sqlite
{
    class Program
    {
        static void Main(string[] args)
        {
            Config config = CreateConfig(Assembly.GetExecutingAssembly());

            using (var system = ActorSystem.Create("example", config.WithFallback(ConfigurationFactory.Default())))
            {
                RedisPersistence.Get(system);
                var bankBookActor = system.ActorOf(BankBookActor.Props("1"));

                bankBookActor.Tell(100);
                bankBookActor.Tell(-50);

                Console.ReadLine();
            }


        }

        private static Config CreateConfig(Assembly assembly)
        {
            var assemblyFilePath = new Uri(assembly.GetName().CodeBase).LocalPath;
            var assemblyDirectoryPath = Path.GetDirectoryName(assemblyFilePath);
            var hoconFileName = "config";
            var hoconFilePath = $@"{assemblyDirectoryPath}{Path.DirectorySeparatorChar}{hoconFileName}.hocon";

            return ConfigurationFactory.ParseString(File.ReadAllText(hoconFilePath));
        }
    }
}
