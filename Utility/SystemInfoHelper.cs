using System.Management;

namespace CodeHelper.Utility
{
    /// <summary>
    /// 获取硬件及系统信息
    /// </summary>
    public class SystemInfoHelper
    {
        /// <summary>
        /// 获取操作系统Id
        /// </summary>
        /// <returns></returns>
        public static string GetSystemId()
        {
            string systemId = "";
            using (ManagementObjectSearcher mos = new ManagementObjectSearcher("select * from Win32_ComputerSystemProduct"))
            {
                foreach (var item in mos.Get())
                {
                    systemId = item["UUID"].ToString();
                    break;
                }
            }
            return systemId;
        }

        /// <summary>
        /// 获取CPU序列号
        /// </summary>
        /// <returns></returns>
        public static string GetCPUSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                string cpuSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    cpuSerialNumber = mo["ProcessorId"].ToString().Trim();
                    break;
                }
                return cpuSerialNumber;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取主板序列号
        /// </summary>
        /// <returns></returns>
        public static string GetBIOSSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_BIOS");
                string biosSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    biosSerialNumber = mo.GetPropertyValue("SerialNumber").ToString().Trim();
                    break;
                }
                return biosSerialNumber;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取硬盘序列号
        /// </summary>
        /// <returns></returns>
        public static string GetHardDiskSerialNumber()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                string hardDiskSerialNumber = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    hardDiskSerialNumber = mo["SerialNumber"].ToString().Trim();
                    break;
                }
                return hardDiskSerialNumber;
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// 获取网卡地址
        /// </summary>
        /// <returns></returns>
        public static string GetNetCardMACAddress()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE ((MACAddress Is Not NULL) AND (Manufacturer <> 'Microsoft'))");
                string netCardMACAddress = "";
                foreach (ManagementObject mo in searcher.Get())
                {
                    netCardMACAddress = mo["MACAddress"].ToString().Trim();
                    break;
                }
                return netCardMACAddress;
            }
            catch
            {
                return "";
            }
        }
    }
}
