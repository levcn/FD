using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;


namespace Fw.WindowsSystem
{
    public class WSystemInfo
    {
        /// <summary>
        /// 返回CPU编号
        /// </summary>
        /// <returns></returns>
        public static string GetCPUID()
        {
            ManagementClass managementClass = new ManagementClass("Win32_Processor");
            ManagementObjectCollection instances = managementClass.GetInstances();
            string cpuid = "";
            foreach (ManagementObject myObject in instances)
            {
                cpuid = myObject.Properties["ProcessorId"].Value.ToString();
                break;
            }
            return cpuid;
        }
    }
}
