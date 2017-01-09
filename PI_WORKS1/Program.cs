using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PI_WORKS
{
    class Program
    {
        static void Main(string[] args)
        {
            var PiWorks = new PiProcess();


            PiWorks.PiProcessHasList();

            PiWorks.PiProcessHasListOptionalParm();

            Console.WriteLine();

            PiWorks.PiProcessHasList(new DateTime(2017, 1, 3)); //optional parameter date filter
            PiWorks.PiProcessHasListOptionalParm(new DateTime(2017, 1, 3)); //optional parameter date filter

            Console.WriteLine();

            PiWorks.PiProcessLinq();
            PiWorks.PiProcessList();

            Console.WriteLine();

            int PlayListNumber = 346;
            Console.WriteLine(" {0} User played {1} distinct song playlist ", PiWorks.GetUsersCountByPlayListNumber(PlayListNumber), PlayListNumber);

            Console.WriteLine(" Maximum number of song list is {0}", PiWorks.GetMaximumNumberOfDistinctSongList());

            Console.Read();
        }
    }

    class PiProcess
    {
        private Dictionary<int, int> ResultSet;
        private System.Diagnostics.Stopwatch stopwatch;
        private const string InputTextFile = "input_text.txt";
        private const char seperation = ',';
        private readonly string ExportFileFormat;

        public PiProcess()
        {
            ResultSet = new Dictionary<int, int>();
            stopwatch = new System.Diagnostics.Stopwatch();
            ExportFileFormat = "{0}?{1}".Replace('?', seperation);
        }

        /// <summary>
        /// Used HashSet<T> type for Song List ,  resultSelt also dictionary Class
        /// OverLoad Mehtod No parameter 
        /// </summary>
        public void PiProcessHasList()
        {
            stopwatch.Reset();
            stopwatch.Start();

            ExecuteProcessHashList();

            stopwatch.Stop();

            Console.WriteLine(" PiProcessHasList    No Date Filter - Time elapsed: {0}",
             stopwatch.Elapsed);
        }

        /// <summary>
        /// Used HashSet<T> type for Song List ,  resultSelt also dictionary Class
        /// OverLoad Mehtod with Date Parameter 
        /// </summary>
        /// <param name="FilterDate"></param>
        public void PiProcessHasList(DateTime FilterDate)
        {
            stopwatch.Reset();
            stopwatch.Start();

            ExecuteProcessHashList(FilterDate);

            stopwatch.Stop();

            Console.WriteLine(" PiProcessHasList    with Date Filter - Time elapsed: {0}",
             stopwatch.Elapsed);
        }

        /// <summary>
        /// Used HashSet<T> type for Song List ,  resultSelt also dictionary Class
        /// date filter is apply by optional parameter.
        /// </summary>
        /// <param name="FilterDate"></param>
        public void PiProcessHasListOptionalParm(DateTime? FilterDate = null)
        {
            stopwatch.Reset();
            stopwatch.Start();

            ExecuteProcessHashListOptionalParm(FilterDate);

            stopwatch.Stop();

            Console.WriteLine(" PiProcessHasListOptionalParm    {0} - Time elapsed: {1}",
            FilterDate == null ? "No Date Filter" : "with Date Filter", stopwatch.Elapsed);
        }

        /// <summary>
        /// Used HashSet<T> type for SongList list,  resultSelt generated using Linq
        /// </summary>
        public void PiProcessLinq()
        {
            stopwatch.Reset();
            stopwatch.Start();

            ExecuteProcessLinq();

            stopwatch.Stop();
            Console.WriteLine(" PiProcessHashListLinq   - Time elapsed: {0}", stopwatch.Elapsed);
        }

        /// <summary>
        ///  Used List<T> type for SongList list  ,  resultSelt also dictionary Class
        ///  also method takes filter date optionaly to process specific date , null process all  date.
        /// </summary>
        public void PiProcessList()
        {
            stopwatch.Reset();
            stopwatch.Start();

            ExecuteProcessList();

            stopwatch.Stop();
            Console.WriteLine(" PiProcessList   - Time elapsed: {0}", stopwatch.Elapsed);

        }

        void ExecuteProcessHashList()
        {
            /* ------Read File ---- */
            Dictionary<int, HashSet<int>> ReadDataList = new Dictionary<int, HashSet<int>>();

            using (StreamReader sr = new StreamReader(InputTextFile))
            {
                string ReadLine = string.Empty;
                string[] ReadLineDelimiteValue;
                int SongID = 0;
                int ClientID = 0;
                HashSet<int> findListByClientID;
                // read header first
                sr.ReadLine();

                while ((ReadLine = sr.ReadLine()) != null)
                {
                    ReadLineDelimiteValue = ReadLine.Split(seperation);


                    ClientID = int.Parse(ReadLineDelimiteValue[2]);
                    SongID = int.Parse(ReadLineDelimiteValue[1]);


                    if (!ReadDataList.TryGetValue(ClientID, out findListByClientID))
                    {
                        ReadDataList.Add(ClientID, new HashSet<int>() { SongID });
                    }
                    else
                    {  // hashSet doesn't allow duplice value insert, add method internally checks duplicate values 
                        findListByClientID.Add(SongID);
                    }
                }

            }

            /* ------Generate Result ---- */

            int NumberOfDistinctSongByClient = 0;
            ResultSet.Clear();

            foreach (var item in ReadDataList)
            {
                NumberOfDistinctSongByClient = item.Value.Count;

                if (!ResultSet.ContainsKey(NumberOfDistinctSongByClient))
                {
                    ResultSet.Add(NumberOfDistinctSongByClient, 1);
                }
                else
                {  //CLIENT_COUNT ++
                    ResultSet[NumberOfDistinctSongByClient]++;
                }
            }

            /* **********EXPORT****** */
            ExportResultSet( "ExecuteProcessHashList", false);
        }

        void ExecuteProcessHashList(DateTime FilterDate)
        {
            /* ------Read File ---- */
            Dictionary<int, HashSet<int>> ReadDataList = new Dictionary<int, HashSet<int>>();

            if (FilterDate == DateTime.MinValue || FilterDate == DateTime.MaxValue)
                throw new Exception("incorrect date parameter value");

            using (StreamReader sr = new StreamReader(InputTextFile))
            {
                string ReadLine = string.Empty;
                string[] ReadLineDelimiteValue;
                int SongID = 0;
                int ClientID = 0;
                HashSet<int> findListByClientID;
                DateTime PlayTs = DateTime.MinValue;
                // read header first
                sr.ReadLine();

                while ((ReadLine = sr.ReadLine()) != null)
                {
                    ReadLineDelimiteValue = ReadLine.Split(seperation);

                    PlayTs = DateTime.Parse(ReadLineDelimiteValue[3]);

                    if (PlayTs != FilterDate)
                        continue;

                    ClientID = int.Parse(ReadLineDelimiteValue[2]);
                    SongID = int.Parse(ReadLineDelimiteValue[1]);


                    if (!ReadDataList.TryGetValue(ClientID, out findListByClientID))
                    {
                        ReadDataList.Add(ClientID, new HashSet<int>() { SongID });
                    }
                    else
                    {  // hashSet doesn't allow duplice value insert, add method internally checks duplicate values 
                        findListByClientID.Add(SongID);
                    }
                }

            }

            /* ------Generate Result ---- */

            int NumberOfDistinctSongByClient = 0;
            ResultSet.Clear();

            foreach (var item in ReadDataList)
            {
                NumberOfDistinctSongByClient = item.Value.Count;

                if (!ResultSet.ContainsKey(NumberOfDistinctSongByClient))
                {
                    ResultSet.Add(NumberOfDistinctSongByClient, 1);
                }
                else
                {  //CLIENT_COUNT ++
                    ResultSet[NumberOfDistinctSongByClient]++;
                }
            }

            /* **********EXPORT****** */
            ExportResultSet( "ExecuteProcessHashList", true);
        }

        void ExecuteProcessHashListOptionalParm(DateTime? FilterDate = null)
        {
            /* ------Read File ---- */
            Dictionary<int, HashSet<int>> ReadDataList = new Dictionary<int, HashSet<int>>();
            bool isDateFilterApplied = FilterDate != null ? true : false;

            using (StreamReader sr = new StreamReader(InputTextFile))
            {
                string ReadLine = string.Empty;
                string[] ReadLineDelimiteValue;
                int SongID = 0;
                int ClientID = 0;
                HashSet<int> findListByClientID;
                DateTime PlayTs = DateTime.MinValue;
                // read header first
                sr.ReadLine();

                while ((ReadLine = sr.ReadLine()) != null)
                {
                    ReadLineDelimiteValue = ReadLine.Split(seperation);

                    if (isDateFilterApplied)
                    {
                        PlayTs = DateTime.Parse(ReadLineDelimiteValue[3]);

                        if (PlayTs != FilterDate)
                            continue;
                    }

                    ClientID = int.Parse(ReadLineDelimiteValue[2]);
                    SongID = int.Parse(ReadLineDelimiteValue[1]);


                    if (!ReadDataList.TryGetValue(ClientID, out findListByClientID))
                    {
                        ReadDataList.Add(ClientID, new HashSet<int>() { SongID });
                    }
                    else
                    {  // hashSet doesn't allow duplice value insert, add method internally checks duplicate values 
                        findListByClientID.Add(SongID);
                    }
                }

            }

            /* ------Generate Result ---- */

            int NumberOfDistinctSongByClient = 0;
            ResultSet.Clear();

            foreach (var item in ReadDataList)
            {
                NumberOfDistinctSongByClient = item.Value.Count;

                if (!ResultSet.ContainsKey(NumberOfDistinctSongByClient))
                {
                    ResultSet.Add(NumberOfDistinctSongByClient, 1);
                }
                else
                {  //CLIENT_COUNT ++
                    ResultSet[NumberOfDistinctSongByClient]++;
                }
            }

            /* **********EXPORT****** */
            ExportResultSet( "ExecuteProcessHashListOptionalParm", isDateFilterApplied);
        }

        void ExecuteProcessLinq()
        {
            /* ------Read File ---- */
            Dictionary<int, HashSet<int>> ReadDataList = new Dictionary<int, HashSet<int>>();

            using (StreamReader sr = new StreamReader(InputTextFile))
            {
                string ReadLine = string.Empty;
                string[] ReadLineDelimiteValue;
                int SongID = 0;
                int ClientID = 0;
                HashSet<int> findListByClientID;
                DateTime PlayTs = DateTime.MinValue;

                // read header first
                sr.ReadLine();

                while ((ReadLine = sr.ReadLine()) != null)
                {
                    ReadLineDelimiteValue = ReadLine.Split(seperation);

                    ClientID = int.Parse(ReadLineDelimiteValue[2]);
                    SongID = int.Parse(ReadLineDelimiteValue[1]);

                    if (!ReadDataList.TryGetValue(ClientID, out findListByClientID))
                    {
                        ReadDataList.Add(ClientID, new HashSet<int>() { SongID });
                    }
                    else
                    {
                        if (!findListByClientID.Contains(SongID))
                        {
                            findListByClientID.Add(SongID);
                        }
                    }
                }

            }

            /* ------Generate Result ---- */

            var ListByClientID = ReadDataList.Select(b => new
            {
                DistinctPlayCount = b.Value.Count,
                ClientNumber = 1
            }).ToList();

            var ExportDatListResult = ListByClientID.GroupBy(a => a.DistinctPlayCount).Select(b => new
            {
                DistinctPlayCount = b.First().DistinctPlayCount,
                ClientCount = b.Select(d => d.ClientNumber).Sum()
            }).OrderBy(m => m.DistinctPlayCount).ToList();


            /****************** EXPORT ******************************/

            string ExportedFileName = "Output_Method_ExecuteProcessLinq.txt";

            using (StreamWriter sr = new StreamWriter(ExportedFileName))
            {
                // caption
                sr.WriteLine(string.Format(ExportFileFormat, "DISTINCT_PLAY_COUNT", "CLIENT_COUNT"));

                foreach (var item in ExportDatListResult)
                {
                    sr.WriteLine(string.Format(ExportFileFormat, item.DistinctPlayCount.ToString(), item.ClientCount.ToString()));
                }
            }

        }

        void ExecuteProcessList()
        {
            /* ------Read File ---- */
            Dictionary<int, List<int>> ReadDataList = new Dictionary<int, List<int>>();

            using (StreamReader sr = new StreamReader(InputTextFile))
            {
                string ReadLine = string.Empty;
                string[] ReadLineDelimiteValue;
                int SongID = 0;
                int ClientID = 0;
                List<int> findListByClientID;

                // read header first
                sr.ReadLine();

                while ((ReadLine = sr.ReadLine()) != null)
                {
                    ReadLineDelimiteValue = ReadLine.Split(seperation);

                    ClientID = int.Parse(ReadLineDelimiteValue[2]);
                    SongID = int.Parse(ReadLineDelimiteValue[1]);

                    if (!ReadDataList.TryGetValue(ClientID, out findListByClientID))
                    {
                        ReadDataList.Add(ClientID, new List<int>() { SongID });
                    }
                    else
                    {
                        if (!findListByClientID.Contains(SongID))
                        {
                            findListByClientID.Add(SongID);
                        }
                    }
                }

            }

            /* ------Generate Result ---- */
            int NumberOfDistinctSongByClient = 0;
            ResultSet.Clear();

            foreach (var item in ReadDataList)
            {                     /// get number of song for each client
                NumberOfDistinctSongByClient = item.Value.Count;

                if (!ResultSet.ContainsKey(NumberOfDistinctSongByClient))
                {
                    ResultSet.Add(NumberOfDistinctSongByClient, 1);
                }
                else
                { /// CLIENT_COUNT by distinct song playlist ++
                    ResultSet[NumberOfDistinctSongByClient]++;
                }
            }

            /* **********EXPORT****** */
            ExportResultSet( "ExecuteProcessList");

        }

        /// <summary>
        /// export process result as csv file
        /// </summary>
        /// <param name="ResultSet"></param>
        /// <param name="MethodName"></param>
        void ExportResultSet(string MethodName, bool IsDateFilterApplied = false)
        {
            string ExportedFileName = string.Format("Output_Method_{0} {1}.txt", MethodName,
               IsDateFilterApplied ? "_DateFilter" : "");

            using (StreamWriter sr = new StreamWriter(ExportedFileName))
            {
                // caption
                sr.WriteLine(string.Format(ExportFileFormat, "DISTINCT_PLAY_COUNT", "CLIENT_COUNT"));

                var sortKeyList = ResultSet.Keys.ToList();
                sortKeyList.Sort();

                foreach (var item in sortKeyList)
                {                                         // keyValue is distinct_play_count, value is number_of_client 
                    sr.WriteLine(string.Format(ExportFileFormat, item.ToString(), ResultSet[item].ToString()));
                }
            }
        }

        /// <summary>
        /// [Q2] How many users played 346 distinct songs? mehtod takes desired distinct song number as parameter
        /// </summary>
        /// <param name="SongNumber"></param>
        /// <returns></returns>
        public int GetUsersCountByPlayListNumber(int PlayListNumber)
        {
            if (ResultSet == null || ResultSet.FirstOrDefault().Key == 0)
                PiProcessHasList();

            int UserCount = 0;
            return ResultSet.TryGetValue(PlayListNumber, out UserCount) ? UserCount : 0;

        }

        /// <summary>
        /// [Q3] What is the maximum number of distinct songs  played?
        /// </summary>
        /// <returns></returns>
        public int GetMaximumNumberOfDistinctSongList()
        {
            if (ResultSet == null || ResultSet.FirstOrDefault().Key == 0)
                PiProcessHasList();

            return ResultSet.Keys.Max();
        }

    }
}
