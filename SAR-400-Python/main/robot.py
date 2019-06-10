import configparser
import os


class Robot:
    host = ""
    port = 8080
    network_stream = None
    stream_reader = None
    wait = False
    return_string = ""
    cultural_info = ""
    connected = False

    def __init__(self):
        config_parser = configparser.ConfigParser()
        config_parser.read(os.getcwd() + '/../resources/connection.properties')
        self.host = config_parser.get("DEFAULT", "host")
        self.port = config_parser.get("DEFAULT", "port")
        self.wait = config_parser.get("DEFAULT", "wait")
        self.cultural_info = config_parser.get("DEFAULT", "locale")

    def __repr__(self):
        return "Robot parameters:" + \
               "\n\tHost:" + self.host + \
               "\n\tPort:" + self.port + \
               "\n\tNetworkStream:" + str(self.network_stream) + \
               "\n\tStreamReader:" + str(self.stream_reader) + \
               "\n\tWait:" + self.wait + \
               "\n\tReturnString:" + self.return_string + \
               "\n\tCultutralInfo:" + self.cultural_info
