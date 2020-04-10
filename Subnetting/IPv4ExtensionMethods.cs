using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Collections;

namespace Subnetting
{
    public static class IPv4ExtensionMethods
    {

        public static string getBinaryString(this IPAddress DecimalIP)
        {
            string Binary = "";
            byte[] byteIP = DecimalIP.GetAddressBytes();
            BitArray Bitz = new BitArray(byteIP);

            for (int i = 0; i < 4; i++)
            {
                for (int y = (i * 8 + 7); y >= (i * 8); y--)
                {
                    if (Bitz[y])
                    { Binary = Binary + "1"; }
                    else
                    { Binary = Binary + "0"; }
                }
            }
            return Binary;
        }

        public static byte ConvertToByte(this BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }

        public static IPAddress getNetworkPortion(this IPAddress ipAddress, IPAddress subnetMask)
        {
            byte[] byteIP = ipAddress.GetAddressBytes();
            byte[] byteSubnet = subnetMask.GetAddressBytes();
            BitArray IP = new BitArray(byteIP);
            BitArray Sub = new BitArray(byteSubnet);
            BitArray FirstOctet = new BitArray(8);
            BitArray SecondOctet = new BitArray(8);
            BitArray ThirdOctet = new BitArray(8);
            BitArray FourthOctet = new BitArray(8);

            for (int i = 0; i < 8; i++)
            {
                FirstOctet[i]   =   IP[i] & Sub[i];
                SecondOctet[i]  =   IP[i + 8] & Sub[i + 8];
                ThirdOctet[i]   =   IP[i + 16] & Sub[i + 16];
                FourthOctet[i]  =   IP[i + 24] & Sub[i + 24];
            }

            byte [] octs = new byte[] { FirstOctet.ConvertToByte(), SecondOctet.ConvertToByte(), ThirdOctet.ConvertToByte(), FourthOctet.ConvertToByte() };
            IPAddress Output = new IPAddress( octs );
            return Output;
        }

        public static IPAddress getHostPortion(this IPAddress ipAddress, IPAddress subnetMask)
        {
            byte[] byteIP = ipAddress.GetAddressBytes();
            byte[] byteSubnet = subnetMask.GetAddressBytes();
            BitArray IP = new BitArray(byteIP);
            BitArray Sub = new BitArray(byteSubnet);
            BitArray FirstOctet = new BitArray(8);
            BitArray SecondOctet = new BitArray(8);
            BitArray ThirdOctet = new BitArray(8);
            BitArray FourthOctet = new BitArray(8);

            for (int i = 0; i < 8; i++)
            {
                FirstOctet[i]   = IP[i] & !Sub[i];
                SecondOctet[i]  = IP[i + 8] & !Sub[i + 8];
                ThirdOctet[i]   = IP[i + 16] & !Sub[i + 16];
                FourthOctet[i]  = IP[i + 24] & !Sub[i + 24];
            }

            byte[] octs = new byte[] { FirstOctet.ConvertToByte(), SecondOctet.ConvertToByte(), ThirdOctet.ConvertToByte(), FourthOctet.ConvertToByte() };
            IPAddress Output = new IPAddress(octs);
            return Output;
        }

        public static IPAddress getIPFromBitwise(this string BinaryIP)
        {
            IPAddress Decimal = IPAddress.Parse("0.0.0.1");
            List<int> octets = new List<int>();
            int octet = 0;
            int cnt = 0;
            string ip = "";

            if (BinaryIP.Length == 32)
            {
                for (int i = 0; i < 4; i++)
                {
                    octet = 0;
                    cnt = 0;

                    for (int j = 7; j >= 0; j--)
                    {
                        if (BinaryIP.ElementAt((i * 8) + j) == '1')
                        {
                            octet = octet + Convert.ToInt32(Math.Pow(2, cnt));
                            Console.WriteLine("octet: " + octet + " (+" + Math.Pow(2, cnt) + ")");
                        }
                        cnt++;
                    }
                    octets.Add(octet);
                }

                for (int i = 0; i < octets.Count; i++)
                {
                    ip = ip + octets.ElementAt(i).ToString();
                    if (i != octets.Count - 1)
                    {
                        ip = ip + ".";
                    }
                }

                Decimal = IPAddress.Parse(ip);
            }

            return Decimal;
        }

        public static bool isIPAddress(this string dotteddecimaladdress, bool isSubnetmask)
        {
            bool plausible = false;
            IPAddress ip;

            if (dotteddecimaladdress.Contains(".") && IPAddress.TryParse(dotteddecimaladdress, out ip))
            {
                Regex regex = new Regex(@"\.");
                string[] octets = regex.Split(dotteddecimaladdress);
                if (octets.Length == 4)
                {
                    int cnt = 0;
                    int oct;
                    if (!isSubnetmask)  // normal IP
                    {
                        for (cnt = 0; cnt < 4; cnt++)
                        {
                            if (Int32.TryParse(octets[cnt], out oct))
                            {
                                if (oct >= 0 && oct <= 255)
                                {
                                    cnt++;
                                }
                            }
                        }
                    }
                    else
                    {
                        int[] sub = new int[] { 0, 128, 192, 224, 240, 248, 252, 254 };
                        //255
                        for (int i = 0; i < 4; i++)
                        {
                            if(Int32.Parse(octets[cnt])==255)
                            {
                                cnt++;
                            }
                        }

                        //not 255
                        if (cnt < 4)
                        {
                            for (int i = cnt; i < 4; i++)
                            {
                                if (sub.Contains(Int32.Parse(octets[cnt])))
                                {
                                    cnt++;
                                    break;
                                }
                            }
                        }

                        //0
                        if (cnt < 4)
                        {
                            for (int i = cnt; i < 4; i++)
                            {
                                if (Int32.Parse(octets[cnt]) == 0)
                                {
                                    cnt++;
                                }
                            }
                        }
                    }

                    if (cnt == 4)
                    {
                        plausible = true;
                    }
                }
            }
            return plausible;
        }

        public static string getCIDRNotation(this IPAddress Subnetmask)
        {
            string cidr = "/???";
            if (!Subnetmask.ToString().isIPAddress(true))
            {
                //throw new ArgumentException("Not a valid Subnetmask!");
            }
            else
            {
                int cnt = 0;
                string bitwisesub = Subnetmask.getBinaryString();
                for (int i = 0; i < bitwisesub.Length; i++)
                {
                    if (bitwisesub.ElementAt(i) == '1')
                    {
                        cnt++;
                    }
                }
                cidr = "/" + cnt.ToString();
            }

            return cidr;
        }

    }
}
