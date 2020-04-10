using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;

namespace Subnetting
{
    class IPv4VLSMCollection
    {
        private IPAddress   ipaddress;
        private IPAddress   subnetmask;
        private IPAddress   hostportion;

        private List<Subnet>    subnets;
        private long        addresses_max;
        private long        addresses_remaining;

        public IPAddress Ipaddress
        {
            get
            {
                return ipaddress;
            }
        }

        public IPAddress Subnetmask
        {
            get
            {
                return subnetmask;
            }
        }

        public IPAddress Hostportion
        {
            get
            {
                return hostportion;
            }

            set
            {
                hostportion = value;
            }
        }

        public IPv4VLSMCollection(IPAddress ipaddress, IPAddress subnetmask, IPAddress hostportion)
        {
            this.ipaddress = ipaddress;
            this.subnetmask = subnetmask;
            this.hostportion = hostportion;
            addresses_max = calcmaxaddresses(subnetmask);
            addresses_remaining = addresses_max;
            subnets = new List<Subnet>();
        }

        private long calcmaxaddresses(IPAddress subnetmask)  // only called once by constructor
        {
            string sub = subnetmask.getBinaryString();
            long maxaddresses = 0;
            int exp = 32;

            for (int i = 0; i < sub.Length; i++)
            {
                if (sub.ElementAt(i) == '1')
                {
                    exp--;
                }
            }

            maxaddresses = Convert.ToInt64(Math.Pow(2, exp));

            return maxaddresses;
        }

        public bool AddSubnet(Subnet subnet)
        {
            bool canadd = false;
            if ((addresses_remaining - subnet.UsedIPs) >= 0)
            { 
                canadd = true;
                addresses_remaining = addresses_remaining - subnet.UsedIPs;



                subnets.Add(subnet);
            }
            

            return canadd;
        }

    }
}
