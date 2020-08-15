using Akka.Actor;
using Akka.Persistence;
using System;

namespace Akka_Persistence_Sqlite
{
    public class BankBookActor : ReceivePersistentActor
    {
        public BankBookActor(string id)
        {
            PersistenceId = id;

            Command<int>(CommandHandle);
            Recover<int>(RecoverHandle);
        }

        public override string PersistenceId { get; }

        public int TotalAmount { get; set; } = 0;

        public static Props Props(string id) =>
                            Akka.Actor.Props.Create(() => new BankBookActor(id));

        private void CommandHandle(int m)
        {
            TotalAmount += m;
            Persist(m, _ => { });
            Console.WriteLine($"TotalAmount:{TotalAmount}");
        }

        private void RecoverHandle(int m)
        {
            TotalAmount += m;
        }
    }
}