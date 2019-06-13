# todo check unused imports
import logging
import os
import socket


class TcpClient:
    socket = None

    def __init__(self):
        # Create a TCP/IP socket
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    def send_data(self, command):
        try:
            # Send data
            # todo add logging
            encoded_command = (str(command) + os.linesep).encode('ascii')
            self.socket.sendall(encoded_command)

            # Look for the response
            exit_code = self.socket.recv()
            logging.debug('Received code {!r}'.format(exit_code))
            return exit_code

        finally:
            logging.debug('closing socket')
            self.socket.close()
