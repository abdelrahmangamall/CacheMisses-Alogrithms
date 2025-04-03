using Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Problem
{

    public class Problem : ProblemBase, IProblem
    {
        #region ProblemBase Methods
        public override string ProblemName { get { return "CacheMisses"; } }

        public override void TryMyCode()
        {
            /* WRITE 6 DIFFERENT CASES FOR TRACE, EACH WITH:
             * 1) DIFFERENT CACHE SIZES
             * 2) EXPECTED OUTPUT
             * 3) RETURNED OUTPUT FROM THE FUNCTION
             * 4) PRINT THE CASE
             */

            int cacheSize;
            int output, expected;
            string[] arr;

            // 1. Small input case (basic scenario)
            cacheSize = 2;
            arr = new string[] { "R0", "R1", "R2", "R1", "R2", "R2", "R1" };
            expected = 3;
            output = PROBLEM_CLASS.RequiredFunction(cacheSize, arr);
            PrintCase(cacheSize, arr, output, expected);

            // 2. Cache fits exactly (no evictions needed)
            cacheSize = 3;
            arr = new string[] { "R10", "R11", "R12", "R10", "R11", "R12" };
            expected = 3;
            output = PROBLEM_CLASS.RequiredFunction(cacheSize, arr);
            PrintCase(cacheSize, arr, output, expected);

            // 3. Worst-case scenario (each request is unique, maximum evictions)
            cacheSize = 3;
            arr = new string[] { "R20", "R21", "R22", "R23", "R24", "R25", "R26" };
            expected = 7;
            output = PROBLEM_CLASS.RequiredFunction(cacheSize, arr);
            PrintCase(cacheSize, arr, output, expected);

            // 4. Mixed scenario (some elements repeat, some do not)
            cacheSize = 3;
            arr = new string[] { "R30", "R31", "R32", "R30", "R33", "R34", "R30", "R31", "R32" };
            expected = 6;
            output = PROBLEM_CLASS.RequiredFunction(cacheSize, arr);
            PrintCase(cacheSize, arr, output, expected);

            // 5. Checking eviction efficiency
            cacheSize = 2;
            arr = new string[] { "R40", "R41", "R42", "R41", "R41", "R40", "R40", "R41", "R41", "R42" };
            expected = 5;
            output = PROBLEM_CLASS.RequiredFunction(cacheSize, arr);
            PrintCase(cacheSize, arr, output, expected);

            // 6. Large sequence with eviction required
            cacheSize = 4;
            arr = new string[] { "R50", "R51", "R52", "R53", "R50", "R54", "R55", "R56", "R50", "R57", "R56", "R54", "R51", "R57", "R50" };
            expected = 9;
            output = PROBLEM_CLASS.RequiredFunction(cacheSize, arr);
            PrintCase(cacheSize, arr, output, expected);

            // 7. Edge Case - Empty sequence
            cacheSize = 4;
            arr = new string[] { };
            expected = 0;
            output = PROBLEM_CLASS.RequiredFunction(cacheSize, arr);
            PrintCase(cacheSize, arr, output, expected);

            // 8. Edge Case - zero cache
            cacheSize = 0;
            arr = new string[] { "R60", "R61", "R62", "R63", "R64", "R65", "R66" };
            expected = 7;
            output = PROBLEM_CLASS.RequiredFunction(cacheSize, arr);
            PrintCase(cacheSize, arr, output, expected);

        }


        Thread tstCaseThr;
        bool caseTimedOut;
        bool caseException;

        protected override void RunOnSpecificFile(string fileName, HardniessLevel level, int timeOutInMillisec)
        {
            /* READ THE TEST CASES FROM THE SPECIFIED FILE, FOR EACH CASE DO:
             * 1) READ ITS INPUT & EXPECTED OUTPUT
             * 2) READ ITS EXPECTED TIMEOUT LIMIT (IF ANY)
             * 3) CALL THE FUNCTION ON THE GIVEN INPUT USING THREAD WITH THE GIVEN TIMEOUT 
             * 4) CHECK THE OUTPUT WITH THE EXPECTED ONE
             */

            int testCases;
            int N = 0;
            int K = 0;
            string[] arr = null;
            int output, actualResult;

            Stream s = new FileStream(fileName, FileMode.Open);
            StreamReader br = new StreamReader(s);

            testCases = Int32.Parse(br.ReadLine());

            int totalCases = testCases;
            int correctCases = 0;
            int wrongCases = 0;
            int timeLimitCases = 0;
            bool readTimeFromFile = false;
            if (timeOutInMillisec == -1)
            {
                readTimeFromFile = true;
            }
            int i = 1;
            while (testCases-- > 0)
            {
                N = Int32.Parse(br.ReadLine());
                K = Int32.Parse(br.ReadLine());
                arr = new string[N];
                for (int j = 0; j < N; j++)
                {
                    arr[j] = br.ReadLine();
                }
                actualResult = Int32.Parse(br.ReadLine());
                br.ReadLine();
                //Console.WriteLine("N = {0}, Res = {1}", N, actualResult);
                output = 0;
                caseTimedOut = true;
                caseException = false;
                {
                    tstCaseThr = new Thread(() =>
                    {
                        try
                        {
                            int sum = 0;
                            int numOfRep = 1;
                            Stopwatch sw = Stopwatch.StartNew();
                            for (int x = 0; x < numOfRep; x++)
                            {
                                sum += PROBLEM_CLASS.RequiredFunction(K, arr);
                            }
                            output = sum / numOfRep;
                            sw.Stop();
                            Console.WriteLine("N = {0}, K = {1}, time in ms = {2}", arr.Length, K, sw.ElapsedMilliseconds);
                        }
                        catch
                        {
                            caseException = true;
                            output = int.MinValue;
                        }
                        caseTimedOut = false;
                    });

                    if (readTimeFromFile)
                    {
                        timeOutInMillisec = Int32.Parse(br.ReadLine()); ;
                    }
                    /*LARGE TIMEOUT FOR SAMPLE CASES TO ENSURE CORRECTNESS ONLY*/
                    if (level == HardniessLevel.Easy)
                    {
                        timeOutInMillisec = 100; //Large Value 
                    }
                    /*=========================================================*/
                    tstCaseThr.Start();
                    tstCaseThr.Join(timeOutInMillisec);
                }

                if (caseTimedOut)       //Timedout
                {
                    Console.WriteLine("Time Limit Exceeded in Case {0}.", i);
                    tstCaseThr.Abort();
                    timeLimitCases++;
                }
                else if (caseException) //Exception 
                {
                    Console.WriteLine("Exception in Case {0}.", i);
                    wrongCases++;
                }
                else if (output == actualResult)    //Passed
                {
                    Console.WriteLine("Test Case {0} Passed!", i);
                    correctCases++;
                }
                else                    //WrongAnswer
                {
                    Console.WriteLine("Wrong Answer in Case {0}.", i);
                    Console.WriteLine(" your answer = " + output + ", correct answer = " + actualResult);
                    wrongCases++;
                }

                i++;
            }
            br.Close();
            s.Close();
            Console.WriteLine();
            Console.WriteLine("# correct = {0}", correctCases);
            Console.WriteLine("# time limit = {0}", timeLimitCases);
            Console.WriteLine("# wrong = {0}", wrongCases);
            Console.WriteLine("\nFINAL EVALUATION (%) = {0}", Math.Round((float)correctCases / totalCases * 100, 0));
        }

        protected override void OnTimeOut(DateTime signalTime)
        {
        }

        /// <summary>
        /// Generate a file of test cases according to the specified params
        /// </summary>
        /// <param name="level">Easy or Hard</param>
        /// <param name="numOfCases">Required number of cases</param>
        /// <param name="includeTimeInFile">specify whether to include the expected time for each case in the file or not</param>
        /// <param name="timeFactor">factor to be multiplied by the actual time</param>
        public override void GenerateTestCases(HardniessLevel level, int numOfCases, bool includeTimeInFile = false, float timeFactor = 1)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helper Methods
        private static void PrintCase(int N, string[] arr, int output, int expected)
        {
            /* PRINT THE FOLLOWING
             * 1) INPUT
             * 2) EXPECTED OUTPUT
             * 3) RETURNED OUTPUT
             * 4) WHETHER IT'S CORRECT OR WRONG
             * */
            Console.WriteLine("Cache Size: {0}", N);

            Console.Write('{');
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write(arr[i] + " ");
            }
            Console.Write('}');
            Console.WriteLine();
            Console.WriteLine("Output = " + output);
            Console.WriteLine("Expected = " + expected);
            if (expected == output)
                Console.WriteLine("CORRECT");
            else
                Console.WriteLine("WRONG");

            Console.WriteLine();
        }
        #endregion

    }
}
