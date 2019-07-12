using System;
using System.Text;
using VSCM.Logging.Contracts.Enums;

namespace VSCM.Logging.Framework.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var sample = new Sample();

            sample.Run();
        }
    }

    class Sample
    {
        public void Run()
        {
            var decisions = Menu();

            PrintDecisions(decisions.Item1, decisions.Item2, decisions.Item3);

            WriteToLog(decisions.Item1, decisions.Item2, decisions.Item3);

            Console.WriteLine();
            Console.WriteLine("Please check the appropriate database for your logged data");
            Console.ReadLine();
        }

        private Tuple<int, int, int> Menu()
        {
            var decision1 = DecisionMenu1();
            var decision2 = 1;

            if (decision1 == 2)
            {
                decision2 = DecisionMenu2();
            }

            var decision3 = DecisionMenu3();

            return new Tuple<int, int, int>(decision1, decision2, decision3);
        }

        private int DecisionMenu1()
        {
            Console.Clear();
            Console.WriteLine("Please select a logging option:");
            Console.WriteLine("1 - Standard Logging");
            Console.WriteLine("2 - Custom Logging");
            Console.WriteLine();

            var decision = Console.ReadLine();

            return int.TryParse(decision, out var decisionResult) ? decisionResult : DecisionMenu1();
        }

        private int DecisionMenu2()
        {
            Console.Clear();
            Console.WriteLine("Please select a logging location:");
            Console.WriteLine("1 - Staging");
            Console.WriteLine("2 - Sandbox");
            Console.WriteLine("3 - Production");
            Console.WriteLine();

            var decision = Console.ReadLine();

            return int.TryParse(decision, out var decisionResult) ? decisionResult : DecisionMenu2();
        }

        private int DecisionMenu3()
        {
            Console.Clear();
            Console.WriteLine("Please select an item to log:");
            Console.WriteLine("1 - Exception Data");
            Console.WriteLine("2 - Performance Data");
            Console.WriteLine("3 - Telemetry Data");
            Console.WriteLine("4 - Transmission Data");
            Console.WriteLine();

            var decision = Console.ReadLine();

            return int.TryParse(decision, out var decisionResult) ? decisionResult : DecisionMenu3();
        }

        private void PrintDecisions(int decision1, int decision2, int decision3)
        {
            Console.Clear();

            var sb = new StringBuilder();
            sb.Append("Writing ");
            sb.Append(GetDecision3Text(decision3));
            sb.Append(" to ");
            sb.Append(GetDecision1Text(decision1));
            sb.Append(" location: ");
            sb.Append(GetDecision2Text(decision2));

            Console.WriteLine(sb.ToString());
        }

        private string GetDecision3Text(int decision3)
        {
            var returnVal = string.Empty;

            switch (decision3)
            {
                case 1:
                    returnVal = "Exception Data";
                    break;
                case 2:
                    returnVal = "Performance Data";
                    break;
                case 3:
                    returnVal = "Telemetry Data";
                    break;
                case 4:
                    returnVal = "Transmission Data";
                    break;
            }

            return returnVal;
        }

        private string GetDecision1Text(int decision1)
        {
            var returnVal = string.Empty;

            switch (decision1)
            {
                case 1:
                    returnVal = "Standard";
                    break;
                case 2:
                    returnVal = "Custom";
                    break;
            }

            return returnVal;
        }

        private string GetDecision2Text(int decision2)
        {
            var returnVal = string.Empty;

            switch (decision2)
            {
                case 1:
                    returnVal = "Staging";
                    break;
                case 2:
                    returnVal = "Sandbox";
                    break;
                case 3:
                    returnVal = "Production";
                    break;
            }

            return returnVal;
        }

        private void WriteToLog(int decision1, int decision2, int decision3)
        {
            if (decision1 == 2)
            {
                SetLogConfiguration(decision2);
            }

            Log(decision3);
        }

        private void SetLogConfiguration(int decision2)
        {
            if (decision2 == 2)
            {
                Logger.SetLoggerConfiguration("Endpoint=sb://production-visibleservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ReDjMgmsSRb9TtwNYJ3JLZjlXomh+YS+np0P1R6a8TI=", string.Empty, "loggingsandboxevents", "loggingeventstransferqueue");
            }
            else if (decision2 == 3)
            {
                Logger.SetLoggerConfiguration("Endpoint=sb://production-visibleservicebus.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ReDjMgmsSRb9TtwNYJ3JLZjlXomh+YS+np0P1R6a8TI=", string.Empty, "loggingproductionevents", "loggingeventstransferqueue");
            }
        }

        private void Log(int decision3)
        {
            switch (decision3)
            {
                case 1:
                    Logger.LogException("framework sample test", "framework sample test", new Exception("framework sample test"), Guid.Empty);
                    break;
                case 2:
                    Logger.LogPerformance("framework sample test", 1000, string.Empty, 0, 0, 0, "framework sample test", string.Empty, string.Empty, Guid.Empty);
                    break;
                case 3:
                    Logger.LogTransmission(Guid.Empty, "0.0.0.0", 0, string.Empty, string.Empty, "framework sample test", string.Empty, DateTime.Now, DateTime.Now, null);
                    break;
                case 4:
                    Logger.LogTransmissionData(ActionKind.Request, new { Test = "framework sample test"}, Guid.Empty);
                    break;
            }
        }
    }
}
