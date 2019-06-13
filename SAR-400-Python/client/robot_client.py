import logging

from client.tcp_client import TcpClient
from robot.robot import Robot


class RobotClient:
    robot = None
    client = None
    connected = False

    def __init__(self):
        self.robot = Robot()
        self.client = TcpClient()

    def is_connected(self):
        return self.connected

    def connect(self):
        if not self.connected:
            server_address = (self.robot.host, self.robot.port)
            logging.debug('Connecting to %s port %s', server_address[0], server_address[1])
            self.client.socket.connect(server_address)
            self.connected = True
        logging.debug("Is robot connected: %s", self.connected)

    def disconnect(self):
        if self.connected:
            self.client.socket.close()
        logging.debug("Is robot connected: %s", self.connected)

    def execute(self, robot_commands=[]):
        self.connect()
        if self.is_connected():
            logging.debug("Start to execute %s amount of commands", len(robot_commands))
            for command in robot_commands:
                self.client.send_data(self.prepare_command(command.command_duration, command.command_joints))
        else:
            logging.debug('Couldn\'t execute commands. Connection to robot has not established')

    # todo check if brackets is needed
    def prepare_command(self, command_duration, commands_joints=[]):
        joint_names = []
        joint_values = []
        for joint in commands_joints:
            joint_names.append(joint.joint_name)
            joint_values.append(joint.joint_value)

        return "robot:motors:" + \
               "".join(joint_names) + \
               ":" + self.robot.action + ":" + \
               "".join(joint_values) + \
               str(command_duration)
        # todo check dimension for time


def __del__(self):
    if self.client is not None:
        self.client.socket.close()
