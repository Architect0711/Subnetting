using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Subnetting
{
    public partial class Form1 : Form
    {

        IPAddress IP;
        IPAddress Subnet;
        IPAddress HostPortion;
        IPAddress NetworkPortion;
        bool validcombination;

        #region GUI

        public Form1()
        {
            InitializeComponent();
            textBoxIPBit.ReadOnly = true;
            textBoxIPBit2.ReadOnly = true;
            textBoxIPBit3.ReadOnly = true;
            textBoxIPBit4.ReadOnly = true;
            textBoxSubnetBit.ReadOnly = true;
            textBoxSubnetBit2.ReadOnly = true;
            textBoxSubnetBit3.ReadOnly = true;
            textBoxSubnetBit4.ReadOnly = true;
            textBoxNetIPBit.ReadOnly = true;
            textBoxNetIPBit2.ReadOnly = true;
            textBoxNetIPBit3.ReadOnly = true;
            textBoxNetIPBit4.ReadOnly = true;
            textBoxHostIPBit.ReadOnly = true;
            textBoxHostIPBit2.ReadOnly = true;
            textBoxHostIPBit3.ReadOnly = true;
            textBoxHostIPBit4.ReadOnly = true;
            textBoxHostIPDec.ReadOnly = true;
            textBoxNetIPDec.ReadOnly = true;
            textBoxCIDR.ReadOnly = true;
            textBoxBroadcastAddress.ReadOnly = true;
            textBoxBroadcastBit.ReadOnly = true;
            textBoxBroadcastBit2.ReadOnly = true;
            textBoxBroadcastBit3.ReadOnly = true;
            textBoxBroadcastBit4.ReadOnly = true;
            textBoxNetworkAddress.ReadOnly = true;
            textBoxNetaddrBit.ReadOnly = true;
            textBoxNetaddrBit2.ReadOnly = true;
            textBoxNetaddrBit3.ReadOnly = true;
            textBoxNetaddrBit4.ReadOnly = true;
            textBoxFirstHost.ReadOnly = true;
            textBoxFirstHostBit.ReadOnly = true;
            textBoxFirstHostBit2.ReadOnly = true;
            textBoxFirstHostBit3.ReadOnly = true;
            textBoxFirstHostBit4.ReadOnly = true;
            textBoxLastHost.ReadOnly = true;
            textBoxLastHostBit.ReadOnly = true;
            textBoxLastHostBit2.ReadOnly = true;
            textBoxLastHostBit3.ReadOnly = true;
            textBoxLastHostBit4.ReadOnly = true;
            textBoxAvailableHosts.ReadOnly = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region IPv4Calc

        private void textBoxIPDec_TextChanged(object sender, EventArgs e)
        {
            if (textBoxIPDec.Text.isIPAddress(false) && IPAddress.TryParse(textBoxIPDec.Text, out IP))
            {
                textBoxIPDec.BackColor = Color.White;
                FillBitwiseNotation(IP.getBinaryString(), ref textBoxIPBit, ref textBoxIPBit2, ref textBoxIPBit3, ref textBoxIPBit4);
                //FillIPBit(IP.GetBinaryString());
                if (textBoxSubnetDec.Text.isIPAddress(true) && IPAddress.TryParse(textBoxSubnetDec.Text, out Subnet))
                {
                    CalcHostAndNetwork();
                }
            }
            else
            {
                FillBitwiseNotation("", ref textBoxIPBit, ref textBoxIPBit2, ref textBoxIPBit3, ref textBoxIPBit4);
                ClearAllTextBoxes();
                if (textBoxIPDec.Text.Length > 0)
                {
                    textBoxIPDec.BackColor = Color.OrangeRed;
                }
                else
                {
                    textBoxIPDec.BackColor = Color.White;
                }
            }
        }

        private void textBoxSubnetDec_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSubnetDec.Text.isIPAddress(true) && IPAddress.TryParse(textBoxSubnetDec.Text, out Subnet))
            {
                textBoxSubnetDec.BackColor = Color.White;
                FillBitwiseNotation(Subnet.getBinaryString(), ref textBoxSubnetBit, ref textBoxSubnetBit2, ref textBoxSubnetBit3, ref textBoxSubnetBit4);
                //FillSubnetBit(Subnet.GetBinaryString());
                textBoxCIDR.Text = Subnet.getCIDRNotation();
                if (textBoxIPDec.Text.isIPAddress(false) && IPAddress.TryParse(textBoxIPDec.Text, out IP))
                {
                    CalcHostAndNetwork();
                }
            }
            else
            {
                FillBitwiseNotation("", ref textBoxSubnetBit, ref textBoxSubnetBit2, ref textBoxSubnetBit3, ref textBoxSubnetBit4);
                textBoxCIDR.Text = "";
                ClearAllTextBoxes();
                if (textBoxSubnetDec.Text.Length > 0)
                {
                    textBoxSubnetDec.BackColor = Color.OrangeRed;
                }
                else
                {
                    textBoxSubnetDec.BackColor = Color.White;
                }
            }
        }

        private void ClearAllTextBoxes()
        {
            enableSubnetting(false);

            textBoxHostIPBit.Text = "";
            textBoxHostIPDec.Text = "";
            FillBitwiseNotation("", ref textBoxHostIPBit, ref textBoxHostIPBit2, ref textBoxHostIPBit3, ref textBoxHostIPBit4);

            textBoxNetIPBit.Text = "";
            textBoxNetIPDec.Text = "";
            FillBitwiseNotation("", ref textBoxNetIPBit, ref textBoxNetIPBit2, ref textBoxNetIPBit3, ref textBoxNetIPBit4);
        }

        private void CalcHostAndNetwork()
        {
            enableSubnetting(true);
            HostPortion = IP.getHostPortion(Subnet);
            NetworkPortion = IP.getNetworkPortion(Subnet);
            textBoxHostIPDec.Text = HostPortion.ToString();
            textBoxNetIPDec.Text = NetworkPortion.ToString();

            FillBitwiseNotation(HostPortion.getBinaryString(), ref textBoxHostIPBit, ref textBoxHostIPBit2, ref textBoxHostIPBit3, ref textBoxHostIPBit4);
            FillBitwiseNotation(NetworkPortion.getBinaryString(), ref textBoxNetIPBit, ref textBoxNetIPBit2, ref textBoxNetIPBit3, ref textBoxNetIPBit4);
            MarkHostAndNetworkPortions(ref textBoxSubnetBit, 1);
            MarkHostAndNetworkPortions(ref textBoxSubnetBit2, 2);
            MarkHostAndNetworkPortions(ref textBoxSubnetBit3, 3);
            MarkHostAndNetworkPortions(ref textBoxSubnetBit4, 4);
        }

        private void enableSubnetting(bool enable)
        {
            if (enable)
            {
                label14.BackColor = Color.Salmon;
                label21.BackColor = Color.Salmon;
                panel4.BackColor = Color.Salmon;

                label15.BackColor = Color.MediumSeaGreen;
                label23.BackColor = Color.MediumSeaGreen;
                panel5.BackColor = Color.MediumSeaGreen;

                panel2.BackColor = Color.LemonChiffon;
                panel3.BackColor = Color.LemonChiffon;
            }
            else
            {
                label14.BackColor = Color.LightGray;
                label21.BackColor = Color.LightGray;
                panel4.BackColor = Color.LightGray;

                label15.BackColor = Color.LightGray;
                label23.BackColor = Color.LightGray;
                panel5.BackColor = Color.LightGray;

                panel2.BackColor = Color.LightGray;
                panel3.BackColor = Color.LightGray;
            }

            validcombination = enable;
            button1.Enabled = enable;
            button2.Enabled = enable;
            textBoxNumberOfHosts.Enabled = enable;
        }

        private void MarkHostAndNetworkPortions(ref RichTextBox Richie, int row)
        {
            int einser = 0;
            int nuller = 0;

            foreach (char item in Richie.Text)
            {
                if (item == '1')
                {
                    einser++;
                }
            }

            nuller = Richie.Text.Length - einser;
            Richie.Select(0, einser);
            Richie.SelectionColor = Color.Red;
            Richie.Select(einser, nuller);
            Richie.SelectionColor = Color.Green;

            switch (row)
            {
                case 1:
                    /*
                    textBoxIPBit.Select(0, einser);
                    textBoxIPBit.SelectionColor = Color.Red;
                    textBoxIPBit.Select(einser, nuller);
                    textBoxIPBit.SelectionColor = Color.Green;
                    
                    textBoxNetIPBit.Select(0, einser);
                    textBoxNetIPBit.SelectionColor = Color.Red;
                    textBoxNetIPBit.Select(einser, nuller);
                    textBoxNetIPBit.SelectionColor = Color.Green;
                    textBoxHostIPBit.Select(0, einser);
                    textBoxHostIPBit.SelectionColor = Color.Red;
                    textBoxHostIPBit.Select(einser, nuller);
                    textBoxHostIPBit.SelectionColor = Color.Green;*/
                    textBoxNetIPBit.Select(0, textBoxNetIPBit.Text.Length);
                    textBoxNetIPBit.SelectionColor = Color.Red;
                    textBoxHostIPBit.Select(0, textBoxHostIPBit.Text.Length);
                    textBoxHostIPBit.SelectionColor = Color.Green;
                    break;
                case 2:
                    /*
                    textBoxIPBit2.Select(0, einser);
                    textBoxIPBit2.SelectionColor = Color.Red;
                    textBoxIPBit2.Select(einser, nuller);
                    textBoxIPBit2.SelectionColor = Color.Green;
                    
                    textBoxNetIPBit2.Select(0, einser);
                    textBoxNetIPBit2.SelectionColor = Color.Red;
                    textBoxNetIPBit2.Select(einser, nuller);
                    textBoxNetIPBit2.SelectionColor = Color.Green;
                    textBoxHostIPBit2.Select(0, einser);
                    textBoxHostIPBit2.SelectionColor = Color.Red;
                    textBoxHostIPBit2.Select(einser, nuller);
                    textBoxHostIPBit2.SelectionColor = Color.Green;*/
                    textBoxNetIPBit2.Select(0, textBoxNetIPBit2.Text.Length);
                    textBoxNetIPBit2.SelectionColor = Color.Red;
                    textBoxHostIPBit2.Select(0, textBoxHostIPBit2.Text.Length);
                    textBoxHostIPBit2.SelectionColor = Color.Green;
                    break;
                case 3:
                    /*
                    textBoxIPBit3.Select(0, einser);
                    textBoxIPBit3.SelectionColor = Color.Red;
                    textBoxIPBit3.Select(einser, nuller);
                    textBoxIPBit3.SelectionColor = Color.Green;
                    
                    textBoxNetIPBit3.Select(0, einser);
                    textBoxNetIPBit3.SelectionColor = Color.Red;
                    textBoxNetIPBit3.Select(einser, nuller);
                    textBoxNetIPBit3.SelectionColor = Color.Green;
                    textBoxHostIPBit3.Select(0, einser);
                    textBoxHostIPBit3.SelectionColor = Color.Red;
                    textBoxHostIPBit3.Select(einser, nuller);
                    textBoxHostIPBit3.SelectionColor = Color.Green;*/
                    textBoxNetIPBit3.Select(0, textBoxNetIPBit3.Text.Length);
                    textBoxNetIPBit3.SelectionColor = Color.Red;
                    textBoxHostIPBit3.Select(0, textBoxHostIPBit3.Text.Length);
                    textBoxHostIPBit3.SelectionColor = Color.Green;
                    break;
                case 4:
                    /*
                    textBoxIPBit4.Select(0, einser);
                    textBoxIPBit4.SelectionColor = Color.Red;
                    textBoxIPBit4.Select(einser, nuller);
                    textBoxIPBit4.SelectionColor = Color.Green;
                    
                    textBoxNetIPBit4.Select(0, einser);
                    textBoxNetIPBit4.SelectionColor = Color.Red;
                    textBoxNetIPBit4.Select(einser, nuller);
                    textBoxNetIPBit4.SelectionColor = Color.Green;
                    textBoxHostIPBit4.Select(0, einser);
                    textBoxHostIPBit4.SelectionColor = Color.Red;
                    textBoxHostIPBit4.Select(einser, nuller);
                    textBoxHostIPBit4.SelectionColor = Color.Green;*/
                    textBoxNetIPBit4.Select(0, textBoxNetIPBit4.Text.Length);
                    textBoxNetIPBit4.SelectionColor = Color.Red;
                    textBoxHostIPBit4.Select(0, textBoxHostIPBit4.Text.Length);
                    textBoxHostIPBit4.SelectionColor = Color.Green;
                    break;
                default:

                    break;
            }
        }

        private void FillBitwiseNotation(string bitwise, ref RichTextBox Row1, ref RichTextBox Row2, ref RichTextBox Row3, ref RichTextBox Row4)
        {
            if (bitwise.Length == 32)
            {
                Row1.Text = bitwise.Substring(0, 8);
                Row2.Text = bitwise.Substring(8, 8);
                Row3.Text = bitwise.Substring(16, 8);
                Row4.Text = bitwise.Substring(24, 8);
            }
            else
            {
                Row1.Text = "";
                Row2.Text = "";
                Row3.Text = "";
                Row4.Text = "";
            }
        }

        #endregion

        #region Subnetting

        private int getReservedHostBits(int requiredHosts)
        {
            int bits = 0;
            for (int i = 0; i < 32; i++)
            {
                if (requiredHosts > (Math.Pow(2, i) - 2))
                {
                    bits++;
                }
                else
                {
                    break;
                }
            }
            return bits;
        }

        private void button1_Click(object sender, EventArgs e)  // Add Subnet
        {
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("10000000000000000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11000000000000000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11100000000000000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11110000000000000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111000000000000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111100000000000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111110000000000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111000000000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111100000000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111110000000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111000000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111100000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111110000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111000000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111100000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111110000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111000000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111100000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111110000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111111000000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111111100000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111111110000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111111111000000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111111111100000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111111111110000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111111111111000000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111111111111100000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111111111111110000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111111111111111000"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111111111111111100"));
                listBoxSubnets.Items.Add("Combinations: " + calcmaxaddresses("11111111111111111111111111111110"));
        }

        public long calcmaxaddresses(string sub)  // only called once by constructor
        {
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

        private void button2_Click(object sender, EventArgs e)  // Clear Subnets
        {
            listBoxSubnets.Items.Clear();
        }

        private void textBoxNumberOfHosts_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBoxSubnets_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion

        private void label25_Click(object sender, EventArgs e)
        {

        }
    }
}
