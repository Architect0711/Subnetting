using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace Subnetting
{
    class Subnet
    {

        #region Variables & Constructor

        // Taken From Constructor
        private int requiredHosts;

        // Self Calculated
        private int actualHosts;
        private int usedIPs;
        private string subnetMaskCIDR;

        // Calculated later
        private IPAddress ip;
        private IPAddress subnetMask;
        private IPAddress networkAddress;
        private IPAddress broadcastAddress;

        public Subnet(int requiredHosts)
        {
            if (requiredHosts > 0 && requiredHosts < (Math.Pow(2, 31)))
            {
                this.requiredHosts = requiredHosts;
                calcSize(requiredHosts);
            }
            else
            {
                throw new ArgumentException("invalid host number", "requiredHosts");
            }
        }
        
        public string SubnetMaskCIDR
        {
            get
            {
                return subnetMaskCIDR;
            }
        }

        public int RequiredHosts
        {
            get
            {
                return requiredHosts;
            }
        }

        public int ActualHosts
        {
            get
            {
                return actualHosts;
            }
        }

        public int UsedIPs
        {
            get
            {
                return usedIPs;
            }
        }

        public IPAddress Ip
        {
            get
            {
                return ip;
            }

            set
            {
                ip = value;
            }
        }

        public IPAddress SubnetMask
        {
            get
            {
                return subnetMask;
            }

            set
            {
                subnetMask = value;
            }
        }

        public IPAddress NetworkAddress
        {
            get
            {
                return networkAddress;
            }

            set
            {
                networkAddress = value;
            }
        }

        public IPAddress BroadcastAddress
        {
            get
            {
                return broadcastAddress;
            }

            set
            {
                broadcastAddress = value;
            }
        }

        #endregion

        private void calcSize(int requiredHosts)
        {
            int hostbits = 0;
            while (requiredHosts > (Math.Pow(2, hostbits)-2))
            {
                hostbits++;
            }
            usedIPs = Convert.ToInt32((Math.Pow(2, hostbits)));
            actualHosts = usedIPs -2;
            subnetMaskCIDR = "/" + (32-hostbits).ToString() ;
        }

    }
}
