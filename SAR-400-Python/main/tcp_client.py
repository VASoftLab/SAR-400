# todo check unused imports
import array
import os
import socket
import logging


class TcpClient:
    socket = None

    def __init__(self):
        # Create a TCP/IP socket
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    def send_data(self, command):
        try:
            # Send data
            # todo add logging
            self.socket.sendall(array.array('B', str(command) + os.linesep))

            # Look for the response
            amount_received = 0
            amount_expected = len(command)

            while amount_received < amount_expected:
                data = self.socket.recv(16)
                amount_received += len(data)
                logging.debug('received {!r}'.format(data))

        finally:
            logging.debug('closing socket')
            self.socket.close()
