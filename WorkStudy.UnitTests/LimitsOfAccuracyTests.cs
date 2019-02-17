using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WorkStudy.Model;
using System.Timers;

namespace WorkStudy.UnitTests
{
    [TestClass]
    public class LimitsOfAccuracyTests
    {
        [TestMethod]
        public void TestLimitsOfAccuracy()
        {
            //n = the number of observations
            //p = the percentage occurrence of this activity against all the other activities
            //L = the limit of accuracy required - we can set this at 10 so the equation:

            //n=4p(100-p)/L x L 
            //n=4p(100-p)/100 

            var activity1 = new Activity() { Id = 1 };
            var activity2 = new Activity() { Id = 2 };
            var activity3 = new Activity() { Id = 3 };
            var activity4 = new Activity() { Id = 4 };
            var activity5 = new Activity() { Id = 5 };
            var activity6 = new Activity() { Id = 6 };

            var activites = new List<Activity>(){ activity1 , activity2 , activity3 , activity4 , activity5 , activity6 };

            var obsActivity1 = new Observation() { ActivityId = activity1.Id };
            var obsActivity2 = new Observation() { ActivityId = activity2.Id };
            var obsActivity3 = new Observation() { ActivityId = activity3.Id };
            var obsActivity4 = new Observation() { ActivityId = activity4.Id };
            var obsActivity5 = new Observation() { ActivityId = activity5.Id };
            var obsActivity6 = new Observation() { ActivityId = activity6.Id };

            List<Observation> obsActivities = new List<Observation>();

            //p =  1 / 6 observations = 0.17
            //var numberRequired = Utilities.CalculateObservationsRequired(activites);

            obsActivities.Add(obsActivity1);
            //n=4p(100-p)/L x L  = 56

            CalculatePercentage(obsActivities);
        }

        private static void CalculatePercentage(List<Observation> obsActivities)
        {
            int activityCount1 = 0;
            int activityCount2 = 0;
            int activityCount3 = 0;
            int activityCount4 = 0;
            int activityCount5 = 0;
            int activityCount6 = 0;


            //double percentage;
            //
            foreach (var x in obsActivities)
            {
                switch (x.ActivityId)
                {
                    case 1:
                        activityCount1++;
                        break;
                    case 2:
                        activityCount2++;
                        break;
                    case 3:
                        activityCount3++;
                        break;
                    case 4:
                        activityCount4++;
                        break;
                    case 5:
                        activityCount5++;
                        break;
                    case 6:
                        activityCount6++;
                        break;
                }
            }
        }

        // this is the observations required on this activity for this operator
        // so x7 is the total req for this op on all acts. bear in mind that this will change every time you make an observation (a moving target)
        // This applies to all below and we may input the acts as we start the study or during so you can’t start with anything until you get a set of readings//
        // eg after
        // 1 observations we saw idle the n will then compute to:
        // (100% idle) n= 4x100 (100-100)/100 = 4
        // if you treat the 100-100 as 1 but
        // 2nd abs is welding then both acts have 50% so n = 4x50(100-50)/100= 100 obi req on idle and 100 on welding.
        // abs 3 is welding then n for welding will 
        //become
        //(67%) n=4x67(100-67)/100= 89 obi req on welding
        //and : idle(33%) n=4x33(100-33)/100= 88 obi req on idle
        //4th abs welding(80%) n=4x80(100-80)/100= 64 obi req on welding idle(20%) and so on.

    }

    public class Example
    {
        private static Timer aTimer;
        public static double currentRunningTime { get; set; }
        public static double previousRunningTime { get; set; }
        public static double lapTime { get; set; }

        private static string stopWatchTime;

        private static int Counter;


        private static TimeSpan TotalTime = new TimeSpan();
        private static TimeSpan TimeElement = new TimeSpan();

        public static void Main()
        {
            SetTimer();

            Console.WriteLine("\nPress the Enter key to exit the application...\n");
            Console.WriteLine("The application started at {0:HH:mm:ss.fff}", DateTime.Now);
            Console.ReadLine();
            aTimer.Stop();
            aTimer.Dispose();

            Console.WriteLine("Terminating the application...");
        }

        private static void SetTimer()
        {
            // Create a timer with a two second interval.
            aTimer = new Timer(1000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {

            TotalTime = TotalTime + TimeElement.Add(new TimeSpan(0, 0, 0, 1));
            double ticks = (double)TotalTime.Ticks / 1000000;
            currentRunningTime = ticks / 600;
            string runningtimeFormatted;
            stopWatchTime = currentRunningTime.ToString("##.###");

            try
            {

                Counter = Counter + 1;
                lapTime = currentRunningTime - previousRunningTime;
                previousRunningTime = currentRunningTime;

                string currentRunningTimeFormatted = currentRunningTime.ToString("##.###");
                //string lapTimeTimeFormatted = lapTime.ToString().Substring(0, 5);

                double ss;

                Random r = new Random();
                int rInt = r.Next(0, 9);
                if (rInt > 0)
                {
                    ss = (double)rInt / 10000;
                    currentRunningTime = currentRunningTime + ss;
                }

                runningtimeFormatted = currentRunningTime.ToString("0.000");
                string lapTimeTimeFormatted = lapTime.ToString("0.000");

                Console.WriteLine($"The Elapsed event was raised at {runningtimeFormatted} - last lap time - {lapTimeTimeFormatted}");
            }


            catch (Exception ex)
            {

            }

        }
    }
}
