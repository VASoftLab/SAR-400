import configparser
import os


class Robot:
    host = ""
    port = 8080
    exit_code = ""
    action = "POSSET"

    def __init__(self):
        config_parser = configparser.ConfigParser()
        config_parser.read(os.getcwd() + '/resources/connection.properties')
        self.host = config_parser.get("DEFAULT", "host")
        self.port = int(config_parser.get("DEFAULT", "port"))
        self.action = config_parser.get("COMMAND", "action")

    def __repr__(self):
        return "Robot parameters:" + \
               "\n\tHost:" + self.host + \
               "\n\tPort:" + self.port + \
               "\n\tExitCode:" + self.exit_code + \
               "\n\tRobot action:" + self.action
