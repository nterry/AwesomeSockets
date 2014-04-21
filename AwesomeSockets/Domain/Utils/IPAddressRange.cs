//This class was modified from a class obtained from Richard Szalay on Stack Overflow at 
//http://stackoverflow.com/questions/2138706/how-to-check-a-input-ip-fall-in-a-specific-ip-range

using System.Net;
using System.Net.Sockets;

public class IPAddressRange
{
    public static bool CheckRange(string lowerAddress, string upperAddress, string ipToCheck)
    {
        return CheckRange(IPAddress.Parse(lowerAddress), 
            IPAddress.Parse(upperAddress), IPAddress.Parse(ipToCheck));
    }

    public static bool CheckRange(IPAddress lowerAddress, IPAddress upperAddress, IPAddress ipToCheck)
    {
        byte[] lowerBytes = lowerAddress.GetAddressBytes();
        byte[] upperBytes = upperAddress.GetAddressBytes();
        byte[] addressBytes = ipToCheck.GetAddressBytes();
        bool lowerBoundary = true, upperBoundary = true;

        if ((lowerAddress.AddressFamily == upperAddress.AddressFamily) && 
            (lowerAddress.AddressFamily == ipToCheck.AddressFamily) && 
            (upperAddress.AddressFamily == ipToCheck.AddressFamily))
        {
            return false;
        }

        return RunCheck(lowerBytes, upperBytes, addressBytes, ref lowerBoundary, ref upperBoundary);
    }

    private static bool RunCheck(byte[] lowerBytes, byte[] upperBytes, byte[] addressBytes, ref bool lowerBoundary, ref bool upperBoundary)
    {
        for (int i = 0; i < lowerBytes.Length &&
            (lowerBoundary || upperBoundary); i++)
        {
            if ((lowerBoundary && addressBytes[i] < lowerBytes[i]) ||
                (upperBoundary && addressBytes[i] > upperBytes[i]))
            {
                return false;
            }

            lowerBoundary &= (addressBytes[i] == lowerBytes[i]);
            upperBoundary &= (addressBytes[i] == upperBytes[i]);
        }

        return true;
    }
}