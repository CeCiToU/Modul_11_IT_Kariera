using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Get the local machine's host entry
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());

            // Obtain the first IP address from the list of addresses associated with the host
            IPAddress iPAddress = ipHostInfo.AddressList[0];

            // Create a remote end point by combining the obtained IP address with a specific port (in this case, port 11000)
            IPEndPoint remoteEP = new IPEndPoint(iPAddress, port: 11000);

            // Create a socket for sending data over a stream using TCP protocol
            Socket sender = new Socket(iPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // Connect the sender socket to the remote end point
                sender.Connect(remoteEP);

                // Display a message indicating successful connection
                Console.WriteLine($"Socket connected to {sender.RemoteEndPoint.ToString()}");

                // Continue sending messages until "exit" is entered
                while (true)
                {
                    // Prompt user for input
                    Console.Write(">");

                    // Read user input
                    string message = Console.ReadLine();

                    // Convert the message to a byte array and include "<EOF>" tag
                    byte[] msg = Encoding.ASCII.GetBytes(s: message + "<EOF>");

                    // Send the byte array over the socket
                    int bytesSent = sender.Send(msg);

                    // Check if the entered message is "exit" to break out of the loop
                    if (message == "exit")
                    {
                        break;
                    }
                }

                // Shutdown and close the sender socket after sending messages
                sender.Shutdown(how: SocketShutdown.Both);
                sender.Close();
            }
            catch (Exception ex)
            {
                // Print any exception messages to the console
                Console.WriteLine(ex.Message);
            }
        }
    }
}
