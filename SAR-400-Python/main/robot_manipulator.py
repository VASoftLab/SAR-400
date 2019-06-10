import sys
import logging

from main.csv_reader import parse_csv
from main.robot_client import RobotClient

logging.debug('Number of arguments: %s arguments.', len(sys.argv))
logging.debug('Argument List: %s', str(sys.argv))

robot_client = None

try:
    commands_to_execute = parse_csv(sys.argv[1], ';')
    robot_client = RobotClient()
    robot_client.connect()
    robot_client.execute(commands_to_execute)
    robot_client.disconnect()
    logging.debug("Successfully executed commands from file %s", sys.argv[1])
except BaseException as exception:
    logging.error("General exception or error occurred. Check logs. Exception: %s", exception)
    if robot_client is not None:
        robot_client.disconnect()
