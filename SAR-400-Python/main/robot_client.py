import logging
import datetime
from main.robot import Robot
from main.client import TcpClient


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
            logging.debug('Connecting to %s port %s'.format(*server_address))
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
            logging.debug('')

    def execute(self, time_to_execute, robot_commands=[]):
        logging.debug('')
        # self.connect(self)

    def prepare_command(self, comand_duration, commands_joints=[]):
        joint_names = []
        joint_values = []
        for joint in commands_joints:
            joint_names.append(joint.joint_name)
            joint_values.append(joint.joint_value)

        return "robot:motors:" + \
               "".join(joint_names) + \
               ":GO:" + \
               "".join(joint_values) + \
               str(comand_duration)
        # todo check dimension for time


def __del__(self):
    if self.client is not None:
        self.client.socket.close()
