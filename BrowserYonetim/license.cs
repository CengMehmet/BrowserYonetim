using System;
using System.Collections.Generic;
using System.Management;

namespace License
{
    internal class license
    {
        public license()
        {
        }

        public static string CPUSeriNo()
        {
            String processorID = "";
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("Select * FROM WIN32_Processor");
            ManagementObjectCollection mObject = searcher.Get();

            foreach (ManagementObject obj in mObject)
            {
                processorID = obj["ProcessorId"].ToString();
            }
            return processorID;
        }

        public static string AnakartSerino()
        {
            String anakartID = "";
            ManagementObjectSearcher MOS = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
            foreach (ManagementObject getserial in MOS.Get())
            {
                anakartID = getserial["SerialNumber"].ToString();
            }
            return anakartID;
        }

        public static string HDDserialNo = string.Empty;

        public static string HDDserino() //HDD Seri No
        {
            List<string> serialsList = HDDSeriNoCek();
            foreach (string s in serialsList)
            {
                HDDserialNo = HDDserialNo + s;
            }
            HDDserialNo = HDDserialNo.TrimStart(); //Baştaki Boşluğu Kaldırıyoruz.
            return HDDserialNo;
        }

        public static List<string> HDDSeriNoCek()
        {
            List<string> serials = new List<string>();
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
            ManagementObjectCollection disks = searcher.Get();
            foreach (ManagementObject disk in disks)
            {
                if (disk["SerialNumber"] == null)
                    serials.Add("");
                else
                    serials.Add(disk["SerialNumber"].ToString());
            }
            return serials;
        }
    }
}