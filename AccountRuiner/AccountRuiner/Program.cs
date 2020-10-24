using System;

namespace AccountRuiner
{
    class Program
    {
        static void Main()
        {
            Console.Title = "Account ruiner";
            Console.Write(@"Token to ruin:
> ");
            string token = Console.ReadLine();

            Console.Write(@"Icon name (empty for none):
> ");
            string img = Console.ReadLine();

            serverN:
            Console.Write(@"Server name:
> ");
            string serverName = Console.ReadLine();
            if (serverName.Length < 1)
                goto serverN;
            Ruiner ruiner = new Ruiner(token, img, serverName);
            ruiner.Ruin();
            Console.WriteLine("Ruining finished azez, press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
