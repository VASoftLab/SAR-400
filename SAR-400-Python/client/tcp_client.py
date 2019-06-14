import logging
import os
import socket
import time


class TcpClient:
    socket = None

    def __init__(self):
        # Create a TCP/IP socket
        self.socket = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    def send_data(self, command):
        try:
            # Send data
            # todo add logging
            logging.debug("Sending command to robot with amount of chars %d", len(command))
            encoded_command = (str(command) + os.linesep).encode('ascii')
            self.socket.sendall(encoded_command)

            # Look for the response
            exit_code = self.socket.recv(1)
            logging.debug('Received code {!r}'.format(exit_code))
            return exit_code
        except BaseException as exception:
            logging.debug('Error occurred during sending data through socket. Closing socket with exception %s',
                          exception)
            self.socket.close()
