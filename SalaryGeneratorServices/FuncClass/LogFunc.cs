using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryGeneratorServices.FuncClass
{
    class LogFunc
    {
        private DateTimeFunc DateTimeFunc = new DateTimeFunc();

        public void WriteErrorLog(string data, string data2, string data3, string data4, string ServicesName, long ServiceProcessID)
        {
            DateTime GetDateTime = DateTimeFunc.GetDateTime();
            int year = GetDateTime.Year;
            int month = GetDateTime.Month;
            int day = GetDateTime.Day;
            long processid = 0;
            string stringmonth = month.ToString();
            string stringday = day.ToString();
            string path = "";
            string message = string.Format("Time: {0}", GetDateTime.ToString("dd/MM/yyyy hh:mm:ss tt"));
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            message += string.Format("Message: {0}", data);
            message += Environment.NewLine;
            message += string.Format("StackTrace: {0}", data2);
            message += Environment.NewLine;
            message += string.Format("Source: {0}", data3);
            message += Environment.NewLine;
            message += string.Format("TargetSite: {0}", data4);
            message += Environment.NewLine;
            message += "-----------------------------------------------------------";
            message += Environment.NewLine;
            stringmonth = (stringmonth.Length == 1 ? "0" + stringmonth : stringmonth);
            stringday = (stringday.Length == 1 ? "0" + stringday : stringmonth);
            processid = ServiceProcessID;
            path = AppDomain.CurrentDomain.BaseDirectory + "ErrorLog\\" + ServicesName + "_" + stringday + stringmonth + year + "_" + processid + ".txt";

            if (!File.Exists(path))
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(message);
                    writer.Close();
                }
            }
        }

        public void WriteProcessLog(string Log, string ServicesName, long ServiceProcessID)
        {
            DateTime GetDateTime = DateTimeFunc.GetDateTime();
            int year = GetDateTime.Year;
            int month = GetDateTime.Month;
            int day = GetDateTime.Day;
            long processid = 0;
            string stringmonth = month.ToString();
            string stringday = day.ToString();
            string path = "";
            
            stringmonth = (stringmonth.Length == 1 ? "0" + stringmonth : stringmonth);
            stringday = (stringday.Length == 1 ? "0" + stringday : stringmonth);
            processid = ServiceProcessID;
            path = AppDomain.CurrentDomain.BaseDirectory + "ProcessLog\\" + ServicesName + "_" + stringday + stringmonth + year + "_" + processid + ".txt";
            
            if (!File.Exists(path))
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.WriteLine(Log);
                    writer.Close();
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(Log);
                    writer.Close();
                }
            }
        }

        public void DataScTransChecking(string Log)
        {
            DateTime GetDateTime = DateTimeFunc.GetDateTime();
            int year = GetDateTime.Year;
            int month = GetDateTime.Month;
            int day = GetDateTime.Day;
            long processid = 0;
            string stringmonth = month.ToString();
            string stringday = day.ToString();
            string path = "";

            stringmonth = (stringmonth.Length == 1 ? "0" + stringmonth : stringmonth);
            stringday = (stringday.Length == 1 ? "0" + stringday : stringmonth);
            //processid = ServiceProcessID;
            path = AppDomain.CurrentDomain.BaseDirectory + "ProcessLog\\CheckDataScTrans_" + stringday + stringmonth + year + "_" + processid + ".txt";

            if (!File.Exists(path))
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.WriteLine(Log);
                    writer.Close();
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(Log);
                    writer.Close();
                }
            }
        }

        public void DataScTransChecking2(string Log, string Filename)
        {
            DateTime GetDateTime = DateTimeFunc.GetDateTime();
            int year = GetDateTime.Year;
            int month = GetDateTime.Month;
            int day = GetDateTime.Day;
            long processid = 0;
            string stringmonth = month.ToString();
            string stringday = day.ToString();
            string path = "";

            stringmonth = (stringmonth.Length == 1 ? "0" + stringmonth : stringmonth);
            stringday = (stringday.Length == 1 ? "0" + stringday : stringmonth);
            //processid = ServiceProcessID;
            path = AppDomain.CurrentDomain.BaseDirectory + "ProcessLog\\"+ Filename + "_" + stringday + stringmonth + year + "_" + processid + ".txt";

            if (!File.Exists(path))
            {
                using (StreamWriter writer = File.CreateText(path))
                {
                    writer.WriteLine(Log);
                    writer.Close();
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(Log);
                    writer.Close();
                }
            }
        }
    }
}
