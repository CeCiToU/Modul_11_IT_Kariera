using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ChatServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Retrieve the local machine's host name
            string hostName = Dns.GetHostName();

            // Retrieve information about the host using the host name
            IPHostEntry ipHostInfo = Dns.GetHostEntry(hostName);

            // Obtain the first IP address from the list of addresses associated with the host
            IPAddress ipAddress = ipHostInfo.AddressList[0];

            // Create a local end point by combining the obtained IP address with a specific port (in this case, port 11000)
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port: 11000);

            // Create a socket that is listening whenever there is a package received
            Socket listener = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                byte[] buffer = new byte[1024];

                // Bind the socket to the local end point
                listener.Bind(localEndPoint);

                // Start listening with a backlog of 100 pending connections
                listener.Listen(backlog: 100);

                // Accept incoming connection and create a new socket for handling communication
                Socket handle = listener.Accept();

                while (true)
                {
                    string message = "";

                    while (true)
                    {
                        // Receive data into the buffer
                        int messageSize = handle.Receive(buffer);

                        // Convert the received data to a string
                        message += Encoding.ASCII.GetString(buffer, index: 0, count: messageSize);

                        // Check if the message contains "<EOF>" indicating the end of the message 'end of file'
                        if (message.Contains("<EOF>"))
                        {
                            // Remove the "<EOF>" tag from the message
                            message = message.Replace(oldValue: "<EOF>", newValue: "");
                            break;
                        }
                    }

                    // Print the received message to the console
                    Console.WriteLine("> " + message);

                    // Check if the received message is "exit"
                    if (message == "exit")
                    {
                        // Shutdown and close the socket
                        handle.Shutdown(how: SocketShutdown.Both);
                        handle.Close();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                // Print any exception messages to the console
                Console.WriteLine(ex.Message);
            }

            // Clear the console and display a goodbye message
            Console.Clear();
            Console.WriteLine("Goodbye");

            // Wait for a key press before exiting the program
            Console.ReadKey(intercept: true);
        }
    }
}
