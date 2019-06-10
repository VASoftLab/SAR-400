import socket
# todo check unused imports
import sys
import array
import os


class TcpClient:

    def send_data(self, command):
        # Create a TCP/IP socket
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

        # Connect the socket to the port where the server is listening
        server_address = ('localhost', 10000)
        print('connecting to {} port {}'.format(*server_address))
        sock.connect(server_address)

        try:
            # Send data
            # todo add logging
            sock.sendall(array.array('B', str(command) + os.linesep))

            # Look for the response
            amount_received = 0
            amount_expected = len(message)

            while amount_received < amount_expected:
                data = sock.recv(16)
                amount_received += len(data)
                print('received {!r}'.format(data))

        finally:
            print('closing socket')
            sock.close()
